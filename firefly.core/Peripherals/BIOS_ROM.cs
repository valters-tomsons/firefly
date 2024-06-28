using firefly.core.Domain;

namespace firefly.core.Peripherals;

public sealed class BIOS : PeripheralObject
{
    public BIOS()
    {
        ExpectedSize = 512 * 1024;
        Range = new((uint)MIPS_DEFAULT_ENUM.BIOS_KSEG1, ExpectedSize);
    }

    public void CreateFromImage(byte[] buffer)
    {
        Data = buffer;
    }
}