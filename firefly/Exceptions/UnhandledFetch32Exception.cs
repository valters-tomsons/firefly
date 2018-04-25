using System;
using System.Collections.Generic;
using System.Text;
using firefly.Peripherals;

namespace firefly.Exceptions
{
    class UnhandledFetch32Exception : Exception
    {
        public UnhandledFetch32Exception() { }

        public UnhandledFetch32Exception(UInt32 Address) {}

        public UnhandledFetch32Exception(UInt32 Address, PeripheralObject obj) { }
    }
}
