using System;
using firefly.core.Domain;

namespace firefly.core.Peripherals
{
    public sealed class BIOS : PeripheralObject
    {
        public BIOS()
        {
            ExpectedSize = 512 * 1024;
            Range = new((UInt32) MIPS_DEFAULT_ENUM.BIOS_KSEG1, ExpectedSize);
        }

        public void CreateImage(byte[] buffer)
        {
            Console.WriteLine("Loading BIOS file.");
            Data = buffer;
        } 
    }
}
