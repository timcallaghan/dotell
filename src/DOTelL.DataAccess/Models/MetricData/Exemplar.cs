namespace DOTelL.DataAccess.Models.MetricData;

public class Exemplar
{
    public Dictionary<string, object?>? FilteredAttributes { get; set; }
    public ulong TimeUnixNano { get; set; }
    public object? Value { get; set; } = null!;
    public string? SpanId { get; set; }
    public string? TraceId { get; set; } = null!;
}