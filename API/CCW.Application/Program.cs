using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CCW.Application;
using CCW.Application.Services;
using CCW.Application.Services.Contracts;
using CCW.Common.AuthorizationPolicies;
using CCW.Common.Services;
using CCW.Common.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var client = new SecretClient(new Uri(builder.Configuration.GetSection("KeyVault:VaultUri").Value),
    credential: new DefaultAzureCredential());

builder.Services.AddSingleton<IApplicationCosmosDbService>(
    InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"), client).GetAwaiter().GetResult());
builder.Services.AddSingleton<IAdminCosmosDbService>(
    InitializeAdminCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"), client).GetAwaiter().GetResult());
builder.Services.AddSingleton<IUserProfileCosmosDbService>(
    InitializeUserProfileCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"), client).GetAwaiter().GetResult());
builder.Services.AddSingleton<IAppointmentCosmosDbService>(
    InitializeAppointmentCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"), client).GetAwaiter().GetResult());
builder.Services.AddSingleton<IEmailService>(
    InitializeEmailServiceInstance(builder.Configuration.GetSection("Graph"), client).GetAwaiter().GetResult());

builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IDocumentAzureStorage, DocumentAzureStorage>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IAuthorizationHandler, IsAdminHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsSystemAdminHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsProcessorHandler>();

builder.Services
    .AddAuthentication()
    .AddJwtBearer("aad", o =>
    {
        o.Authority = builder.Configuration.GetSection("JwtBearerAAD:Authority").Value;
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudiences = new List<string> { builder.Configuration.GetSection("JwtBearerAAD:ValidAudiences").Value }
        };
        o.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = AuthenticationFailed,
        };
    })
    .AddJwtBearer("b2c", o =>
    {
        o.Authority = builder.Configuration.GetSection("JwtBearerB2C:Authority").Value;
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudiences = new List<string> { builder.Configuration.GetSection("JwtBearerB2C:ValidAudiences").Value }
        };
        o.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = AuthenticationFailed,
        };
    });


builder.Services
    .AddAuthorization(options =>
    {
        var apiPolicy = new AuthorizationPolicyBuilder("aad", "b2c")
            .AddAuthenticationSchemes("aad", "b2c")
            .RequireAuthenticatedUser()
            .Build();

        options.AddPolicy("ApiPolicy", apiPolicy);

        options.AddPolicy("AADUsers", new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes("aad")
            .Build());

        options.AddPolicy("B2CUsers", new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes("b2c")
            .Build());

        options.AddPolicy("RequireAdminOnly",
            policy =>
            {
                policy.RequireRole("CCW-ADMIN-ROLE");
                policy.Requirements.Add(new RoleRequirement("CCW-ADMIN-ROLE"));
            });

        options.AddPolicy("RequireSystemAdminOnly", policy =>
        {
            policy.RequireRole("CCW-SYSTEM-ADMINS-ROLE");
            policy.Requirements.Add(new RoleRequirement("CCW-SYSTEM-ADMINS-ROLE"));
        });

        options.AddPolicy("RequireProcessorOnly", policy =>
        {
            policy.RequireRole("CCW-PROCESSORS-ROLE");
            policy.Requirements.Add(new RoleRequirement("CCW-PROCESSORS-ROLE"));
        });
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Application CCW",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                      "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                      "Example: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var origins = builder.Configuration.GetSection("JwtBearerAAD:Origins").Value.Split(",");

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader())
);

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

app.UseHealthChecks("/health");

app.UseCors();

app.UseAuthorization();
app.MapControllers();

app.Run();

static async Task<ApplicationCosmosDbService> InitializeCosmosClientInstanceAsync(
    IConfigurationSection configurationSection, SecretClient secretClient)
{
    var databaseName = configurationSection["DatabaseName"];
    var containerName = configurationSection["ContainerName"];
    var historicalContainerName = configurationSection["HistoricalContainerName"];
    var legacyContainerName = configurationSection["LegacyContainerName"];
    CosmosClientOptions clientOptions = new CosmosClientOptions();
#if DEBUG
    var key = configurationSection["CosmosDbEmulatorConnectionString"];
    clientOptions.WebProxy = new WebProxy()
    {
        BypassProxyOnLocal = true,
    };
#else
    var key = secretClient.GetSecret("cosmos-db-connection-primary").Value.Value;
#endif
    var client = new CosmosClient(key, clientOptions);
    var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/userId");
    await database.Database.CreateContainerIfNotExistsAsync(historicalContainerName, "/userId");
    await database.Database.CreateContainerIfNotExistsAsync(legacyContainerName, "/id");
    var cosmosDbService = new ApplicationCosmosDbService(client, databaseName, containerName, legacyContainerName, historicalContainerName);
    return cosmosDbService;
}

static async Task<AdminCosmosDbService> InitializeAdminCosmosClientInstanceAsync(
    IConfigurationSection configurationSection, SecretClient secretClient)
{
    var databaseName = configurationSection["AdminDatabaseName"];
    var containerName = configurationSection["AdminContainerName"];
    CosmosClientOptions clientOptions = new CosmosClientOptions();
#if DEBUG
    var key = configurationSection["CosmosDbEmulatorConnectionString"];
    clientOptions.WebProxy = new WebProxy()
    {
        BypassProxyOnLocal = true,
    };
#else
    var key = secretClient.GetSecret("cosmos-db-connection-primary").Value.Value;
#endif
    var client = new CosmosClient(key, clientOptions);
    var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
    var cosmosDbService = new AdminCosmosDbService(client, databaseName, containerName);

    return cosmosDbService;
}

static Task<EmailService> InitializeEmailServiceInstance(
    IConfigurationSection configurationSection, SecretClient secretClient)
{
    var tenantId = configurationSection["TenantId"];
    var clientId = configurationSection["ClientId"];
    var clientSecret = configurationSection["ClientSecret"];

    var emailService = new EmailService(tenantId, clientId, clientSecret);

    return Task.FromResult(emailService);
}

static async Task<UserProfileCosmosDbService> InitializeUserProfileCosmosClientInstanceAsync(
    IConfigurationSection configurationSection, SecretClient secretClient)
{
    var databaseName = configurationSection["UserProfileDatabaseName"];
    var containerName = configurationSection["UserProfileContainerName"];
    var userContainerName = configurationSection["UserContainerName"];
    CosmosClientOptions clientOptions = new CosmosClientOptions();
#if DEBUG
    var key = configurationSection["CosmosDbEmulatorConnectionString"];
    clientOptions.WebProxy = new WebProxy()
    {
        BypassProxyOnLocal = true,
    };
#else
    var key = secretClient.GetSecret("cosmos-db-connection-primary").Value.Value;
#endif
    var client = new CosmosClient(key, clientOptions);
    var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
    await database.Database.CreateContainerIfNotExistsAsync(userContainerName, "/id");
    var cosmosDbService = new UserProfileCosmosDbService(client, databaseName, containerName, userContainerName);

    return cosmosDbService;
}

static async Task<AppointmentCosmosDbService> InitializeAppointmentCosmosClientInstanceAsync(
    IConfigurationSection configurationSection,
    SecretClient secretClient)
{
    var appointmentDatabaseName = configurationSection["AppointmentDatabaseName"];
    var appointmentContainerName = configurationSection["AppointmentContainerName"];
    var appointmentManagementContainerName = configurationSection["AppointmentManagementContainerName"];
#if DEBUG
    var key = configurationSection["CosmosDbEmulatorConnectionString"];
#else
    var key = secretClient.GetSecret("cosmos-db-connection-primary").Value.Value;
#endif
    CosmosClientOptions clientOptions = new CosmosClientOptions();
    var client = new CosmosClient(
        key,
        new CosmosClientOptions()
        {
            AllowBulkExecution = true,
            MaxRetryAttemptsOnRateLimitedRequests = 100,
            MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromMinutes(5),
#if DEBUG
            WebProxy = new WebProxy()
            {
                BypassProxyOnLocal = true
            },
#endif
        });

    var appointmentDatabase = await client.CreateDatabaseIfNotExistsAsync(appointmentDatabaseName);
    await appointmentDatabase.Database.CreateContainerIfNotExistsAsync(appointmentContainerName, "/id");
    await appointmentDatabase.Database.CreateContainerIfNotExistsAsync(appointmentManagementContainerName, "/id");

    var appointmentCosmosDbService = new AppointmentCosmosDbService(client, appointmentDatabaseName, appointmentContainerName, appointmentManagementContainerName);

    return appointmentCosmosDbService;
}

Task AuthenticationFailed(AuthenticationFailedContext arg)
{
    Console.WriteLine("Authentication Failed");
    return Task.FromResult(0);
}