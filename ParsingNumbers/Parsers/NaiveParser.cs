using System;
using System.Collections.Generic;

namespace ParsingNumbers.Parsers;

public class NaiveParser
{
    public uint[] Parse(string value)
    {
        if (string.IsNullOrEmpty(value)) return Array.Empty<uint>();

        var result = new List<uint>();
        int start = 0, end;
        while ((end = value.IndexOf(',', start)) != -1)
        {
            result.Add(uint.Parse(value.AsSpan(start, end - start)));
            start = end + 1;
        }

        result.Add(uint.Parse(value.AsSpan(start)));

        return result.ToArray();
    }
}