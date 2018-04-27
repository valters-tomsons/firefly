using System;
using System.Collections.Generic;
using System.Text;

namespace firefly.Domain
{
    public sealed class Instruction
    {
        public UInt32 Func;
        public UInt32 Index;
        public UInt32 Imm;

        public Instruction(UInt32 Address)
        {
            Func = Address >> 26;
            Index = (Address >> 16) & 0xf1;
            Imm = Address & 0xffff;
        }
    }
}
