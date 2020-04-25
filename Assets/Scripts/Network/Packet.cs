// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
namespace Game
{
    using Google.Protobuf;
    using System.Collections;
    using System.Collections.Generic;

    using PacketCallback = System.Action<int, string, object>;

    public abstract class Packet
    {
        #region PACKETDATA

        /// <summary>The _callback</summary>
        protected PacketCallback _callback = null;

        ///// <summary>Gets the send bytes.</summary>
        //public abstract byte[] sendBytes
        //{
        //    get;
        //}

        ///// <summary>Sets the recv BYTES.</summary>
        //public abstract byte[] recvBytes
        //{
        //    set;
        //}

        ///// <summary>Sets the recv error.</summary>
        //public abstract string recvError
        //{
        //    set;
        //}

        ///// <summary>
        ///// 协议号
        ///// </summary>
        //public abstract int code
        //{
        //    get;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //public string sendURL
        //{
        //    get;
        //    protected set;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public abstract Dictionary<string, string> headers
        //{
        //    get;
        //}

        /// <summary>Initializes the specified code.</summary>
        /// <param name="code">The code.</param>
        /// <param name="callback">The callback.</param>
        public virtual Packet Init(int code, PacketCallback callback)
        {
            _callback = callback;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public virtual void AddData(IMessage data)
        {

        }

        ///// <summary>Generates this instance.serial</summary>
        //public virtual void Generate(int serial)
        //{

        //}

        public virtual IMessage MsgParam
        {
            get { return null; }
        }

        public virtual void Invoke(int code, string message, object data)
        {
            K.Events.EventInvoker.Invoke(_callback, code, message, data);
        }

        #endregion
    }
}
