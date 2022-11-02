using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CCW.Application;
using CCW.Application.Entities;
using CCW.Application.Mappers;
using CCW.Application.Models;
using CCW.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var client = new SecretClient(new Uri(builder.Configuration.GetSection("KeyVault:VaultUri").Value),
    credential: new DefaultAzureCredential());

builder.Services.AddSingleton<ICosmosDbService>(
    InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb"), client).GetAwaiter().GetResult());

builder.Services.AddSingleton<IMapper<PermitApplication, CCW.Application.Entities.Application>, PermitApplicationToApplicationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Alias[]>, PermitApplicationToAliasMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Address>, PermitApplicationToAddressMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Citizenship>, PermitApplicationToCitizenshipMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Contact>, PermitApplicationToContactMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, DOB>, PermitApplicationToDOBMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, IdInfo>, PermitApplicationToIdInfoMapper> ();
builder.Services.AddSingleton<IMapper<PermitApplication, MailingAddress?>, PermitApplicationToMailingAddressMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, PersonalInfo>, PermitApplicationToPersonalInfoMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, PhysicalAppearance>, PermitApplicationToPhysicalAppearanceMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, Address[]>, PermitApplicationToPreviousAddressesMapper> ();
builder.Services.AddSingleton<IMapper<PermitApplication, Weapon[]>, PermitApplicationToWeaponMapper> ();
builder.Services.AddSingleton<IMapper<PermitApplication, History[]>, PermitApplicationToHistoryMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, License>, PermitApplicationToLicenseMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, QualifyingQuestions>, PermitApplicationToQualifyingQuestionsMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, WorkInformation>, PermitApplicationToWorkInformationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, SpouseInformation>, PermitApplicationToSpouseInformationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, Application>, PermitRequestApplicationToApplicationMapper>();
builder.Services.AddSingleton<IMapper<PermitApplication, SpouseAddressInformation>, PermitApplicationToSpouseAddressInformationMapper>();
builder.Services.AddSingleton<IMapper<bool, PermitApplicationRequestModel, PermitApplication>, RequestPermitApplicationModelToEntityMapper> ();
builder.Services.AddSingleton<IMapper<PermitApplication, PermitApplicationResponseModel>, EntityToPermitApplicationResponseMapper>();
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
builder.Services.AddSingleton<IMapper<PermitApplicationRequestModel, SpouseAddressInformation>, RequestPermitApplicationToSpouseAddressInformationMapper>();

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