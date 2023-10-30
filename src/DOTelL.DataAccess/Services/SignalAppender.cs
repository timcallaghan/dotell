using System.Text.Json;
using DOTelL.DataAccess.Extensions;
using DOTelL.DataAccess.Models;
using DOTelL.DataAccess.Models.LogData;
using DOTelL.DataAccess.Models.TraceData;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Proto.Collector.Logs.V1;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using OpenTelemetry.Proto.Collector.Trace.V1;
using Metric = DOTelL.DataAccess.Models.Metric;
using Status = OpenTelemetry.Proto.Trace.V1.Status;
using Trace = DOTelL.DataAccess.Models.Trace;

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

                    var log = new Log
                    {
                        Resource = resourceLog.Resource.Attributes.ToAttributeDictionary(),
                        ResourceSchemaUrl = resourceLog.SchemaUrl,
                        ScopeName = scopeLog.Scope?.Name,
                        ScopeVersion = scopeLog.Scope?.Version,
                        ScopeAttributes = scopeLog.Scope?.Attributes.ToAttributeDictionary(),
                        SchemaUrl = scopeLog.SchemaUrl,
                        TimeUnixNano = logRecord.TimeUnixNano,
                        ObservedTimeUnixNano = observedTime,
                        SeverityNumber = logRecord.SeverityNumber,
                        SeverityText = logRecord.SeverityText,
                        Attributes = logRecord.Attributes?.ToAttributeDictionary(),
                        Flags = logRecord.Flags,
                        TraceId = logRecord.TraceId?.ToTraceId(),
                        SpanId = logRecord.SpanId?.ToSpanId()
                    };

                    if (logRecord.Body is not null)
                    {
                        log.Body = new LogBody
                        {
                            Value = logRecord.Body.ExtractObject()
                        };
                    }
                    
                    _logger.LogInformation(JsonSerializer.Serialize(log));
                    
                    _dbContext.Logs.Add(log);
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
                        Resource = resourceMetric.Resource.Attributes.ToAttributeDictionary(),
                        ResourceSchemaUrl = resourceMetric.SchemaUrl,
                        ScopeName = scopeMetrics.Scope?.Name,
                        ScopeVersion = scopeMetrics.Scope?.Version,
                        ScopeAttributes = scopeMetrics.Scope?.Attributes.ToAttributeDictionary(),
                        SchemaUrl = scopeMetrics.SchemaUrl,
                        Name = metric.Name,
                        Description = metric.Description ?? string.Empty,
                        Unit = metric.Unit ?? string.Empty,
                        MetricType = metric.DataCase,
                        Data = metric.ExtractData()
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
                    var trace = new Trace
                    {
                        Resource = resourceSpan.Resource.Attributes.ToAttributeDictionary(),
                        ResourceSchemaUrl = resourceSpan.SchemaUrl,
                        ScopeName = scopeSpan.Scope?.Name,
                        ScopeVersion = scopeSpan.Scope?.Version,
                        ScopeAttributes = scopeSpan.Scope?.Attributes.ToAttributeDictionary(),
                        SchemaUrl = scopeSpan.SchemaUrl,
                        TraceId = span.TraceId.ToTraceId()!,
                        SpanId = span.SpanId.ToSpanId()!,
                        TraceState = span.TraceState,
                        ParentSpanId = span.ParentSpanId?.ToSpanId(),
                        Flags = span.Flags,
                        Name = span.Name,
                        Kind = span.Kind,
                        StartTimeUnixNano = span.StartTimeUnixNano,
                        EndTimeUnixNano = span.EndTimeUnixNano,
                        Attributes = span.Attributes?.ToAttributeDictionary(),
                        Message = span.Status?.Message,
                        Code = span.Status?.Code ?? Status.Types.StatusCode.Unset
                    };

                    if (span.Events is not null)
                    {
                        trace.Events = span.Events.Select(o => new TraceEvent
                        {
                            TimeUnixNano = o.TimeUnixNano,
                            Name = o.Name,
                            Attributes = o.Attributes?.ToAttributeDictionary()
                        }).ToList();
                    }

                    if (span.Links is not null)
                    {
                        trace.Links = span.Links.Select(o => new TraceLink
                        {
                            TraceId = o.TraceId.ToTraceId()!,
                            SpanId = o.SpanId.ToSpanId()!,
                            TraceState = o.TraceState,
                            Attributes = o.Attributes?.ToAttributeDictionary()
                        }).ToList();
                    }
                    
                    _dbContext.Traces.Add(trace);
                }
            }
        }
        
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Traces were processed.");
        return response;
    }
}