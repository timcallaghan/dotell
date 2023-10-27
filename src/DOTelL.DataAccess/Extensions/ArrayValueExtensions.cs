using OpenTelemetry.Proto.Common.V1;

namespace DOTelL.DataAccess.Extensions;

public static class ArrayValueExtensions
{
    public static List<object?> ExtractObject(this ArrayValue arrayValue)
    {
        var result = new List<object?>();

        foreach (var element in arrayValue.Values)
        {
            result.Add(element.ExtractObject());
        }

        return result;
    }
}