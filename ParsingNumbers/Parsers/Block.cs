using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using System.Runtime.Intrinsics;

namespace ParsingNumbers.Parsers;

public class Block
{
    private const int BlockSize = 16;

    public int NumberSize { get; }
    public int Amount { get; }
    public int Processed { get; }
    public Vector128<byte> Mask { get; }

    public Block(ushort value)
    {
        var (patterns, processed) = FindPatterns(value);
        if (patterns.Count == 0) return;

        NumberSize = CalculateSize(patterns);
        Amount = patterns.Count;
        Processed = processed;

        var bytes = new byte[BlockSize];
        for (var i = 0; i < patterns.Count; i++)
        {
            var padded = PadPattern(patterns[i]);
            padded.CopyTo(bytes, i * NumberSize);
        }

        Mask = Vector128.Create(bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7],
            bytes[8], bytes[9], bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15]);
    }

    private static (List<Pattern>, int) FindPatterns(ushort value)
    {
        var bits = new BitArray(BitConverter.GetBytes(value)).Cast<bool>().ToArray();

        var result = new List<Pattern>();
        var start = 0;
        var processed = 0;
        for (var i = 0; i < bits.Length; i++)
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
                result.Add(new Pattern(start, i - start));
            }

            if (!bits[i]) processed = i + 1;
        }

        return (result, processed);
    }

    private static int CalculateSize(List<Pattern> patterns)
    {
        var maxLength = patterns.Aggregate((x, y) => x.Length > y.Length ? x : y).Length;
        var numberSize = FindNextPowerOfTwo(maxLength);
        while (numberSize * patterns.Count > BlockSize)
        {
            patterns.RemoveAt(patterns.Count - 1);
            maxLength = patterns.Aggregate((x, y) => x.Length > y.Length ? x : y).Length;
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

    private byte[] PadPattern(Pattern pattern)
    {
        var result = new byte[NumberSize];

        var pad = NumberSize - pattern.Length;
        for (var i = 0; i < pad; i++)
        {
            result[i] = 0x80;
        }

        pattern.ToBytes().CopyTo(result, pad);

        return result;
    }
}
