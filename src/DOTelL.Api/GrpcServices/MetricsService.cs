using DOTelL.DataAccess.Services;
using Grpc.Core;
using OpenTelemetry.Proto.Collector.Metrics.V1;

namespace DOTelL.Api.GrpcServices;

public class MetricsService : OpenTelemetry.Proto.Collector.Metrics.V1.MetricsService.MetricsServiceBase
{
    private readonly ISignalAppender _signalAppender;

    public MetricsService(
        ISignalAppender signalAppender)
    {
        _signalAppender = signalAppender ?? throw new ArgumentNullException(nameof(signalAppender));
    }

    public override async Task<ExportMetricsServiceResponse> Export(ExportMetricsServiceRequest request, ServerCallContext context)
    {
        return await _signalAppender.AppendMetricsAsync(request, context);
    }
}