using Pract1Frame.Application.Interfaces;
using Pract1Frame.Application.Services;
using Pract1Frame.Domain.Interfaces;
using Pract1Frame.Infrastructure.Repository;
using Pract1Frame.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Laptops API",
        Version = "v1",
        Description = "API для управления ноутбуками"
    });
});

builder.Services.AddSingleton<ILaptop, InMemoryLaptop>();
builder.Services.AddScoped<ILaptopServices, LaptopService>();

var app = builder.Build();

app.UseMiddleware<RequestIdMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>(); 
app.UseMiddleware<TimingMiddleware>(); 

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Laptops API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();