using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf.Collections;
using OpenTelemetry.Proto.Common.V1;
using OpenTelemetry.Proto.Resource.V1;
using OpenTelemetry.Proto.Trace.V1;

namespace DOTelL.DataAccess.Models;

public class Trace
{
    public int Id { get; set; }
    
    [Column(TypeName = "jsonb")]
    public Resource Resource { get; set; } = null!;
    
    public string? ResourceSchemaUrl { get; set; }

    [Column(TypeName = "jsonb")]
    public InstrumentationScope? Scope { get; set; }

    public string? SchemaUrl { get; set; }
    public string TraceId { get; set; } = null!;
    public string SpanId { get; set; } = null!;
    public string? TraceState { get; set; }
    public string? ParentSpanId { get; set; }
    public uint? Flags { get; set; }
    public string Name { get; set; } = null!;
    public Span.Types.SpanKind Kind { get; set; }
    public ulong StartTimeUnixNano { get; set; }
    public ulong EndTimeUnixNano { get; set; }
    
    [Column(TypeName = "jsonb")]
    public RepeatedField<KeyValue>? Attributes { get; set; }
    
    [Column(TypeName = "jsonb")]
    public RepeatedField<Span.Types.Event>? Events { get; set; }
    
    [Column(TypeName = "jsonb")]
    public RepeatedField<Span.Types.Link>? Links { get; set; }

    public string? Message { get; set; }
    public Status.Types.StatusCode Code { get; set; }
}