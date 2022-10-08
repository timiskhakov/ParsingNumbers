namespace ParsingNumbers.Parsers;

public readonly struct Span
{
    public byte Start { get; }
    public int Length { get; }

    public Span(byte start, int length)
    {
        Start = start;
        Length = length;
    }

    public byte[] ToBytes()
    {
        var bytes = new byte[Length];
        var start = Start;
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = start++;
        }

        return bytes;
    }
}
