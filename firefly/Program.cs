using System;
using System.IO;
using firefly.Cpu;
using firefly.Peripherals;

namespace firefly
{
    class Program
    {
        private static CPU CPU;

        static void Main(string[] args)
        {
            InitCPU();
            Console.ReadLine();
        }

        static void InitCPU()
        {
            CPU = new CPU();
            LoadBIOS();
        }

        static void LoadBIOS()
        {
            
        }
    }
}
