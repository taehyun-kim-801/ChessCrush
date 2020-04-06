using System;
using System.Collections;

namespace ChessCrush.Game
{
    public class InputMemoryBitStream
    {
        public BitArray buffer { get; private set; }
        public int bitHead { get; private set; }
        private int bitCapacity;

        public InputMemoryBitStream(BitArray buffer)
        {
            this.buffer = buffer;
            bitHead = 0;
            bitCapacity = buffer.Length;
        }

        #region ReadBits
        public void ReadBits<T>(out T data, int length) where T : struct
        {
            BitArray dataBits = new BitArray(length);
            for (int i = 0; i < length; i++)
                dataBits[i] = buffer[bitHead + i];
            byte[] dataBytes = new byte[length / 8 + (length % 8 > 0 ? 1 : 0)];
            dataBits.CopyTo(dataBytes, 0);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(dataBytes);
            bitHead += length;

            if (typeof(T) == typeof(bool))
                data = (T)(object)BitConverter.ToBoolean(dataBytes, 0);
            else if (typeof(T) == typeof(char))
                data = (T)(object)BitConverter.ToChar(dataBytes, 0);
            else if (typeof(T) == typeof(byte))
                data = (T)(object)dataBytes[0];
            else if (typeof(T) == typeof(sbyte))
                data = (T)(object)dataBytes[0];
            else if (typeof(T) == typeof(short))
            {
                byte[] newArr = new byte[sizeof(short)];
                dataBytes.CopyTo(newArr, newArr.Length - dataBytes.Length);
                data = (T)(object)BitConverter.ToInt16(newArr, 0);
            }
            else if (typeof(T) == typeof(ushort))
            {
                byte[] newArr = new byte[sizeof(ushort)];
                dataBytes.CopyTo(newArr, newArr.Length - dataBytes.Length);
                data = (T)(object)BitConverter.ToUInt16(newArr, 0);
            }
            else if (typeof(T) == typeof(int))
            {
                byte[] newArr = new byte[sizeof(int)];
                dataBytes.CopyTo(newArr, newArr.Length - dataBytes.Length);
                data = (T)(object)BitConverter.ToInt32(newArr, 0);
            }
            else if (typeof(T) == typeof(uint))
            {
                byte[] newArr = new byte[sizeof(uint)];
                dataBytes.CopyTo(newArr, newArr.Length - dataBytes.Length);
                data = (T)(object)BitConverter.ToUInt32(newArr, 0);
            }
            else if (typeof(T) == typeof(long))
            {
                byte[] newArr = new byte[sizeof(long)];
                dataBytes.CopyTo(newArr, newArr.Length - dataBytes.Length);
                data = (T)(object)BitConverter.ToInt64(newArr, 0);
            }
            else if (typeof(T) == typeof(ulong))
            {
                byte[] newArr = new byte[sizeof(ulong)];
                dataBytes.CopyTo(newArr, newArr.Length - dataBytes.Length);
                data = (T)(object)BitConverter.ToUInt64(newArr, 0);
            }
            else if (typeof(T) == typeof(float))
            {
                byte[] newArr = new byte[sizeof(float)];
                dataBytes.CopyTo(newArr, newArr.Length - dataBytes.Length);
                data = (T)(object)BitConverter.ToSingle(newArr, 0);
            }
            else if (typeof(T) == typeof(double))
            {
                byte[] newArr = new byte[sizeof(double)];
                dataBytes.CopyTo(newArr, newArr.Length - dataBytes.Length);
                data = (T)(object)BitConverter.ToDouble(newArr, 0);
            }
            else
                throw new ArgumentException("InputMemoryBitStream.ReadBits parameter can be only primitive type except decimal");
        }
        public void ReadBits(out string data)
        {
            ReadBits(out int length, sizeof(int) * 8);
            var arr = new char[length];
            for (int i = 0; i < length; i++)
                ReadBits(out arr[i], sizeof(char) * 8);
            data = new string(arr);
        }
        #endregion
    }
}
