using System;
using firefly.Domain;

namespace firefly.Exceptions
{
    class UnhandledInstructionException : Exception
    {
        public UnhandledInstructionException(Instruction i) : base(Message(i))
        {

        }

        private new static string Message(Instruction i)
        {
            return $"Unhandled instruction 0x{i.Func:X} tried to execute.";
        }
    }
}
