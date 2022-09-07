using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace ParsingNumbers.Parsers;

public class SimdParser
{
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
        var b = stackalloc byte[16];
        while (counter <= value.Length - 16)
        {
            for (var i = 0; i < 16; i++)
            {
                b[i] = (byte)value[counter + i];
            }
            var (chunk, processed) = ParseChunk(b);

            chunk.CopyTo(result, parsed);

            parsed += chunk.Length;
            counter += processed;
        }

        var spanParsed = _spanParser.Parse(value[counter..]);
        spanParsed.CopyTo(result, parsed);

        return result;
    }

    private unsafe (uint[], int) ParseChunk(byte* b)
    {
        var input = Sse3.LoadDquVector128(b);
        var t0 = Sse2.CompareLessThan(input.AsSByte(), ZerosAsSByte);
        var t1 = Sse2.CompareLessThan(input.AsSByte(), AfterNinesAsSByte);
        var andNot = Sse2.AndNot(t0, t1);
        var moveMask = (ushort)Sse2.MoveMask(andNot);

        var block = _blocks[moveMask];
        var shuffled = Ssse3.Shuffle(input, block.Mask);

        uint[] result = block.NumberSize switch
        {
            1 => ParseOneDigitNumbers(shuffled, block.Amount),
            2 => ParseTwoDigitNumbers(shuffled, block.Amount),
            4 => ParseFourDigitNumbers(shuffled, block.Amount),
            8 => ParseEightDigitNumbers(shuffled, block.Amount),
            16 => ParseSixteenDigitNumbers(shuffled, block.Amount),
            _ => throw new ArgumentOutOfRangeException()
        };

        return (result, block.Processed);
    }

    private static uint[] ParseOneDigitNumbers(Vector128<byte> vector, int amount)
    {
        var result = new uint[amount];
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = GetElement(vector, i);
        }

        return result;
    }

    private static uint[] ParseTwoDigitNumbers(Vector128<byte> vector, int amount)
    {
        var numbers = new uint[amount];
        for (var i = 0; i < numbers.Length; i++)
        {
            numbers[i] = 10 * GetElement(vector, i * 2) + GetElement(vector, i * 2 + 1);
        }

        return numbers;
    }

    private static uint[] ParseFourDigitNumbers(Vector128<byte> vector, int amount)
    {
        var numbers = new uint[amount];
        for (var i = 0; i < numbers.Length; i++)
        {
            numbers[i] =
                1000 * GetElement(vector, i * 4) +
                100 * GetElement(vector, i * 4 + 1) +
                10 * GetElement(vector, i * 4 + 2) +
                GetElement(vector, i * 4 + 3);
        }

        return numbers;
    }

    private static uint[] ParseEightDigitNumbers(Vector128<byte> vector, int amount)
    {
        var numbers = new uint[amount];
        for (var i = 0; i < numbers.Length; i++)
        {
            numbers[i] =
                10000000 * GetElement(vector, i * 8) +
                1000000 * GetElement(vector, i * 8 + 1) +
                100000 * GetElement(vector, i * 8 + 2) +
                10000 * GetElement(vector, i * 8 + 3) +
                1000 * GetElement(vector, i * 8 + 4) +
                100 * GetElement(vector, i * 8 + 5) +
                10 * GetElement(vector, i * 8 + 6) +
                GetElement(vector, i * 8 + 7);
        }

        return numbers;
    }

    private static uint[] ParseSixteenDigitNumbers(Vector128<byte> vector, int amount)
    {
        var numbers = new uint[amount];
        for (var i = 0; i < numbers.Length; i++)
        {
            numbers[i] =
                1000000000 * GetElement(vector, i * 10 + 6) +
                100000000 * GetElement(vector, i * 10 + 7) +
                10000000 * GetElement(vector, i * 10 + 8) +
                1000000 * GetElement(vector, i * 10 + 9) +
                100000 * GetElement(vector, i * 10 + 10) +
                10000 * GetElement(vector, i * 10 + 11) +
                1000 * GetElement(vector, i * 10 + 12) +
                100 * GetElement(vector, i * 10 + 13) +
                10 * GetElement(vector, i * 10 + 14) +
                GetElement(vector, i * 10 + 15);
        }

        return numbers;
    }

    private static uint GetElement(Vector128<byte> vector, int position)
    {
        var element = vector.GetElement(position);
        return (uint) (element > 0 ? element - (byte)'0' : 0);
    }

    private static unsafe int CountCommas(string value)
    {
        var result = 0;

        var i = 0;
        fixed (char* p = value)
        {
            var comma = Vector256.Create(',');
            for (; i < value.Length - Vector256<ushort>.Count; i += Vector256<ushort>.Count)
            {
                var block = Avx.LoadVector256((ushort*)p + i);
                var match = Avx2.CompareEqual(comma, block);
                var mask = RemoveOddBits(Avx2.MoveMask(match.AsByte()));
                result += (int)Popcnt.PopCount((uint)mask);
            }
        }

        for (; i < value.Length; i++)
        {
            if (value[i] == ',') result++;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int RemoveOddBits(int n)
    {
        n = ((n & 0x44444444) >> 1) | (n & 0x11111111);
        n = ((n & 0x30303030) >> 2) | (n & 0x03030303);
        n = ((n & 0x0F000F00) >> 4) | (n & 0x000F000F);
        n = ((n & 0x00FF0000) >> 8) | (n & 0x000000FF);
        return n;
    }
}