using OpenTelemetry.Proto.Common.V1;

namespace DOTelL.DataAccess.Extensions;

public static class AnyValueExtensions
{
    public static object? ExtractObject(this AnyValue? anyValue)
    {
        if (anyValue is null)
        {
            return null;
        }
        
        return anyValue.ValueCase switch
        {
            AnyValue.ValueOneofCase.None => null,
            AnyValue.ValueOneofCase.BoolValue => anyValue.BoolValue,
            AnyValue.ValueOneofCase.IntValue => anyValue.IntValue,
            AnyValue.ValueOneofCase.DoubleValue => anyValue.DoubleValue,
            AnyValue.ValueOneofCase.StringValue => anyValue.StringValue,
            AnyValue.ValueOneofCase.BytesValue => anyValue.BytesValue.ToBase64(),
            AnyValue.ValueOneofCase.ArrayValue => anyValue.ArrayValue.ExtractObject(),
            AnyValue.ValueOneofCase.KvlistValue => anyValue.KvlistValue.ExtractObject(),
            _ => null
        };
    }
}