using CCW.Application.Entities;
using CCW.Application.Mappers;
using CCW.Application.Models;
using CCW.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Application.Controllers;


[Route("/Api/" + Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PermitApplicationController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel> _summaryPermitApplicationResponseMapper;
    private readonly IMapper<PermitApplication, PermitApplicationResponseModel> _permitApplicationResponseMapper;
    private readonly IMapper<bool, PermitApplicationRequestModel, PermitApplication> _permitApplicationMapper;
    private readonly IMapper<History, HistoryResponseModel> _historyMapper;
    private readonly ILogger<PermitApplicationController> _logger;

    public PermitApplicationController(
        ICosmosDbService cosmosDbService,
        IMapper<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel> summaryPermitApplicationResponseMapper,
        IMapper<PermitApplication, PermitApplicationResponseModel> permitApplicationResponseMapper,
        IMapper<bool, PermitApplicationRequestModel, PermitApplication> permitApplicationMapper,
        IMapper<History, HistoryResponseModel> historyMapper,
        ILogger<PermitApplicationController> logger
        )
    {
        _summaryPermitApplicationResponseMapper = summaryPermitApplicationResponseMapper;
        _permitApplicationResponseMapper = permitApplicationResponseMapper;
        _permitApplicationMapper = permitApplicationMapper;
        _historyMapper = historyMapper;
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _logger = logger;
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get(string userEmailOrOrderId, bool isOrderId = false, bool isComplete = false)
    {
        var result = await _cosmosDbService.GetAsync(userEmailOrOrderId, isOrderId, isComplete, cancellationToken:default);

        return Ok(_permitApplicationResponseMapper.Map(result));
    }

    [HttpGet("getHistory")]
    public async Task<IActionResult> GetHistory(string applicationIdOrOrderId, bool isOrderId = false)
    {
        var result = await _cosmosDbService.GetApplicationHistoryAsync(applicationIdOrOrderId, cancellationToken:default, isOrderId);

        IEnumerable<HistoryResponseModel> responseModels = result.Select(x => _historyMapper.Map(x));
        return Ok(responseModels);
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _cosmosDbService.GetAllApplicationsAsync(cancellationToken:default);

        return Ok(result.Select(x=> _summaryPermitApplicationResponseMapper.Map(x)));
    }

    [HttpGet("list")]
    public async Task<IActionResult> List(int startIndex, int count, string? filter = null)
    {
        var result = await _cosmosDbService.ListAsync(startIndex, count, cancellationToken:default);

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
    public async Task<IActionResult> Create([FromBody] PermitApplicationRequestModel permitApplicationRequest)
    {
        var result = await _cosmosDbService.AddAsync(_permitApplicationMapper.Map(true, permitApplicationRequest), cancellationToken: default);
        return Ok(result);
    }

    [Route("update")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PermitApplicationRequestModel application)
    {
        await _cosmosDbService.UpdateAsync(_permitApplicationMapper.Map(false, application), cancellationToken:default);
        return NoContent();
    }


    [Route("delete")]
    [HttpDelete]
    public async Task<IActionResult> Delete(string applicationId, string userId) // TODO: userId should be taken from the security context, NOT the UI...
    {
        await _cosmosDbService.DeleteAsync(applicationId, userId, cancellationToken:default);
        return NoContent();
    }

}
