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
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(
        IApplicationServiceClient applicationHttpClient,
        ICosmosDbService cosmosDbService,
        IMapper<AppointmentWindowCreateRequestModel, AppointmentWindow> requestCreateApptMapper,
        IMapper<AppointmentWindowUpdateRequestModel, AppointmentWindow> requestUpdateApptMapper,
        IMapper<AppointmentWindow, AppointmentWindowResponseModel> responseMapper,
        ILogger<AppointmentController> logger)
    {
        _applicationHttpClient = applicationHttpClient;
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _requestCreateApptMapper = requestCreateApptMapper;
        _requestUpdateApptMapper = requestUpdateApptMapper;
        _responseMapper = responseMapper;
        _logger = logger;
    }


    [Authorize(Policy = "AADUsers")]
    [HttpPost("uploadFile", Name = "uploadFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFile(IFormFile fileToPersist)
    {
        try
        {
            using (var reader = new StreamReader(fileToPersist.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<AppointmentUploadModelMap>();
                var records = csv.GetRecords<AppointmentUploadModel>();

                //one line at a time
                foreach (var record in records)
                {
                    List<AppointmentWindow> appointmentsToAdd = new List<AppointmentWindow>();
                    List<AppointmentWindow> appointmentsToDelete = new List<AppointmentWindow>();

                    DateTime dateTimeNow = DateTime.Now;
                    DateTime startDateAndTime = Convert.ToDateTime(record.Date).Add(TimeSpan.Parse(record.Time));
                    DateTime endDateAndTime = Convert.ToDateTime(record.Date).Add(TimeSpan.Parse(record.Time))
                        .AddMinutes(record.Duration);

                    if (dateTimeNow > startDateAndTime)
                    {
                        continue;
                    }

                    if (record.Action.ToLower().Contains("create"))
                    {
                        for (int i = 0; i < record.Slots; i++)
                        {
                            AppointmentWindow appt = new AppointmentWindow
                            {
                                Id = Guid.NewGuid(),
                                ApplicationId = null,
                                End = endDateAndTime,
                                Start = startDateAndTime,
                                Status = null,
                                Name = null,
                                Permit = null,
                                Payment = null,
                                IsManuallyCreated = false,
                            };

                            appointmentsToAdd.Add(appt);
                        }
                    }

                    if (record.Action.ToLower().Contains("delete"))
                    {
                        for (int i = 0; i < record.Slots; i++)
                        {
                            AppointmentWindow appt = new AppointmentWindow
                            {
                                Id = Guid.NewGuid(),
                                ApplicationId = null,
                                End = endDateAndTime,
                                Start = startDateAndTime,
                                Status = null,
                                Name = null,
                                Permit = null,
                                Payment = null,
                                IsManuallyCreated = false,
                            };

                            appointmentsToDelete.Add(appt);
                        }
                    }

                    if (appointmentsToAdd.Count > 0)
                    {
                        await _cosmosDbService.AddDeleteTimeSlotsAsync(appointmentsToAdd, isCreateAction: true,
                            cancellationToken: default);
                    }

                    if (appointmentsToDelete.Count > 0)
                    {
                        await _cosmosDbService.AddDeleteTimeSlotsAsync(appointmentsToDelete, isCreateAction: false,
                            cancellationToken: default);
                    }
                }
            }
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to upload file.");
        }

        return Ok();
    }


    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [HttpGet("getAvailability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAppointmentTimes()
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


    [Authorize(Policy = "B2CUsers")]
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

            AppointmentWindow appt = _requestUpdateApptMapper.Map(appointment);
            await _cosmosDbService.UpdateAsync(appt, cancellationToken: default);

        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to update appointment.");
        }

        return Ok();
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
            AppointmentWindow appt = _requestUpdateApptMapper.Map(appointment);
            await _cosmosDbService.UpdateAsync(appt, cancellationToken: default);

            var response = await _applicationHttpClient.UpdateApplicationAppointmentAsync(appointment.ApplicationId,
                appointment.Start, cancellationToken: default);

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

            var applicationId = appointment.ApplicationId;

            appointment.ApplicationId = null;
            appointment.Status = null;
            appointment.Name = null;
            appointment.Permit = null;
            appointment.Payment = null;
            appointment.IsManuallyCreated = true;

            await _cosmosDbService.UpdateAsync(appointment, cancellationToken: default);

            var response = await _applicationHttpClient.RemoveApplicationAppointmentAsync(applicationId,
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