using Google.Protobuf.Collections;
using OpenTelemetry.Proto.Common.V1;

namespace DOTelL.DataAccess.Extensions;

public static class AttributeExtensions
{
    public static Dictionary<string, object?> ToAttributeDictionary(this RepeatedField<KeyValue> attributes)
    {
        var result = new Dictionary<string, object?>();

        foreach (var attribute in attributes)
        {
            result.Add(attribute.Key, attribute.Value.ExtractObject());
        }

        return result;
    }
}