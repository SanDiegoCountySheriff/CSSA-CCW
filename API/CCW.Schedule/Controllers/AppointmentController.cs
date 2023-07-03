using AutoMapper;
using CCW.Common.Models;
using CCW.Schedule.Clients;
using CCW.Schedule.Entities;
using CCW.Schedule.Models;
using CCW.Schedule.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Schedule.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IApplicationServiceClient _applicationHttpClient;
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper _mapper;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(
        IApplicationServiceClient applicationHttpClient,
        ICosmosDbService cosmosDbService,
        IMapper mapper,
        ILogger<AppointmentController> logger)
    {
        _applicationHttpClient = applicationHttpClient;
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _mapper = mapper;
        _logger = logger;
    }

    [Authorize(Policy = "AADUsers")]
    [HttpPost("createNewAppointments", Name = "createNewAppointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewAppointments([FromBody] AppointmentManagementRequestModel appointmentManagementRequest)
    {
        try
        {
            AppointmentManagement appointmentManagement = _mapper.Map<AppointmentManagement>(appointmentManagementRequest);
            var numberOfAppointmentsCreated = await _cosmosDbService.CreateAppointmentsFromAppointmentManagementTemplate(appointmentManagement, cancellationToken: default);

            return Ok(numberOfAppointmentsCreated);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to create appointments.");
        }
    }


    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [HttpGet("getAvailability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAvailability()
    {
        try
        {
            GetUserId(out var userId);

            var result = await _cosmosDbService.GetAvailableTimesAsync(cancellationToken: default);
            var appointments = _mapper.Map<List<AppointmentWindowResponseModel>>(result);

            return Ok(appointments);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve available appointments.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [HttpGet("getAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _cosmosDbService.GetAllBookedAppointmentsAsync(cancellationToken: default);
            var appointments = _mapper.Map<List<AppointmentWindowResponseModel>>(result);

            return Ok(appointments);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve all booked appointments.");
        }
    }


    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [HttpGet("get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(string applicationId)
    {
        try
        {
            GetUserId(out var userId);

            var appointment = await _cosmosDbService.GetAsync(applicationId, cancellationToken: default);
            return Ok(_mapper.Map<AppointmentWindowResponseModel>(appointment));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve appointment.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] AppointmentWindowCreateRequestModel appointmentRequest)
    {
        try
        {
            GetUserId(out var userId);

            var appointment = _mapper.Map<AppointmentWindow>(appointmentRequest);
            var appointmentCreated = await _cosmosDbService.AddAsync(appointment, cancellationToken: default);

            return Ok(_mapper.Map<AppointmentWindowResponseModel>(appointmentCreated));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to create appointment.");
        }
    }


    [Authorize(Policy = "B2CUsers")]
    [Route("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AppointmentWindowUpdateRequestModel appointment)
    {
        try
        {
            GetUserId(out var userId);

            if (appointment.Id == Guid.Empty.ToString())
            {
                var nextSlot = await _cosmosDbService.GetAvailableSlotByDateTime(appointment.Start, cancellationToken: default);
                if (nextSlot == null || nextSlot.Count < 1)
                    throw new ArgumentOutOfRangeException("start");

                var slot = nextSlot.First();
                appointment.Id = slot.Id.ToString();
                appointment.End = slot.End;
                appointment.IsManuallyCreated = slot.IsManuallyCreated;
            }

            AppointmentWindow appt = _mapper.Map<AppointmentWindow>(appointment);
            await _cosmosDbService.UpdateAsync(appt, cancellationToken: default);

            return Ok(_mapper.Map<AppointmentWindowResponseModel>(appt));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to update appointment.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [Route("updateUserAppointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> UpdateUserAppointment([FromBody] AppointmentWindowUpdateRequestModel appointment)
    {
        try
        {
            var existingAppointments = await _cosmosDbService.ResetApplicantAppointmentsAsync(appointment.ApplicationId, cancellationToken: default);

            if (appointment.Id == Guid.Empty.ToString())
            {
                var nextSlot = await _cosmosDbService.GetAvailableSlotByDateTime(appointment.Start, cancellationToken: default);
                if (nextSlot == null || nextSlot.Count < 1)
                    throw new ArgumentOutOfRangeException("start");

                var slot = nextSlot.First();
                appointment.Id = slot.Id.ToString();
                appointment.End = slot.End;
                appointment.IsManuallyCreated = slot.IsManuallyCreated;
            }

            AppointmentWindow appt = _mapper.Map<AppointmentWindow>(appointment);
            await _cosmosDbService.UpdateAsync(appt, cancellationToken: default);

            var response = await _applicationHttpClient.UpdateApplicationAppointmentAsync(appointment.ApplicationId,
                appointment.Start.ToString(Constants.DateTimeFormat), appointment.Id, cancellationToken: default);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to update appointment.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [Route("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpDelete]
    public async Task<IActionResult> Delete(string appointmentId)
    {
        try
        {
            await _cosmosDbService.DeleteAsync(appointmentId, cancellationToken: default);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to delete appointment.");
        }

        return Ok();
    }

    [Authorize(Policy = "AADUsers")]
    [Route("deleteAppointmentsByDate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAppointmentsByDate(DateTime date)
    {
        try
        {
            var response = await _cosmosDbService.DeleteAllAppointmentsByDate(date, cancellationToken: default);
            return Ok(response);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to delete appointments by date.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("deleteAppointmentsByTimeSlot")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAppointmentsByTimeSlot(DateTime date)
    {
        try
        {
            var response = await _cosmosDbService.DeleteAppointmentsByTimeSlot(date, cancellationToken: default);
            return Ok(response);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to delete appointments by date.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("checkInAppointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> CheckInAppointment(string appointmentId)
    {
        try
        {
            var appointment = await _cosmosDbService.GetAppointmentByIdAsync(appointmentId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            var applicationId = appointment.ApplicationId;
            appointment.Status = AppointmentStatus.CheckedIn;

            await _cosmosDbService.UpdateAsync(appointment, cancellationToken: default);

            var response = await _applicationHttpClient.AppointmentCheckInByApplicationId(applicationId,
                cancellationToken: default);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to reopen appointment.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("noShowAppointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> NoShowAppointment(string appointmentId)
    {
        try
        {
            var appointment = await _cosmosDbService.GetAppointmentByIdAsync(appointmentId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            var applicationId = appointment.ApplicationId;
            appointment.Status = AppointmentStatus.NoShow;

            await _cosmosDbService.UpdateAsync(appointment, cancellationToken: default);

            var response = await _applicationHttpClient.AppointmentNoShowByApplicationId(applicationId,
                cancellationToken: default);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to reopen appointment.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("setAppointmentScheduled")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> SetAppointmentScheduled(string appointmentId)
    {
        try
        {
            var appointment = await _cosmosDbService.GetAppointmentByIdAsync(appointmentId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            var applicationId = appointment.ApplicationId;
            appointment.Status = AppointmentStatus.Scheduled;

            await _cosmosDbService.UpdateAsync(appointment, cancellationToken: default);

            var response = await _applicationHttpClient.AppointmentScheduledByApplicationId(applicationId,
                cancellationToken: default);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to reopen appointment.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("reopenSlot")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpDelete]
    public async Task<IActionResult> ReopenSlot(string appointmentId)
    {
        try
        {
            var appointment = await _cosmosDbService.GetAppointmentByIdAsync(appointmentId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            if (appointment.IsManuallyCreated)
            {
                await _cosmosDbService.DeleteAsync(appointment.Id.ToString(), cancellationToken: default);
            }
            else
            {
                var applicationId = appointment.ApplicationId;

                appointment.ApplicationId = null;
                appointment.Status = AppointmentStatus.Available;
                appointment.Name = null;
                appointment.Permit = null;
                appointment.Payment = null;
                appointment.IsManuallyCreated = false;

                await _cosmosDbService.UpdateAsync(appointment, cancellationToken: default);
            }


            var response = await _applicationHttpClient.RemoveApplicationAppointmentAsync(appointment.ApplicationId,
                cancellationToken: default);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to reopen appointment.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("reopenSlotByApplicationId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> ReopenSlotByApplicationId(string applicationId)
    {
        try
        {
            var appointment = await _cosmosDbService.GetAsync(applicationId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            if (appointment.IsManuallyCreated)
            {
                await _cosmosDbService.DeleteAsync(appointment.Id.ToString(), cancellationToken: default);
            }
            else
            {
                appointment.ApplicationId = null;
                appointment.Status = AppointmentStatus.Available;
                appointment.Name = null;
                appointment.Permit = null;
                appointment.Payment = null;
                appointment.IsManuallyCreated = false;

                await _cosmosDbService.UpdateAsync(appointment, cancellationToken: default);
            }

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to reopen appointment.");
        }
    }

    [Authorize(Policy = "B2Cusers")]
    [Route("removeApplicationFromAppointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> RemoveApplicationFromAppointment(string applicationId, string appointmentId)
    {
        try
        {
            string applicationIdFromAppointment = "";
            var appointment = await _cosmosDbService.GetAppointmentByIdAsync(appointmentId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            if (appointment.IsManuallyCreated)
            {
                await _cosmosDbService.DeleteAsync(appointment.Id.ToString(), cancellationToken: default);
            }
            else
            {
                applicationIdFromAppointment = appointment.ApplicationId;

                appointment.ApplicationId = null;
                appointment.Status = AppointmentStatus.Available;
                appointment.Name = null;
                appointment.Permit = null;
                appointment.Payment = null;
                appointment.IsManuallyCreated = false;

                await _cosmosDbService.UpdateAsync(appointment, cancellationToken: default);

            }

            return Ok();

        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to reopen appointment.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("deleteSlotByApplicationId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpDelete]
    public async Task<IActionResult> DeleteSlotByApplicationId(string applicationId)
    {
        try
        {
            var appointment = await _cosmosDbService.GetAsync(applicationId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            await _cosmosDbService.DeleteAsync(appointment.Id.ToString(), cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to delete appointment.");
        }
    }


    private void GetUserId(out string userId)
    {
        userId = HttpContext.User.Claims
            .Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
            .Select(c => c.Value).FirstOrDefault();

        if (userId == null)
        {
            throw new ArgumentNullException("userId", "Invalid token.");
        }
    }
}