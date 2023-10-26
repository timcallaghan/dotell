using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace DOTelL.DataAccess.Services;

internal class DatabaseBootstrapper : IDatabaseBootstrapper
{
    private readonly ILogger<DatabaseBootstrapper> _logger;

    public DatabaseBootstrapper(ILogger<DatabaseBootstrapper> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task EnsureCreated(SignalDbContext dbContext)
    {
        _logger.LogInformation("Checking if database needs to be created");

        await dbContext.Database.MigrateAsync();
        
        if (dbContext.Database.GetDbConnection() is NpgsqlConnection npgsqlConnection)
        {
            await npgsqlConnection.OpenAsync();
            try
            {
                await npgsqlConnection.ReloadTypesAsync();
            }
            finally
            {
                await npgsqlConnection.CloseAsync();
            }
        }
    }
}