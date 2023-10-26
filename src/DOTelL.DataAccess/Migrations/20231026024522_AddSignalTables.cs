using Google.Protobuf.Collections;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OpenTelemetry.Proto.Common.V1;
using OpenTelemetry.Proto.Logs.V1;
using OpenTelemetry.Proto.Metrics.V1;
using OpenTelemetry.Proto.Resource.V1;
using OpenTelemetry.Proto.Trace.V1;

#nullable disable

namespace DOTelL.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSignalTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:data_oneof_case", "none,gauge,sum,histogram,exponential_histogram,summary")
                .Annotation("Npgsql:Enum:severity_number", "unspecified,trace,trace2,trace3,trace4,debug,debug2,debug3,debug4,info,info2,info3,info4,warn,warn2,warn3,warn4,error,error2,error3,error4,fatal,fatal2,fatal3,fatal4")
                .Annotation("Npgsql:Enum:span_kind", "unspecified,internal,server,client,producer,consumer")
                .Annotation("Npgsql:Enum:status_code", "unset,ok,error");

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Resource = table.Column<Resource>(type: "jsonb", nullable: false),
                    ResourceSchemaUrl = table.Column<string>(type: "text", nullable: true),
                    Scope = table.Column<InstrumentationScope>(type: "jsonb", nullable: true),
                    SchemaUrl = table.Column<string>(type: "text", nullable: true),
                    TimeUnixNano = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ObservedTimeUnixNano = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    SeverityNumber = table.Column<SeverityNumber>(type: "severity_number", nullable: true),
                    SeverityText = table.Column<string>(type: "text", nullable: true),
                    Body = table.Column<AnyValue>(type: "jsonb", nullable: true),
                    Attributes = table.Column<RepeatedField<KeyValue>>(type: "jsonb", nullable: true),
                    Flags = table.Column<long>(type: "bigint", nullable: true),
                    TraceId = table.Column<string>(type: "text", nullable: true),
                    SpanId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Resource = table.Column<Resource>(type: "jsonb", nullable: false),
                    ResourceSchemaUrl = table.Column<string>(type: "text", nullable: true),
                    Scope = table.Column<InstrumentationScope>(type: "jsonb", nullable: true),
                    SchemaUrl = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    MetricType = table.Column<Metric.DataOneofCase>(type: "data_oneof_case", nullable: false),
                    Gauge = table.Column<Gauge>(type: "jsonb", nullable: true),
                    Sum = table.Column<Sum>(type: "jsonb", nullable: true),
                    Histogram = table.Column<Histogram>(type: "jsonb", nullable: true),
                    ExponentialHistogram = table.Column<ExponentialHistogram>(type: "jsonb", nullable: true),
                    Summary = table.Column<Summary>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Traces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Resource = table.Column<Resource>(type: "jsonb", nullable: false),
                    ResourceSchemaUrl = table.Column<string>(type: "text", nullable: true),
                    Scope = table.Column<InstrumentationScope>(type: "jsonb", nullable: true),
                    SchemaUrl = table.Column<string>(type: "text", nullable: true),
                    TraceId = table.Column<string>(type: "text", nullable: false),
                    SpanId = table.Column<string>(type: "text", nullable: false),
                    TraceState = table.Column<string>(type: "text", nullable: true),
                    ParentSpanId = table.Column<string>(type: "text", nullable: true),
                    Flags = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Kind = table.Column<Span.Types.SpanKind>(type: "span_kind", nullable: false),
                    StartTimeUnixNano = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    EndTimeUnixNano = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Attributes = table.Column<RepeatedField<KeyValue>>(type: "jsonb", nullable: true),
                    Events = table.Column<RepeatedField<Span.Types.Event>>(type: "jsonb", nullable: true),
                    Links = table.Column<RepeatedField<Span.Types.Link>>(type: "jsonb", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<Status.Types.StatusCode>(type: "status_code", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traces", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Metrics");

            migrationBuilder.DropTable(
                name: "Traces");
        }
    }
}
