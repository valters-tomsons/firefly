using System;
using System.IO;
using firefly.Peripherals;

namespace firefly.Cpu
{
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
    }
}
