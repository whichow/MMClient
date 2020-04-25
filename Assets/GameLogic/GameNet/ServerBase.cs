using Framework.Core;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Msg.ClientMessage;
using System;
using System.Collections.Generic;

namespace Game
{
    public abstract class ServerBase : IUpdateable, IDispose
    {
        #region getter
        protected Dictionary<int, NetMsgEventHandle> DictNetHandle { get; private set; }

        /// <summary>
        /// 获取网络消息处理器
        /// </summary>
        /// <param name="msgID"></param>
        /// <returns></returns>
        protected NetMsgEventHandle GetNetHandler(int msgID)
        {
            NetMsgEventHandle value = null;
            if (!DictNetHandle.TryGetValue(msgID, out value))
            {
                Debuger.LogError("找不到Key:" + msgID);
            }
            return value;
        }

        protected bool _blEnable;
        public bool blEnable
        {
            get { return _blEnable; }
        }
        #endregion

        protected ByteStream _recvStream;
        protected List<NetMsgRecvData> _lstRecvDatas;
        protected List<int> _lstRequestID;
        protected GameRequestBase _curRequest = null;

        public ServerBase()
        {
            _recvStream = new ByteStream();
            _lstRecvDatas = new List<NetMsgRecvData>();
            _lstRequestID = new List<int>();
            InitNetMsgHandle();
        }

        protected virtual void InitNetMsgHandle()
        {
            _blEnable = true;
            DictNetHandle = new Dictionary<int, NetMsgEventHandle>();
        }

        protected void RegisterNetMsgType<T>(int msgId, MessageParser msgParser, Action<T> method) where T : IMessage
        {
            if (DictNetHandle.ContainsKey(msgId))
            {
                Debuger.LogError("[ServerBase.RegisterNetMsgType() => msgId:" + msgId + "重复注册]");
                return;
            }
            DictNetHandle.Add(msgId, new NetMsgEventHandle(msgId, msgParser, method));
        }

        protected void RegisterNetMsgType<T>(MessageDescriptor descriptor, Action<T> method) where T : IMessage
        {
            int protoID = 0;
            //protoID = ProtocolHelper.GetMsgID(descriptor.ClrType);
            protoID = ProtocolHelper.GetMsgID(descriptor);
            if (protoID > 0)
            {
                //Debuger.Log("注册消息: " + descriptor.Name + " = " + protoID);
                RegisterNetMsgType(protoID, descriptor.Parser, method);
            }
        }

        protected virtual void OnDataError()
        {
            if (_curRequest != null)
            {
                _curRequest.Dispose();
                _curRequest = null;
            }
        }

        protected void ProccessOneMsg(S2C_ONE_MSG value)
        {
#if DEBUG_MY
            Debuger.Log(string.Format("[收到S2C_ONE_MSG - {0}] {1}", value.MsgCode, value.ToString()));
#endif
            NetMsgRecvData lastData = null;
            _recvStream.Clear();
            byte[] bytes = value.Data.ToByteArray();
            _recvStream.AddBytes(bytes);
            while (_recvStream.BytesAvailable >= 4)
            {
                int msgId = _recvStream.ReadInt(); //消息id
                int msgLen = _recvStream.ReadInt(); //消息长度
                byte[] msgData = _recvStream.ReadBytes(msgLen);
                if (DictNetHandle.ContainsKey(msgId))
                {
                    NetMsgEventHandle handle = DictNetHandle[msgId];
                    NetMsgRecvData data = NetPacketHelper.Read(msgId, msgData, handle, value.MsgCode, false);
                    if (data != null)
                    {
                        _lstRecvDatas.Add(data);

#if DEBUG_MY
                        MessageParser parser = GameApp.Instance.GameServer.GetNetHandler(msgId).mMsgParser;
                        var data2 = data.mData as IMessage;
                        Debuger.Log(string.Format("[收到数据消息体:{0}-{1}] {2}", data2.Descriptor.Name, msgId, data2.ToString()));
#endif
                    }
                    lastData = data;
                }
                else
                {
                    Debuger.LogError("[ServerBase.ProccessOneMsg() <= 消息号:" + msgId + "未注册]");
                }
            }
            // 兼容老项目的请求回调用的，改完老项目可删除
            if (lastData != null)
            {
                lastData.mIsCompleted = true;
                lastData = null;
            }
            else
            {
                Debuger.LogErrorFormat("[S2C_ONE_MSG.Data为空] {0}", value.MsgCode);
            }
        }

        protected Queue<C2S_ONE_MSG> _postDataPools = new Queue<C2S_ONE_MSG>();
        protected virtual void Send(int msgId, IMessage msg)
        {
#if DEBUG_MY
            Debuger.Log(string.Format("[发送数据消息体:{0}-{1}] {2}", msg.Descriptor.Name, msgId, msg.ToString()));
#endif

            if (_lstRequestID.Contains(msgId))
                return;
            _lstRequestID.Add(msgId);
            C2S_ONE_MSG pd = new C2S_ONE_MSG();
            pd.MsgCode = msgId;
            pd.Data = msg.ToByteString();
            _postDataPools.Enqueue(pd);
            CheckSendData();
        }

        public virtual void Update()
        {
            NetMsgRecvData recvData = null;
            while ((recvData = GetRecvDate()) != null)
            {
                recvData.Exec();
                if (_lstRequestID.Contains(recvData.mSendMsgID))
                    _lstRequestID.Remove(recvData.mSendMsgID);
            }
            CheckSendData();
        }

        private void CheckSendData()
        {
            if (_curRequest != null && !_curRequest.BlEnd)
                return;
            if (_curRequest != null)
            {
                _curRequest.Dispose();
                _curRequest = null;
            }
            if (_postDataPools.Count <= 0)
            {
                _curRequest = null;
                _lstRequestID.Clear();
                return;
            }
            DoRequestSend();
        }

        protected abstract void OnReceiveData(object data);
        protected abstract void DoRequestSend();

        private NetMsgRecvData GetRecvDate()
        {
            if (_lstRecvDatas == null || _lstRecvDatas.Count == 0)
                return null;
            NetMsgRecvData data = _lstRecvDatas[0];
            _lstRecvDatas.RemoveAt(0);
            return data;
        }

        public virtual void Dispose()
        {
            if (DictNetHandle != null)
            {
                foreach (var kv in DictNetHandle)
                    kv.Value.Dispose();
                DictNetHandle.Clear();
                DictNetHandle = null;
            }
            if (_lstRecvDatas != null)
            {
                _lstRecvDatas.Clear();
                _lstRecvDatas = null;
            }

            if (_lstRequestID != null)
            {
                _lstRequestID.Clear();
                _lstRequestID = null;
            }

            if (_postDataPools != null)
            {
                _postDataPools.Clear();
                _postDataPools = null;
            }
            if (_curRequest != null)
            {
                _curRequest.Dispose();
                _curRequest = null;
            }
        }
    }
}