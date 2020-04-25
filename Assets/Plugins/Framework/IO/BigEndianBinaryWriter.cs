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
    public sealed class BigEndianBinaryWriter : IBinaryWriter
    {
        #region Members

        private readonly Stream _stream;

        public Stream BaseStream
        {
            get
            {
                return this._stream;
            }
        }

        #endregion

        #region Constructors

        public BigEndianBinaryWriter(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            _stream = stream;
        }

        #endregion

        #region Static Methods

        public static void WriteByte(Stream stream, byte value)
        {
            stream.WriteByte(value);
        }

        public static void WriteInt16(Stream stream, short value)
        {
            stream.Write(new byte[]
            {
                (byte)(value >> 8),
                (byte)(value)
            }, 0, 2);
        }

        public static void WriteInt32(Stream stream, int value)
        {
            stream.Write(new byte[]
            {
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)(value)
            }, 0, 4);
        }

        #endregion

        #region Public Methods

        public void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }

        public void WriteBytes(byte[] value)
        {
            if (value != null && value.Length > 0)
            {
                _stream.Write(value, 0, value.Length);
            }
        }

        public void WriteBoolean(bool value)
        {
            var bValue = Convert.ToByte(value);
            _stream.WriteByte(bValue);
        }

        public void WriteChar(char value)
        {
            var bValue = Convert.ToByte(value);
            _stream.WriteByte(bValue);
        }

        public void WriteInt16(short value)
        {
            var buffer = new byte[]
            {
                (byte)(value >> 8),
                (byte)(value)
            };
            _stream.Write(buffer, 0, 2);
        }

        public void WriteInt32(int value)
        {
            var buffer = new byte[]
            {
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)(value)
            };
            _stream.Write(buffer, 0, 4);
        }

        public void WriteInt64(long value)
        {
            var buffer = new byte[]
            {
                (byte)(value >> 56),
                (byte)(value >> 48),
                (byte)(value >> 40),
                (byte)(value >> 32),
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)(value)
            };
            _stream.Write(buffer, 0, 8);
        }

        public void WriteSingle(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            _stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteDouble(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            _stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                this.WriteInt16(0);
                return;
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(value);
            int count = bytes.Length;
            if (count <= short.MaxValue)
            {
                this.WriteInt16((short)count);
            }
            else
            {
                this.WriteInt16(-1);
                this.WriteInt32(count);
            }
            _stream.Write(bytes, 0, bytes.Length);
        }

        #endregion

    }
}
