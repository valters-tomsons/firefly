using System;

namespace firefly.Cpu
{
    sealed class CPU
    {
        //General Purpose Registers
        private UInt32[] R = new UInt32[32];

        // Multiply/Divide result Registers
        private UInt32 HI;
        private UInt32 LO;

        //Program Counter Register
        private UInt32 PC;

        public Interconnector Interconnector;
        public CPU()
        {
            Reset();
        }

        public void Reset()
        {
            Console.WriteLine("Starting CPU with default values.");
            PC = (UInt32) MIPS_DEFAULT_ENUM.BIOS_KSEG1;

            Run_Inst();
        }

        public void Run_Inst()
        {
            //fetch

            PC = PC + 4;

            //Execute(instruction);
        }

        private void Execute(UInt32 Instruction)
        {

        }
    }
}
