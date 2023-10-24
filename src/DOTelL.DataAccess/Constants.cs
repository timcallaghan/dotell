namespace DOTelL.DataAccess;

internal static class Constants
{
    internal static class Database
    {
        internal const string FileConnectionString = "Data Source=DOTelL.db";
        internal const string InMemoryConnectionString = "DataSource=:memory:";

        internal static class Tables
        {
            internal static class Logs
            {
                internal const string Name = "Logs";
                
                internal const string CreateStatement = $@"CREATE TABLE IF NOT EXISTS {Name} (
                    Resource JSON,
                    Scope JSON,
                    SchemaUrl VARCHAR,
                    TimeUnixNano UBIGINT,
                    ObservedTimeUnixNano UBIGINT,
                    SeverityNumber UTINYINT,
                    SeverityText VARCHAR,
                    Body JSON,
                    Attributes JSON,
                    Flags UINTEGER,
                    TraceId VARCHAR,
                    SpanId VARCHAR);";
            }
            
            internal static class Metrics
            {
                internal const string CreateStatement = @"CREATE TABLE IF NOT EXISTS Metrics (
                    Resource JSON,
                    Scope JSON,
                    SchemaUrl VARCHAR,
                    Name VARCHAR,
                    Description VARCHAR,
                    Unit VARCHAR,
                    Data JSON);";
            }
            
            internal static class Traces
            {
                internal const string CreateStatement = @"CREATE TABLE IF NOT EXISTS Traces (
                    Resource JSON,
                    Scope JSON,
                    SchemaUrl VARCHAR,
                    TraceId VARCHAR,
                    SpanId VARCHAR,
                    TraceState VARCHAR,
                    ParentSpanId VARCHAR,
                    Flags UINTEGER,
                    Name VARCHAR,
                    Kind UTINYINT,
                    StartTimeUnixNano UBIGINT,
                    EndTimeUnixNano UBIGINT,
                    Attributes JSON,
                    Events JSON,
                    Links JSON,
                    Message VARCHAR,
                    Code UTINYINT);";
            }
        }
    }
}