using CCW.Admin.Models;
using CCW.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Linq;

namespace CCW.Admin.Controllers;

[Route("api/[controller]")]
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
    public async Task<AgencyProfileSettings> Get(string applicationId)
    {
        try
        {
            var response = _cosmosDbService.GetSettingsAsync(applicationId, cancellationToken: default);

            return response.Result;

        }
        catch (Exception e)
        {
            _logger.LogWarning($"An error occur while trying to retrieve agency settings: {e.Message}");

            throw new Exception("An error occur while trying to retrieve agency settings.");
        }
    }

    [Route("create")]
    [HttpPut]
    public async Task<AgencyProfileSettings> Create([FromBody] AgencyProfileRequestModel agencyProfile)
    {
        try
        {
            AgencyProfileSettings agencyProfileSettings = new AgencyProfileSettings
            {
                Id = Guid.NewGuid().ToString(),
                AgencySheriffName = agencyProfile.AgencySheriffName,
                AgencyName = agencyProfile.AgencyName,
                ChiefOfPoliceName = agencyProfile.ChiefOfPoliceName,
                PrimaryThemeColor = agencyProfile.PrimaryThemeColor,
                SecondaryThemeColor = agencyProfile.SecondaryThemeColor,
                AgencyLogo = agencyProfile.AgencyLogo,
                AgencyLogoDataURL = agencyProfile.AgencyLogoDataURL,
            };

            var newAgencyProfile = await _cosmosDbService.AddAsync(agencyProfileSettings, cancellationToken: default);

            return newAgencyProfile;

        }
        catch (Exception e)
        {
            _logger.LogWarning($"An error occur while trying to retrieve agency settings: {e.Message}");

            throw new Exception("An error occur while trying to retrieve agency settings.");
        }
    }
}
