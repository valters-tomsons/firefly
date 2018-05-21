using System;

namespace firefly.core.Domain
{
    public struct Instruction
    {
        public UInt32 Address;

        //31:26 bits of instruction
        public UInt32 Func => Address >> 26;

        //5:0 bits of instruction
        public UInt32 SubFunc => Address & 0x3f;

        //Register index in bits 20:16
        public UInt32 Index_T => (Address >> 16) & 0x1F;

        //Register index in bits 25:21
        public UInt32 Index_S => (Address >> 21) & 0x1F;

        //Register index in bits 15:11
        public UInt32 Index_D => (Address >> 11) & 0x1F;

        //Immedaite value in bits 16:0
        public UInt32 Imm => Address & 0xffff;

        //Immediate value in bits 16:0 as sign-extended 32bit
        public Int32 Imm_Se => (Int16)(Address & 0xffff);

        //Shift Immediate values stored in 10:6
        public UInt32 Imm_Shift => (Address >> 6) & 0x1F;

        //Jump target in bits 25:0
        public UInt32 Imm_Jump => Address & 0x3ffffff;


        public Instruction(UInt32 Address)
        {
            this.Address = Address;
        }
    }
}
