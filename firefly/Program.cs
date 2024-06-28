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
            Start();
            Console.ReadLine();
        }

        private static void Start()
        {
            var biosFile = LoadBIOS("scph5502.BIN");
            if (biosFile is null) throw new NotImplementedException();

            CPU = new CPU();
            CPU.Interconnector.BIOS.CreateFromImage(biosFile);

            CPU.Interpreter.Start();
        }

        private static byte[] LoadBIOS(string file)
        {
            var _currentDir = AppDomain.CurrentDomain.BaseDirectory + "bios";
            var _currentFile = $"{_currentDir}/{file}";

            if (!File.Exists(_currentFile))
            {
                Console.WriteLine("BIOS file doesn't exist.");
                return null;
            }

            var data = File.ReadAllBytes(_currentFile);
            Console.WriteLine($"BIOS File loaded: {data.Length / 1024}KB");
            return data;
        }
    }
}
