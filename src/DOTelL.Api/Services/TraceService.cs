using DOTelL.DataAccess;
using Grpc.Core;
using OpenTelemetry.Proto.Collector.Trace.V1;

namespace DOTelL.Api.Services;

public class TraceService : OpenTelemetry.Proto.Collector.Trace.V1.TraceService.TraceServiceBase
{
    private readonly ISignalAppender _signalAppender;

    public TraceService(
        ISignalAppender signalAppender)
    {
        _signalAppender = signalAppender ?? throw new ArgumentNullException(nameof(signalAppender));
    }

    public override async Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request, ServerCallContext context)
    {
        return await _signalAppender.AppendTracesAsync(request, context);
    }
}