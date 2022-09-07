using BenchmarkDotNet.Running;

namespace ParsingNumbers;

internal static class Program
{
    private static void Main()
    {
        BenchmarkRunner.Run<Comparison>();
    }
}