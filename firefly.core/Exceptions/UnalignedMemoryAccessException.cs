using System;

namespace firefly.core.Exceptions;

class UnalignedMemoryAccessException(uint Address) : Exception(Message(Address))
{
    private new static string Message(uint Address)
    {
        return $"0x:{Address:X} is out of range.";
    }
}
