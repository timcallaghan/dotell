using DOTelL.DataAccess.Options;
using DuckDB.NET.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DOTelL.DataAccess.Services;

internal class DatabaseBootstrapper : IDatabaseBootstrapper
{
    private readonly ILogger<DatabaseBootstrapper> _logger;
    private readonly DatabaseOptions _options;

    public DatabaseBootstrapper(
        ILogger<DatabaseBootstrapper> logger,
        IOptions<DatabaseOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (options == null) throw new ArgumentNullException(nameof(options));
        _options = options.Value;
    }

    public async Task EnsureCreated()
    {
        _logger.LogInformation("Checking if database needs to be created");

        await using var connection = new DuckDBConnection(ConnectionString);
        await connection.OpenAsync();

        await using var duckDbCommand = connection.CreateCommand();
        
        duckDbCommand.CommandText = Constants.Database.Tables.Logs.CreateStatement;
        await duckDbCommand.ExecuteNonQueryAsync();
        
        duckDbCommand.CommandText = Constants.Database.Tables.Metrics.CreateStatement;
        await duckDbCommand.ExecuteNonQueryAsync();
        
        duckDbCommand.CommandText = Constants.Database.Tables.Traces.CreateStatement;
        await duckDbCommand.ExecuteNonQueryAsync();
        
        await connection.CloseAsync();
    }

    private string ConnectionString => _options.UseInMemory
        ? Constants.Database.InMemoryConnectionString
        : Constants.Database.FileConnectionString;
}