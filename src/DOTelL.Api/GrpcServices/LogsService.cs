using DOTelL.DataAccess;
using DOTelL.DataAccess.Services;
using Grpc.Core;
using OpenTelemetry.Proto.Collector.Logs.V1;

namespace DOTelL.Api.Services;

public class LogsService : OpenTelemetry.Proto.Collector.Logs.V1.LogsService.LogsServiceBase
{
    private readonly ISignalAppender _signalAppender;

    public LogsService(
        ISignalAppender signalAppender)
    {
        _signalAppender = signalAppender ?? throw new ArgumentNullException(nameof(signalAppender));
    }

    public override async Task<ExportLogsServiceResponse> Export(ExportLogsServiceRequest request, ServerCallContext context)
    {
        return await _signalAppender.AppendLogsAsync(request, context);
    }
}