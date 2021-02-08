using System;
using System.IO;
using firefly.core.Cpu;

namespace firefly
{
    static class Program
    {
        private static CPU CPU;

        static void Main(string[] args)
        {
            InitCPU();
            StartCPU();
            Console.ReadLine();
        }

        private static byte[] LoadBIOS(string file)
        {
            string _currentDir = AppDomain.CurrentDomain.BaseDirectory + "bios";
            string _currentFile = $"{_currentDir}/{file}";

            if (File.Exists(_currentFile))
            {
                var data = File.ReadAllBytes(_currentFile);
                Console.WriteLine($"BIOS File loaded: {data.Length / 1024}KB");
                return data;
            }
            else
            {
                Console.WriteLine("BIOS file doesn't exist.");
                return null;
            }
        }

        private static void InitCPU()
        {
            const string biosfile = "scph5501.BIN";

            CPU = new CPU();
            CPU.Interconnector.BIOS_Image.CreateImage(LoadBIOS(biosfile));
        }

        private static void StartCPU()
        {
            CPU.Interpreter.Start();
        }
    }
}
