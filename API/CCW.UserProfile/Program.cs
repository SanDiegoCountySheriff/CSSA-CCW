using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CCW.Common.Services;
using CCW.UserProfile;
using CCW.UserProfile.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

var client = new SecretClient(new Uri(builder.Configuration.GetSection("KeyVault:VaultUri").Value),
    credential: new DefaultAzureCredential());

builder.Services.AddSingleton<IDatabaseContainerResolver>(InitializeDatabaseContainerResolver(builder.Configuration.GetSection("CosmosDb"),
    builder.Configuration.GetSection("TenantDatabaseNameResolution"),
    client).GetAwaiter().GetResult());

builder.Services.AddScoped<ICosmosDbService, CosmosDbService>();

builder.Services.AddAutoMapper(typeof(Program));

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
    });

var b2cAuthoritiesSection = builder.Configuration.GetSection("JwtBearerB2C").GetChildren();
var authenticationSchemes = new List<string>();

foreach (var configurationSection in b2cAuthoritiesSection)
{
    var authorities = configurationSection.GetChildren().ToDictionary(x => x.Key, x => x.Value);

    builder.Services.AddAuthentication()
            .AddJwtBearer(configurationSection.Key, o =>
            {
                o.Authority = authorities["Authority"];
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudiences = new List<string> { authorities["ValidAudiences"] }
                };
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = AuthenticationFailed,
                };
            });

    authenticationSchemes.Add(configurationSection.Key);
}

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
            .AddAuthenticationSchemes(authenticationSchemes.ToArray())
            .Build());

        options.AddPolicy("RequireAdminOnly",policy =>
        {
            policy.RequireRole("CCW-ADMIN-ROLE");
            policy.RequireAuthenticatedUser();
            policy.AddAuthenticationSchemes("aad");
            policy.Build();
        });

        options.AddPolicy("RequireSystemAdminOnly", policy =>
        {
            policy.RequireRole("CCW-SYSTEM-ADMINS-ROLE");
            policy.RequireAuthenticatedUser();
            policy.AddAuthenticationSchemes("aad");
            policy.Build();
        });

        options.AddPolicy("RequireAdminOrSystemAdminOnly", policy =>
        {
            policy.RequireRole(new string[] { "CCW-ADMIN-ROLE", "CCW-SYSTEM-ADMINS-ROLE" });
            policy.RequireAuthenticatedUser();
            policy.AddAuthenticationSchemes("aad");
            policy.Build();
        });

        options.AddPolicy("RequireProcessorOnly", policy =>
        {
            policy.RequireRole("CCW-PROCESSORS-ROLE");
            policy.RequireAuthenticatedUser();
            policy.AddAuthenticationSchemes("aad");
            policy.Build();
        });
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UserProfile CCW",
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
app.UseTenantMiddleware();
app.MapControllers();

app.Run();

static async Task<DatabaseContainerResolver> InitializeDatabaseContainerResolver(
    IConfigurationSection configurationSection,
    IConfigurationSection tenantSection,
    SecretClient secretClient
)
{
    var databaseName = configurationSection["DatabaseName"];
    var adminUsersContainerName = configurationSection["AdminUsersContainerName"];
    var usersContainerName = configurationSection["UsersContainerName"];
    CosmosClientOptions clientOptions = new CosmosClientOptions();
#if DEBUG
    var key = configurationSection["CosmosDbEmulatorConnectionString"];
    clientOptions.WebProxy = new WebProxy()
    {
        BypassProxyOnLocal = true,
    };
    clientOptions.ConnectionMode = ConnectionMode.Gateway;
#else
    var key = secretClient.GetSecret("cosmos-db-connection-primary").Value.Value;
#endif
    var client = new CosmosClient(key, clientOptions);
    var tenants = tenantSection.GetChildren().ToDictionary(x => x.Key, x => x.Value);
    var databases = new Dictionary<string, Database>();

    foreach (var tenant in tenants)
    {
        var database = await client.CreateDatabaseIfNotExistsAsync($"{databaseName}-{tenant.Value}");
        await database.Database.CreateContainerIfNotExistsAsync(adminUsersContainerName, "/id");
        await database.Database.CreateContainerIfNotExistsAsync(usersContainerName, "/id");
        databases.Add(tenant.Key, database);
    }

    return new DatabaseContainerResolver(databases);
}

Task AuthenticationFailed(AuthenticationFailedContext arg)
{
    Console.WriteLine("Authentication Failed");
    return Task.FromResult(0);
}
