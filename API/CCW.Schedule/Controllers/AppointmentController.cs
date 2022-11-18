using CCW.Schedule.Models;
using CCW.Schedule.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CCW.Schedule.Entities;
using CCW.Schedule.Mappers;


namespace CCW.Schedule.Controllers;

[Route("/Api/" + Constants.AppName + "/v1/[controller]")]
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

    [HttpPost("uploadFile", Name = "uploadFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFile(IFormFile fileToPersist)
    {
        try
        {
            List<AppointmentWindow> appointments = new List<AppointmentWindow>();

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ";",
                Comment = '%'
            };

            using (var reader = new StreamReader(fileToPersist.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<AppointmentUploadModelMap>();
                var records = csv.GetRecords<AppointmentUploadModel>();

                foreach (var record in records)
                {
                    AppointmentWindow appt = new AppointmentWindow
                    {
                        Id = Guid.NewGuid(),
                        ApplicationId = null,
                        End = record.End,
                        Start = record.Start,
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
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("An error occur. Please ty again.");
        }

        return Ok();
    }

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
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("An error occur. Please ty again.");
        }
    }

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
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("An error occur. Please ty again.");
        }
    }

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
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("An error occur. Please ty again.");
        }
    }

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
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("An error occur. Please ty again.");
        }
    }

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
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("An error occur. Please ty again.");
        }

        return Ok();
    }

    [Route("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpDelete]
    public async Task<IActionResult> Delete(string appointmentId)
    {
        try
        {
            await _cosmosDbService.DeleteAsync(appointmentId, cancellationToken: default);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("An error occur. Please ty again.");
        }

        return Ok();
    }
}