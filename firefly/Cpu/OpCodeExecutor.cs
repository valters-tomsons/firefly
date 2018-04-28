using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using firefly.Domain;

namespace firefly.Cpu
{
    sealed class OpCodeExecutor
    {
        private CPU CPU;
        private Dictionary<UInt32, Action<Instruction>> OpCodeTable;

        public OpCodeExecutor(CPU cpu)
        {
            Init_OpCodeTable();
            CPU = cpu;
        }

        private void Init_OpCodeTable()
        {
            OpCodeTable = new Dictionary<UInt32, Action<Instruction>>
            {
                { 0b001111, LUI },
                { 0b001101, ORI },
            };
        }

        public void Execute(Instruction i)
        {
            //Console.WriteLine($"{i.Func:X}");
            OpCodeTable[i.Func](i);
        }

        //Load Upper Immediate
        private void LUI(Instruction i)
        {
            //set low 16bits to 0
            UInt32 v = i.Imm << 16;
            CPU.R[i.Index] = v;
        }

        private void ORI(Instruction i)
        {
            UInt32 v;
        }
    }
}
