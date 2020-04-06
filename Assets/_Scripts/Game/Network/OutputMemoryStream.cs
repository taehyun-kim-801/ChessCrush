using System;

namespace ChessCrush.Game
{
    public class OutputMemoryStream
    {
        public byte[] buffer { get; private set; }
        public int head { get; private set; }
        private int capacity;

        public OutputMemoryStream()
        {
            buffer = new byte[32];
            head = 0;
            capacity = 0;
        }

        #region Write
        public void Write<T>(T data) where T: struct
        {
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
                throw new ArgumentException("OutputMemoryStream.Write parameter can be only primitive type except decimal");
            if (BitConverter.IsLittleEndian)
                Array.Reverse(dataBytes);

            int resultHead = head + dataBytes.Length;
            if(resultHead>capacity)
            {
                byte[] newBuffer = new byte[buffer.Length * 2];
                buffer.CopyTo(newBuffer, 0);
                buffer = newBuffer;
                capacity = buffer.Length;
            }

            dataBytes.CopyTo(buffer, head);
            head = resultHead;
        }

        public void Write(string data)
        {
            Write(data.Length);
            foreach(var c in data)
            {
                Write(c);
            }
        }

        #endregion
    }
}
