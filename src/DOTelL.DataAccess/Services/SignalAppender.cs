using DOTelL.DataAccess.Options;
using DuckDB.NET.Data;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Proto.Collector.Logs.V1;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using OpenTelemetry.Proto.Collector.Trace.V1;

namespace DOTelL.DataAccess.Services;

internal class SignalAppender : ISignalAppender
{
    private readonly ILogger<SignalAppender> _logger;
    private readonly DatabaseOptions _options;

    public SignalAppender(
        ILogger<SignalAppender> logger,
        IOptions<DatabaseOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (options == null) throw new ArgumentNullException(nameof(options));
        _options = options.Value;
    }

    public async Task<ExportLogsServiceResponse> AppendLogsAsync(ExportLogsServiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Processing logs...");
        
        await using var connection = new DuckDBConnection(_options.ConnectionString);
        await connection.OpenAsync();
        
        using (var appender = connection.CreateAppender(Constants.Database.Tables.Logs.Name))
        {
            foreach (var resourceLog in request.ResourceLogs)
            {
                var resource = resourceLog.Resource?.ToString();
                
                foreach (var scopeLog in resourceLog.ScopeLogs)
                {
                    var scope = scopeLog.Scope?.ToString();
                    
                    foreach (var logRecord in scopeLog.LogRecords)
                    {
                        var row = appender.CreateRow();

                        var observedTime = logRecord.ObservedTimeUnixNano > 0L
                            ? logRecord.ObservedTimeUnixNano
                            : (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000000L;
                        
                        row.AppendValue(resource)
                            .AppendValue(scope)
                            .AppendValue(scopeLog.SchemaUrl)
                            .AppendValue(logRecord.TimeUnixNano)
                            .AppendValue(observedTime)
                            .AppendValue((byte)logRecord.SeverityNumber)
                            .AppendValue(logRecord.SeverityText)
                            .AppendValue(logRecord.Body?.ToString())
                            .AppendValue(logRecord.Attributes?.ToString())
                            .AppendValue(logRecord.Flags)
                            .AppendValue(logRecord.TraceId?.ToString())
                            .AppendValue(logRecord.SpanId?.ToString())
                            .EndRow();
                    }
                }
            }
        }
        
        await connection.CloseAsync();
        
        _logger.LogInformation("Logs were processed.");
        
        var response = new ExportLogsServiceResponse();
        return response;
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