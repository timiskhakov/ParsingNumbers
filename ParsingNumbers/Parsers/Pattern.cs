namespace ParsingNumbers.Parsers;

public readonly struct Pattern
{
    public int Start { get; }
    public int Length { get; }

    public Pattern(int start, int length)
    {
        Start = start;
        Length = length;
    }

    public byte[] ToBytes()
    {
        var bytes = new byte[Length];
        var start = (byte)Start;
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = start++;
        }

        return bytes;
    }
}
