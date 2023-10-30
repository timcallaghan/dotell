using TelemetryGenerator;
using TelemetryGenerator.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.ConfigureOTel();
builder.Services.AddHttpClient();
builder.Services.Configure<ExternalApiOptions>(builder.Configuration.GetSection(ExternalApiOptions.SectionName));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();