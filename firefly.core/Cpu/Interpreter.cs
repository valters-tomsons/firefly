﻿using System;
using System.Collections.Generic;
using firefly.core.Domain;

namespace firefly.core.Cpu;

public sealed class Interpreter
{
    private readonly CPU CPU;

    private Dictionary<uint, Action<Instruction>> OpCodeTable;
    private Dictionary<uint, Action<Instruction>> SpecialCodeTable;
    private Dictionary<uint, Action<Instruction>> COP0CodeTable;

    private bool isRunning;
    private readonly bool ignoreInstructionInvalidExceptions = false;

    public Interpreter(CPU cpu)
    {
        Init_OpCodeTable();
        Init_SpecialTable();
        Init_COP0Table();
        CPU = cpu;
    }

    public void Start()
    {
        if (CPU.Interconnector.BIOS.Data != null)
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
        OpCodeTable = new Dictionary<uint, Action<Instruction>>
            {
                { 0xF, LUI },
                { 0xD, ORI },
                { 0x2B, SW },
                { 0x9, ADDIU},
                { 0x2, JMP },
                { 0x5, BNE },
                { 0x8, ADDI },
                { 0x23, LW },

                { 0x0, SPECIAL },
                { 0x10, MTC0 }
            };
    }

    private void Init_SpecialTable()
    {
        SpecialCodeTable = new Dictionary<uint, Action<Instruction>>
            {
                { 0x0, SLL },
                { 0x25, OR }
            };
    }

    private void Init_COP0Table()
    {
        COP0CodeTable = new Dictionary<uint, Action<Instruction>>
            {
                {0xC, SetCOP0StatusRegister}
            };
    }

    private void PreserveZero()
    {
        CPU.R[0] = 0;
    }

    public void EmulateCycle()
    {
        PreserveZero();

        uint PC = CPU.PC;

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
        catch (KeyNotFoundException)
        {
            Logger.Message($"Unhandled Instruction 0x{i.Address:X} 0x{i.Func:X}", LogSeverity.Error);
            Console.WriteLine();

            if (!ignoreInstructionInvalidExceptions)
            {
                throw new Exceptions.UnhandledInstructionException(i);
            }
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
                if (CPU.R[i.Index_T] << (short)i.Imm_Shift == 0x0)
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
                    "{0, 12} {1, 12}",
                    OpCodeTable[i.Func].Method.Name,
                    $"0x{i.Address:X}"
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
        try
        {
            SpecialCodeTable[i.SubFunc](i);
        }
        catch (KeyNotFoundException)
        {
            Logger.Message($"Unhandled SPECIAL Instruction 0x{i.Address:X} 0x{i.Func:X} 0x{i.SubFunc:X}", LogSeverity.Error);
            Console.WriteLine();
            if (!ignoreInstructionInvalidExceptions)
            {
                throw new Exceptions.UnhandledInstructionException(i);
            }
        }
    }

    private void Branch(uint offset)
    {
        //offset immediates are shifted two places since PC addresses must be 32bit at all times
        offset <<= 2;

        //branch to value and compensate for hardcoded PC+4 in EmulateCycle()
        CPU.PC = CPU.PC + offset - 4;
    }

    //Move to Coprocessor 0
    private void MTC0(Instruction i)
    {
        uint cop_r = i.Index_D;

        try
        {
            COP0CodeTable[cop_r](i);
        }
        catch (KeyNotFoundException)
        {
            Logger.Message($"Unhandled COP0 Instruction 0x{i.Address:X} 0x{cop_r:X}", LogSeverity.Error);
            Console.WriteLine();
            if (!ignoreInstructionInvalidExceptions)
            {
                throw new Exceptions.UnhandledInstructionException(i);
            }
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
        uint v = CPU.R[i.Index_S] | i.Imm;
        CPU.R[i.Index_T] = v;
    }

    //Store Word
    private void SW(Instruction i)
    {
        if ((CPU.SR & 0x10000) == 0)
        {
            uint addr = CPU.R[i.Index_S] + i.Imm;
            CPU.Store_32(addr, CPU.R[i.Index_T]);
        }
        else
        {
            Logger.Message($"Cache isolated, skipping writes | SR = {CPU.SR}", LogSeverity.Information, true);
        }
    }

    //Shift Left Logical
    private void SLL(Instruction i)
    {
        uint v = CPU.R[i.Index_T] << (short)i.Imm_Shift;
        CPU.R[i.Index_D] = v;
    }

    //Add Immediate Unsigned
    private void ADDIU(Instruction i)
    {
        uint v = CPU.R[i.Index_S] + (uint)i.Imm_Se;
        CPU.R[i.Index_T] = v;
    }

    //Jump (J)
    private void JMP(Instruction i)
    {
        CPU.PC = (CPU.PC & 0xf0000000) | (i.Imm_Jump << 2);
    }

    private void OR(Instruction i)
    {
        uint v = CPU.R[i.Index_S] | CPU.R[i.Index_T];
        CPU.R[i.Index_D] = v;
    }

    //Branch If Not Equal
    private void BNE(Instruction i)
    {
        if (i.Index_S != i.Index_T)
        {
            Branch((ushort)i.Imm_Se);
        }
    }

    //Add Immediate
    private void ADDI(Instruction i)
    {
        var I = i.Imm_Se;
        var t = i.Index_T;
        var s = i.Index_S;
        var st = (int)CPU.R[s];

        int v = checked(st + I);
        CPU.R[t] = (uint)v;
    }

    private void LW(Instruction i)
    {
        if ((CPU.SR & 0x10000) == 0)
        {
            uint addr = CPU.R[i.Index_S] + (uint)i.Imm_Se;
            uint v = CPU.Read_32(addr);
            CPU.R[i.Index_T] = v;
        }
        else
        {
            Logger.Message($"Cache isolated, skipping reads | SR = {CPU.SR}", LogSeverity.Information, true);
        }
    }

    private void SetCOP0StatusRegister(Instruction i)
    {
        uint v = CPU.R[i.Index_T];
        CPU.SR = v;
    }

    #endregion
}