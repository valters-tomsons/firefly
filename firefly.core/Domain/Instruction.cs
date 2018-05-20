﻿using System;

namespace firefly.core.Domain
{
    public struct Instruction
    {
        public UInt32 Raw;
        public UInt32 Func;
        public UInt32 Index_T;
        public UInt32 Index_S;
        public UInt32 Imm;
        public Int32 Imm_Se;

        public Instruction(UInt32 Address)
        {
            Raw = Address;

            //31:26 bits of instruction
            Func = Address >> 26;

            //Register index in bits 20:16
            Index_T = (Address >> 16) & 0x1F;

            //Register index in bits 25:21
            Index_S  = (Address >> 21) & 0x1F;

            //Immedaite value in bits 16:0
            Imm = Address & 0xffff;

            //Immediate value in bits 16:0 as sign-extended 32bit
            Imm_Se = (Int16)(Raw & 0xffff);
        }
    }
}
