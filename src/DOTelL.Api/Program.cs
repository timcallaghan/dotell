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
app.MapGrpcService<LogsService>();
app.MapGrpcService<MetricsService>();
app.MapGrpcService<TraceService>();
app.MapGet("/",
    () =>
        "DOTelL API currently only only supports OTLP gRPC endpoints over HTTP/2 connections.");

app.Run();