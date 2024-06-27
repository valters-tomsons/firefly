using System;

namespace firefly.core.Peripherals
{
    public class PeripheralObject
    {
        public Domain.Range Range { get; protected set; }
        public UInt32 ExpectedSize { get; protected set; }
        public byte[] Data { get; set; }

        //Fetch 32-bit little endian at offset
        public UInt32 Read_32(UInt32 offset)
        {
            UInt32[] b = new UInt32[4];
            for (int i = 0; i < 4; i++)
            {
                b[i] = Data[offset + i];
            }

            UInt32 result = b[0] | (b[1] << 8) | (b[2] << 16) | (b[3] << 24);
            return result;
        }


    }
}
