using System;
using firefly.Domain;
using firefly.Exceptions;

namespace firefly.Cpu
{
    sealed class CPU
    {
        //General Purpose Registers
        public UInt32[] R = new UInt32[32];

        // Multiply/Divide result Registers
        private UInt32 HI;
        private UInt32 LO;

        //Program Counter Register
        private UInt32 PC;

        public Interconnector Interconnector;
        public OpCodeExecutor Executor;

        public CPU()
        {
            Reset();
        }

        public void Reset()
        {
            Console.WriteLine("Starting CPU with default values.");

            InitRegisters();
            Interconnector = new Interconnector();
            Executor = new OpCodeExecutor(this);

            Console.WriteLine($"0x{Read_32(PC):X}");

            Instruction inst = Decode(Read_32(PC));
            Executor.Execute(inst);
        }

        private void InitRegisters()
        {
            PC = (UInt32)MIPS_DEFAULT_ENUM.BIOS_KSEG1;

            for (int i = 0; i < R.Length; i++)
            {
                if (i == 0)
                {
                    R[i] = 0;
                }
                else
                {
                    R[i] = (UInt32)MIPS_DEFAULT_ENUM.PLACEHOLDER;
                }
            }
        }

        public UInt32 Read_32(UInt32 Address)
        {
            return Interconnector.Read_32(Interconnector.BIOS_Image, Address);
        }

        private Instruction Decode(UInt32 Address)
        {
            return new Instruction(Address);
        }
    }
}
