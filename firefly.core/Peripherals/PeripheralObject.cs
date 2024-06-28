namespace firefly.core.Peripherals;

public class PeripheralObject
{
    public Domain.Range Range { get; protected set; }
    public uint ExpectedSize { get; protected set; }
    public byte[] Data { get; set; }

    //Fetch 32-bit little endian at offset
    public uint Read_32(uint offset)
    {
        uint[] b = new uint[4];
        for (int i = 0; i < 4; i++)
        {
            b[i] = Data[offset + i];
        }

        uint result = b[0] | (b[1] << 8) | (b[2] << 16) | (b[3] << 24);
        return result;
    }
}
