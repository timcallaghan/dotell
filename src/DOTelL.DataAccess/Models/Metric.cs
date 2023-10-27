using System.ComponentModel.DataAnnotations.Schema;

namespace DOTelL.DataAccess.Models;

public class Metric
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
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Unit { get; set; } = null!;
    public OpenTelemetry.Proto.Metrics.V1.Metric.DataOneofCase MetricType { get; set; }
    
    [Column(TypeName = "jsonb")]
    public object? Data { get; set; }
}