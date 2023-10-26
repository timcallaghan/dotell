using DOTelL.DataAccess.Options;
using DOTelL.DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Proto.Logs.V1;
using OpenTelemetry.Proto.Metrics.V1;
using OpenTelemetry.Proto.Trace.V1;

namespace DOTelL.DataAccess;

public static class ConfigureServices
{
    public static void ConfigureDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));
        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("SignalDbContext"));
        dataSourceBuilder.MapEnum<SeverityNumber>();
        dataSourceBuilder.MapEnum<Metric.DataOneofCase>();
        dataSourceBuilder.MapEnum<Span.Types.SpanKind>();
        dataSourceBuilder.MapEnum<Status.Types.StatusCode>();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<SignalDbContext>(options => options.UseNpgsql(dataSource));
        services.AddScoped<ISignalAppender, SignalAppender>();
        services.AddSingleton<IDatabaseBootstrapper, DatabaseBootstrapper>();
    }
}