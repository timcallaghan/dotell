using DOTelL.Api.GrpcServices;
using DOTelL.Api.Services;
using DOTelL.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.ConfigureDataAccess(builder.Configuration);
builder.Services.AddHostedService<DatabaseBootstrapService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<LogsService>().RequireHost("*:4317");
app.MapGrpcService<MetricsService>().RequireHost("*:4317");
app.MapGrpcService<TraceService>().RequireHost("*:4317");
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();