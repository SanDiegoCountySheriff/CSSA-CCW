
//using Azure.Security.KeyVault.Secrets;
//using Microsoft.Azure.Cosmos;

//namespace CCW.UserProfile;

//public static class Test
//{
//    public static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfigurationSection configSection, SecretClient secretClient)
//    {
//        var databaseName = configSection["DatabaseName"];
//        var containerName = configSection["ContainerName"];
//        async Task<CosmosClient> CosmosClientFactory()
//        {
//            var connectionString = await secretClient.GetSecretAsync("cosmos-db-connection-primary");
//            return new CosmosClient(connectionString.Value.Value);
//        }
//        services.AddSingleton(CosmosClientFactory);
//        services.AddSingleton<ICosmosContainerFactory, CosmosContainerFactory>();
//        services.AddSingleton<ICosmosDbService, CosmosDbService>(sp =>
//            new CosmosDbService(sp.GetRequiredService<ICosmosContainerFactory>(), databaseName, containerName,
//                sp.GetRequiredService<ILogger<CosmosDbService>>()));
//        return services;
//    }
//}
//internal interface ICosmosDbService
//{
//    Task<string> GetAThingAsync();
//}
//internal class CosmosDbService : ICosmosDbService
//{
//    private readonly ICosmosContainerFactory _containerFactory;
//    private readonly string _databaseName;
//    private readonly string _containerName;
//    private readonly ILogger<CosmosDbService> _logger;
//    public CosmosDbService(ICosmosContainerFactory containerFactory, string databaseName, string containerName, ILogger<CosmosDbService> logger)
//    {
//        _containerFactory = containerFactory;
//        _databaseName = databaseName;
//        _containerName = containerName;
//        _logger = logger;
//    }
//    private async Task<Container> GetContainerAsync()
//    {
//        return await _containerFactory.GetAsync(_databaseName, _containerName);
//    }
//    public async Task<string> GetAThingAsync()
//    {
//        var container = await GetContainerAsync();
//        // do the stuff
//        return "yo";
//    }
//}
//public interface ICosmosContainerFactory
//{
//    Task<Container> GetAsync(string databaseId, string containerId);
//}
//public class CosmosContainerFactory : ICosmosContainerFactory
//{
//    private Lazy<Task<CosmosClient>> ClientInitializer { get; }
//    public CosmosContainerFactory(Func<Task<CosmosClient>> cosmosClientFactory)
//    {
//        ClientInitializer = new(cosmosClientFactory, LazyThreadSafetyMode.ExecutionAndPublication);
//    }
//    public async Task<Container> GetAsync(string databaseId, string containerId)
//    {
//        return (await ClientInitializer.Value).GetContainer(databaseId, containerId);
//    }
//}