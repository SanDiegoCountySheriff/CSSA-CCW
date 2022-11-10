using CCW.Schedule.Models;
using CCW.Schedule.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CCW.Schedule.Entities;
using CCW.Schedule.Mappers;
using System;
using Newtonsoft.Json.Linq;

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
    public async Task<IActionResult> UploadFile(
        IFormFile fileToPersist,
        CancellationToken cancellationToken)
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

                await _cosmosDbService.AddAvailableTimesAsync(appointments);
            }

            return Ok();
        }
        catch
        {
            // log error
        }

        return Ok();
    }

    [HttpGet("getAvailability")]
    public async Task<IActionResult> GetAppointmentTimes()
    {
        var result = await _cosmosDbService.GetAvailableTimesAsync();
        var appointments = result.Select(x => _responseMapper.Map(x));

        return Ok(appointments);
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _cosmosDbService.GetAllBookedAppointmentsAsync();
        var appointments = result.Select(x => _responseMapper.Map(x));

        return Ok(appointments);
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get(string applicationId)
    {
        var appointment = await _cosmosDbService.GetAsync(applicationId);
        return Ok(_responseMapper.Map(appointment));
    }

    [Route("create")]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] AppointmentWindowCreateRequestModel appointment)
    {
        AppointmentWindow appt = _requestCreateApptMapper.Map(appointment);
        var appointmentCreated = await _cosmosDbService.AddAsync(appt);

        return Ok(_responseMapper.Map(appointmentCreated));
    }

    [Route("update")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AppointmentWindowUpdateRequestModel appointment)
    {
        AppointmentWindow appt = _requestUpdateApptMapper.Map(appointment);
        await _cosmosDbService.UpdateAsync(appt);

        return Ok();
    }

    [Route("delete")]
    [HttpDelete]
    public async Task<IActionResult> Delete(string appointmentId)
    {
        await _cosmosDbService.DeleteAsync(appointmentId);

        return Ok();
    }
}