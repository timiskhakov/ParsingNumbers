using BenchmarkDotNet.Attributes;
using ParsingNumbers.Parsers;
using System.Collections.Generic;
using System.Text;

namespace ParsingNumbers;

[MemoryDiagnoser]
public class Comparison
{
    private NaiveParser _naiveParser = null!;
    private OptimizedParser _optimizedParser = null!;
    private SimdParser _simdParser = null!;

    public static IEnumerable<string> Inputs() => new[]
    {
        "123456789",
        "123456789,987654321",
        CreateInput(100),
        CreateInput(10000),
        CreateInput(1000000),
    };

    private static string CreateInput(int n)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < n; i++)
        {
            sb.Append(i).Append(',');
        }

        return sb.ToString(0, sb.Length - 1);
    }

    [GlobalSetup]
    public void Setup()
    {
        _naiveParser = new NaiveParser();
        _optimizedParser = new OptimizedParser();
        _simdParser = new SimdParser();
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Inputs))]
    public void Naive(string value)
    {
        _naiveParser.Parse(value);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Inputs))]
    public void Optimized(string value)
    {
        _optimizedParser.Parse(value);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Inputs))]
    public void Simd(string value)
    {
        _simdParser.Parse(value);
    }
}