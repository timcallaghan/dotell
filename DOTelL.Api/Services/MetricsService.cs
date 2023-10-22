using Grpc.Core;
using OpenTelemetry.Proto.Collector.Metrics.V1;

namespace DOTelL.Api.Services;

public class MetricsService : OpenTelemetry.Proto.Collector.Metrics.V1.MetricsService.MetricsServiceBase
{
    private readonly ILogger<MetricsService> _logger;

    public MetricsService(ILogger<MetricsService> logger)
    {
        _logger = logger;
    }

    public override Task<ExportMetricsServiceResponse> Export(ExportMetricsServiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Metrics were processed");

        var response = new ExportMetricsServiceResponse();
        return Task.FromResult(response);
    }
}