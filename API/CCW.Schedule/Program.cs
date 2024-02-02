using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CCW.Common.AuthorizationPolicies;
using CCW.Schedule;
using CCW.Schedule.Services;
using CCW.Schedule.Services.Contracts;
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

builder.Services.AddSingleton<IAppointmentCosmosDbService>(
    InitializeAppointmentCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"), client).GetAwaiter().GetResult());

builder.Services.AddSingleton<IApplicationCosmosDbService>(InitializeApplicationCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"), client).GetAwaiter().GetResult());

builder.Services.AddHeaderPropagation(o =>
{
    o.Headers.Add("Authorization");
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IAuthorizationHandler, IsAdminHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsSystemAdminHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsProcessorHandler>();

builder.Services
    .AddAuthentication("aad")
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
        Title = "Appointments CCW",
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

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins(new string[] { "http://localhost:4000", "http://localhost:3000" }).AllowAnyMethod().AllowAnyHeader().AllowCredentials())
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
app.UseHeaderPropagation();
app.MapControllers();

app.Run();

static async Task<AppointmentCosmosDbService> InitializeAppointmentCosmosClientInstanceAsync(
    IConfigurationSection configurationSection,
    SecretClient secretClient)
{
    var appointmentDatabaseName = configurationSection["AppointmentDatabaseName"];
    var appointmentContainerName = configurationSection["AppointmentContainerName"];
    var holidayContainerName = configurationSection["HolidayContainerName"];
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
#if DEBUG
            WebProxy = new WebProxy()
            {
                BypassProxyOnLocal = true
            },
#endif
        });

    var appointmentDatabase = await client.CreateDatabaseIfNotExistsAsync(appointmentDatabaseName);
    await appointmentDatabase.Database.CreateContainerIfNotExistsAsync(appointmentContainerName, "/id");
    await appointmentDatabase.Database.CreateContainerIfNotExistsAsync(holidayContainerName, "/id");

    var appointmentCosmosDbService = new AppointmentCosmosDbService(client, appointmentDatabaseName, appointmentContainerName, holidayContainerName);

    return appointmentCosmosDbService;
}

static async Task<ApplicationCosmosDbService> InitializeApplicationCosmosClientInstanceAsync(
    IConfigurationSection configurationSection,
    SecretClient secretClient)
{
    var applicationDatabaseName = configurationSection["ApplicationDatabaseName"];
    var applicationContainerName = configurationSection["ApplicationContainerName"];
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
#if DEBUG
            WebProxy = new WebProxy()
            {
                BypassProxyOnLocal = true
            },
#endif
        });

    var applicationDatabase = await client.CreateDatabaseIfNotExistsAsync(applicationDatabaseName);
    await applicationDatabase.Database.CreateContainerIfNotExistsAsync(applicationContainerName, "/userId");

    var applicationCosmosDbService = new ApplicationCosmosDbService(client, applicationDatabaseName, applicationContainerName);

    return applicationCosmosDbService;
}

Task AuthenticationFailed(AuthenticationFailedContext arg)
{
    Console.WriteLine("Authentication Failed");
    return Task.FromResult(0);
}