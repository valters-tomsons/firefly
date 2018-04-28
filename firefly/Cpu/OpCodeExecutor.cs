using System;
using System.Collections.Generic;
using firefly.Domain;

namespace firefly.Cpu
{
    sealed class OpCodeExecutor
    {
        private readonly CPU CPU;

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
                { 0xF, LUI },
                { 0xD, ORI },
            };
        }

        public void Execute(Instruction i)
        {
            OpCodeTable[i.Func](i);
        }

        //Load Upper Immediate
        private void LUI(Instruction i)
        {
            //set low 16bits to 0
            var v = i.Imm << 16;
            CPU.R[i.Index_T] = v;
        }

        //OR Immediate
        private void ORI(Instruction i)
        {
            UInt32 v;
        }
    }
}
