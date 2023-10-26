using System.ComponentModel.DataAnnotations.Schema;
using OpenTelemetry.Proto.Common.V1;
using OpenTelemetry.Proto.Metrics.V1;
using OpenTelemetry.Proto.Resource.V1;

namespace DOTelL.DataAccess.Models;

public class Metric
{
    public int Id { get; set; }
    
    [Column(TypeName = "jsonb")]
    public Resource Resource { get; set; } = null!;
    
    public string? ResourceSchemaUrl { get; set; }
    
    [Column(TypeName = "jsonb")]
    public InstrumentationScope? Scope { get; set; }

    public string? SchemaUrl { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Unit { get; set; } = null!;
    public OpenTelemetry.Proto.Metrics.V1.Metric.DataOneofCase MetricType { get; set; }
    
    [Column(TypeName = "jsonb")]
    public Gauge? Gauge { get; set; }
    
    [Column(TypeName = "jsonb")]
    public Sum? Sum { get; set; }
    
    [Column(TypeName = "jsonb")]
    public Histogram? Histogram { get; set; }
    
    [Column(TypeName = "jsonb")]
    public ExponentialHistogram? ExponentialHistogram { get; set; }
    
    [Column(TypeName = "jsonb")]
    public Summary? Summary { get; set; }
}