using System;
using System.Collections.Generic;
using firefly.core.Domain;

namespace firefly.core.Cpu
{
    public sealed class OpCodeExecutor
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
            if (CPU.Interconnector.BIOS_Image.Data != null)
            {
                isRunning = true;
                while (isRunning)
                {
                    EmulateCycle();
                }
            }
            else
            {
                Console.WriteLine("Cannot start interpreter without BIOS image.");
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
            Console.WriteLine($"0x{i.RAW:X} 0x{CPU.R[8]:X}");

            CPU.PC += 4;

            Execute(i);
        }

        #region CPU_OPCODES

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
            UInt32 addr = CPU.R[i.Index_S] + i.Imm;
            CPU.Store_32(addr, CPU.R[i.Index_T]);
        }

        #endregion
    }
}
