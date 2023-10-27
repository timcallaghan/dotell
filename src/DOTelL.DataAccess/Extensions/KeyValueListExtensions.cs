using OpenTelemetry.Proto.Common.V1;

namespace DOTelL.DataAccess.Extensions;

public static class KeyValueListExtensions
{
    public static List<KeyValuePair<string, object?>> ExtractObject(this KeyValueList keyValueList)
    {
        var result = new List<KeyValuePair<string, object?>>();

        foreach (var keyValue in keyValueList.Values)
        {
            result.Add(new KeyValuePair<string, object?>(keyValue.Key, keyValue.Value.ExtractObject()));
        }

        return result;
    }
}