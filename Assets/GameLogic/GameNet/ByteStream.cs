using System;

namespace Game
{
    public class ByteStream
    {
        public static int DefaultSize = 64;
        public byte[] mBuff { get; private set; }
        public int mUsedLen { get; set; }
        public int mCapacity { get; private set; }
        private int _curIndex;

        public ByteStream()
        {

        }

        public byte ReadByte()
        {
            _curIndex++;
            return mBuff[_curIndex - 1];
        }

        public byte[] ReadBytes(int len)
        {
            byte[] bs = new byte[len];
            Buffer.BlockCopy(mBuff, _curIndex, bs, 0, len);
            _curIndex += len;
            return bs;
        }

        public int ReadInt()
        {
            int a = (int)ReadByte();
            int b = (int)ReadByte();
            return (a << 8) + b;
        }

        public void AddBytes(byte[] value)
        {
            mBuff = value;
            mUsedLen = value.Length;
            _curIndex = 0;
        }

        public int BytesAvailable
        {
            get { return mUsedLen - _curIndex; }
        }

        public void Clear()
        {
            _curIndex = 0;
            mUsedLen = 0;
        }
    }
}