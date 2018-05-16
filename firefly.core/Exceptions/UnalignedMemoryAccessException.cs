using firefly.core.Peripherals;
using System;

namespace firefly.core.Exceptions
{
    class UnalignedMemoryAccessException : Exception
    {
        public UnalignedMemoryAccessException(UInt32 Address) : base (Message(Address))
        {

        }

        private new static string Message(UInt32 Address)
        {
            return $"0x:{Address:X} is out of range.";
        }
    }
}
