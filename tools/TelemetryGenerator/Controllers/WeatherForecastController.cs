using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TelemetryGenerator.Models;
using TelemetryGenerator.Options;
using TelemetryGenerator.Services;

namespace TelemetryGenerator.Controllers;

[ApiController]
[Route("forecast")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ActivitySource _activitySource;
    private readonly Counter<long> _freezingDaysCounter;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ExternalApiOptions _externalApiOptions;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        Instrumentation instrumentation,
        IHttpClientFactory httpClientFactory,
        IOptions<ExternalApiOptions> options)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _activitySource = instrumentation.ActivitySource;
        _freezingDaysCounter = instrumentation.FreezingDaysCounter;
        _externalApiOptions = options.Value;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        using var scope = _logger.BeginScope("{Id}", Guid.NewGuid().ToString("N"));
        
        // ReSharper disable once ExplicitCallerInfoArgument
        using var activity = _activitySource.StartActivity("calculate forecast");

        var rng = new Random();
        var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
            })
            .ToArray();
        
        _freezingDaysCounter.Add(forecast.Count(f => f.TemperatureC < 0));

        _logger.LogInformation(
            "WeatherForecasts generated {count}: {forecasts}",
            forecast.Length,
            forecast);

        return forecast;
    }
    
    [HttpGet]
    [Route("external/{depth:int}")]
    public async Task<IEnumerable<WeatherForecast>> GetExternal(int depth)
    {
        if (depth <= 0 || string.IsNullOrWhiteSpace(_externalApiOptions.Endpoint))
        {
            return Get();
        }
        
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(_externalApiOptions.Endpoint);
        var response = await client.GetAsync($"forecast/external/{--depth}");
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>())!;
    }
}