using CCW.Schedule.Models;
using CCW.Schedule.Services;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Schedule.Controllers;

[Route(Constants.AppName + "/Api/v1/[controller]")]
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

    [HttpGet("getAvailability")]
    public async Task<IActionResult> GetAppointmentTimes()
    {
        var result = await _cosmosDbService.GetAvailableTimesAsync();
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