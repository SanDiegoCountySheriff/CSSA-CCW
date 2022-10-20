using CCW.Application.Extensions;
using CCW.Application.Models;
using CCW.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Application.Controllers;

[ApiController]
[Route(Constants.AppName + "/api/v1/[controller]")]
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
        var result = await _cosmosDbService.GetAsync(applicationId);

        return Ok(result.ToUiResponseModel());
    }

    [HttpGet("list")]
    public async Task<IActionResult> List(int startIndex, int count, string? filter = null)
    {
        var result = await _cosmosDbService.ListAsync(startIndex, count);

        if (result != null)
        {
            List<PermitApplicationResponseModel> permitApplications = new List<PermitApplicationResponseModel>(result.Count());
            foreach (var item in result)
            {
                permitApplications.Add(item.ToUiResponseModel());
            }

            return new OkObjectResult(permitApplications);
        }

        return new OkObjectResult(new List<PermitApplicationResponseModel>(0));
    }

    [Route("create")]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] PermitApplicationRequestModel application)
    {
        var result = await _cosmosDbService.AddAsync(application.ToNewDbModel());
        return Ok(result);
    }

    [Route("update")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PermitApplicationRequestModel application)
    {
        var existingApplication = await _cosmosDbService.GetAsync(application.id.ToString());

        await _cosmosDbService.UpdateAsync(application.ToExistingDbModel(existingApplication));
        return NoContent();
    }


    [Route("delete")]
    [HttpDelete]
    public async Task<IActionResult> Delete(string applicationId, string userId) // TODO: userId should be taken from the security context, NOT the UI...
    {
        await _cosmosDbService.DeleteAsync(applicationId, userId);
        return NoContent();
    }
}
