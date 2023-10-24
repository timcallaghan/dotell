using DOTelL.DataAccess.Options;
using DOTelL.DataAccess.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DOTelL.DataAccess;

public static class ConfigureServices
{
    public static void ConfigureDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));
        services.AddScoped<ISignalAppender, SignalAppender>();
        services.AddSingleton<IDatabaseBootstrapper, DatabaseBootstrapper>();
    }
}