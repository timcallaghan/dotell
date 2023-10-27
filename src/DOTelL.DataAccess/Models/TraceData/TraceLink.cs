namespace DOTelL.DataAccess.Models.TraceData;

public class TraceLink
{
    public string TraceId { get; set; } = null!;
    public string SpanId { get; set; } = null!;
    public string? TraceState { get; set; }
    public Dictionary<string, object?>? Attributes { get; set; }
    public uint? Flags { get; set; }
}