using CCW.Schedule.Models;
using CCW.Schedule.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using CsvHelper;
using CCW.Schedule.Entities;
using CCW.Schedule.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace CCW.Schedule.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper<AppointmentWindowCreateRequestModel, AppointmentWindow> _requestCreateApptMapper;
    private readonly IMapper<AppointmentWindowUpdateRequestModel, AppointmentWindow> _requestUpdateApptMapper;
    private readonly IMapper<AppointmentWindow, AppointmentWindowResponseModel> _responseMapper;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(
        ICosmosDbService cosmosDbService,
        IMapper<AppointmentWindowCreateRequestModel, AppointmentWindow> requestCreateApptMapper,
        IMapper<AppointmentWindowUpdateRequestModel, AppointmentWindow> requestUpdateApptMapper,
        IMapper<AppointmentWindow, AppointmentWindowResponseModel> responseMapper,
        ILogger<AppointmentController> logger)
    {
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _requestCreateApptMapper = requestCreateApptMapper;
        _requestUpdateApptMapper = requestUpdateApptMapper;
        _responseMapper = responseMapper;
        _logger = logger;
    }

    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [HttpPost("uploadFile", Name = "uploadFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFile(IFormFile fileToPersist)
    {
        try
        {
            List<AppointmentWindow> appointments = new List<AppointmentWindow>();

            using (var reader = new StreamReader(fileToPersist.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<AppointmentUploadModelMap>();
                var records = csv.GetRecords<AppointmentUploadModel>();

                foreach (var record in records)
                {
                    DateTime dateTimeNow = DateTime.Now;
                    DateTime startDateAndTime = Convert.ToDateTime(record.StartDate).Add(TimeSpan.Parse(record.StartTime));

                    if (dateTimeNow > startDateAndTime)
                    {
                        continue;
                    }

                    AppointmentWindow appt = new AppointmentWindow
                    {
                        Id = Guid.NewGuid(),
                        ApplicationId = null,
                        End = Convert.ToDateTime(record.EndDate).Add(TimeSpan.Parse(record.EndTime)),
                        Start = Convert.ToDateTime(record.StartDate).Add(TimeSpan.Parse(record.StartTime)),
                        Status = null,
                        Name = null,
                        Permit = null,
                        Payment = null,
                        IsManuallyCreated = false,
                    };
                    appointments.Add(appt);
                }

                await _cosmosDbService.AddAvailableTimesAsync(appointments, cancellationToken: default);
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

    [Authorize(Policy = "B2CUsers")]
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
    [Authorize(Policy = "AADUsers")]
    [Route("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AppointmentWindowUpdateRequestModel appointment)
    {
        try
        {
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

    [Authorize(Policy = "RequireAdminOnly")]
    [Authorize(Policy = "RequireSystemAdminOnly")]
    [Authorize(Policy = "RequireProcessorOnly")]
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
}