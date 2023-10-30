using System.Diagnostics;
using Google.Protobuf;

namespace DOTelL.DataAccess.Extensions;

public static class ByteStringExtensions
{
    public static string? ToTraceId(this ByteString? byteString)
    {
        if (byteString is null || byteString.IsEmpty)
        {
            return null;
        }

        return ActivityTraceId.CreateFromBytes(byteString.ToByteArray()).ToHexString();
    }
    
    public static string? ToSpanId(this ByteString? byteString)
    {
        if (byteString is null || byteString.IsEmpty)
        {
            return null;
        }

        return ActivitySpanId.CreateFromBytes(byteString.ToByteArray()).ToHexString();
    }
}