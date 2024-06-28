using System;
using firefly.core.Domain;

namespace firefly.core.Cpu;

public sealed class CPU
{
    //General Purpose Registers
    public uint[] R = new uint[32];

    // Multiply/Divide result Registers
    private uint HI;
    private uint LO;

    //Program Counter Register
    public uint PC;
    public Instruction NextInstruction;

    public Interconnector Interconnector;
    public Interpreter Interpreter;

    //COP0 : Status Register
    public uint SR = 0;

    public CPU()
    {
        Reset();
    }

    public void Reset()
    {
        Console.WriteLine("Starting CPU with default values.");

        InitRegisters();
        Interconnector = new Interconnector();
        Interpreter = new Interpreter(this);
    }

    private void InitRegisters()
    {
        PC = (uint)MIPS_DEFAULT_ENUM.BIOS_KSEG1;
        NextInstruction = new Instruction(0x0);

        for (int i = 0; i < R.Length; i++)
        {
            if (i == 0)
            {
                R[i] = 0;
            }
            else
            {
                R[i] = (uint)MIPS_DEFAULT_ENUM.PLACEHOLDER;
            }
        }
    }

    public uint Read_32(uint Address)
    {
        return Interconnector.Read_32(Interconnector.BIOS, Address);
    }

    public void Store_32(uint Address, uint v)
    {
        Interconnector.Store_32(Address, v);
    }
}
