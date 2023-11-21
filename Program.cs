using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;
using UrlShortener.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Uri azKey = new Uri(builder.Configuration.GetSection("KeyVaultURL").Value!);
var azCredential = new DefaultAzureCredential(true);
builder.Configuration.AddAzureKeyVault(azKey, azCredential);

var cs = builder.Configuration.GetSection("dbcon").Value;

builder.Services.AddDbContext<DbConnection>(options
    => options.UseMySql(cs, ServerVersion.Parse("8.0")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => "Welcome to Parra's Url Shortener");

AddressEndPoint.MapRoutes(app);

app.Run();
