namespace DOTelL.DataAccess.Models.MetricData;

public class ExponentialHistogramDataPoint
{
    public Dictionary<string, object?>? Attributes { get; set; }
    public ulong? StartTimeUnixNano { get; set; }
    public ulong TimeUnixNano { get; set; }
    public ulong Count { get; set; }
    public double? Sum { get; set; }
    public int? Scale { get; set; }
    public ulong? ZeroCount { get; set; }
    public Buckets? Positive { get; set; }
    public Buckets? Negative { get; set; }
    public uint? Flags { get; set; }
    public List<Exemplar>? Exemplars { get; set; }
    public double? Min { get; set; }
    public double? Max { get; set; }
    public double? ZeroThreshold { get; set; }
}