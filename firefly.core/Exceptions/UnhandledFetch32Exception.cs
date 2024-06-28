using System;
using firefly.core.Peripherals;

namespace firefly.core.Exceptions;

class UnhandledFetch32Exception(uint Address, PeripheralObject obj) : Exception(Message(Address, obj))
{
    private new static string Message(uint Address, PeripheralObject obj)
    {
        return $"0x:{Address:X} is out of {obj} range.";
    }
}
