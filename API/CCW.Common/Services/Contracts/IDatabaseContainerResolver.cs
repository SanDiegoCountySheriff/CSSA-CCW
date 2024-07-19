using Microsoft.Azure.Cosmos;

namespace CCW.Common.Services;

public interface IDatabaseContainerResolver
{
    Container GetContainer(string tenantId,  string containerName);
}
