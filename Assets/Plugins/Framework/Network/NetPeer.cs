// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 2016-10-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace K.Network
{
    using Events;
    using Protocol;

    /// <summary>
    /// 连接节点
    /// </summary>
    public class NetPeer : Object
    {
        #region Events

        /// <summary>
        /// 
        /// </summary>
        public readonly SimpleEvent<NetPeer> onConnected = new SimpleEvent<NetPeer>();
        /// <summary>
        /// 
        /// </summary>
        public readonly SimpleEvent<NetPeer> onDisconnected = new SimpleEvent<NetPeer>();

        #endregion

        #region Members

        /// <summary>
        /// 消息响应
        /// </summary>
        private Dictionary<MessageID, SimpleEvent<Message>> _registerMsgEvents = new Dictionary<MessageID, SimpleEvent<Message>>();

        /// <summary>
        /// 节点连接
        /// </summary>
        private Connector _connector;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool connected
        {
            get { return _connector != null && _connector.connected; }
        }

        /// <summary>
        /// 节点连接
        /// </summary>
        public Connector connector
        {
            get { return _connector; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connector"></param>
        public NetPeer(Connector connector)
        {
            _connector = connector;
            if (_connector != null)
            {
                _connector.peer = this;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //protected override void OnDispose()
        //{
        //    if (_connector != null)
        //    {
        //        Dispose(_connector);
        //        _connector = null;
        //    }
        //    base.OnDispose();
        //}

        #endregion

        #region Public Methods 

        /// <summary>
        /// 连接
        /// </summary>
        public void Connect(Connector connector)
        {
            if (connector == null || connector == _connector)
            {
                return;
            }

            if (_connector != null)
            {
                _connector.peer = null;
                //Dispose(_connector);
            }

            _connector = connector;

            if (_connector != null)
            {
                _connector.peer = this;
            }
        }

        /// <summary>
        /// 主动断开连接
        /// </summary>
        public virtual void Disconnect()
        {
            if (_connector != null)
            {
                //Dispose(_connector);
                _connector = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(Message msg)
        {
            if (this.connector == null || !this.connector.connected)
            {
                //Util.Log("连接器未连接");
                return;
            }
            var packet = Packet.PackMessage(msg);
            this.connector.Send(packet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgs"></param>
        public void SendMsgs(Message[] msgs)
        {

        }

        /// <summary>
        /// 连接器已断开
        /// </summary>
        public virtual void OnDisconnected()
        {
            if (_connector != null)
            {
                //Dispose(_connector);
                _connector = null;
            }
            onDisconnected.Invoke(this);
        }

        /// <summary>
        /// 收到数据包
        /// </summary>
        /// <param name="packet"></param>
        internal void OnReceived(Packet packet)
        {
            var msgs = Packet.UnpackDataToMsgs(this.connector, packet.buffer, packet.offset, packet.length);
            if (msgs == null || msgs.Count == 0)
            {
                return;
            }
            //
            ProcessMsg(msgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="msgAction"></param>
        public void RegisterReceiveMsg(MessageID msgID, Action<Message> msgAction)
        {
            SimpleEvent<Message> msgEvent;
            if (!_registerMsgEvents.TryGetValue(msgID, out msgEvent))
            {
                _registerMsgEvents.Add(msgID, msgEvent = new SimpleEvent<Message>());
            }
            msgEvent.Add(msgAction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="msgAction"></param>
        public void UnregisterReceiveMsg(MessageID msgID, Action<Message> msgAction)
        {
            SimpleEvent<Message> msgEvent;
            if (_registerMsgEvents.TryGetValue(msgID, out msgEvent))
            {
                msgEvent.Remove(msgAction);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        protected virtual void ProcessMsg(IList<Message> msgs)
        {
            foreach (var msg in msgs)
            {
                SimpleEvent<Message> msgEvent;
                if (_registerMsgEvents.TryGetValue(msg.messageID, out msgEvent))
                {
                    msgEvent.Invoke(msg);
                }
            }
        }

        #endregion 

    }
}
