using CCW.Application.Models;
using CCW.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Application.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class PermitApplicationController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;

    private readonly ILogger<PermitApplicationController> _logger;

    public PermitApplicationController(ICosmosDbService cosmosDbService, ILogger<PermitApplicationController> logger)
    {
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _logger = logger;
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get(string applicationId)
    {
        return Ok(await _cosmosDbService.GetAsync(applicationId));
    }

    [Route("create")]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] PermitApplication application)
    {
        return Ok(await _cosmosDbService.AddAsync(application));
    }

    [Route("update")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PermitApplication application)
    {
        await _cosmosDbService.UpdateAsync(application);
        return NoContent();
    }


    [Route("delete")]
    [HttpDelete]
    public async Task<IActionResult> Delete(string applicationId, string userId)
    {
        await _cosmosDbService.DeleteAsync(applicationId, userId);
        return NoContent();
    }
}
