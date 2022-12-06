using CCW.Document;
using CCW.Document.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

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

//Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<IAzureStorage, AzureStorage>();

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

Task AuthenticationFailed(AuthenticationFailedContext arg)
{
    Console.WriteLine("Authentication Failed");
    return Task.FromResult(0);
}