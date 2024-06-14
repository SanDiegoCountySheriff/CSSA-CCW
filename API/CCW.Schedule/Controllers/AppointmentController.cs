using AutoMapper;
using CCW.Common.Enums;
using CCW.Common.Models;
using CCW.Common.RequestModels;
using CCW.Common.ResponseModels;
using CCW.Schedule.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PublicHoliday;

namespace CCW.Schedule.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentCosmosDbService _appointmentCosmosDbService;
    private readonly IApplicationCosmosDbService _applicationCosmosDbService;
    private readonly IMapper _mapper;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(
        IAppointmentCosmosDbService appointmentCosmosDbService,
        IApplicationCosmosDbService applicationCosmosDbService,
        IMapper mapper,
        ILogger<AppointmentController> logger)
    {
        _appointmentCosmosDbService = appointmentCosmosDbService;
        _applicationCosmosDbService = applicationCosmosDbService;
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
            var (numberOfAppointmentsCreated, holidayAppointmentsSkipped) = await _appointmentCosmosDbService.CreateAppointmentsFromAppointmentManagementTemplate(appointmentManagement, cancellationToken: default);

            return Ok(JsonConvert.SerializeObject((numberOfAppointmentsCreated, holidayAppointmentsSkipped)));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to create appointments.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getAppointmentManagementTemplate", Name = "getAppointmentManagementTemplate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAppointmentManagementTemplate()
    {
        try
        {
            return Ok(await _appointmentCosmosDbService.GetAppointmentManagementTemplate());
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to get the appointment management template.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getAgencyHolidays", Name = "getAgencyHolidays")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAgencyHolidays()
    {
        try
        {
            return Ok(await _appointmentCosmosDbService.GetOrganizationalHolidays());
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to get the appointment management template.");
        }
    }


    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [HttpGet("getAvailability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAvailability(bool includePastAppointments = false)
    {
        try
        {
            var result = await _appointmentCosmosDbService.GetAvailableTimesAsync(includePastAppointments, cancellationToken: default);
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
    [HttpGet("getBookedAppointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBookedAppointments(bool includePastAppointments = false)
    {
        try
        {
            var result = await _appointmentCosmosDbService.GetBookedAppointmentsAsync(includePastAppointments, cancellationToken: default);
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
    [HttpGet("getNumberOfNewAppointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetNumberOfNewAppointments(int numberOfDays)
    {
        try
        {
            var result = await _appointmentCosmosDbService.GetNumberOfNewAppointments(numberOfDays, cancellationToken: default);

            return Ok(result);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to get the number of new appointments.");
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
            var result = await _appointmentCosmosDbService.GetAllBookedAppointmentsAsync(cancellationToken: default);
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

    [Authorize(Policy = "AADUsers")]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] AppointmentWindowCreateRequestModel appointmentRequest)
    {
        try
        {
            var appointment = _mapper.Map<AppointmentWindow>(appointmentRequest);
            appointment.Id = Guid.NewGuid();
            var appointmentCreated = await _appointmentCosmosDbService.AddAsync(appointment, cancellationToken: default);

            return Ok(_mapper.Map<AppointmentWindowResponseModel>(appointmentCreated));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to create appointment.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("getNextAvailableAppointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<IActionResult> GetNextAvailableAppointment()
    {
        try
        {
            var appointmentStartTime = await _appointmentCosmosDbService.GetNextAvailableAppointment();

            return Ok(appointmentStartTime);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to create appointment.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("rescheduleAppointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> RescheduleAppointment([FromBody] AppointmentWindowUpdateRequestModel appointmentRequest)
    {
        try
        {
            GetUserId(out var userId);

            var existingAppointment = await _appointmentCosmosDbService.GetAppointmentByUserIdAsync(userId, cancellationToken: default);

            if (existingAppointment.IsManuallyCreated)
            {
                await _appointmentCosmosDbService.DeleteAsync(existingAppointment.Id.ToString(), cancellationToken: default);
            }
            else
            {
                existingAppointment.ApplicationId = null;
                existingAppointment.Status = AppointmentStatus.Available;
                existingAppointment.Name = null;
                existingAppointment.UserId = null;
                existingAppointment.Permit = null;
                existingAppointment.Payment = null;
                existingAppointment.IsManuallyCreated = false;
                existingAppointment.AppointmentCreatedDate = null;

                await _appointmentCosmosDbService.UpdateAsync(existingAppointment, cancellationToken: default);
            }

            if (appointmentRequest.Id == Guid.Empty.ToString())
            {
                var nextSlot = await _appointmentCosmosDbService.GetAvailableSlotByDateTime(appointmentRequest.Start, cancellationToken: default);

                if (nextSlot == null || nextSlot.Count < 1)
                {
                    throw new ArgumentOutOfRangeException("start");
                }

                var slot = nextSlot.First();
                appointmentRequest.Id = slot.Id.ToString();
                appointmentRequest.UserId = userId;
                appointmentRequest.End = slot.End;
                appointmentRequest.IsManuallyCreated = slot.IsManuallyCreated;
            }

            AppointmentWindow appointment = _mapper.Map<AppointmentWindow>(appointmentRequest);
            await _appointmentCosmosDbService.UpdateAsync(appointment, cancellationToken: default);

            return Ok(_mapper.Map<AppointmentWindowResponseModel>(appointment));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to reschedule appointment.");
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
                var nextSlot = await _appointmentCosmosDbService.GetAvailableSlotByDateTime(appointment.Start, cancellationToken: default);
                if (nextSlot == null || nextSlot.Count < 1)
                    throw new ArgumentOutOfRangeException("start");

                var slot = nextSlot.First();
                appointment.Id = slot.Id.ToString();
                appointment.UserId = userId;
                appointment.End = slot.End;
                appointment.IsManuallyCreated = slot.IsManuallyCreated;
            }

            AppointmentWindow appt = _mapper.Map<AppointmentWindow>(appointment);
            await _appointmentCosmosDbService.UpdateAsync(appt, cancellationToken: default);

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
            var existingAppointments = await _appointmentCosmosDbService.ResetApplicantAppointmentsAsync(appointment.ApplicationId, cancellationToken: default);

            if (appointment.Id == Guid.Empty.ToString())
            {
                var nextSlot = await _appointmentCosmosDbService.GetAvailableSlotByDateTime(appointment.Start, cancellationToken: default);
                if (nextSlot == null || nextSlot.Count < 1)
                    throw new ArgumentOutOfRangeException("start");

                var slot = nextSlot.First();
                appointment.Id = slot.Id.ToString();
                appointment.End = slot.End;
                appointment.IsManuallyCreated = slot.IsManuallyCreated;
            }

            AppointmentWindow appt = _mapper.Map<AppointmentWindow>(appointment);

            await _appointmentCosmosDbService.UpdateAsync(appt, cancellationToken: default);
            GetAADUserName(out string userName);

            var existingApplication = await _applicationCosmosDbService.GetUserApplicationAsync(appointment.ApplicationId, cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            existingApplication.Application.AppointmentDateTime = appointment.Start;
            existingApplication.Application.AppointmentStatus = AppointmentStatus.Scheduled;
            existingApplication.Application.AppointmentId = appointment.Id;

            await _applicationCosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

            return Ok();
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
            await _appointmentCosmosDbService.DeleteAsync(appointmentId, cancellationToken: default);
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
            var response = await _appointmentCosmosDbService.DeleteAllAppointmentsByDate(date, cancellationToken: default);
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
            var response = await _appointmentCosmosDbService.DeleteAppointmentsByTimeSlot(date, cancellationToken: default);
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
            var appointment = await _appointmentCosmosDbService.GetAppointmentByIdAsync(appointmentId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            var applicationId = appointment.ApplicationId;
            appointment.Status = AppointmentStatus.CheckedIn;

            await _appointmentCosmosDbService.UpdateAsync(appointment, cancellationToken: default);

            GetAADUserName(out string userName);

            var existingApplication = await _applicationCosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            existingApplication.Application.AppointmentStatus = AppointmentStatus.CheckedIn;

            await _applicationCosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

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
    [Route("noShowAppointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> NoShowAppointment(string appointmentId)
    {
        try
        {
            var appointment = await _appointmentCosmosDbService.GetAppointmentByIdAsync(appointmentId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            var applicationId = appointment.ApplicationId;
            appointment.Status = AppointmentStatus.NoShow;

            await _appointmentCosmosDbService.UpdateAsync(appointment, cancellationToken: default);

            GetAADUserName(out string userName);

            var existingApplication = await _applicationCosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            existingApplication.Application.Status = ApplicationStatus.AppointmentNoShow;
            existingApplication.Application.AppointmentStatus = AppointmentStatus.NoShow;

            await _applicationCosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

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
    [Route("setAppointmentScheduled")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> SetAppointmentScheduled(string appointmentId)
    {
        try
        {
            var appointment = await _appointmentCosmosDbService.GetAppointmentByIdAsync(appointmentId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            var applicationId = appointment.ApplicationId;
            appointment.Status = AppointmentStatus.Scheduled;

            await _appointmentCosmosDbService.UpdateAsync(appointment, cancellationToken: default);

            GetAADUserName(out string userName);

            var existingApplication = await _applicationCosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            existingApplication.Application.Status = ApplicationStatus.ReadyForAppointment;
            existingApplication.Application.AppointmentStatus = AppointmentStatus.Scheduled;

            await _applicationCosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

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
    [Route("reopenSlot")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpDelete]
    public async Task<IActionResult> ReopenSlot(string appointmentId)
    {
        try
        {
            var appointment = await _appointmentCosmosDbService.GetAppointmentByIdAsync(appointmentId, cancellationToken: default);
            var appointmentApplicationID = "";

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }
            else
            {
                appointmentApplicationID = appointment.ApplicationId;
            }

            if (appointment.IsManuallyCreated)
            {
                await _appointmentCosmosDbService.DeleteAsync(appointment.Id.ToString(), cancellationToken: default);
            }
            else
            {
                appointment.ApplicationId = null;
                appointment.UserId = null;
                appointment.Status = AppointmentStatus.Available;
                appointment.Name = null;
                appointment.Permit = null;
                appointment.Payment = null;
                appointment.IsManuallyCreated = false;
                appointment.AppointmentCreatedDate = null;

                await _appointmentCosmosDbService.UpdateAsync(appointment, cancellationToken: default);
            }

            GetAADUserName(out string userName);

            var existingApplication = await _applicationCosmosDbService.GetUserApplicationAsync(appointmentApplicationID, cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            existingApplication.Application.AppointmentDateTime = null;
            existingApplication.Application.AppointmentStatus = AppointmentStatus.NotScheduled;
            existingApplication.Application.AppointmentId = null;

            await _applicationCosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

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
    [Route("reopenSlotByApplicationId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> ReopenSlotByApplicationId(string applicationId)
    {
        try
        {
            var appointment = await _appointmentCosmosDbService.GetAsync(applicationId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            if (appointment.IsManuallyCreated)
            {
                await _appointmentCosmosDbService.DeleteAsync(appointment.Id.ToString(), cancellationToken: default);
            }
            else
            {
                appointment.ApplicationId = null;
                appointment.UserId = null;
                appointment.Status = AppointmentStatus.Available;
                appointment.Name = null;
                appointment.Permit = null;
                appointment.Payment = null;
                appointment.IsManuallyCreated = false;
                appointment.AppointmentCreatedDate = null;

                await _appointmentCosmosDbService.UpdateAsync(appointment, cancellationToken: default);
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
    public async Task<IActionResult> RemoveApplicationFromAppointment(string appointmentId)
    {
        try
        {
            GetUserId(out var userId);

            var appointment = await _appointmentCosmosDbService.GetAppointmentByIdAsync(appointmentId, cancellationToken: default);

            if (appointment.ApplicationId == null || appointment.UserId != userId)
            {
                return NotFound();
            }

            if (appointment.IsManuallyCreated)
            {
                await _appointmentCosmosDbService.DeleteAsync(appointment.Id.ToString(), cancellationToken: default);
            }
            else
            {
                appointment.ApplicationId = null;
                appointment.UserId = null;
                appointment.Status = AppointmentStatus.Available;
                appointment.Name = null;
                appointment.Permit = null;
                appointment.Payment = null;
                appointment.IsManuallyCreated = false;
                appointment.AppointmentCreatedDate = null;

                await _appointmentCosmosDbService.UpdateAsync(appointment, cancellationToken: default);
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
            var appointment = await _appointmentCosmosDbService.GetAsync(applicationId, cancellationToken: default);

            if (appointment.ApplicationId == null)
            {
                return NotFound();
            }

            await _appointmentCosmosDbService.DeleteAsync(appointment.Id.ToString(), cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to delete appointment.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("getHolidays")]
    [ProducesResponseType(typeof(HolidaysResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public IActionResult GetHolidays()
    {
        try
        {
            var response = new HolidaysResponseModel
            {
                Holidays = new()
            };
            var holidays = new USAPublicHoliday().PublicHolidayNames(2023);
            var realHolidays = new USAPublicHoliday().PublicHolidaysInformation(2023);
            var cesarChavezAdded = false;

            foreach (var holiday in realHolidays)
            {
                if (holiday.HolidayDate >= new DateTime(2023, 3, 31) && !cesarChavezAdded)
                {
                    response.Holidays.Add("Cesar Chavez Day, Mar 31");
                    cesarChavezAdded = true;
                }
                response.Holidays.Add(holiday.GetName() + ", " + holiday.HolidayDate.ToString("MMM") + " " + holiday.HolidayDate.Day.ToString());
            }

            return Ok(response);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to get holidays.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("postOrganizationHolidays")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<IActionResult> PostOrganizationHolidays(OrganizationHolidaysRequestModel requestModel)
    {
        try
        {
            var existingHolidays = await _appointmentCosmosDbService.GetOrganizationalHolidays();

            var organizationHolidays = new OrganizationHolidays()
            {
                Holidays = new()
            };

            if (existingHolidays != null)
            {
                organizationHolidays.Id = existingHolidays.Id;
            }
            else
            {
                organizationHolidays.Id = Guid.NewGuid();
            }

            foreach (var holidayRequest in requestModel.HolidayRequestModels)
            {
                if (holidayRequest.Name == "Cesar Chavez Day")
                {
                    var cesarChavez = new OrganizationalHoliday()
                    {
                        Name = "Cesar Chavez Day",
                        Month = 3,
                        Day = 31,
                    };
                    organizationHolidays.Holidays.Add(cesarChavez);
                }
                else
                {
                    var officialHolidays = new USAPublicHoliday().PublicHolidaysInformation(DateTime.Now.Year);
                    var holiday = officialHolidays.Where(h => h.GetName() == holidayRequest.Name).FirstOrDefault();
                    var organizationalHoliday = new OrganizationalHoliday()
                    {
                        Name = holiday.GetName(),
                        Month = holiday.HolidayDate.Month,
                        Day = holiday.HolidayDate.Day,
                    };
                    organizationHolidays.Holidays.Add(organizationalHoliday);
                }
            }

            await _appointmentCosmosDbService.AddOrganizationalHoliday(organizationHolidays, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to get holidays.");
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

    private void GetAADUserName(out string userName)
    {
        userName = HttpContext.User.Claims
            .Where(c => c.Type == "preferred_username").Select(c => c.Value)
            .FirstOrDefault();

        if (userName == null)
        {
            throw new ArgumentNullException("userName", "Invalid token.");
        }
    }
}