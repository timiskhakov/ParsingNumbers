using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using System.Runtime.Intrinsics;

namespace ParsingNumbers.Parsers;

public class Pattern
{
    private const int InputSize = 16;

    public int NumberSize { get; }
    public int Amount { get; }
    public int Processed { get; }
    public byte[] Array { get; }
    public Vector128<byte> Mask { get; }

    public Pattern(ushort value)
    {
        if (value == 0) return;

        var spans = FindSpans(value).ToList();
        NumberSize = CalculateNumberSize(spans);
        Amount = spans.Count;
        Processed = CalculateProcessed(value, spans.Last());

        var bytes = new byte[InputSize];
        for (var i = 0; i < spans.Count; i++)
        {
            var padded = Pad(spans[i]);
            padded.CopyTo(bytes, i * NumberSize);
        }

        Array = bytes;

        Mask = Vector128.Create(
            bytes[0], bytes[1], bytes[2], bytes[3],
            bytes[4], bytes[5], bytes[6], bytes[7],
            bytes[8], bytes[9], bytes[10], bytes[11],
            bytes[12], bytes[13], bytes[14], bytes[15]);
    }

    private static IEnumerable<Span> FindSpans(ushort value)
    {
        var bits = new BitArray(BitConverter.GetBytes(value)).Cast<bool>().ToArray();

        byte start = 0;
        for (byte i = 0; i < bits.Length; i++)
        {
            if (i == 0)
            {
                if (bits[i]) start = i;
                continue;
            }

            if (bits[i] && !bits[i - 1])
            {
                start = i;
                continue;
            }

            if (!bits[i] && bits[i - 1])
            {
                yield return new Span(start, i - start);
            }
        }
    }

    private static int CalculateNumberSize(IList<Span> spans)
    {
        var maxLength = spans.Aggregate((x, y) => x.Length > y.Length ? x : y).Length;
        var numberSize = FindNextPowerOfTwo(maxLength);
        while (numberSize * spans.Count > InputSize)
        {
            spans.RemoveAt(spans.Count - 1);
            maxLength = spans.Aggregate((x, y) => x.Length > y.Length ? x : y).Length;
            numberSize = FindNextPowerOfTwo(maxLength);
        }

        return numberSize;
    }

    private static int FindNextPowerOfTwo(int v)
    {
        v--;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 16;
        v++;

        return v;
    }

    private static int CalculateProcessed(ushort value, Span lastSpan)
    {
        var bits = new BitArray(BitConverter.GetBytes(value)).Cast<bool>().ToArray();
        for (var i = lastSpan.Start + lastSpan.Length; i < bits.Length; i++)
        {
            if (bits[i]) return i;
        }

        return bits.Length;
    }
    
    private byte[] Pad(Span span)
    {
        var result = new byte[NumberSize];

        var pad = NumberSize - span.Length;
        for (var i = 0; i < pad; i++)
        {
            result[i] = 0x80;
        }

        span.ToBytes().CopyTo(result, pad);

        return result;
    }
}
