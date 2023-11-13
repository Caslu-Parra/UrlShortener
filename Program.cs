using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;
using UrlShortener.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbConnection>(options
    => options.UseMySql(builder.Configuration.GetConnectionString("DataBase"), ServerVersion.Parse("8.0")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => "Welcome to Parra's Url Shortener");

AddressEndPoint.MapRoutes(app);

app.Run();
