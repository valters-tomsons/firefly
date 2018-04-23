using System;
using System.IO;

namespace firefly.Peripherals
{
    public sealed class BIOS
    {
        const UInt32 ExpectedSize = (512 * 1024);
        public static byte[] Data = new byte[ExpectedSize];

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

        //Fetch 32-bit little endian at offset
        public UInt32 Read_32(UInt32 offset)
        {
            UInt32[] b = new UInt32[4];
            for (int i = 0; i < 4; i++)
            {
                b[i] = Data[offset + i];
            }

            UInt32 result = b[0] | (b[1] << 8) | (b[2] << 16) | (b[3] << 24);
            return result;
        }
    }
}
