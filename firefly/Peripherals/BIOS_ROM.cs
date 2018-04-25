using System;
using System.IO;
using firefly.Cpu;
using firefly.Domain;

namespace firefly.Peripherals
{
    public sealed class BIOS : PeripheralObject
    {
        public BIOS()
        {
            ExpectedSize = 512 * 1024;
            Range = new Range((UInt32) MIPS_DEFAULT_ENUM.BIOS_KSEG1, ExpectedSize);
        }

        public void CreateImage(string name)
        {
            string _currentDir = AppDomain.CurrentDomain.BaseDirectory + "bios";
            string _currentFile = $"{_currentDir}/{name}";

            if (File.Exists(_currentFile))
            {
                var data = File.ReadAllBytes(_currentFile);
                Console.WriteLine($"BIOS File loaded: {data.Length / 1024}KB");
                Data = data;
            }
            else
            {
                Console.WriteLine("BIOS file doesn't exist.");
            }
        }
    }
}
