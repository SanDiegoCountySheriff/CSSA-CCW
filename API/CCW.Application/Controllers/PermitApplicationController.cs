using CCW.Application.Entities;
using CCW.Application.Mappers;
using CCW.Application.Models;
using CCW.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CCW.Application.Controllers;


[Route(Constants.AppName + "/v1/[controller]")]
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

    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [Route("create")]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] PermitApplicationRequestModel permitApplicationRequest)
    {
        try
        {
            var result = await _cosmosDbService.AddAsync(_permitApplicationMapper.Map(true, permitApplicationRequest), cancellationToken: default);
            
            return Ok(_permitApplicationResponseMapper.Map(result));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to create permit application.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [HttpGet("get")]
    public async Task<IActionResult> Get(string userEmailOrOrderId, bool isOrderId = false, bool isComplete = false)
    {
        try
        {
            var result = await _cosmosDbService.GetLastApplicationAsync(userEmailOrOrderId, isOrderId, isComplete, cancellationToken: default);
            
            return (result != null) ? Ok(_permitApplicationResponseMapper.Map(result)) : NotFound();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve permit application.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [HttpGet("getUserEmail")]
    public async Task<IActionResult> Get(string userEmail)
    {
        try
        {
            IEnumerable<PermitApplicationResponseModel> responseModels = new List<PermitApplicationResponseModel>();
            var result = await _cosmosDbService.GetAllUserApplicationsAsync(userEmail, cancellationToken: default);

            if(result.Any())
            {
                responseModels = result.Select(x => _permitApplicationResponseMapper.Map(x));
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve all user permit applications.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getHistory")]
    public async Task<IActionResult> GetHistory(string applicationIdOrOrderId, bool isOrderId = false)
    {
        try
        {
            IEnumerable<HistoryResponseModel> responseModels = new List<HistoryResponseModel>();
            var result = await _cosmosDbService.GetApplicationHistoryAsync(applicationIdOrOrderId, cancellationToken: default, isOrderId);

            if (result.Any())
            {
                responseModels = result.Select(x => _historyMapper.Map(x));
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve permit application history.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            IEnumerable<SummarizedPermitApplicationResponseModel> responseModels = new List<SummarizedPermitApplicationResponseModel>();
            var result = await _cosmosDbService.GetAllApplicationsAsync(cancellationToken: default);
            if (result.Any())
            {
                responseModels = result.Select(x => _summaryPermitApplicationResponseMapper.Map(x));
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve all permit applications.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [HttpGet("search")]
    public async Task<IActionResult> Search(string searchValue)
    {
        //TODO add cosmos query this will return error right now
        try
        {
            IEnumerable<PermitApplicationResponseModel> responseModels = new List<PermitApplicationResponseModel>();
            var result = await _cosmosDbService.SearchApplicationsAsync(searchValue, cancellationToken: default);

            if (result.Any())
            {
                responseModels = result.Select(x => _permitApplicationResponseMapper.Map(x));
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to search all permit applications.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [Route("update")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PermitApplicationRequestModel application)
    {
        try
        {
            await _cosmosDbService.UpdateAsync(_permitApplicationMapper.Map(false, application), cancellationToken: default);
            return NoContent();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to update permit application.");
        }
    }
}