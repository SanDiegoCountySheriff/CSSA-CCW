using CCW.UserProfile.AuthorizationPolicies;
using CCW.UserProfile.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using CCW.UserProfile;
using Microsoft.OpenApi.Models;
using Serilog;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog(logger);

// Add services to the container.
var client = new SecretClient(new Uri(builder.Configuration.GetSection("KeyVault:VaultUri").Value),
    credential: new DefaultAzureCredential());

builder.Services.AddSingleton<ICosmosDbService>(
    InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"), client).GetAwaiter().GetResult());

builder.Services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();
builder.Services.AddAuthorization(config =>
{
    // Add a new Policy with requirement to check for Admin
    config.AddPolicy("ShouldBeAnAdmin", options =>
    {
        options.RequireAuthenticatedUser();
        options.AuthenticationSchemes.Add(
                JwtBearerDefaults.AuthenticationScheme);
        options.Requirements.Add(new AdminRequirement());
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();



builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CCW",
        Description = "CCW API"
    });
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JSON Web Token based security",
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();

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

app.UseSerilogRequestLogging();
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(
    IConfigurationSection configurationSection, SecretClient secretClient)
{
    var databaseName = configurationSection["DatabaseName"];
    var containerName = configurationSection["ContainerName"];
    var account = configurationSection["Account"];
    var key = secretClient.GetSecret("cosmos-db-connection-primary").Value.Value;
    var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
    var cosmosDbService = new CosmosDbService(client, databaseName, containerName);
    return cosmosDbService;
}