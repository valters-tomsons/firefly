using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firefly.Domain
{
    public class Range
    {
        public UInt32 Start;
        public UInt32 Length;

        public Range(UInt32 Start, UInt32 Length)
        {
            this.Start = Start;
            this.Length = Length;
        }

        public UInt32 Contains(UInt32 Address)
        {
            if (Address >= Start && Address < Start + Length)
            {
                return Address - Start;
            }
            else
            {
                return 0;
            }
        }
    }
}
