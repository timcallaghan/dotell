namespace DOTelL.DataAccess.Models.MetricData;

public class HistogramDataPoint
{
    public Dictionary<string, object?>? Attributes { get; set; }
    public ulong? StartTimeUnixNano { get; set; }
    public ulong TimeUnixNano { get; set; }
    public ulong Count { get; set; }
    public double? Sum { get; set; }
    public List<ulong>? BucketCounts { get; set; }
    public List<double>? ExplicitBounds { get; set; }
    public List<Exemplar>? Exemplars { get; set; }
    public uint? Flags { get; set; }
    public double? Min { get; set; }
    public double? Max { get; set; }
}