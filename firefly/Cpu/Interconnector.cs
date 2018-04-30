using System;
using firefly.Peripherals;
using firefly.Exceptions;

namespace firefly.Cpu
{
    //Connects CPU to all the Peripherals
    sealed class Interconnector
    {
        public static BIOS BIOS_Image;

        public Interconnector()
        {
            InitBIOS();
        }

        private void InitBIOS()
        {
            BIOS_Image = new BIOS();
            BIOS_Image.CreateImage("scph5501.BIN");
            Console.WriteLine("BIOS Initialized at Interconnect");
        }

        public UInt32 Read_32(PeripheralObject Object, UInt32 Address)
        {
            if (Object.Range.Contains(Address, out var offset))
            {
                return Object.Read_32(offset);
            }

            throw new UnhandledFetch32Exception(Address, Object);
        }

        public UInt32 Store_32(UInt32 Address, UInt32 v)
        {
            throw new UnhandledStore32Exception(Address, v);
        }
    }
}
