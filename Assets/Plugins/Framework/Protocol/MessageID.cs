// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 2016-10-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************

namespace K.Protocol
{
    /// <summary>
    /// 消息标识
    /// </summary>
    public struct MessageID
    {
        #region STATIC

        /// <summary>
        /// 
        /// </summary>
        public static MessageID Null = new MessageID(0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public static implicit operator short(MessageID id)
        {
            return id.value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public static implicit operator MessageID(short id)
        {
            return new MessageID(id);
        }

        #endregion

        #region MEMBER

        /// <summary>
        /// 
        /// </summary>
        public short value
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public MessageID(short id)
        {
            this.value = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var name = Message.GetMessageNameByID(this);
            return string.IsNullOrEmpty(name) ? value.ToString() : name;
        }

        #endregion
    }
}
