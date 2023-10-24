using DOTelL.DataAccess;
using DOTelL.DataAccess.Services;

namespace DOTelL.Api.Services;

public class DatabaseBootstrapService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseBootstrapService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var databaseBootstrapper = scope.ServiceProvider.GetRequiredService<IDatabaseBootstrapper>();

        await databaseBootstrapper.EnsureCreated();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}