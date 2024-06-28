using firefly.core.Domain;
using firefly.core.Exceptions;
using firefly.core.Peripherals;

namespace firefly.core.Cpu;

//Connects CPU to all the Peripherals
public sealed class Interconnector
{
    public static readonly Range MEM_CONTROL = new(0x1f801000, 36);
    public static readonly Range RAM_SIZE = new(0x1f801060, 4);
    public static readonly Range CACHE_CONTROL = new(0xfffe0130, 4);

    public readonly BIOS BIOS = new();

    public uint Read_32(PeripheralObject Object, uint Address)
    {
        if (Address % 4 != 0)
        {
            throw new UnalignedMemoryAccessException(Address);
        }

        if (Object.Range.Contains(Address, out var offset))
        {
            return Object.Read_32(offset);
        }

        throw new UnhandledFetch32Exception(Address, Object);
    }

    //Store 32bit word into address
    public void Store_32(uint Address, uint v)
    {
        if (Address % 4 != 0)
        {
            throw new UnalignedMemoryAccessException(Address);
        }

        if (MEM_CONTROL.Contains(Address, out uint offsetc))
        {
            Logger.Message($"Unimplemented Store_32 0x{offsetc:X} 0x{Address:X}", LogSeverity.Error, true);
        }
        else if (RAM_SIZE.Contains(Address, out uint offsetr))
        {
            Logger.Message($"Unimplemented RAM_SIZE 0x{offsetr:X} 0x{Address:X}", LogSeverity.Warning, true);
        }
        else if (CACHE_CONTROL.Contains(Address, out uint offsetcc))
        {
            Logger.Message($"Unimplemented CACHE_CONTROL 0x{offsetcc:X} 0x{Address:X}", LogSeverity.Warning, true);
        }
        else
        {
            Logger.Message($"Unimplemented Store_32 0x{Address:X}", LogSeverity.Error, true);
        }
    }
}
