using System;
using firefly.core.Domain;

namespace firefly.core.Exceptions;

class UnhandledInstructionException(Instruction i) : Exception(Message(i))
{
    private new static string Message(Instruction i)
    {
        return $"Unhandled instruction 0x{i.Func:X} tried to execute.";
    }
}
