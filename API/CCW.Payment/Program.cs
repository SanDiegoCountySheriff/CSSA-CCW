using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CCW.Payment;
using CCW.Payment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var client = new SecretClient(new Uri(builder.Configuration.GetSection("KeyVault:VaultUri").Value),
    credential: new DefaultAzureCredential());

builder.Services.AddSingleton<ICosmosDbService>(
    InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"), client).GetAwaiter().GetResult());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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
}

app.UseCors("corsapp");

app.UseHttpsRedirection();

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
    var cosmosDbService = new CosmosDbService(client, databaseName, containerName);
    return cosmosDbService;
}