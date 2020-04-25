using Msg.ClientMessage;
using System;
//using Msg.ClientMessageId;
using System.Collections.Generic;

namespace Game
{
    public class GamePostRequest : GameRequestBase
    {
        public bool mBlEnd { get; private set; }
        //public static List<MSGID> _lstFilters = new List<MSGID>() { MSGID.C2SChatMsgPullRequest, MSGID.C2SHeartbeat, MSGID.C2SGuideDataSaveRequest, MSGID.C2SChatMsgPullRequest };

        public GamePostRequest(string url, Action<object> onCompleteMethod, Action onErrorMethod = null)
            : base(url, onCompleteMethod, onErrorMethod)
        {
            _lstMsgId = new List<int>();
            mBlEnd = false;
            mBlNeedCheckMask = true;
        }

        public override void InitPostData(byte[] data)
        {
            base.InitPostData(data);
            mBlNeedCheckMask = _count != _lstMsgId.Count;
        }

        private int _count = 0;
        public override void AddMsgID(int msgId)
        {
            base.AddMsgID(msgId);
            _lstMsgId.Add(msgId);
            //if (_lstFilters.Contains((MSGID)msgId))
            //    _count++;
        }

        int compType;
        private ByteStream _bStream = new ByteStream();
        protected override void DoParseData()
        {
            if (string.IsNullOrEmpty(_webRequestAsync.webRequest.downloadHandler.text))
            {
                Debuger.LogError("[GamePostRequest.DoParseData() => get data was empty!!!]");
                CheckReSend();
                return;
            }
            byte[] data = _webRequestAsync.webRequest.downloadHandler.data;
            //_bStream.Clear();
            //_bStream.AddBytes(data);
            //byte[] bodyBytes = null;
            //byte compByte;
            //Debuger.Log("[GamePostRequest.DoParseData() => decompress before msg data len:" + data.Length + "]");
            //if (data.Length == 1)
            //{
            //    //empty message data
            //    compByte = _bStream.ReadByte();
            //}
            //else
            //{
            //    bodyBytes = _bStream.ReadBytes(data.Length - 1);
            //    compByte = _bStream.ReadByte();
            //}
            //if (bodyBytes != null)
            //{
            //    compType = (int)compByte;
            //    if (compType == 1)
            //        bt = NetByteHelper.DecompressDataByZip(bodyBytes);
            //    if (compType == 2)
            //        data = NetByteHelper.DecompressDataBySnappy(bodyBytes);
            //    else
            //        data = bodyBytes;
            //}
            //Debuger.Log("[GamePostRequest DoParseData() <== decompress end byte length:" + data.Length + ", compType:" + compType + ", cost time:" + (UnityEngine.Time.realtimeSinceStartup - _flSendTime) + "]");
            S2C_MSG_DATA msgData = null;
            try
            {
                msgData = S2C_MSG_DATA.Parser.ParseFrom(data);
            }
            catch (System.Exception ex)
            {
                Debuger.LogErrorFormat("PB解析错误：Length={0} \n{1}", data.Length, ex);
            }
            mBlEnd = true;
            _status = HttpsStatus.None;
            if (_onCompleteMethod != null && msgData != null)
                _onCompleteMethod.Invoke(msgData);
        }
    }
}