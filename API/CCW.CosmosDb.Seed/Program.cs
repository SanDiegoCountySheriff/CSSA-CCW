using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos.Scripts;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Reflection;
using CCW.CosmosDb.Seed.Model;

await MainMenu();

static async Task<bool> MainMenu()
{
    var builder = new ConfigurationBuilder()
                 .AddJsonFile($"appsettings.json", true, true);

    var config = builder.Build();
    string connectionString = config["ConnectionStrings:cosmosDb"];

    var dir = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
    string filePath = string.Format("{0}\\{1}", dir.Parent, "Data.json");

    Console.WriteLine("File Path: {0}", filePath);

    var jsonData = await IngestJsonFile(filePath);

    var roots = JsonConvert.DeserializeObject<List<Root>>(jsonData);

    foreach (var root in roots)
    {
        string databaseId = root.database;
        string containerId = root.container;
        var items = root.Items;
        string partitionKey = root.PartitionKey;
        string recordType = root.RecordType;

        Console.WriteLine("Root details to process:");
        Console.WriteLine("\tDatabase: {0}", databaseId);
        Console.WriteLine("\tContainer: {0}", containerId);
        Console.WriteLine("\tRecordType: {0}", recordType);
        Console.WriteLine();
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1) Seed Fresh Data From File");
        Console.WriteLine("2) Delete Database and Container");
        Console.WriteLine("3) Get Record Count");
        Console.WriteLine("4) Exit");
        Console.WriteLine();
        Console.Write("\r\nSelect an option: ");
        switch (Console.ReadLine())
        {
            case "1":
                {
                    await InitializeCosmosDbAsync(connectionString, databaseId, containerId, items, partitionKey, recordType);
                    var result = await SubMenu();

                    if (!result)
                        return false;
                    break;
                }
            case "2":
                {
                    await WipeCosmosDb(connectionString, databaseId, containerId);
                    var result = await SubMenu();

                    if (!result)
                        return false;
                    break;
                }
            case "3":
                {
                    await GetRecordCount(connectionString, databaseId, containerId);
                    var result = await SubMenu();

                    if (!result)
                        return false;
                    break;
                }
            case "4":
                break;
            default:
                break;
        }
    }
    return false;
}

static async Task<bool> SubMenu()
{
    Console.WriteLine("Ready to process next database and container? (y/n)");
    Console.Write("\r\nSelect an option: ");
    switch (Console.ReadLine())
    {
        case "n":
            return false;
        default:
            return true;
    }
}

static async Task WipeCosmosDb(string connectionString, string databaseId, string containerId)
{
    CosmosClient cosmosClient = new CosmosClient(connectionString);

    try
    {
        var database = cosmosClient.GetDatabase(databaseId);
        var container = database.GetContainer(containerId);
        await container.DeleteContainerAsync();
        await database.DeleteAsync();
        Console.WriteLine("Database and container were deleted.");
    }
    catch (Exception e)
    {
        throw e;
    }
}

static async Task InitializeCosmosDbAsync(string connectionString, string databaseId,
    string containerId, List<dynamic> items, string partitionKey, string recordType)
{
    var cosmosClient = new CosmosClient(connectionString);

    try
    {
        if (items != null || items.Count <= 0)
        {
            var pk = string.Format("/{0}", partitionKey);

            // The database we will create
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", database.Database.Id);

            var container = await database.Database.CreateContainerIfNotExistsAsync(containerId, pk);
            Console.WriteLine("Created Container: {0}\n", container.Container.Id);
            Console.WriteLine("Total Items to Upsert: {0}\n", items.Count);

            foreach (var item in items)
            {
                var json = JsonConvert.SerializeObject(item);

                switch (recordType)
                {
                    case "User":
                        {
                            var user = JsonConvert.DeserializeObject<UserModel>(json);

                            ItemResponse<UserModel> response = await container.Container.UpsertItemAsync(user);

                            Console.WriteLine("Upserted item in database with id: {0} Operation consumed {1} RUs.\n", response.Resource.Id, response.RequestCharge);
                        }
                        break;
                    case "Application":
                        {
                            var range = JsonConvert.DeserializeObject<ApplicationModel>(json);

                            ItemResponse<ApplicationModel> response = await container.Container.UpsertItemAsync(range);

                            Console.WriteLine("Upserted item in database with id: {0} Operation consumed {1} RUs.\n", response.Resource.Id, response.RequestCharge);
                        }
                        break;
                    case "Appointment":
                        {
                            var range = JsonConvert.DeserializeObject<AppointmentModel>(json);

                            ItemResponse<AppointmentModel> response = await container.Container.UpsertItemAsync(range);

                            Console.WriteLine("Upserted item in database with id: {0} Operation consumed {1} RUs.\n", response.Resource.Id, response.RequestCharge);
                        }
                        break;
                    case "Other":
                        {
                            var range = JsonConvert.DeserializeObject<OtherModel>(json);

                            ItemResponse<OtherModel> response = await container.Container.UpsertItemAsync(range);

                            Console.WriteLine("Upserted item in database with id: {0} Operation consumed {1} RUs.\n", response.Resource.Id, response.RequestCharge);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("Error: File not found!");
        }
    }
    catch (CosmosException ex)
    {
        throw ex;
    }
    catch (Exception ex)
    {
        throw ex;
    }
}

static async Task<string> IngestJsonFile(string filePath)
{
    string text = "";

    if (File.Exists(filePath))
    {
        text = File.ReadAllText(filePath);
    }

    return text;
}

static async Task<int> GetRecordCount(string connectionString, string databaseId, string containerId)
{
    int recordCount = 0;

    // The Cosmos client instance
    CosmosClient cosmosClient = new CosmosClient(connectionString);

    try
    {
        var database = cosmosClient.GetDatabase(databaseId);
        var container = database.GetContainer(containerId);

        IOrderedQueryable<dynamic> docQueryable = container.GetItemLinqQueryable<dynamic>(true);
        recordCount = await docQueryable.CountAsync();

        Console.WriteLine("Database[{0}] and container[{1}] has {2} records.", databaseId, containerId, recordCount);
    }
    catch (Exception e)
    {
        throw e;
    }

    return recordCount;
}

