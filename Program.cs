using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UrlShortener.Database;
using UrlShortener.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
{
    Title = "Parra's URL Shortener",
    Version = "v1",
    Description = "UrlShorterner is a web API that receives a webpage URL and then shortens it into a four characters string"
}));

var cs = builder.Configuration.GetSection("DbConn").Value;

builder.Services.AddDbContext<DbConnection>(options
    => options.UseMySql(cs, ServerVersion.Parse("8.0")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

app.MapGroup("addresses")
   .WithTags("Addresses")
   .MapAddresesRoutes();

app.Run();