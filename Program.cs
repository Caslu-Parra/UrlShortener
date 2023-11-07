using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MySqlX.XDevAPI.Common;
using Renci.SshNet.Messages;
using UrlShortener.Database;
using UrlShortener.Models;
using UrlShortener.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbConnection>(options
    => options.UseMySql(builder.Configuration.GetConnectionString("DataBase"), ServerVersion.Parse("8.0")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Welcome to Parra's Url Shortener");

AddressEndPoint.MapRoutes(app);

app.Run();
