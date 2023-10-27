using OpenTelemetry.Proto.Metrics.V1;

namespace DOTelL.DataAccess.Models.MetricData;

public class Histogram
{
    public List<HistogramDataPoint> DataPoints { get; set; } = null!;
    public AggregationTemporality AggregationTemporality { get; set; }
}