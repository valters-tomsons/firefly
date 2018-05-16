using System;

namespace firefly.core.Exceptions
{
    class UnhandledStore32Exception : Exception
    {

        public UnhandledStore32Exception(UInt32 Address, UInt32 v) : base(Message(Address, v))
        {

        }

        private new static string Message(UInt32 Address, UInt32 v)
        {
            return $"Unhandled Store_32 operation into address 0x:{Address:X}";
        }
    }
}
