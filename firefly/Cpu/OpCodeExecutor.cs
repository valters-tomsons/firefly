using System;
using System.Collections.Generic;
using System.Text;
using firefly.Domain;

namespace firefly.Cpu
{
    sealed class OpCodeExecutor
    {
        private CPU CPU;

        public OpCodeExecutor(CPU cpu)
        {
            CPU = cpu;
        }

        public void Execute(Instruction i)
        {
            Console.WriteLine($"{i.Func:X}");
            Console.WriteLine($"{i.Index:X}");
            Console.WriteLine($"{i.Imm:X}");
        }
    }
}
