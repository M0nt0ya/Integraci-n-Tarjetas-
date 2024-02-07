using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.GerenteNacionalDBContext;
using Trade.Infrastructure.Controller;
using Trade.Infrastructure.Repository.Querys;

using Trade.Infrastructure.BackgroundServices;
using Trade.Application;
using Trade.Infrastructure.Repository.Commands;
using Trade.Domain.Interfaces;
using Trade.Domain.Entities;
using Kiosko.Domain.Repository;
using Kiosko.Infrastructure.Repository;
using Kiosko.Application.Services;
using Kiosko.Infrastructure.Controllers;
using YourNamespace.Controllers;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddDbContext<GerenteNacionalDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IExternalApiService, ExternalApiService>();

builder.Services.AddScoped<ITradeRepository, MutationsTrade>();

builder.Services.AddScoped<ITransactionTrade , TransactionTrade>();

builder.Services.AddScoped<VentasAppRepository>();
builder.Services.AddScoped<VentasAppController>();

//Kiosko

builder.Services.AddScoped<IKioskoRepository, KioskoEntityFrameworkRepository>();
builder.Services.AddScoped<KioskoServicesMethods>();
builder.Services.AddScoped<KioskoController>();
builder.Services.AddScoped<KioskoJobsController>();

//Servicios
builder.Services.AddScoped<TradeAppService>();

// Cron Job Para Trade
builder.Services.AddHostedService<CronJobService>();

builder.Services.AddControllers();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseRouting();
// app.UseAuthorization();
 app.MapControllers();
// app.UseCors("AccessPolicy");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
