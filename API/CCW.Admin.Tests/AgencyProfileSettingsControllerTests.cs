using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCW.Admin.Mappers;
using CCW.Admin.Services;
using CCW.Admin.Entities;
using CCW.Admin.Models;
using CCW.Admin.Controllers;

namespace CCW.Admin.Tests;

internal class AgencyProfileSettingsControllerTests
{
    protected Mock<ICosmosDbService> _cosmosDbService { get; }
    protected Mock<IMapper<AgencyProfileSettingsRequestModel, AgencyProfileSettings>> _requestMapper { get; }
    protected Mock<IMapper<AgencyProfileSettings, AgencyProfileSettingsResponseModel>> _responseMapper { get; }
    protected Mock<ILogger<SystemSettingsController>> _logger { get; }

    public AgencyProfileSettingsControllerTests()
    {
        _cosmosDbService = new Mock<ICosmosDbService>();
        _requestMapper = new Mock<IMapper<AgencyProfileSettingsRequestModel, AgencyProfileSettings>>();
        _responseMapper = new Mock<IMapper<AgencyProfileSettings, AgencyProfileSettingsResponseModel>>();
        _logger = new Mock<ILogger<SystemSettingsController>>();
    }
}