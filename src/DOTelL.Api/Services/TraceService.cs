using Grpc.Core;
using OpenTelemetry.Proto.Collector.Trace.V1;

namespace DOTelL.Api.Services;

public class TraceService : OpenTelemetry.Proto.Collector.Trace.V1.TraceService.TraceServiceBase
{
    private readonly ILogger<TraceService> _logger;

    public TraceService(ILogger<TraceService> logger)
    {
        _logger = logger;
    }

    public override Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Traces were processed");

        var response = new ExportTraceServiceResponse();
        return Task.FromResult(response);
    }
}