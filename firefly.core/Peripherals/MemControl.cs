using System;
using System.Collections.Generic;
using System.Text;
using firefly.core.Domain;

namespace firefly.core.Peripherals
{
    public sealed class MemControl : PeripheralObject
    {
        public MemControl()
        {
            Range = new Range(0x1f801000, 36);
        }
    }
}
