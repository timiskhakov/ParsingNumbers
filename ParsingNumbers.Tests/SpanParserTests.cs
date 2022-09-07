using ParsingNumbers.Parsers;
using Xunit;

namespace ParsingNumbers.Tests;

public class SpanParserTests
{
    [Fact]
    public void Parse_ShortInput()
    {
        var parser = new SpanParser();

        Assert.Equal(Data.ShortExpected, parser.Parse(Data.ShortInput));
    }
}
