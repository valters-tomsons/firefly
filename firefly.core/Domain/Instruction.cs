namespace firefly.core.Domain;

public struct Instruction(uint Address)
{
    public uint Address = Address;

    //31:26 bits of instruction
    public readonly uint Func => Address >> 26;

    //5:0 bits of instruction
    public readonly uint SubFunc => Address & 0x3f;

    //Register index in bits 20:16
    public readonly uint Index_T => (Address >> 16) & 0x1F;

    //Register index in bits 25:21
    public readonly uint Index_S => (Address >> 21) & 0x1F;

    //Register index in bits 15:11
    public readonly uint Index_D => (Address >> 11) & 0x1F;

    //Immedaite value in bits 16:0
    public readonly uint Imm => Address & 0xffff;

    //Immediate value in bits 16:0 as sign-extended 32bit
    public readonly int Imm_Se => (short)(Address & 0xffff);

    //Shift Immediate values stored in 10:6
    public readonly uint Imm_Shift => (Address >> 6) & 0x1F;

    //Jump target in bits 25:0
    public readonly uint Imm_Jump => Address & 0x3ffffff;
}
