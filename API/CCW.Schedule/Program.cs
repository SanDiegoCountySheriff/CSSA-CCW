using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CCW.Schedule;
using CCW.Schedule.Entities;
using CCW.Schedule.Mappers;
using CCW.Schedule.Models;
using CCW.Schedule.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var client = new SecretClient(new Uri(builder.Configuration.GetSection("KeyVault:VaultUri").Value),
    credential: new DefaultAzureCredential());

builder.Services.AddSingleton<ICosmosDbService>(
    InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"), client).GetAwaiter().GetResult());

builder.Services.AddSingleton<IMapper<AppointmentWindowCreateRequestModel, AppointmentWindow>, AppointmentWindowCreateRequestModelToEntityMapper>();
builder.Services.AddSingleton<IMapper<AppointmentWindowUpdateRequestModel, AppointmentWindow>, AppointmentWindowUpdateRequestModelToEntityMapper>();
builder.Services.AddSingleton<IMapper<AppointmentWindow, AppointmentWindowResponseModel>, EntityToAppointmentWindowResponseModelMapper>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger(o =>
{
    o.RouteTemplate = Constants.AppName + "/swagger/{documentname}/swagger.json";
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1/swagger.json", $"CCW {Constants.AppName} v1");
    options.RoutePrefix = $"{Constants.AppName}/swagger";

    options.EnableTryItOutByDefault();
});

app.UseCors("corsapp");
app.UseAuthorization();
app.MapControllers();

app.UseHealthChecks("/health");

app.Run();

static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(
    IConfigurationSection configurationSection, SecretClient secretClient)
{
    var databaseName = configurationSection["DatabaseName"];
    var containerName = configurationSection["ContainerName"];
    var key = secretClient.GetSecret("cosmos-db-connection-primary").Value.Value;
    var client = new Microsoft.Azure.Cosmos.CosmosClient(key);
    var logger = new Logger<CosmosDbService>(new LoggerFactory());
    var cosmosDbService = new CosmosDbService(client, databaseName, containerName, logger);
    return cosmosDbService;
}