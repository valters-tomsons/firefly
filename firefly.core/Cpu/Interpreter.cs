using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using firefly.core.Domain;

namespace firefly.core.Cpu
{
    public sealed class Interpreter
    {
        private readonly CPU CPU;
        private Dictionary<UInt32, Action<Instruction>> OpCodeTable;
        private Boolean isRunning = false;
        //private Thread InterpreterThread;

        public Interpreter(CPU cpu)
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
                { 0x2B, SW },
                { 0x0, SLL },
                { 0x9, ADDIU},
                { 0x2, JMP }
            };
        }

        private void PreserveZero()
        {
            CPU.R[0] = 0;
        }

        public void EmulateCycle()
        {
            PreserveZero();

            UInt32 PC = CPU.PC;

            Instruction i = CPU.NextInstruction;
            CPU.NextInstruction = new Instruction(CPU.Read_32(PC));

            Console.WriteLine("{0, 12} {1, 12} {2, 12}", OpCodeTable[i.Func].Method.Name, $"0x{i.Raw:X}", $"0x{CPU.R[8]:X}");

            CPU.PC += 4;

            Execute(i);
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

        //Shift Left Logical
        private void SLL(Instruction i)
        {
            UInt32 v = CPU.R[i.Index_T] << (Int16)i.Imm_Shift;
            CPU.R[i.Index_D] = v;
        }

        //Add Immediate Unsigned
        private void ADDIU(Instruction i)
        {
            UInt32 v = CPU.R[i.Index_S] + (UInt32)i.Imm_Se;
            CPU.R[i.Index_T] = v;
        }

        //Jump (J)
        private void JMP(Instruction i)
        {
            CPU.PC = (CPU.PC & 0xf0000000) | (i.Imm_Jump << 2);
        }

        #endregion
    }
}
