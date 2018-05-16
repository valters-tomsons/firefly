using System;

namespace firefly.core.Domain
{
    //Contains default (reset values) for any register
    //All should be unsigned 32-bit integers

    enum MIPS_DEFAULT_ENUM : UInt32
    {
        PLACEHOLDER = 0xdeadbeef,

        BIOS_KSEG1 = 0xbfc00000
    }
}