using ParsingNumbers.Parsers;
using Xunit;

namespace ParsingNumbers.Tests;

public class OptimizedParserTests
{
    private readonly OptimizedParser _parser = new();

    [Fact]
    public void Parse_1DigitNumbers()
    {
        Assert.Equal(Data.Expected1DigitNumbers, _parser.Parse(Data.Input1DigitNumbers));
    }

    [Fact]
    public void Parse_2DigitNumbers()
    {
        Assert.Equal(Data.Expected2DigitNumbers, _parser.Parse(Data.Input2DigitNumbers));
    }

    [Fact]
    public void Parse_3DigitNumbers()
    {
        Assert.Equal(Data.Expected3DigitNumbers, _parser.Parse(Data.Input3DigitNumbers));
    }

    [Fact]
    public void Parse_4DigitNumbers()
    {
        Assert.Equal(Data.Expected4DigitNumbers, _parser.Parse(Data.Input4DigitNumbers));
    }

    [Fact]
    public void Parse_5DigitNumbers()
    {
        Assert.Equal(Data.Expected5DigitNumbers, _parser.Parse(Data.Input5DigitNumbers));
    }

    [Fact]
    public void Parse_6DigitNumbers()
    {
        Assert.Equal(Data.Expected6DigitNumbers, _parser.Parse(Data.Input6DigitNumbers));
    }

    [Fact]
    public void Parse_7DigitNumbers()
    {
        Assert.Equal(Data.Expected7DigitNumbers, _parser.Parse(Data.Input7DigitNumbers));
    }

    [Fact]
    public void Parse_8DigitNumbers()
    {
        Assert.Equal(Data.Expected8DigitNumbers, _parser.Parse(Data.Input8DigitNumbers));
    }

    [Fact]
    public void Parse_9DigitNumbers()
    {
        Assert.Equal(Data.Expected9DigitNumbers, _parser.Parse(Data.Input9DigitNumbers));
    }

    [Fact]
    public void Parse_10DigitNumbers()
    {
        Assert.Equal(Data.Expected10DigitNumbers, _parser.Parse(Data.Input10DigitNumbers));
    }
    
    [Fact]
    public void InputVariousNumbers1()
    {
        Assert.Equal(Data.ExpectedVariousNumbers1, _parser.Parse(Data.InputVariousNumbers1));
    }

    [Fact]
    public void InputVariousNumbers2()
    {
        Assert.Equal(Data.ExpectedVariousNumbers2, _parser.Parse(Data.InputVariousNumbers2));
    }

    [Fact]
    public void Parse_ManyNumbers()
    {
        Assert.Equal(Data.ExpectedManyNumbers, _parser.Parse(Data.InputManyNumbers));
    }

    [Fact]
    public void Parse_MillionVariousNumbers()
    {
        Assert.Equal(Data.CreateExpected(1_000_000), _parser.Parse(Data.CreateInput(1_000_000)));
    }
}