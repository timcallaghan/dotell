namespace DOTelL.DataAccess.Models.TraceData;

public class TraceEvent
{
    public ulong TimeUnixNano { get; set; }
    public string Name { get; set; } = null!;
    public Dictionary<string, object?>? Attributes { get; set; }
}