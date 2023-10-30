namespace TelemetryGenerator.Options;

public class ExternalApiOptions
{
    public const string SectionName = "ExternalApi";

    public string Endpoint { get; set; } = null!;
}