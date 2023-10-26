using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf.Collections;
using OpenTelemetry.Proto.Common.V1;
using OpenTelemetry.Proto.Logs.V1;
using OpenTelemetry.Proto.Resource.V1;

namespace DOTelL.DataAccess.Models;

public class Log
{
    public int Id { get; set; }
    
    [Column(TypeName = "jsonb")]
    public Resource Resource { get; set; } = null!;
    
    public string? ResourceSchemaUrl { get; set; }

    [Column(TypeName = "jsonb")]
    public InstrumentationScope? Scope { get; set; }

    public string? SchemaUrl { get; set; }
    public ulong TimeUnixNano { get; set; }
    public ulong ObservedTimeUnixNano { get; set; }
    public SeverityNumber? SeverityNumber { get; set; }
    public string? SeverityText { get; set; }
    
    [Column(TypeName = "jsonb")]
    public AnyValue? Body { get; set; }
    
    [Column(TypeName = "jsonb")]
    public RepeatedField<KeyValue>? Attributes { get; set; }

    public uint? Flags { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
}