using ParsingNumbers.Parsers;
using Xunit;

namespace ParsingNumbers.Tests;

public class NaiveParserTests
{
    [Fact]
    public void Parse_ShortInput()
    {
        var parser = new NaiveParser();

        Assert.Equal(Data.ShortExpected, parser.Parse(Data.ShortInput));
    }
}