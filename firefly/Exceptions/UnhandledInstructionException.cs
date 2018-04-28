using System;
using firefly.Domain;

namespace firefly.Exceptions
{
    class UnhandledInstructionException : Exception
    {
        public UnhandledInstructionException(Instruction Address)
        {

        }
    }
}
