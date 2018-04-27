using System;
using System.Collections.Generic;
using System.Text;

namespace firefly.Exceptions
{
    class UnhandledInstructionException : Exception
    {
        public UnhandledInstructionException(UInt32 Address)
        {

        }
    }
}
