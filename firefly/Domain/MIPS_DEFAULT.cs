using System;

namespace firefly.Domain
{
    //Contains default (reset values) for any register
    //All should be unsigned 32-bit integers

    enum MIPS_DEFAULT_ENUM : UInt32
    {
        PLACEHOLDER = 0x1337beef,

        BIOS_KSEG1 = 0xbfc00000
    }
}