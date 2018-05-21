using System;
using firefly.core.Domain;
using firefly.core.Exceptions;
using firefly.core.Peripherals;

namespace firefly.core.Cpu
{
    //Connects CPU to all the Peripherals
    public sealed class Interconnector
    {
        public BIOS BIOS_Image;

        public Range MEM_CONTROL = new Range(0x1f801000, 36);
        public Range RAM_SIZE = new Range(0x1f801060, 4);
        public Range CACHE_CONTROL = new Range(0xfffe0130, 4);

        public Interconnector()
        {
            InitBIOS();
        }

        private void InitBIOS()
        {
            Console.WriteLine("Initializing BIOS.");
            BIOS_Image = new BIOS();
        }

        public UInt32 Read_32(PeripheralObject Object, UInt32 Address)
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
        public void Store_32(UInt32 Address, UInt32 v)
        {
            if (Address % 4 != 0)
            {
                throw new UnalignedMemoryAccessException(Address);
            }

            if (MEM_CONTROL.Contains(Address, out UInt32 offsetc))
            {
                Logger.Message($"Unimplemented Store_32 0x{offsetc:X} 0x{Address:X}", LogSeverity.Error, true);
            }
            else if (RAM_SIZE.Contains(Address, out UInt32 offsetr))
            {
                Logger.Message($"Unimplemented RAM_SIZE 0x{offsetr:X} 0x{Address:X}", LogSeverity.Warning, true);
            }
            else
            {
                Logger.Message($"Unimplemented Store_32 0x{Address:X}", LogSeverity.Error, true);
            }
        }
    }
}
