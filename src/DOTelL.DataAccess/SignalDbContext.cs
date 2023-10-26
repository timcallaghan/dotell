using DOTelL.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Proto.Logs.V1;
using OpenTelemetry.Proto.Trace.V1;

namespace DOTelL.DataAccess;

public class SignalDbContext : DbContext
{
    public DbSet<Log> Logs { get; set; } = null!;
    public DbSet<Metric> Metrics { get; set; } = null!;
    public DbSet<Trace> Traces { get; set; } = null!;

    public SignalDbContext(DbContextOptions<SignalDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<SeverityNumber>();
        modelBuilder.HasPostgresEnum<OpenTelemetry.Proto.Metrics.V1.Metric.DataOneofCase>();
        modelBuilder.HasPostgresEnum<Span.Types.SpanKind>();
        modelBuilder.HasPostgresEnum<Status.Types.StatusCode>();
        base.OnModelCreating(modelBuilder);
    }
}