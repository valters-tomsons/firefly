using System;
using firefly.core.Cpu;

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
        }
    }
}
