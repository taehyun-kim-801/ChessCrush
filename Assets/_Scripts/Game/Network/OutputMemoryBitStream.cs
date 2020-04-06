using System;
using System.Collections;

namespace ChessCrush.Game
{
    public class OutputMemoryBitStream
    {
        public BitArray buffer { get; private set; }
        public int bitHead { get; private set; }
        private int bitCapacity;
        public int ByteHead { get { return (bitHead + 7) >> 3; } }

        public OutputMemoryBitStream()
        {
            buffer = new BitArray(256);
            bitHead = 0;
            bitCapacity = buffer.Length;
        }

        #region WriteBits
        public void WriteBits<T>(T data, int bitCount) where T: struct
        {
            int nextBitHead = bitHead + bitCount;
            if(nextBitHead>bitCapacity)
            {
                var newArr = new BitArray(buffer.Length * 2);
                for(int i=0;i<bitCapacity;i++)
                {
                    newArr[i] = buffer[i];
                }
                bitCapacity = newArr.Length;
                buffer = newArr;
            }

            byte[] dataBytes;
            if (typeof(T) == typeof(bool)) dataBytes = BitConverter.GetBytes(Convert.ToBoolean(data));
            else if (typeof(T) == typeof(char)) dataBytes = BitConverter.GetBytes(Convert.ToChar(data));
            else if (typeof(T) == typeof(byte)) dataBytes = BitConverter.GetBytes(Convert.ToByte(data));
            else if (typeof(T) == typeof(sbyte)) dataBytes = BitConverter.GetBytes(Convert.ToSByte(data));
            else if (typeof(T) == typeof(short)) dataBytes = BitConverter.GetBytes(Convert.ToInt16(data));
            else if (typeof(T) == typeof(ushort)) dataBytes = BitConverter.GetBytes(Convert.ToUInt16(data));
            else if (typeof(T) == typeof(int)) dataBytes = BitConverter.GetBytes(Convert.ToInt32(data));
            else if (typeof(T) == typeof(uint)) dataBytes = BitConverter.GetBytes(Convert.ToUInt32(data));
            else if (typeof(T) == typeof(long)) dataBytes = BitConverter.GetBytes(Convert.ToInt64(data));
            else if (typeof(T) == typeof(ulong)) dataBytes = BitConverter.GetBytes(Convert.ToUInt64(data));
            else if (typeof(T) == typeof(float)) dataBytes = BitConverter.GetBytes(Convert.ToSingle(data));
            else if (typeof(T) == typeof(double)) dataBytes = BitConverter.GetBytes(Convert.ToDouble(data));
            else
                throw new ArgumentException("OutputMemoryBitStream.WriteBits parameter can be only primitive type except decimal");

            if (BitConverter.IsLittleEndian)
                Array.Reverse(dataBytes);
            var dataBits = new BitArray(dataBytes);
            for (int i = bitHead; i < nextBitHead; i++)
                buffer[i] = dataBits[i - bitHead];
            bitHead = nextBitHead;
        }
        public void WriteBits(string data)
        {
            WriteBits(data.Length, sizeof(int) * 8);
            foreach(var c in data)
            {
                WriteBits(c, sizeof(char) * 8);
            }
        }
        #endregion
    }
}
