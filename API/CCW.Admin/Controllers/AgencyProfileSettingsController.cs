using CCW.Admin.Entities;
using CCW.Admin.Mappers;
using CCW.Admin.Models;
using CCW.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Admin.Controllers;


[Route("/Api/" + Constants.AppName + "/v1/[controller]")]
[ApiController]
public class SystemSettingsController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper<AgencyProfileSettingsRequestModel, AgencyProfileSettings> _requestMapper;
    private readonly IMapper<AgencyProfileSettings, AgencyProfileSettingsResponseModel> _responseMapper;
    private readonly ILogger<SystemSettingsController> _logger;

    public SystemSettingsController(
        ICosmosDbService cosmosDbService,
        IMapper<AgencyProfileSettingsRequestModel, AgencyProfileSettings> requestMapper,
        IMapper<AgencyProfileSettings, AgencyProfileSettingsResponseModel> responseMapper,
        ILogger<SystemSettingsController> logger
        )
    {
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _logger = logger;
    }

    [HttpGet("get")]
    public async Task<AgencyProfileSettingsResponseModel> Get()
    {
        try
        {
            var response = await _cosmosDbService.GetSettingsAsync(cancellationToken: default);

            return _responseMapper.Map(response);

        }
        catch (Exception e)
        {
            _logger.LogWarning($"An error occur while trying to retrieve agency settings: {e.Message}");
            throw new Exception("An error occur while trying to retrieve agency settings.");
        }
    }

    [Route("update")]
    [HttpPut]
    public async Task<AgencyProfileSettingsResponseModel> Update([FromBody] AgencyProfileSettingsRequestModel agencyProfileRequest)
    {
        try
        {
            AgencyProfileSettings agencyProfileSettings = _requestMapper.Map(agencyProfileRequest);

            var newAgencyProfile = await _cosmosDbService.UpdateSettingsAsync(agencyProfileSettings, cancellationToken: default);

            return _responseMapper.Map(newAgencyProfile);

        }
        catch (Exception e)
        {
            _logger.LogWarning($"An error occur while trying to retrieve agency settings: {e.Message}");
            throw new Exception("An error occur while trying to retrieve agency settings.");
        }
    }
}