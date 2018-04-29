using System;
using System.Collections.Generic;
using System.Text;
using firefly.Peripherals;

namespace firefly.Exceptions
{
    class UnhandledFetch32Exception : Exception
    {
        public UnhandledFetch32Exception(UInt32 Address, PeripheralObject obj) : base(Message(Address, obj))
        {

        }

        private new static string Message(UInt32 Address, PeripheralObject obj)
        {
            return $"0x:{Address:X} is out of {obj.ToString()} range.";
        }
    }
}
