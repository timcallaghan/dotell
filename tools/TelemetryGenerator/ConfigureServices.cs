using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TelemetryGenerator.Services;

namespace TelemetryGenerator;

public static class ConfigureServices
{
    public static void ConfigureOTel(this WebApplicationBuilder appBuilder)
    {
        Action<ResourceBuilder> configureResource = r => r.AddService(
            serviceName: appBuilder.Configuration.GetValue("ServiceName", defaultValue: "telemetry-generator")!,
            serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
            serviceInstanceId: Environment.MachineName);
        
        appBuilder.Services.AddSingleton<Instrumentation>();
        
        appBuilder.Services.AddOpenTelemetry()
            .ConfigureResource(configureResource)
            .WithTracing(builder =>
            {
                builder
                    .AddSource(Instrumentation.ActivitySourceName)
                    .SetSampler(new AlwaysOnSampler())
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint =
                            new Uri(appBuilder.Configuration.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);
                    });
            })
            .WithMetrics(builder =>
            {
                builder
                    .AddMeter(Instrumentation.MeterName)
                    .AddRuntimeInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint =
                            new Uri(appBuilder.Configuration.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);
                    });
            });
        
            appBuilder.Logging.ClearProviders();
            appBuilder.Logging.AddOpenTelemetry(options =>
            {
                var resourceBuilder = ResourceBuilder.CreateDefault();
                configureResource(resourceBuilder);
                options.SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint =
                            new Uri(appBuilder.Configuration.GetValue("Otlp:Endpoint",
                                defaultValue: "http://localhost:4317")!);
                    });
            });
    }
}