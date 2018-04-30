using System;
using System.Collections.Generic;
using firefly.Domain;

namespace firefly.Cpu
{
    sealed class OpCodeExecutor
    {
        private readonly CPU CPU;
        private Dictionary<UInt32, Action<Instruction>> OpCodeTable;
        private bool isRunning = false;
        //private Thread InterpreterThread;

        public OpCodeExecutor(CPU cpu)
        {
            Init_OpCodeTable();
            CPU = cpu;
        }

        public void Start()
        {
            isRunning = true;
            while (isRunning)
            {
                EmulateCycle();
            }
        }

        public void Pause()
        {
            isRunning = false;
        }

        private void Init_OpCodeTable()
        {
            OpCodeTable = new Dictionary<UInt32, Action<Instruction>>
            {
                { 0xF, LUI },
                { 0xD, ORI },
                { 0x2B, SW }
            };
        }

        private void PreserveZero()
        {
            CPU.R[0] = 0;
        }

        public void Execute(Instruction i)
        {
            try
            {
                OpCodeTable[i.Func](i);
            }
            catch (KeyNotFoundException)
            {
                throw new Exceptions.UnhandledInstructionException(i);
            }
        }

        public void EmulateCycle()
        {
            PreserveZero();

            //Fetch & Decode
            Instruction i = CPU.Decode(CPU.Read_32(CPU.PC));

            CPU.PC += 4;

            Execute(i);
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
            UInt32 v = CPU.R[i.Index_S] | i.Imm;
            CPU.R[i.Index_T] = v;
        }

        //Store Word
        private void SW(Instruction i)
        {

        }
    }
}
