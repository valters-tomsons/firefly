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
        private Dictionary<UInt32, Action<Instruction>> SpecialCodeTable;

        private Boolean isRunning = false;
        //private Thread InterpreterThread;

        public Interpreter(CPU cpu)
        {
            Init_OpCodeTable();
            Init_SpecialTable();
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
                { 0x0, SPECIAL },
                { 0xF, LUI },
                { 0xD, ORI },
                { 0x2B, SW },
                { 0x9, ADDIU},
                { 0x2, JMP }
            };
        }

        private void Init_SpecialTable()
        {
            SpecialCodeTable = new Dictionary<UInt32, Action<Instruction>>
            {
                { 0x0, SLL }
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
            CPU.PC += 4;

            Execute(i);
        }

        public void Execute(Instruction i)
        {
            LogInstruction(i);

            try
            {
                OpCodeTable[i.Func](i);
            }
            catch (KeyNotFoundException e)
            {
                Logger.Message($"Unhandled Instruction 0x{i.Address:X} {i.Func:X}", LogSeverity.Error);
                Console.WriteLine();
                throw new Exceptions.UnhandledInstructionException(i);
            }
        }

        private void LogInstruction(Instruction i)
        {
            Console.WriteLine();

            try
            {
                //Log SPECIAL subfunctions
                if (i.Func == 0x0)
                {
                    //SLL NOP
                    if (CPU.R[i.Index_T] << (Int16)i.Imm_Shift == 0x0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;

                        Console.Write(
                            "{0, 12}",
                            "NOP"
                        );

                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(
                            "{0, 12} {1, 12} {2, 12}",
                            SpecialCodeTable[i.SubFunc].Method.Name,
                            $"0x{i.Address:X}",
                            $"0x{i.SubFunc:X}"
                        );
                    }
                    
                }
                else
                {
                    //Log OPCODES
                    Console.Write(
                        "{0, 12} {1, 12} {2, 12}",
                        OpCodeTable[i.Func].Method.Name,
                        $"0x{i.Address:X}",
                        $"0x{CPU.R[0]:X}"
                    );
                }
                
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.Write(
                    "{0, 12} {1, 12} {2, 12}", 
                    "NULL", $"0x{i.Address:X}",
                    $"0x{i.Imm_Jump:X}"
                    );

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private void SPECIAL(Instruction i)
        {
            SpecialCodeTable[i.SubFunc](i);
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
