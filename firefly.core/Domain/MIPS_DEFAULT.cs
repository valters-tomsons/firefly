namespace firefly.core.Domain;

/// <summary>
/// Contains default/reset values for registers
/// </summary>
enum MIPS_DEFAULT_ENUM : uint
{
    PLACEHOLDER = 0xdeadbeef,

    BIOS_KSEG1 = 0xbfc00000
}