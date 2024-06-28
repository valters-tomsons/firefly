namespace firefly.core.Domain;

public struct Range(uint Start, uint Length)
{
    public uint Start = Start;
    public uint Length = Length;

    public readonly bool Contains(uint Address, out uint Offset)
    {
        if (Address >= Start && Address < Start + Length)
        {
            Offset = Address - Start;
            return true;
        }

        Offset = uint.MaxValue;
        return false;
    }
}
