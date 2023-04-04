using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CCW.Application;
using CCW.Application.Clients;
using CCW.Application.Entities;
using CCW.Application.Mappers;
using CCW.Application.Models;
using CCW.Application.Services;
using CCW.Common.AuthorizationPolicies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var client = new SecretClient(new Uri(builder.Configuration.GetSection("KeyVault:VaultUri").Value),
    credential: new DefaultAzureCredential());

builder.Services.AddSingleton<ICosmosDbService>(
    InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"), client).GetAwaiter().GetResult());

builder.Services.AddHeaderPropagation(o =>
{
    o.Headers.Add("Authorization");
});

builder.Services.AddHttpClient<IDocumentServiceClient, DocumentServiceClient>("DocumentHttpClient", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetSection("DocumentApi:BaseUrl").Value);
    c.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(builder.Configuration.GetSection("DocumentApi:Timeout").Value));
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    
}).AddHeaderPropagation();

builder.Services.AddHttpClient<IAdminServiceClient, AdminServiceClient>("AdminHttpClient", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetSection("AdminApi:BaseUrl").Value);
    c.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(builder.Configuration.GetSection("AdminApi:Timeout").Value));
    c.DefaultRequestHeaders.Add("Accept", "application/json");

}).AddHeaderPropagation();

builder.Services.AddHttpClient<IUserProfileServiceClient, UserProfileServiceClient>("UserProfileClient", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetSection("UserProfileApi:BaseUrl").Value);
    c.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(builder.Configuration.GetSection("UserProfileApi:Timeout").Value));
    c.DefaultRequestHeaders.Add("Accept", "application/json");

}).AddHeaderPropagation();

builder.Services.AddSingleton<IMapper<PaymentHistory, PaymentHistoryResponseModel>, PaymentHistoryToPaymentHistoryResponseModelMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, PaymentHistory[]>, PermitApplicationToPaymentHistoryMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, PaymentHistory[]>, RequestPermitApplicationToPaymentHistoryMapper>();

builder.Services.AddSingleton<IMapper<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel>, EntityToSummarizedPermitApplicationModelMapper>();
builder.Services.AddSingleton<IMapper<History, HistoryResponseModel>, HistoryToHistoryResponseModelMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Application>, PermitApplicationToApplicationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Alias[]>, PermitApplicationToAliasMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Address>, PermitApplicationToAddressMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Citizenship>, PermitApplicationToCitizenshipMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Contact>, PermitApplicationToContactMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, DOB>, PermitApplicationToDOBMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, IdInfo>, PermitApplicationToIdInfoMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, MailingAddress?>, PermitApplicationToMailingAddressMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, PersonalInfo>, PermitApplicationToPersonalInfoMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, PhysicalAppearance>, PermitApplicationToPhysicalAppearanceMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Address[]>, PermitApplicationToPreviousAddressesMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Weapon[]>, PermitApplicationToWeaponMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, History[]>, PermitApplicationToHistoryMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, License>, PermitApplicationToLicenseMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, QualifyingQuestions>, PermitApplicationToQualifyingQuestionsMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, WorkInformation>, PermitApplicationToWorkInformationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, SpouseInformation>, PermitApplicationToSpouseInformationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, Application>, PermitRequestApplicationToApplicationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, SpouseAddressInformation>, PermitApplicationToSpouseAddressInformationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, ImmigrantInformation>, PermitApplicationToImmigrantInformationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, UploadedDocument[]>, PermitApplicationToUploadDocumentMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, BackgroundCheck>, PermitApplicationToBackgroundCheckMapper>();
builder.Services.AddSingleton<IMapper<bool, PermitApplicationRequestModel, PermitApplication>, RequestPermitApplicationModelToEntityMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, PermitApplicationResponseModel>, EntityToPermitApplicationResponseMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, UserApplication>, PermitApplicationToUserApplicationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, UserPermitApplicationResponseModel>, EntityToUserPermitApplicationResponseMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, Alias[]>, RequestPermitApplicationToAliasMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, Address>, RequestPermitApplicationToAddressMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, Citizenship>, RequestPermitApplicationToCitizenshipMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, Contact>, RequestPermitApplicationToContactMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, DOB>, RequestPermitApplicationToDOBMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, IdInfo>, RequestPermitApplicationToIdInfoMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, MailingAddress?>, RequestPermitApplicationToMailingAddressMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, PersonalInfo>, RequestPermitApplicationToPersonalInfoMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, PhysicalAppearance>, RequestPermitApplicationToPhysicalAppearanceMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, Address[]>, RequestPermitApplicationToPreviousAddressesMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, Weapon[]>, RequestPermitApplicationToWeaponMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, History[]>, RequestPermitApplicationToHistoryMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, License>, RequestPermitApplicationToLicenseMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, QualifyingQuestions>, RequestPermitApplicationToQualifyingQuestionsMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, WorkInformation>, RequestPermitApplicationToWorkInformationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, SpouseInformation>, RequestPermitApplicationToSpouseInformationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, ImmigrantInformation>, RequestPermitApplicationToImmigrantInformationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, SpouseAddressInformation>, RequestPermitApplicationToSpouseAddressInformationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, UploadedDocument[]>, RequestPermitApplicationToUploadDocumentMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, BackgroundCheck>, RequestPermitApplicationToBackgroundCheckMapper>();
builder.Services.AddSingleton<IMapper<string, UserPermitApplicationRequestModel, Application>, UserPermitRequestApplicationToApplicationMapper>();
builder.Services.AddSingleton<IMapper<bool, string, UserPermitApplicationRequestModel, PermitApplication>, RequestUserPermitApplicationModelToEntityMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, Alias[]>, UserRequestPermitApplicationToAliasMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, Address>, UserRequestPermitApplicationToAddressMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, Citizenship>, UserRequestPermitApplicationToCitizenshipMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, Contact>, UserRequestPermitApplicationToContactMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, DOB>, UserRequestPermitApplicationToDOBMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, IdInfo>, UserRequestPermitApplicationToIdInfoMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, MailingAddress?>, UserRequestPermitApplicationToMailingAddressMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, PersonalInfo>, UserRequestPermitApplicationToPersonalInfoMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, PhysicalAppearance>, UserRequestPermitApplicationToPhysicalAppearanceMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, Address[]>, UserRequestPermitApplicationToPreviousAddressesMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, Weapon[]>, UserRequestPermitApplicationToWeaponMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, License>, UserRequestPermitApplicationToLicenseMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, QualifyingQuestions>, UserRequestPermitApplicationToQualifyingQuestionsMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, WorkInformation>, UserRequestPermitApplicationToWorkInformationMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, SpouseInformation>, UserRequestPermitApplicationToSpouseInformationMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, ImmigrantInformation>, UserRequestPermitApplicationToImmigrantInformationMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, SpouseAddressInformation>, UserRequestPermitApplicationToSpouseAddressInformationMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, UploadedDocument[]>, UserRequestPermitApplicationToUploadDocumentMapper>();
builder.Services.AddSingleton<IMapper<UserPermitApplicationRequestModel, BackgroundCheck>, UserRequestPermitApplicationToBackgroundCheckMapper>();


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

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader())
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

app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

app.UseCors();

app.UseAuthorization();
app.UseHeaderPropagation();
app.MapControllers();

app.Run();

static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(
    IConfigurationSection configurationSection, SecretClient secretClient)
{
    var databaseName = configurationSection["DatabaseName"];
    var containerName = configurationSection["ContainerName"];
    var key = secretClient.GetSecret("cosmos-db-connection-primary").Value.Value;
    CosmosClientOptions clientOptions = new CosmosClientOptions();
#if DEBUG        
    clientOptions.ConnectionMode = ConnectionMode.Gateway;
#endif
    var client = new Microsoft.Azure.Cosmos.CosmosClient(key, clientOptions);
    var cosmosDbService = new CosmosDbService(client, databaseName, containerName);
    return cosmosDbService;
}

Task AuthenticationFailed(AuthenticationFailedContext arg)
{
    Console.WriteLine("Authentication Failed");
    return Task.FromResult(0);
}