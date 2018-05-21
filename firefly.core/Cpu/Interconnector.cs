using System;
using firefly.core.Exceptions;
using firefly.core.Peripherals;

namespace firefly.core.Cpu
{
    //Connects CPU to all the Peripherals
    public sealed class Interconnector
    {
        public BIOS BIOS_Image;
        public MemControl MemControl;

        public Interconnector()
        {
            InitBIOS();
            InitMemControl();
        }

        private void InitBIOS()
        {
            Console.WriteLine("Initializing BIOS.");
            BIOS_Image = new BIOS();
        }

        private void InitMemControl()
        {
            Console.WriteLine("Initializing Memory controller (MemControl)");
            MemControl = new MemControl();
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

            if (MemControl.Range.Contains(Address, out UInt32 offset))
            {
                Console.WriteLine("Store32 unimplemented");
            }
            else if (Address == 0x1F801060)
            {
                Console.WriteLine("RAM_SIZE register unimplemented");
            }
            else
            {
                throw new UnhandledStore32Exception(Address, v);
            }
        }
    }
}
