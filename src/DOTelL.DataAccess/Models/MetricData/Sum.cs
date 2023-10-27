using OpenTelemetry.Proto.Metrics.V1;

namespace DOTelL.DataAccess.Models.MetricData;

public class Sum
{
    public List<NumberDataPoint> DataPoints { get; set; } = null!;
    public AggregationTemporality AggregationTemporality { get; set; }
    public bool IsMonotonic { get; set; }
}