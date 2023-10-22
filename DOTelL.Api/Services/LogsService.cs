using Grpc.Core;
using OpenTelemetry.Proto.Collector.Logs.V1;

namespace DOTelL.Api.Services;

public class LogsService : OpenTelemetry.Proto.Collector.Logs.V1.LogsService.LogsServiceBase
{
    private readonly ILogger<LogsService> _logger;

    public LogsService(ILogger<LogsService> logger)
    {
        _logger = logger;
    }

    public override Task<ExportLogsServiceResponse> Export(ExportLogsServiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Logs were processed");

        var response = new ExportLogsServiceResponse();
        return Task.FromResult(response);
    }
}