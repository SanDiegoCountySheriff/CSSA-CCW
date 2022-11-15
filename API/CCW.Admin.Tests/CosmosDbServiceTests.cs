using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCW.Admin.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Moq;

namespace CCW.Admin.Tests;

internal class CosmosDbServiceTests
{
    protected string _databaseNameMock { get; }
    protected string _containerNameMock { get; }
    protected Mock<CosmosClient> _cosmosClientMock { get; }
    protected Mock<ILogger<CosmosDbService>> _loggerMock { get; }


    public CosmosDbServiceTests()
    {
        _databaseNameMock = "admin-database";
        _containerNameMock = "admin";
        _cosmosClientMock = new Mock<CosmosClient>();
        _loggerMock = new Mock<ILogger<CosmosDbService>>();
    }
}