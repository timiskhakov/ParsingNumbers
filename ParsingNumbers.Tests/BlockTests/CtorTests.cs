using ParsingNumbers.Parsers;
using Xunit;

namespace ParsingNumbers.Tests.BlockTests;

public class CtorTests
{
    [Fact]
    public void ValidInput43690()
    {
        var block = new Block(43690);

        Assert.Equal(1, block.NumberSize);
        Assert.Equal(7, block.Amount);
    }

    [Fact]
    public void ValidInput21845()
    {
        var block = new Block(21845);

        Assert.Equal(1, block.NumberSize);
        Assert.Equal(8, block.Amount);
        Assert.Equal(16, block.Processed);
    }

    [Fact]
    public void ValidInput702()
    {
        var block = new Block(702);

        Assert.Equal(8, block.NumberSize);
        Assert.Equal(2, block.Amount);
    }

    [Fact]
    public void ValidInput28238()
    {
        var block = new Block(28238);

        Assert.Equal(4, block.NumberSize);
        Assert.Equal(4, block.Amount);
    }
}
