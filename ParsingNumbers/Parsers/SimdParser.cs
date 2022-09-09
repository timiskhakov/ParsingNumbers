using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace ParsingNumbers.Parsers;

public class SimdParser
{
    private static readonly Vector128<byte> RawMask = Vector128.Create(
        0, 2, 4, 6, 8, 10, 12, 14,
        0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80);
    private static readonly Vector128<byte> Commas = Vector128.Create((byte)',');
    private static readonly Vector128<byte> Zeros = Vector128.Create((byte)'0');
    private static readonly Vector128<sbyte> Mul10 = Vector128.Create(10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1);
    private static readonly Vector128<short> Mul100 = Vector128.Create(100, 1, 100, 1, 100, 1, 100, 1);
    private static readonly Vector128<short> Mul10000 = Vector128.Create(10000, 1, 10000, 1, 10000, 1, 10000, 1);
    private static readonly Vector128<sbyte> ZerosAsSByte = Vector128.Create((byte)'0').AsSByte();
    private static readonly Vector128<sbyte> AfterNinesAsSByte = Vector128.Create((byte)((byte)'9' + 1)).AsSByte();
    private readonly Dictionary<int, Block> _blocks = new();
    private readonly SpanParser _spanParser = new();

    public SimdParser()
    {
        for (ushort i = 0; i < ushort.MaxValue; i++)
        {
            _blocks.Add(i, new Block(i));
        }
    }

    public unsafe uint[] Parse(string value)
    {
        if (string.IsNullOrEmpty(value)) return Array.Empty<uint>();

        var result = new uint[CountCommas(value) + 1];
        var parsed = 0;
        var counter = 0;
        fixed (char* c = value)
        {
            Span<uint> output = stackalloc uint[8];
            while (counter <= value.Length - 16)
            {
                var input = LoadInput(c + counter);
                var processed = ParseChunk(input, output, out var amount);
                for (var i = 0; i < amount; i++)
                {
                    result[parsed + i] = output[i];
                }

                parsed += amount;
                counter += processed;
            }
        }

        var spanParsed = _spanParser.Parse(value[counter..]);
        spanParsed.CopyTo(result, parsed);

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int ParseChunk(Vector128<byte> input, Span<uint> output, out int amount)
    {
        var t0 = Sse2.CompareLessThan(input.AsSByte(), ZerosAsSByte);
        var t1 = Sse2.CompareLessThan(input.AsSByte(), AfterNinesAsSByte);
        var andNot = Sse2.AndNot(t0, t1);
        var moveMask = Sse2.MoveMask(andNot);
        var block = _blocks[moveMask];
        var shuffled = Ssse3.Shuffle(input, block.Mask);

        switch(block.NumberSize)
        {
            case 1:
                ParseOneDigitNumbers(shuffled, block.Amount, output);
                break;
            case 2:
                ParseTwoDigitNumbers(shuffled, block.Amount, output);
                break;
            case 4:
                ParseFourDigitNumbers(shuffled, block.Amount, output);
                break;
            case 8:
                ParseEightDigitNumbers(shuffled, block.Amount, output);
                break;
            case 16:
                ParseSingeNumber(shuffled, output);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        amount = block.Amount;

        return block.Processed;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ParseOneDigitNumbers(Vector128<byte> vector, int amount, Span<uint> output)
    {
        var t0 = Sse2.SubtractSaturate(vector, Zeros);
        for (var i = 0; i < amount; i++)
        {
            output[i] = t0.GetElement(i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ParseTwoDigitNumbers(Vector128<byte> vector, int amount, Span<uint> output)
    {
        var t0 = Sse2.SubtractSaturate(vector, Zeros);
        var t1 = Ssse3.MultiplyAddAdjacent(t0, Mul10);
        for (var i = 0; i < amount; i++)
        {
            output[i] = (uint)t1.GetElement(i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ParseFourDigitNumbers(Vector128<byte> vector, int amount, Span<uint> output)
    {
        var t0 = Sse2.SubtractSaturate(vector, Zeros);
        var t1 = Ssse3.MultiplyAddAdjacent(t0, Mul10);
        var t2 = Sse2.MultiplyAddAdjacent(t1, Mul100);
        for (var i = 0; i < amount; i++)
        {
            output[i] = (uint)t2.GetElement(i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ParseEightDigitNumbers(Vector128<byte> vector, int amount, Span<uint> output)
    {
        var t0 = Sse2.SubtractSaturate(vector, Zeros);
        var t1 = Ssse3.MultiplyAddAdjacent(t0, Mul10);
        var t2 = Sse2.MultiplyAddAdjacent(t1, Mul100);
        var t3 = Sse41.PackUnsignedSaturate(t2, t2);
        var t4 = Sse2.MultiplyAddAdjacent(t3.AsInt16(), Mul10000);
        for (var i = 0; i < amount; i++)
        {
            output[i] = (uint)t4.GetElement(i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ParseSingeNumber(Vector128<byte> vector, Span<uint> output)
    {
        Span<char> chars = stackalloc char[16];
        for (var i = 0; i < chars.Length; i++)
        {
            chars[i] = (char) vector.GetElement(i);
        }
        
        var start = 0;
        for (var i = 0; i < chars.Length; i++)
        {
            if (chars[i] > '0' - 1) break;
            start++;
        }
        
        output[0] = uint.Parse(chars[start..]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe Vector128<byte> LoadInput(char* c)
    {
        var raw = Avx.LoadDquVector256((ushort*)c).AsByte();
        var lower = Ssse3.Shuffle(raw.GetLower(), RawMask);
        var upper = Ssse3.Shuffle(raw.GetUpper(), RawMask);
        return Vector128.Create(lower.GetLower(), upper.GetLower());
    }

    private static unsafe uint CountCommas(string value)
    {
        uint result = 0;
        var i = 0;
        fixed (char* c = value)
        {
            for (; i < value.Length - Vector256<ushort>.Count; i += Vector256<ushort>.Count)
            {
                var input = LoadInput(c + i);
                var match = Sse2.CompareEqual(Commas, input);
                var mask = Sse2.MoveMask(match);
                result += Popcnt.PopCount((uint)mask);
            }
        }

        for (; i < value.Length; i++)
        {
            if (value[i] == ',') result++;
        }

        return result;
    }
}