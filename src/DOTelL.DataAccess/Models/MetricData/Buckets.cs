namespace DOTelL.DataAccess.Models.MetricData;

public class Buckets
{
    public int Offset { get; set; }
    public List<ulong> BucketCounts { get; set; } = null!;
}