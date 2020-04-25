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
    public interface IBinaryReader
    {
        Stream BaseStream
        {
            get;
        }

        int Read(byte[] buffer, int offset, int count);

        byte ReadByte();

        byte[] ReadBytes(int length);

        bool ReadBoolean();

        char ReadChar();

        short ReadInt16();

        int ReadInt32();

        long ReadInt64();

        float ReadSingle();

        double ReadDouble();

        string ReadString();
    }
}
