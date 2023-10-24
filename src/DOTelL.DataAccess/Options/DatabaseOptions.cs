namespace DOTelL.DataAccess.Options;

public class DatabaseOptions
{
    public const string SectionName = "Database";

    public bool UseInMemory { get; set; }
    
    internal string ConnectionString => UseInMemory
        ? Constants.Database.InMemoryConnectionString
        : Constants.Database.FileConnectionString;
}