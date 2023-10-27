using OpenTelemetry.Proto.Metrics.V1;

namespace DOTelL.DataAccess.Models.MetricData;

public class ExponentialHistogram
{
    public List<ExponentialHistogramDataPoint> DataPoints { get; set; } = null!;
    public AggregationTemporality AggregationTemporality { get; set; }
}