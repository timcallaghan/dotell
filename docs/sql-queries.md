# Sample SQL Queries

## Distinct resources

Find the distinct set of unique resources across all traces.

```sql
SELECT DISTINCT "Resource"->'service.name' AS "ServiceName" FROM "Traces"
```

## Root spans

Find the root span (trace entry point) for each unique traceId.

```sql
SELECT 
  "Resource"->'service.name' AS "ServiceName",
  "TraceId",
  "SpanId",
  "Name"
FROM "Traces"
WHERE "ParentSpanId" IS NULL
```

## Ordered spans within a trace

Required:
* `TraceId`

The following CTE query will resolve the root trace span for the given `TraceId` and then do a depth-first traversal of the trace graph.

```sql
WITH RECURSIVE trace_spans(Id, TraceId, SpanId, ParentSpanId, path) AS (
    SELECT "Id", "TraceId", "SpanId", "ParentSpanId", ARRAY[t."Id"] 
    FROM "Traces" t 
    WHERE "TraceId" = 'd1ecc067e485922cdf2d647621370b12' AND "ParentSpanId" IS NULL
  UNION ALL
    SELECT t."Id", t."TraceId", t."SpanId", t."ParentSpanId", path || t."Id"
    FROM trace_spans ts, "Traces" t
    WHERE t."ParentSpanId" = ts.SpanId
)
SELECT ts.path, t.*
FROM trace_spans ts
INNER JOIN "Traces" t
ON ts.TraceId = t."TraceId"
  AND ts.SpanId = t."SpanId"
ORDER BY ts.path, t."StartTimeUnixNano"
```