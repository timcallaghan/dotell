using Google.Protobuf.Collections;
using DOTelL.DataAccess.Models.MetricData;

namespace DOTelL.DataAccess.Extensions;

public static class MetricExtensions
{
    public static object? ExtractData(this OpenTelemetry.Proto.Metrics.V1.Metric metric)
    {
        return metric.DataCase switch
        {
            OpenTelemetry.Proto.Metrics.V1.Metric.DataOneofCase.None => null,
            OpenTelemetry.Proto.Metrics.V1.Metric.DataOneofCase.Gauge => metric.Gauge.ExtractData(),
            OpenTelemetry.Proto.Metrics.V1.Metric.DataOneofCase.Sum => metric.Sum.ExtractData(),
            OpenTelemetry.Proto.Metrics.V1.Metric.DataOneofCase.Histogram => metric.Histogram.ExtractData(),
            OpenTelemetry.Proto.Metrics.V1.Metric.DataOneofCase.ExponentialHistogram => metric.ExponentialHistogram.ExtractData(),
            OpenTelemetry.Proto.Metrics.V1.Metric.DataOneofCase.Summary => metric.Summary.ExtractData(),
            _ => null
        };
    }

    private static Gauge ExtractData(this OpenTelemetry.Proto.Metrics.V1.Gauge gauge)
    {
        return new Gauge
        {
            DataPoints = gauge.DataPoints.Select(o => o.ExtractData()).ToList()
        };
    }
    
    private static Sum ExtractData(this OpenTelemetry.Proto.Metrics.V1.Sum sum)
    {
        return new Sum
        {
            DataPoints = sum.DataPoints.Select(o => o.ExtractData()).ToList(),
            AggregationTemporality = sum.AggregationTemporality,
            IsMonotonic = sum.IsMonotonic
        };
    }
    
    private static Histogram ExtractData(this OpenTelemetry.Proto.Metrics.V1.Histogram histogram)
    {
        return new Histogram
        {
            DataPoints = histogram.DataPoints.Select(o => o.ExtractData()).ToList(),
            AggregationTemporality = histogram.AggregationTemporality,
        };
    }
    
    private static ExponentialHistogram ExtractData(this OpenTelemetry.Proto.Metrics.V1.ExponentialHistogram exponentialHistogram)
    {
        return new ExponentialHistogram
        {
            DataPoints = exponentialHistogram.DataPoints.Select(o => o.ExtractData()).ToList(),
            AggregationTemporality = exponentialHistogram.AggregationTemporality,
        };
    }
    
    private static Summary ExtractData(this OpenTelemetry.Proto.Metrics.V1.Summary summary)
    {
        return new Summary
        {
            DataPoints = summary.DataPoints.Select(o => o.ExtractData()).ToList()
        };
    }

    private static NumberDataPoint ExtractData(this OpenTelemetry.Proto.Metrics.V1.NumberDataPoint numberDataPoint)
    {
        return new NumberDataPoint
        {
            Attributes = numberDataPoint.Attributes?.ToAttributeDictionary(),
            StartTimeUnixNano = numberDataPoint.StartTimeUnixNano,
            TimeUnixNano = numberDataPoint.TimeUnixNano,
            Value = numberDataPoint.ExtractValue(),
            Exemplars = numberDataPoint.Exemplars?.ExtractData(),
            Flags = numberDataPoint.Flags
        };
    }
    
    private static HistogramDataPoint ExtractData(this OpenTelemetry.Proto.Metrics.V1.HistogramDataPoint histogramDataPoint)
    {
        return new HistogramDataPoint
        {
            Attributes = histogramDataPoint.Attributes?.ToAttributeDictionary(),
            StartTimeUnixNano = histogramDataPoint.StartTimeUnixNano,
            TimeUnixNano = histogramDataPoint.TimeUnixNano,
            Count = histogramDataPoint.Count,
            Sum = histogramDataPoint.Sum,
            BucketCounts = histogramDataPoint.BucketCounts?.Select(o => o).ToList(),
            ExplicitBounds = histogramDataPoint.ExplicitBounds?.Select(o => o).ToList(),
            Exemplars = histogramDataPoint.Exemplars?.ExtractData(),
            Flags = histogramDataPoint.Flags,
            Min = histogramDataPoint.Min,
            Max = histogramDataPoint.Max
        };
    }
    
    private static ExponentialHistogramDataPoint ExtractData(this OpenTelemetry.Proto.Metrics.V1.ExponentialHistogramDataPoint histogramDataPoint)
    {
        return new ExponentialHistogramDataPoint
        {
            Attributes = histogramDataPoint.Attributes?.ToAttributeDictionary(),
            StartTimeUnixNano = histogramDataPoint.StartTimeUnixNano,
            TimeUnixNano = histogramDataPoint.TimeUnixNano,
            Count = histogramDataPoint.Count,
            Sum = histogramDataPoint.Sum,
            Scale = histogramDataPoint.Scale,
            ZeroCount = histogramDataPoint.ZeroCount,
            Positive = histogramDataPoint.Positive?.ExtractData(),
            Negative = histogramDataPoint.Negative?.ExtractData(),
            Flags = histogramDataPoint.Flags,
            Exemplars = histogramDataPoint.Exemplars?.ExtractData(),
            Min = histogramDataPoint.Min,
            Max = histogramDataPoint.Max,
            ZeroThreshold = histogramDataPoint.ZeroThreshold
        };
    }

    private static object? ExtractValue(this OpenTelemetry.Proto.Metrics.V1.NumberDataPoint numberDataPoint)
    {
        return numberDataPoint.ValueCase switch
        {
            OpenTelemetry.Proto.Metrics.V1.NumberDataPoint.ValueOneofCase.AsDouble => numberDataPoint.AsDouble,
            OpenTelemetry.Proto.Metrics.V1.NumberDataPoint.ValueOneofCase.AsInt => numberDataPoint.AsInt,
            _ => null
        };
    }

    private static List<Exemplar>? ExtractData(this RepeatedField<OpenTelemetry.Proto.Metrics.V1.Exemplar>? exemplars)
    {
        if (exemplars is null)
        {
            return null;
        }

        return exemplars.Select(o => new Exemplar
        {
            FilteredAttributes = o.FilteredAttributes?.ToAttributeDictionary(),
            TimeUnixNano = o.TimeUnixNano,
            Value = o.ExtractData(),
            SpanId = o.SpanId?.ToBase64(),
            TraceId = o.TraceId?.ToBase64()
        }).ToList();
    }
    
    private static object? ExtractData(this OpenTelemetry.Proto.Metrics.V1.Exemplar exemplar)
    {
        return exemplar.ValueCase switch
        {
            OpenTelemetry.Proto.Metrics.V1.Exemplar.ValueOneofCase.AsDouble => exemplar.AsDouble,
            OpenTelemetry.Proto.Metrics.V1.Exemplar.ValueOneofCase.AsInt => exemplar.AsInt,
            _ => null
        };
    }

    private static Buckets ExtractData(
        this OpenTelemetry.Proto.Metrics.V1.ExponentialHistogramDataPoint.Types.Buckets buckets)
    {
        return new Buckets
        {
            Offset = buckets.Offset,
            BucketCounts = buckets.BucketCounts.Select(o => o).ToList()
        };
    }

    private static SummaryDataPoint ExtractData(this OpenTelemetry.Proto.Metrics.V1.SummaryDataPoint summaryDataPoint)
    {
        return new SummaryDataPoint
        {
            Attributes = summaryDataPoint.Attributes?.ToAttributeDictionary(),
            StartTimeUnixNano = summaryDataPoint.StartTimeUnixNano,
            TimeUnixNano = summaryDataPoint.TimeUnixNano,
            Count = summaryDataPoint.Count,
            Sum = summaryDataPoint.Sum,
            QuantileValues = summaryDataPoint.QuantileValues?.Select(o => o.ExtractData()).ToList(),
            Flags = summaryDataPoint.Flags
        };
    }

    private static ValueAtQuantile ExtractData(
        this OpenTelemetry.Proto.Metrics.V1.SummaryDataPoint.Types.ValueAtQuantile valueAtQuantile)
    {
        return new ValueAtQuantile
        {
            Quantile = valueAtQuantile.Quantile,
            Value = valueAtQuantile.Value
        };
    }
}