namespace DOTelL.DataAccess.Services;

public interface IDatabaseBootstrapper
{
    Task EnsureCreated(SignalDbContext dbContext);
}