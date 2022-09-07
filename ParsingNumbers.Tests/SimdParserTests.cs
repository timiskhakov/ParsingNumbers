using ParsingNumbers.Parsers;
using System.Text;
using Xunit;

namespace ParsingNumbers.Tests;

public class SimdParserTests
{
    [Fact]
    public void Parse_1DigitNumbers()
    {
        var parser = new SimdParser();

        var result = parser.Parse("1,2,3,4,5,6,7,8,9");

        Assert.Equal(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, result);
    }

    [Fact]
    public void Parse_2DigitNumbers()
    {
        var parser = new SimdParser();

        var result = parser.Parse("12,34,56,78,90,10");

        Assert.Equal(new uint[] { 12, 34, 56, 78, 90, 10 }, result);
    }

    [Fact]
    public void Parse_4DigitNumbers()
    {
        var parser = new SimdParser();

        var result = parser.Parse("1234,5678,9010,1230");

        Assert.Equal(new uint[] { 1234, 5678, 9010, 1230 }, result);
    }

    [Fact]
    public void Parse_8DigitNumbers()
    {
        var parser = new SimdParser();

        var result = parser.Parse("12345678,90101230,12345678,90101230");

        Assert.Equal(new uint[] { 12345678, 90101230, 12345678, 90101230 }, result);
    }

    [Fact]
    public void Parse_LongInput()
    {
        var parser = new SimdParser();

        var actual = parser.Parse(Data.LongInput);

        Assert.Equal(Data.LongExpected, actual);
    }

    [Fact]
    public void Parse_TenDigitsNumber()
    {
        var parser = new SimdParser();

        var actual = parser.Parse("1234567890,1234567890");

        Assert.Equal(new uint[] { 1234567890, 1234567890 }, actual);
    }

    [Fact]
    public void Parse_ManyNumbers()
    {
        var parser = new SimdParser();

        var actual = parser.Parse(CreateInput(1000000));

        Assert.Equal(CreateExpected(1000000), actual);
    }

    private static string CreateInput(int n)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < n; i++)
        {
            sb.Append(i).Append(',');
        }

        return sb.ToString(0, sb.Length - 1);
    }

    private static uint[] CreateExpected(int n)
    {
        var result = new uint[n];
        for (uint i = 0; i < n; i++)
        {
            result[i] = i;
        }

        return result;
    }
}
