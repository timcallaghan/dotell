using DOTelL.DataAccess.Models;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Proto.Collector.Logs.V1;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using OpenTelemetry.Proto.Collector.Trace.V1;
using Metric = DOTelL.DataAccess.Models.Metric;
using Status = OpenTelemetry.Proto.Trace.V1.Status;

namespace DOTelL.DataAccess.Services;

internal class SignalAppender : ISignalAppender
{
    private readonly ILogger<SignalAppender> _logger;
    private readonly SignalDbContext _dbContext;

    public SignalAppender(
        ILogger<SignalAppender> logger,
        SignalDbContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<ExportLogsServiceResponse> AppendLogsAsync(ExportLogsServiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Processing logs...");
        
        var response = new ExportLogsServiceResponse();
        var partialSuccess = new ExportLogsPartialSuccess();
        response.PartialSuccess = partialSuccess;
        
        foreach (var resourceLog in request.ResourceLogs)
        {
            if (resourceLog.Resource is null)
            {
                _logger.LogWarning("Unable to process log records - resource is not specified in the payload");
                partialSuccess.RejectedLogRecords += resourceLog.ScopeLogs.SelectMany(o => o.LogRecords).Count();
                partialSuccess.ErrorMessage =
                    "Unable to process log records - resource is not specified in the payload";
                continue;
            }
            
            foreach (var scopeLog in resourceLog.ScopeLogs)
            {
                foreach (var logRecord in scopeLog.LogRecords)
                {
                    var observedTime = logRecord.ObservedTimeUnixNano > 0L
                        ? logRecord.ObservedTimeUnixNano
                        : (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000000L;
                    
                    _dbContext.Logs.Add(new Log
                    {
                        Resource = resourceLog.Resource,
                        ResourceSchemaUrl = resourceLog.SchemaUrl,
                        Scope = scopeLog.Scope,
                        SchemaUrl = scopeLog.SchemaUrl,
                        TimeUnixNano = logRecord.TimeUnixNano,
                        ObservedTimeUnixNano = observedTime,
                        SeverityNumber = logRecord.SeverityNumber,
                        SeverityText = logRecord.SeverityText,
                        Body = logRecord.Body,
                        Attributes = logRecord.Attributes,
                        Flags = logRecord.Flags,
                        TraceId = logRecord.TraceId.ToBase64(),
                        SpanId = logRecord.SpanId.ToBase64()
                    });
                }
            }
        }

        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Logs were processed.");
        return response;
    }

    public async Task<ExportMetricsServiceResponse> AppendMetricsAsync(ExportMetricsServiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Processing metrics...");
        
        var response = new ExportMetricsServiceResponse();
        var partialSuccess = new ExportMetricsPartialSuccess();
        response.PartialSuccess = partialSuccess;

        foreach (var resourceMetric in request.ResourceMetrics)
        {
            if (resourceMetric.Resource is null)
            {
                _logger.LogWarning("Unable to process metric records - resource is not specified in the payload");
                partialSuccess.RejectedDataPoints += resourceMetric.ScopeMetrics.SelectMany(o => o.Metrics).Count();
                partialSuccess.ErrorMessage =
                    "Unable to process metric records - resource is not specified in the payload";
                continue;
            }
            
            foreach (var scopeMetrics in resourceMetric.ScopeMetrics)
            {
                foreach (var metric in scopeMetrics.Metrics)
                {
                    _dbContext.Metrics.Add(new Metric
                    {
                        Resource = resourceMetric.Resource,
                        ResourceSchemaUrl = resourceMetric.SchemaUrl,
                        Scope = scopeMetrics.Scope,
                        SchemaUrl = scopeMetrics.SchemaUrl,
                        Name = metric.Name,
                        Description = metric.Description ?? string.Empty,
                        Unit = metric.Unit ?? string.Empty,
                        MetricType = metric.DataCase,
                        Gauge = metric.Gauge,
                        Sum = metric.Sum,
                        Histogram = metric.Histogram,
                        ExponentialHistogram = metric.ExponentialHistogram,
                        Summary = metric.Summary
                    });
                }
            }
        }
        
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Metrics were processed.");
        return response;
    }

    public async Task<ExportTraceServiceResponse> AppendTracesAsync(ExportTraceServiceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Processing traces...");
        
        var response = new ExportTraceServiceResponse();
        var partialSuccess = new ExportTracePartialSuccess();
        response.PartialSuccess = partialSuccess;
        
        foreach (var resourceSpan in request.ResourceSpans)
        {
            if (resourceSpan.Resource is null)
            {
                _logger.LogWarning("Unable to process trace records - resource is not specified in the payload");
                partialSuccess.RejectedSpans += resourceSpan.ScopeSpans.SelectMany(o => o.Spans).Count();
                partialSuccess.ErrorMessage =
                    "Unable to process trace records - resource is not specified in the payload";
                continue;
            }
            
            foreach (var scopeSpan in resourceSpan.ScopeSpans)
            {
                foreach (var span in scopeSpan.Spans)
                {
                    _dbContext.Traces.Add(new Trace
                    {
                        Resource = resourceSpan.Resource,
                        ResourceSchemaUrl = resourceSpan.SchemaUrl,
                        Scope = scopeSpan.Scope,
                        SchemaUrl = scopeSpan.SchemaUrl,
                        TraceId = span.TraceId.ToBase64(),
                        SpanId = span.SpanId.ToBase64(),
                        TraceState = span.TraceState,
                        ParentSpanId = span.ParentSpanId?.ToBase64(),
                        Flags = span.Flags,
                        Name = span.Name,
                        Kind = span.Kind,
                        StartTimeUnixNano = span.StartTimeUnixNano,
                        EndTimeUnixNano = span.EndTimeUnixNano,
                        Attributes = span.Attributes,
                        Events = span.Events,
                        Links = span.Links,
                        Message = span.Status?.Message,
                        Code = span.Status?.Code ?? Status.Types.StatusCode.Unset
                    });
                }
            }
        }
        
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Traces were processed.");
        return response;
    }
}