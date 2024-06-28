using System;

namespace firefly.core.Exceptions;

class UnhandledStore32Exception(uint Address, uint v) : Exception(Message(Address, v))
{
    private new static string Message(uint Address, uint v)
    {
        return $"Unhandled Store_32 operation into address 0x:{Address:X}";
    }
}
