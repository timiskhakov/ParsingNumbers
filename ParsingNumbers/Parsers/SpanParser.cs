using System;

namespace ParsingNumbers.Parsers;

public class SpanParser
{
    public uint[] Parse(string value)
    {
        if (string.IsNullOrEmpty(value)) return Array.Empty<uint>();

        var commaCount = 0;
        foreach (var v in value)
        {
            if (v == ',') commaCount++;
        }

        var result = new uint[commaCount + 1];
        var start = 0;
        for (var i = 0; i < result.Length - 1; i++)
        {
            var end = start;
            while (value[end] != ',') end++;
            result[i] = uint.Parse(value.AsSpan(start, end - start));
            start = end + 1;
        }

        result[^1] = uint.Parse(value.AsSpan(start));

        return result;
    }
}
