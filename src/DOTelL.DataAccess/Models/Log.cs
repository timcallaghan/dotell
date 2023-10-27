using System.ComponentModel.DataAnnotations.Schema;
using DOTelL.DataAccess.Models.LogData;
using Google.Protobuf.Collections;
using OpenTelemetry.Proto.Common.V1;
using OpenTelemetry.Proto.Logs.V1;

namespace DOTelL.DataAccess.Models;

public class Log
{
    public int Id { get; set; }
    
    [Column(TypeName = "jsonb")]
    public Dictionary<string, object?> Resource { get; set; } = null!;
    
    public string? ResourceSchemaUrl { get; set; }
    public string? ScopeName { get; set; }
    public string? ScopeVersion { get; set; }

    [Column(TypeName = "jsonb")]
    public Dictionary<string, object?>? ScopeAttributes { get; set; }

    public string? SchemaUrl { get; set; }
    public ulong TimeUnixNano { get; set; }
    public ulong ObservedTimeUnixNano { get; set; }
    public SeverityNumber? SeverityNumber { get; set; }
    public string? SeverityText { get; set; }
    
    [Column(TypeName = "jsonb")]
    public LogBody? Body { get; set; }
    
    [Column(TypeName = "jsonb")]
    public Dictionary<string, object?>? Attributes { get; set; }

    public uint? Flags { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
}