using CCW.Schedule.Models;
using CCW.Schedule.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using CsvHelper;
using CCW.Schedule.Entities;
using CCW.Schedule.Mappers;
using Microsoft.AspNetCore.Authorization;
using System;
using CCW.Schedule.Clients;
using CCW.Common.Models;

namespace CCW.Schedule.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IApplicationServiceClient _applicationHttpClient;
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper<AppointmentWindowCreateRequestModel, AppointmentWindow> _requestCreateApptMapper;
    private readonly IMapper<AppointmentWindowUpdateRequestModel, AppointmentWindow> _requestUpdateApptMapper;
    private readonly IMapper<AppointmentWindow, AppointmentWindowResponseModel> _responseMapper;
    private readonly IMapper<AppointmentManagementRequestModel, AppointmentManagement> _requestApptManagementMapper;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(
        IApplicationServiceClient applicationHttpClient,
        ICosmosDbService cosmosDbService,
        IMapper<AppointmentWindowCreateRequestModel, AppointmentWindow> requestCreateApptMapper,
        IMapper<AppointmentWindowUpdateRequestModel, AppointmentWindow> requestUpdateApptMapper,
        IMapper<AppointmentWindow, AppointmentWindowResponseModel> responseMapper,
        IMapper<AppointmentManagementRequestModel, AppointmentManagement> requestApptManagementMapper,
        ILogger<AppointmentController> logger)
    {
        _applicationHttpClient = applicationHttpClient;
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _requestCreateApptMapper = requestCreateApptMapper;
        _requestUpdateApptMapper = requestUpdateApptMapper;
        _requestApptManagementMapper = requestApptManagementMapper;
        _responseMapper = responseMapper;
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
            AppointmentManagement appointmentManagement = _requestApptManagementMapper.Map(appointmentManagementRequest);
            var numberOfAppointmentsCreated = await _cosmosDbService.CreateAppointmentsFromAppointmentManagementTemplate(appointmentManagement, cancellationToken: default);

            return Ok(numberOfAppointmentsCreated);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to create appointments.");
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
            var appointments = result.Select(x => _responseMapper.Map(x));

            return Ok(appointments);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve available appointments.");
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
            var appointments = result.Select(x => _responseMapper.Map(x));

            return Ok(appointments);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve all booked appointments.");
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
            return Ok(_responseMapper.Map(appointment));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve appointment.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] AppointmentWindowCreateRequestModel appointment)
    {
        try
        {
            GetUserId(out var userId);

            AppointmentWindow appt = _requestCreateApptMapper.Map(appointment);
            var appointmentCreated = await _cosmosDbService.AddAsync(appt, cancellationToken: default);

            return Ok(_responseMapper.Map(appointmentCreated));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to create appointment.");
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

            AppointmentWindow appt = _requestUpdateApptMapper.Map(appointment);
            await _cosmosDbService.UpdateAsync(appt, cancellationToken: default);

            return Ok(_responseMapper.Map(appt));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to update appointment.");
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

            AppointmentWindow appt = _requestUpdateApptMapper.Map(appointment);
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
            throw new Exception("An error occur while trying to update appointment.");
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
            throw new Exception("An error occur while trying to delete appointment.");
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
            throw new Exception("An error occur while trying to delete appointments by date.");
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
            throw new Exception("An error occur while trying to delete appointments by date.");
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
            throw new Exception("An error occur while trying to reopen appointment.");
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
            throw new Exception("An error occur while trying to reopen appointment.");
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
            throw new Exception("An error occur while trying to reopen appointment.");
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
            throw new Exception("An error occur while trying to reopen appointment.");
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
            throw new Exception("An error occur while trying to reopen appointment.");
        }
    }

    [Authorize(Policy = "B2Cusers")]
    [Route("removeApplicationFromAppointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> removeApplicationFromAppointment(string applicationId, string appointmentId)
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
            throw new Exception("An error occur while trying to reopen appointment.");
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
            throw new Exception("An error occur while trying to delete appointment.");
        }
    }


    private void GetUserId(out string? userId)
    {
        userId = this.HttpContext.User.Claims
            .Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
            .Select(c => c.Value).FirstOrDefault();

        if (userId == null)
        {
            throw new ArgumentNullException("userId", "Invalid token.");
        }
    }
}