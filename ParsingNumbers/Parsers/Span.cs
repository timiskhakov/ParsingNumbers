namespace ParsingNumbers.Parsers;

public readonly struct Span
{
    private readonly byte _start;
    public int Length { get; }

    public Span(byte start, int length)
    {
        _start = start;
        Length = length;
    }

    public byte[] ToBytes()
    {
        var bytes = new byte[Length];
        var start = _start;
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = start++;
        }

        return bytes;
    }
}
