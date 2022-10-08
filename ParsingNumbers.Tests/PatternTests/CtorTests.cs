using ParsingNumbers.Parsers;
using System.Runtime.Intrinsics;
using Xunit;

namespace ParsingNumbers.Tests.PatternTests;

public class CtorTests
{
    [Fact]
    public void ValidInput43690()
    {
        var pattern = new Pattern(43690);

        Assert.Equal(1, pattern.NumberSize);
        Assert.Equal(7, pattern.Amount);
        Assert.Equal(15, pattern.Processed);
        Assert.Equal(Vector128.Create((byte)1, 3, 5, 7, 9, 11, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0), pattern.Mask);
    }

    [Fact]
    public void ValidInput21845()
    {
        var pattern = new Pattern(21845);

        Assert.Equal(1, pattern.NumberSize);
        Assert.Equal(8, pattern.Amount);
        Assert.Equal(16, pattern.Processed);
        Assert.Equal(Vector128.Create((byte)0, 2, 4, 6, 8, 10, 12, 14, 0, 0, 0, 0, 0, 0, 0, 0), pattern.Mask);
    }

    [Fact]
    public void ValidInput702()
    {
        var pattern = new Pattern(702);

        Assert.Equal(8, pattern.NumberSize);
        Assert.Equal(2, pattern.Amount);
        Assert.Equal(9, pattern.Processed);
        Assert.Equal(Vector128.Create(128, 128, 128, 1, 2, 3, 4, 5, 128, 128, 128, 128, 128, 128, 128, 7), pattern.Mask);
    }

    [Fact]
    public void ValidInput28238()
    {
        var pattern = new Pattern(28238);

        Assert.Equal(4, pattern.NumberSize);
        Assert.Equal(4, pattern.Amount);
        Assert.Equal(16, pattern.Processed);
        Assert.Equal(Vector128.Create(128, 1, 2, 3, 128, 128, 128, 6, 128, 9, 10, 11, 128, 128, 13, 14), pattern.Mask);
    }

    [Fact]
    public void ValidInput43772()
    {
        var pattern = new Pattern(43772);
        
        Assert.Equal(8, pattern.NumberSize);
        Assert.Equal(2, pattern.Amount);
        Assert.Equal(11, pattern.Processed);
        Assert.Equal(Vector128.Create(128, 128, 2, 3, 4, 5, 6, 7, 128, 128, 128, 128, 128, 128, 128, 9), pattern.Mask);
    }
    
    [Fact]
    public void ValidInput16213()
    {
        var pattern = new Pattern(16213);
        
        Assert.Equal(1, pattern.NumberSize);
        Assert.Equal(4, pattern.Amount);
        Assert.Equal(8, pattern.Processed);
        Assert.Equal(Vector128.Create((byte)0, 2, 4, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), pattern.Mask);
    }

    [Fact]
    public void ValidInput32597()
    {
        var pattern = new Pattern(32597);

        Assert.Equal(1, pattern.NumberSize);
        Assert.Equal(4, pattern.Amount);
        Assert.Equal(8, pattern.Processed);
        Assert.Equal(Vector128.Create((byte)0, 2, 4, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), pattern.Mask);
    }
}
