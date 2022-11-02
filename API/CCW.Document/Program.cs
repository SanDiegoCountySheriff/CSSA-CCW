using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CCW.Document;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
//var client = new SecretClient(new Uri(builder.Configuration.GetSection("KeyVault:VaultUri").Value),
//    credential: new DefaultAzureCredential());

//var storageConnection = client.GetSecret("storage-account-test-conn2").Value;

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