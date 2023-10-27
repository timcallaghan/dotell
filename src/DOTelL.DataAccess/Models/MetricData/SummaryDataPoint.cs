namespace DOTelL.DataAccess.Models.MetricData;

public class SummaryDataPoint
{
    public Dictionary<string, object?>? Attributes { get; set; }
    public ulong? StartTimeUnixNano { get; set; }
    public ulong TimeUnixNano { get; set; }
    public ulong Count { get; set; }
    public double? Sum { get; set; }
    public List<ValueAtQuantile>? QuantileValues { get; set; }
    public uint? Flags { get; set; }
}