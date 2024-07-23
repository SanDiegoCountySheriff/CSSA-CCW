using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using CCW.Common.AuthorizationPolicies;
using CCW.Common.Services;
using CCW.Common.Services.Contracts;
using CCW.Document;
using CCW.Document.Services;
using CCW.Document.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

var client = new SecretClient(new Uri(builder.Configuration.GetSection("KeyVault:VaultUri").Value),
    credential: new DefaultAzureCredential());

builder.Services.AddScoped<IAuthorizationHandler, IsAdminHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsSystemAdminHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsProcessorHandler>();
builder.Services.AddSingleton<IStorageContainerResolver>(
    InitializeStorageContainerResolver(
        builder.Configuration.GetSection("Storage"),
        builder.Configuration.GetSection("TenantStorageNameResolution"),
        client)
    .GetAwaiter()
    .GetResult());

builder.Services.AddSingleton<ITenantIdResolver>(InitializeTenantIdResolver(builder.Configuration.GetSection("TenantIdResolution")));

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

//Add services to the container.
builder.Services.AddScoped<IAzureStorage, AzureStorage>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Document CCW",
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
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["StorageConnectionString:blob"], preferMsi: true);
});

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

async Task<StorageContainerResolver> InitializeStorageContainerResolver(
    IConfigurationSection configurationSection,
    IConfigurationSection tenantSection,
    SecretClient secretClient)
{
    var agencyContainerName = configurationSection.GetSection("AgencyContainerName").Value;
    var publicContainerName = configurationSection.GetSection("PublicContainerName").Value;
    var adminUserContainerName = configurationSection.GetSection("AdminUserContainerName").Value;
    var adminApplicationContainerName = configurationSection.GetSection("AdminApplicationContainerName").Value;

#if DEBUG
    var storageConnection = configurationSection.GetSection("LocalConnectionString").Value;
#else
    var storageConnection = client.GetSecret("storage-ct-connection-primary").Value.Value;
#endif

    var handler = new HttpClientHandler()
    {
        Proxy = new WebProxy()
        {
            BypassProxyOnLocal = true
        }
    };
    var blobClientOptions = new BlobClientOptions()
    {
        Transport = new HttpClientTransport(handler)
    };

    var blobContainers = new Dictionary<string, BlobContainerClient>();

    var tenants = tenantSection.GetChildren().ToDictionary(x => x.Key, x => x.Value);

    foreach (var tenant in tenants)
    {
#if DEBUG
        var agencyContainer = new BlobContainerClient(storageConnection, $"{agencyContainerName}-{tenant.Value}", blobClientOptions);
#else
        var agencyContainer = new BlobContainerClient(storageConnection, $"{agencyContainerName}-{tenant.Value}");
#endif
        await agencyContainer.CreateIfNotExistsAsync();

        blobContainers.Add($"{tenant.Key}-{agencyContainerName}", agencyContainer);

#if DEBUG
        var publicContainer = new BlobContainerClient(storageConnection, $"{publicContainerName}-{tenant.Value}", blobClientOptions);
#else
        var publicContainer = new BlobContainerClient(storageConnection, $"{publicContainerName}-{tenant.Value}");
#endif
        await publicContainer.CreateIfNotExistsAsync();

        blobContainers.Add($"{tenant.Key}-{publicContainerName}", publicContainer);

#if DEBUG
        var adminUserContainer = new BlobContainerClient(storageConnection, $"{adminUserContainerName}-{tenant.Value}", blobClientOptions);
#else
        var adminUserContainer = new BlobContainerClient(storageConnection, $"{adminUserContainerName}-{tenant.Value}");
#endif
        await adminUserContainer.CreateIfNotExistsAsync();

        blobContainers.Add($"{tenant.Key}-{adminUserContainerName}", adminUserContainer);

#if DEBUG
        var adminApplicationContainer = new BlobContainerClient(storageConnection, $"{adminApplicationContainerName}-{tenant.Value}", blobClientOptions);
#else
        var adminApplicationContainer = new BlobContainerClient(storageConnection, $"{adminApplicationContainerName}-{tenant.Value}");
#endif
        await adminApplicationContainer.CreateIfNotExistsAsync();

        blobContainers.Add($"{tenant.Key}-{adminApplicationContainerName}", adminApplicationContainer);
    }

    return new StorageContainerResolver(blobContainers);
}

static TenantIdResolver InitializeTenantIdResolver(IConfigurationSection configurationSection)
{
    var tenantIds = configurationSection.GetChildren().ToDictionary(x => x.Key, x => x.Value);

    return new TenantIdResolver(tenantIds);
}

Task AuthenticationFailed(AuthenticationFailedContext arg)
{
    Console.WriteLine("Authentication Failed");
    return Task.FromResult(0);
}