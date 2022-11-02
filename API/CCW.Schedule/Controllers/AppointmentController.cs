using CCW.Schedule.Models;
using CCW.Schedule.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace CCW.Schedule.Controllers;

[Route("/Api/" + Constants.AppName + "/v1/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;

    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(ICosmosDbService cosmosDbService, ILogger<AppointmentController> logger)
    {
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
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
                        ApplicantId = null,
                        End = record.End,
                        Start = record.Start,
                        Status = null,
                        Name = null,
                        Permit = null,
                        Payment = null,
                        IsManuallyCreated = false,
                        Id = Guid.NewGuid().ToString(),
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
        return Ok(result);
    }


    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _cosmosDbService.GetAllBookedAppointmentsAsync();
        return Ok(result);
    }


    [HttpGet("get")]
    public async Task<IActionResult> Get(string applicationId)
    {
        return Ok(await _cosmosDbService.GetAsync(applicationId));
    }

    [Route("create")]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] AppointmentWindow appointment)
    {
        return Ok(await _cosmosDbService.AddAsync(appointment));
    }

    [Route("update")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AppointmentWindow appointment)
    {
        await _cosmosDbService.UpdateAsync(appointment);
        return NoContent();
    }

    [Route("delete")]
    [HttpDelete]
    public async Task<IActionResult> Delete(string appointmentId, string userId)
    {
        await _cosmosDbService.DeleteAsync(appointmentId, userId);
        return NoContent();
    }
}