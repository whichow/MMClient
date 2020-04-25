// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 2016-10-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;
using System.IO;

namespace K.IO
{
    public class BigEndianBinaryReader : IBinaryReader
    {
        #region Members

        private readonly Stream _stream;

        public Stream BaseStream
        {
            get { return _stream; }
        }

        #endregion

        #region Constructors

        public BigEndianBinaryReader(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            _stream = stream;
        }

        #endregion

        #region Static Methods

        public static byte ReadByte(Stream stream)
        {
            return (byte)stream.ReadByte();
        }

        public static short ReadInt16(Stream stream)
        {
            var bytes = ReadBytesFromStream(stream, 2);
            return (short)(bytes[0] << 8 | bytes[1]);
        }

        public static ushort ReadUInt16(Stream stream)
        {
            var bytes = ReadBytesFromStream(stream, 2);
            return (ushort)(bytes[0] << 8 | bytes[1]);
        }

        public static int ReadInt32(Stream stream)
        {
            var bytes = ReadBytesFromStream(stream, 4);
            return bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3];
        }

        #endregion

        #region Public Methods 

        public int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public byte ReadByte()
        {
            return (byte)_stream.ReadByte();
        }

        public byte[] ReadBytes(int length)
        {
            return ReadBytesFromStream(_stream, length);
        }

        public bool ReadBoolean()
        {
            return Convert.ToBoolean(this.ReadByte());
        }

        public char ReadChar()
        {
            return Convert.ToChar(this.ReadByte());
        }

        public short ReadInt16()
        {
            var bytes = ReadBytesFromStream(_stream, 2);
            return (short)(bytes[0] << 8 | bytes[1]);
        }

        public int ReadInt32()
        {
            var bytes = ReadBytesFromStream(_stream, 4);
            return bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3];
        }

        public long ReadInt64()
        {
            var array = ReadBytesFromStream(_stream, 8);
            return (long)((ulong)array[0] << 56 | (ulong)array[1] << 48 | (ulong)array[2] << 40 | (ulong)array[3] << 32 | (ulong)array[4] << 24 | (ulong)array[5] << 16 | (ulong)array[6] << 8 | (ulong)array[7]);
        }

        public float ReadSingle()
        {
            var array = ReadBytesFromStream(_stream, 4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array, 0, 4);
            }
            return BitConverter.ToSingle(array, 0);
        }

        public double ReadDouble()
        {
            var array = ReadBytesFromStream(_stream, 8);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array, 0, 8);
            }
            return BitConverter.ToDouble(array, 0);
        }

        public string ReadString()
        {
            int count = this.ReadInt16();
            if (count == 0)
            {
                return string.Empty;
            }

            if (count < 0)
            {
                count = this.ReadInt32();
            }

            var bytes = ReadBytesFromStream(_stream, count);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        #endregion

        #region Private Methods

        private static byte[] ReadBytesFromStream(Stream stream, int length)
        {
            var array = new byte[length];
            stream.Read(array, 0, length);
            return array;
        }

        #endregion
    }
}
