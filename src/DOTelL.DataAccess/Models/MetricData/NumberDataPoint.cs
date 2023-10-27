namespace DOTelL.DataAccess.Models.MetricData;

public class NumberDataPoint
{
    public Dictionary<string, object?>? Attributes { get; set; }
    public ulong? StartTimeUnixNano { get; set; }
    public ulong TimeUnixNano { get; set; }
    public object? Value { get; set; } = null!;
    public List<Exemplar>? Exemplars { get; set; }
    public uint? Flags { get; set; }
}