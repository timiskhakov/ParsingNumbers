using ParsingNumbers.Parsers;
using System.Runtime.Intrinsics;
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
        Assert.Equal(15, block.Processed);
        Assert.Equal(Vector128.Create((byte)1, 3, 5, 7, 9, 11, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0), block.Mask);
    }

    [Fact]
    public void ValidInput21845()
    {
        var block = new Block(21845);

        Assert.Equal(1, block.NumberSize);
        Assert.Equal(8, block.Amount);
        Assert.Equal(16, block.Processed);
        Assert.Equal(Vector128.Create((byte)0, 2, 4, 6, 8, 10, 12, 14, 0, 0, 0, 0, 0, 0, 0, 0), block.Mask);
    }

    [Fact]
    public void ValidInput702()
    {
        var block = new Block(702);

        Assert.Equal(8, block.NumberSize);
        Assert.Equal(2, block.Amount);
        Assert.Equal(16, block.Processed);
        Assert.Equal(Vector128.Create(128, 128, 128, 1, 2, 3, 4, 5, 128, 128, 128, 128, 128, 128, 128, 7), block.Mask);
    }

    [Fact]
    public void ValidInput28238()
    {
        var block = new Block(28238);

        Assert.Equal(4, block.NumberSize);
        Assert.Equal(4, block.Amount);
        Assert.Equal(16, block.Processed);
        Assert.Equal(Vector128.Create(128, 1, 2, 3, 128, 128, 128, 6, 128, 9, 10, 11, 128, 128, 13, 14), block.Mask);
    }
}
