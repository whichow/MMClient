// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 2016-10-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System.IO;

namespace K.IO
{
    public interface IBinaryWriter
    {
        Stream BaseStream
        {
            get;
        }

        void WriteByte(byte value);

        void WriteBytes(byte[] value);

        void WriteBoolean(bool value);

        void WriteChar(char value);

        void WriteDouble(double value);

        void WriteSingle(float value);

        void WriteInt16(short value);

        void WriteInt32(int value);

        void WriteInt64(long value);

        void WriteString(string value);
    }
}
