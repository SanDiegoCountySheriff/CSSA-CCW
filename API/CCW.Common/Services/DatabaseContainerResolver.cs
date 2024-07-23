using Microsoft.Azure.Cosmos;

namespace CCW.Common.Services;

public class DatabaseContainerResolver : IDatabaseContainerResolver
{
    private readonly Dictionary<string, Database> _databases;

    public DatabaseContainerResolver(Dictionary<string, Database> databases)
    {
        _databases = databases;
    }

    public Container GetContainer(string tenantId, string containerName)
    {
        return _databases[tenantId].GetContainer(containerName);
    }
}
