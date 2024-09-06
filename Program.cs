using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UrlShortener.Database;
using UrlShortener.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
{
    Title = "Encurtador de URL do Parra",
    Version = "v1",
    Description = "Esta API encurta uma URL longa para um novo endereço de 4 caracteres"
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
   .WithTags("Endereços")
   .MapAddresesRoutes();

app.Run();