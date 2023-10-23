using Grpc.Core;
using OpenTelemetry.Proto.Collector.Logs.V1;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using OpenTelemetry.Proto.Collector.Trace.V1;

namespace DOTelL.DataAccess;

public interface ISignalAppender
{
    Task<ExportLogsServiceResponse> AppendLogsAsync(ExportLogsServiceRequest request, ServerCallContext context);
    Task<ExportMetricsServiceResponse> AppendMetricsAsync(ExportMetricsServiceRequest request, ServerCallContext context);
    Task<ExportTraceServiceResponse> AppendTracesAsync(ExportTraceServiceRequest request, ServerCallContext context);
}