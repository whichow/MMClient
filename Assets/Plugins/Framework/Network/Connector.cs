// ***********************************************************************
// Company          : 
// Author           : KimCh
// Copyright(c)     : KimCh
//
// Last Modified By : KimCh
// Last Modified On : KimCh
// ***********************************************************************
using System;
using UnityEngine;

namespace K.Network
{
    using Events;
    using Protocol;

    /// <summary>
    /// 连接器
    /// </summary>
    public abstract class Connector : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// 成功连接事件(返回失败信息)
        /// </summary>
        public readonly SimpleEvent<Connector, string> onConnected = new SimpleEvent<Connector, string>();
        /// <summary>
        /// 断开连接事件(返回失败原因)
        /// </summary>
        public readonly SimpleEvent<Connector, string> onDisconnected = new SimpleEvent<Connector, string>();
        /// <summary>
        /// 接收事件
        /// </summary>
        public readonly SimpleEvent<Connector, Message> onReceived = new SimpleEvent<Connector, Message>();

        #endregion

        #region Properties

        /// <summary>
        /// 远程端点
        /// </summary>
        public virtual string remoteAddress
        {
            get { return null; }
        }

        /// <summary>
        /// 连接
        /// </summary>
        public virtual bool connected
        {
            get { return true; }
        }

        /// <summary>
        /// 节点
        /// </summary>
        public NetPeer peer
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public Connector()
        {

        }

        //protected override void OnDispose()
        //{
        //    try
        //    {
        //        this.Disconnect();
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.LogException(ex);
        //    }
        //    base.OnDispose();
        //}

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public virtual void Start()
        {
            // 主线程调用
        }

        /// <summary>
        /// 连接
        /// </summary>
        public virtual void Connect()
        {
            onConnected.Invoke(this, null);
        }

        /// <summary>
        /// 断开
        /// </summary>
        public virtual void Disconnect()
        {
            if (this.peer != null)
            {
                this.peer.OnDisconnected();
            }
            onDisconnected.Invoke(this, null);
        }

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="packet"></param>
        public virtual void Send(Packet packet)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        public void OnReceive(Packet packet)
        {
            if (this.peer != null)
            {
                this.peer.OnReceived(packet);
                return;
            }

            var msgs = Packet.UnpackDataToMsgs(this, packet.buffer, packet.offset, packet.length);
            if (msgs == null || msgs.Count == 0)
            {
                return;
            }

            for (int i = 0; i < msgs.Count; i++)
            {
                this.onReceived.Invoke(this, msgs[i]);
            }
        }

        #endregion

    }
}
