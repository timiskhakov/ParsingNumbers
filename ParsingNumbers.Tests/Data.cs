namespace ParsingNumbers.Tests;

internal static class Data
{
    internal const string ShortInput = "1,2,3,4,5,6,7,8,9";
    internal static uint[] ShortExpected = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    internal const string LongInput = "123,124,12,252,523,412,431,5,4263,51,412,423,6346,51,412,4313,5246,2651,3412,4";
    internal static uint[] LongExpected = { 123, 124, 12, 252, 523, 412, 431, 5, 4263, 51, 412, 423, 6346, 51, 412, 4313, 5246, 2651, 3412, 4 };
}