using CCW.Application.Entities;
using CCW.Application.Mappers;
using CCW.Application.Models;
using CCW.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Application.Controllers;

[ApiController]
[Route(Constants.AppName + "/Api/v1/[controller]")]
public class PermitApplicationController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;

    private readonly IMapper<PermitApplication, PermitApplicationResponseModel> _permitApplicationResponseMapper;
    private readonly IMapper<bool, PermitApplicationRequestModel, PermitApplication> _permitApplicationMapper;

    private readonly ILogger<PermitApplicationController> _logger;

    public PermitApplicationController(
        ICosmosDbService cosmosDbService,
        IMapper<PermitApplication, PermitApplicationResponseModel> permitApplicationResponseMapper,
        IMapper<bool, PermitApplicationRequestModel, PermitApplication> permitApplicationMapper,
        ILogger<PermitApplicationController> logger
        )
    {
        _permitApplicationResponseMapper = permitApplicationResponseMapper;
        _permitApplicationMapper = permitApplicationMapper;
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _logger = logger;
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get(string userEmail, bool isComplete = false)
    {
        var result = await _cosmosDbService.GetAsync(userEmail, isComplete);

        return Ok(_permitApplicationResponseMapper.Map(result));
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
                permitApplications.Add(_permitApplicationResponseMapper.Map(item));
            }

            return new OkObjectResult(permitApplications);
        }

        return new OkObjectResult(new List<PermitApplicationResponseModel>(0));
    }

    [Route("create")]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] PermitApplicationRequestModel application)
    {
        var result = await _cosmosDbService.AddAsync(_permitApplicationMapper.Map(true, application));
        return Ok(result);
    }

    [Route("update")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PermitApplicationRequestModel application)
    {
        var existingApplication = await _cosmosDbService.GetAsync(application.Id.ToString(), false);

        await _cosmosDbService.UpdateAsync(_permitApplicationMapper.Map(false, application));
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
