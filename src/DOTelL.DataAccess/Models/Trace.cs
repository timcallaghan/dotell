using System.ComponentModel.DataAnnotations.Schema;
using DOTelL.DataAccess.Models.TraceData;
using OpenTelemetry.Proto.Trace.V1;

namespace DOTelL.DataAccess.Models;

public class Trace
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
    public Dictionary<string, object?>? Attributes { get; set; }
    
    [Column(TypeName = "jsonb")]
    public List<TraceEvent>? Events { get; set; }
    
    [Column(TypeName = "jsonb")]
    public List<TraceLink>? Links { get; set; }

    public string? Message { get; set; }
    public Status.Types.StatusCode Code { get; set; }
}