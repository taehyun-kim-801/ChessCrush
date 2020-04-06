using System;

namespace ChessCrush.Game
{
    public class InputMemoryStream
    {
        public byte[] buffer { get; private set; }
        public int head { get; private set; }
        private int capacity;
        public int RemainingDataSize { get { return capacity - head; } }

        public InputMemoryStream(byte[] buffer)
        {
            this.buffer = buffer;
            head = 0;
            capacity = buffer.Length;
        }

        #region Read
        public void Read<T>(out T data) where T : struct
        {
            byte[] dataByte;
            if (typeof(T) == typeof(bool))
            {
                dataByte = new byte[sizeof(bool)];
                Array.Copy(buffer, head, dataByte, 0, sizeof(bool));
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(dataByte);
                data = (T)(object)BitConverter.ToBoolean(dataByte, 0);
                head += sizeof(bool);
            }
            else if (typeof(T) == typeof(char))
            {
                dataByte = new byte[sizeof(char)];
                Array.Copy(buffer, head, dataByte, 0, sizeof(char));
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(dataByte);
                data = (T)(object)BitConverter.ToChar(dataByte, 0);
                head += sizeof(char);
            }
            else if (typeof(T) == typeof(byte))
            {
                data = (T)(object)buffer[head];
                head += sizeof(byte);
            }
            else if (typeof(T) == typeof(sbyte))
            {
                data = (T)(object)buffer[head];
                head += sizeof(sbyte);
            }
            else if (typeof(T) == typeof(short))
            {
                dataByte = new byte[sizeof(short)];
                Array.Copy(buffer, head, dataByte, 0, sizeof(short));
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(dataByte);
                data = (T)(object)BitConverter.ToInt16(dataByte, 0);
                head += sizeof(short);
            }
            else if (typeof(T) == typeof(ushort))
            {
                dataByte = new byte[sizeof(ushort)];
                Array.Copy(buffer, head, dataByte, 0, sizeof(ushort));
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(dataByte);
                data = (T)(object)BitConverter.ToUInt16(dataByte, 0);
                head += sizeof(ushort);
            }
            else if (typeof(T) == typeof(int))
            {
                dataByte = new byte[sizeof(int)];
                Array.Copy(buffer, head, dataByte, 0, sizeof(int));
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(dataByte);
                data = (T)(object)BitConverter.ToInt32(dataByte, 0);
                head += sizeof(int);
            }
            else if (typeof(T) == typeof(uint))
            {
                dataByte = new byte[sizeof(uint)];
                Array.Copy(buffer, head, dataByte, 0, sizeof(uint));
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(dataByte);
                data = (T)(object)BitConverter.ToUInt32(dataByte, 0);
                head += sizeof(uint);
            }
            else if (typeof(T) == typeof(long))
            {
                dataByte = new byte[sizeof(long)];
                Array.Copy(buffer, head, dataByte, 0, sizeof(long));
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(dataByte);
                data = (T)(object)BitConverter.ToInt64(dataByte, 0);
                head += sizeof(long);
            }
            else if (typeof(T) == typeof(ulong))
            {
                dataByte = new byte[sizeof(ulong)];
                Array.Copy(buffer, head, dataByte, 0, sizeof(ulong));
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(dataByte);
                data = (T)(object)BitConverter.ToUInt64(dataByte, 0);
                head += sizeof(ulong);
            }
            else if (typeof(T) == typeof(float))
            {
                dataByte = new byte[sizeof(float)];
                Array.Copy(buffer, head, dataByte, 0, sizeof(float));
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(dataByte);
                data = (T)(object)BitConverter.ToSingle(dataByte, 0);
                head += sizeof(float);
            }
            else if (typeof(T) == typeof(double))
            {
                dataByte = new byte[sizeof(double)];
                Array.Copy(buffer, head, dataByte, 0, sizeof(double));
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(dataByte);
                data = (T)(object)BitConverter.ToDouble(dataByte, 0);
                head += sizeof(double);
            }
            else
                throw new ArgumentException("InputMemoryStream.Read parameter can be only primitive type except decimal");
        }
        public void Read(out string data)
        {
            Read(out int length);
            char[] arr = new char[length];
            for (int i = 0; i < length; i++)
                Read(out arr[i]);
            data = new string(arr);
        }
        #endregion
    }
}
