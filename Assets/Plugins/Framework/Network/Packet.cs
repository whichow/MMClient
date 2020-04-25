// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 2016-12-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;
using System.IO;
using System.Collections.Generic;

namespace K.Network
{
    using Protocol;

    /// <summary>
    /// 
    /// </summary>
    public class Packet
    {
        #region Members

        /// <summary>
        /// 
        /// </summary>
        private byte[] _buffer;
        /// <summary>
        /// 
        /// </summary>
        private int _offset;
        /// <summary>
        /// 
        /// </summary>
        private int _length;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public byte[] buffer
        {
            get { return _buffer; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int offset
        {
            get { return _offset; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int length
        {
            get { return _length; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int capacity
        {
            get { return _buffer != null ? _buffer.Length : 0; }
        }

        #endregion

        #region Constructors         

        public Packet(byte[] buffer)
        {
            _buffer = buffer;
            _offset = 0;
            _length = buffer.Length;
        }

        public Packet(byte[] buffer, int offset, int length)
        {
            _buffer = buffer;
            _offset = offset;
            _length = length;
        }

        #endregion

        #region STATIC

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Packet PackMessage(Message msg)
        {
            if (msg == null)
            {
                return default(Packet);
            }

            using (var ms = new MemoryStream(1024))
            {
                ms.WriteByte(1);
                Message.Serialize(ms, msg);
                return new Packet(ms.ToArray());
            }
        }

        /// <summary>
        /// //todo:houqixiugai
        /// </summary>
        /// <param name="msgs"></param>
        /// <returns></returns>
        public static Packet PackMessages(Message[] msgs)
        {
            if (msgs != null && msgs.Length > 0)
            {
                using (var ms = new MemoryStream(1024))
                {
                    ms.WriteByte((byte)msgs.Length);

                    for (int i = 0; i < msgs.Length; i++)
                    {
                        Message.Serialize(ms, msgs[i]);
                    }
                    return new Packet(ms.ToArray());
                }
            }
            return null;
        }

        /// <summary>
        /// todo:houqi xiugai
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static List<Message> UnpackDataToMsgs(Connector connector, byte[] data, int offset, int length)
        {
            var msgs = new List<Message>();
            using (var msgStream = new MemoryStream(data, offset, length))
            {
                int count = msgStream.ReadByte();
                for (int i = 0; i < count; i++)
                {
                    var msg = Message.Deserialize(msgStream);
                    if (msg != null)
                    {
                        msg.connector = connector;
                        msg.receiveTime = DateTime.UtcNow;
                        msgs.Add(msg);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return msgs;
        }

        /// <summary>
        /// todo:houqi xiugai
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="msgStream"></param>
        /// <returns></returns>
        public static List<Message> UnpackDataToMsgs(Connector connector, MemoryStream msgStream)
        {
            var msgs = new List<Message>();
            int count = msgStream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                var msg = Message.Deserialize(msgStream);
                if (msg != null)
                {
                    msg.connector = connector;
                    msg.receiveTime = DateTime.UtcNow;
                    msgs.Add(msg);
                }
                else
                {
                    return null;
                }
            }
            return msgs;
        }

        #endregion
    }
}
