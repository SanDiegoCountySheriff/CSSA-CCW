using CCW.Admin.Models;
using CCW.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Linq;

namespace CCW.Admin.Controllers;

[Route(Constants.AppName + "api/[controller]")]
[ApiController]
public class SystemSettingsController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;

    private readonly ILogger<SystemSettingsController> _logger;

    public SystemSettingsController(ICosmosDbService cosmosDbService, ILogger<SystemSettingsController> logger)
    {
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _logger = logger;
    }

    [HttpGet("get")]
    public async Task<AgencyProfileSettings> Get()
    {
        try
        {
            var response = _cosmosDbService.GetSettingsAsync(cancellationToken: default);

            return response.Result;

        }
        catch (Exception e)
        {
            _logger.LogWarning($"An error occur while trying to retrieve agency settings: {e.Message}");

            throw new Exception("An error occur while trying to retrieve agency settings.");
        }
    }

    [Route("update")]
    [HttpPut]
    public async Task<AgencyProfileSettings> Update([FromBody] AgencyProfileRequestModel agencyProfileRequest)
    {
        try
        {
            AgencyProfileSettings agencyProfileSettings = CreateNewAgencyProfileSettings(agencyProfileRequest);

            var newAgencyProfile = await _cosmosDbService.UpdateSettingsAsync(agencyProfileSettings, cancellationToken: default);

            return newAgencyProfile;

        }
        catch (Exception e)
        {
            _logger.LogWarning($"An error occur while trying to retrieve agency settings: {e.Message}");

            throw new Exception("An error occur while trying to retrieve agency settings.");
        }
    }

    private AgencyProfileSettings CreateNewAgencyProfileSettings(AgencyProfileRequestModel agencyProfileRequest)
    {
        AgencyProfileSettings agencyProfileSettings = new AgencyProfileSettings
        {
            Id = Guid.NewGuid().ToString(),
            AgencySheriffName = agencyProfileRequest.AgencySheriffName,
            AgencyName = agencyProfileRequest.AgencyName,
            ChiefOfPoliceName = agencyProfileRequest.ChiefOfPoliceName,
            PrimaryThemeColor = agencyProfileRequest.PrimaryThemeColor,
            SecondaryThemeColor = agencyProfileRequest.SecondaryThemeColor,
            AgencyLogo = agencyProfileRequest.AgencyLogo,
            AgencyLogoDataURL = agencyProfileRequest.AgencyLogoDataURL,
        };

        return agencyProfileSettings;
    }
}
