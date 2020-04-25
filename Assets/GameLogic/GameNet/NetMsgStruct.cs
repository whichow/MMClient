using Game;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Game
{
    public class NetMsgEventHandle
    {
        public MessageParser mMsgParser { get; private set; }
        public Delegate mCallBack { get; private set; }
        public int mMsgId { get; private set; }

        public NetMsgEventHandle(int msgId, MessageParser parser, Delegate method)
        {
            mMsgParser = parser;
            mMsgId = msgId;
            mCallBack = method;
        }

        public void Dispose()
        {
            mMsgParser = null;
            mCallBack = null;
        }
    }

    public class NetMsgRecvData
    {
        public Delegate mCallBack;
        public object mData;
        public int mSendMsgID;

        /// <summary>
        /// 兼容老项目的请求回调用的，改完老项目可删除
        /// </summary>
        public bool mIsCompleted;

        public NetMsgRecvData(Delegate method, object data, int sendMsgID, bool isCompleted)
        {
            mCallBack = method;
            mData = data;
            mSendMsgID = sendMsgID;
            mIsCompleted = isCompleted;
        }

        public void Exec()
        {
            //LogHelper.Log(mCallBack.Method.Name);
            if (mCallBack != null)
                mCallBack.Method.Invoke(mCallBack.Target, new object[] { mData });
            mCallBack = null;

            //临时处理原网络请求回调方法 , 各个模块移完后此处删除
            KServer.ProcessData(new ArrayList() { mData });
            var packet = PacketManager.Instance.GetPacketAndRemove(mSendMsgID);
            if (packet != null)
            {
                m_tempPacketDic[mSendMsgID] = packet;
            }
            else
            {
                m_tempPacketDic.TryGetValue(mSendMsgID, out packet);
            }
            ArrayList list;
            if (!m_tempAgrsDic.TryGetValue(mSendMsgID, out list))
            {
                list = new ArrayList();
                m_tempAgrsDic.Add(mSendMsgID, list);
            }
            list.Add(mData);
            if (mIsCompleted)
            {
                if (packet != null)
                {
                    packet.Invoke(0, "", list);
                }
                m_tempAgrsDic.Remove(mSendMsgID);
                m_tempPacketDic.Remove(mSendMsgID);
            }
            mData = null;
        }

        private static Dictionary<int, Packet> m_tempPacketDic = new Dictionary<int, Packet>();
        private static Dictionary<int, ArrayList> m_tempAgrsDic = new Dictionary<int, ArrayList>();
    }

    //public class HttpFrom
    //{
    //    private string _url;
    //    private string _datas;

    //    public HttpFrom(string url)
    //    {
    //        _url = url;
    //        _datas = "";
    //    }

    //    public void AddValue(string key, string value)
    //    {
    //        if (string.IsNullOrEmpty(_datas))
    //            _datas = key + "=" + value;
    //        else
    //            _datas += ("&" + key + "=" + value);
    //    }

    //    public void AddValue(string key, int value)
    //    {
    //        AddValue(key, value.ToString());
    //    }

    //    public void AddValue(string key, float value)
    //    {
    //        AddValue(key, value.ToString());
    //    }

    //    public override string ToString()
    //    {
    //        return _url + "?" + _datas;
    //    }
    //}

    public class NetPacketHelper
    {
        public static NetMsgRecvData Read(int msgId, byte[] bts, NetMsgEventHandle handle, int sendMsgID, bool isCompleted)
        {
            MemoryStream ms = new MemoryStream(bts);
            ms.SetLength(bts.Length);

            NetMsgRecvData data = null;
            try
            {
                IMessage md = handle.mMsgParser.ParseFrom(bts);
                data = new NetMsgRecvData(handle.mCallBack, md, sendMsgID, isCompleted);
            }
            catch (Exception ex)
            {
                Debuger.LogError("[NetPacketHelper.Read() => 反序列化数据出错，ex:" + ex + "]");
            }
            ms.Close();
            return data;
        }
    }
}