using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace TelemetryGenerator.Services;

public class Instrumentation : IDisposable
{
    internal const string ActivitySourceName = "TelemetryGenerator";
    internal const string MeterName = "TelemetryGenerator.Freezing";
    private readonly Meter _meter;

    public Instrumentation()
    {
        string? version = typeof(Instrumentation).Assembly.GetName().Version?.ToString();
        ActivitySource = new ActivitySource(ActivitySourceName, version);
        _meter = new Meter(MeterName, version);
        FreezingDaysCounter = _meter.CreateCounter<long>("weather.days.freezing", "The number of days where the temperature is below freezing");
    }

    public ActivitySource ActivitySource { get; }

    public Counter<long> FreezingDaysCounter { get; }

    public void Dispose()
    {
        ActivitySource.Dispose();
        _meter.Dispose();
    }
}