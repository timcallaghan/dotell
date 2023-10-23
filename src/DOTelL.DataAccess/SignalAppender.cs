using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Proto.Collector.Logs.V1;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using OpenTelemetry.Proto.Collector.Trace.V1;

namespace DOTelL.DataAccess;

public class SignalAppender : ISignalAppender
{
    private readonly ILogger<SignalAppender> _logger;

    public SignalAppender(ILogger<SignalAppender> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<ExportLogsServiceResponse> AppendLogsAsync(ExportLogsServiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Logs were processed");
        
        var response = new ExportLogsServiceResponse();
        return Task.FromResult(response);
    }

    public Task<ExportMetricsServiceResponse> AppendMetricsAsync(ExportMetricsServiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Metrics were processed");
        
        var response = new ExportMetricsServiceResponse();
        return Task.FromResult(response);
    }

    public Task<ExportTraceServiceResponse> AppendTracesAsync(ExportTraceServiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Traces were processed");
        
        var response = new ExportTraceServiceResponse();
        return Task.FromResult(response);
    }
}