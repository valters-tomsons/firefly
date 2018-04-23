using System;

namespace firefly.Cpu
{
    //Contains default (reset values) for any register
    //All should be unsigned 32-bit integers

    enum MIPS_DEFAULT_ENUM : UInt32
    {
        //BIOS address in KSEG1
        BIOS_KSEG1 = 0xbfc00000
    }
}