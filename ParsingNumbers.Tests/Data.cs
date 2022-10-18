using System.Text;

namespace ParsingNumbers.Tests;

internal static class Data
{
    internal const string Input1DigitNumbers = "1,2,3,4,5,6,7,8,9";
    internal static readonly uint[] Expected1DigitNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    internal const string Input2DigitNumbers = "12,34,56,78,90,10";
    internal static readonly uint[] Expected2DigitNumbers = { 12, 34, 56, 78, 90, 10 };

    internal const string Input3DigitNumbers = "123,321,633,364,142,745,235,363,124,534,734,235";
    internal static readonly uint[] Expected3DigitNumbers = { 123, 321, 633, 364, 142, 745, 235, 363, 124, 534, 734, 235 };

    internal const string Input4DigitNumbers = "1234,5678,9010,1230";
    internal static readonly uint[] Expected4DigitNumbers = { 1234, 5678, 9010, 1230 };

    internal const string Input5DigitNumbers = "12345,56789,90109,12300";
    internal static readonly uint[] Expected5DigitNumbers = { 12345, 56789, 90109, 12300 };

    internal const string Input6DigitNumbers = "123456,123456,456789,456789,345678,234567";
    internal static readonly uint[] Expected6DigitNumbers = { 123456, 123456, 456789, 456789, 345678, 234567 };

    internal const string Input7DigitNumbers = "1234567,2345678,3456789,4567890";
    internal static readonly uint[] Expected7DigitNumbers = { 1234567, 2345678, 3456789, 4567890 };

    internal const string Input8DigitNumbers = "12345678,90101230,12345678,90101230";
    internal static readonly uint[] Expected8DigitNumbers = { 12345678, 90101230, 12345678, 90101230 };

    internal const string Input9DigitNumbers = "123456789,123456789";
    internal static readonly uint[] Expected9DigitNumbers = { 123456789, 123456789 };

    internal const string Input10DigitNumbers = "1234567890,1234567890";
    internal static readonly uint[] Expected10DigitNumbers = { 1234567890, 1234567890 };

    internal const string InputVariousNumbers1 = "1,2,3,4,123456,1";
    internal static readonly uint[] ExpectedVariousNumbers1 = { 1, 2, 3, 4, 123456, 1 };

    internal const string InputVariousNumbers2 = "213141241,12,636,123,35,123,213141241,856,123";
    internal static readonly uint[] ExpectedVariousNumbers2 = { 213141241, 12, 636, 123, 35, 123, 213141241, 856, 123 };

    internal const string InputManyNumbers = "123,124,12,252,523,412,431,5,4263,51,412,423,6346,51,412,4313,5246,2651,3412,4,12345678";
    internal static readonly uint[] ExpectedManyNumbers = { 123, 124, 12, 252, 523, 412, 431, 5, 4263, 51, 412, 423, 6346, 51, 412, 4313, 5246, 2651, 3412, 4, 12345678 };

    internal static string CreateInput(int n)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < n; i++)
        {
            sb.Append(i).Append(',');
        }

        return sb.ToString(0, sb.Length - 1);
    }

    internal static uint[] CreateExpected(int n)
    {
        var result = new uint[n];
        for (uint i = 0; i < n; i++)
        {
            result[i] = i;
        }

        return result;
    }
}