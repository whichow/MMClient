// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Game
{
    using Google.Protobuf;
    using Msg.ClientMessage;
    using PacketCallback = System.Action<int, string, object>;

    /// <summary>
    /// ProtoBuf
    /// </summary>
    public class PacketBinary : Packet
    {
        #region Helper

        //private static MemoryStream _HelpMS1 = new MemoryStream(51200);
        //private static MemoryStream _HelpMS2 = new MemoryStream(10240);

        //private static MemoryStream GetHelpMS1()
        //{
        //    _HelpMS1.SetLength(0);
        //    return _HelpMS1;
        //}

        //private static MemoryStream GetHelpMS1(byte[] buffer)
        //{
        //    _HelpMS1.SetLength(0);
        //    _HelpMS1.Write(buffer, 0, buffer.Length);
        //    _HelpMS1.Flush();
        //    _HelpMS1.Position = 0;
        //    return _HelpMS1;
        //}

        //private static MemoryStream GetHelpMS1(byte[] buffer, int index, int count)
        //{
        //    _HelpMS1.SetLength(0);
        //    _HelpMS1.Write(buffer, index, count);
        //    _HelpMS1.Flush();
        //    _HelpMS1.Position = 0;
        //    return _HelpMS1;
        //}

        //private static MemoryStream GetHelpMS2()
        //{
        //    _HelpMS2.SetLength(0);
        //    return _HelpMS2;
        //}

        //private static MemoryStream GetHelpMS2(byte[] buffer)
        //{
        //    _HelpMS2.SetLength(0);
        //    _HelpMS2.Write(buffer, 0, buffer.Length);
        //    _HelpMS2.Flush();
        //    _HelpMS2.Position = 0;
        //    return _HelpMS2;
        //}

        //private static MemoryStream GetHelpMS2(byte[] buffer, int index, int count)
        //{
        //    _HelpMS2.SetLength(0);
        //    _HelpMS2.Write(buffer, index, count);
        //    _HelpMS2.Flush();
        //    _HelpMS2.Position = 0;
        //    return _HelpMS2;
        //}

        #endregion

        #region Field

        ///// <summary>
        ///// 数据包
        ///// </summary>
        //private byte[] _datagram;

        /// <summary>
        /// 
        /// </summary>
        private C2S_ONE_MSG _msgData;
        /// <summary>
        /// 
        /// </summary>
        private IMessage _msgParam;

        #endregion

        public override IMessage MsgParam
        {
            get { return this._msgParam; }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public override int code
        //{
        //    get { return _msgData.MsgCode; }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //public string codeName
        //{
        //    get
        //    {
        //        //return MessageHelper.GetMessageName((ushort)code);
        //        Debuger.Log("codeName待处理");
        //        return code.ToString();
        //        //return App.Instance.GameServer.GetNetHandle(code).mMsgType.Name;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //public override Dictionary<string, string> headers
        //{
        //    get { return null; }
        //}
//        /// <summary>
//        /// 
//        /// </summary>
//        public override byte[] sendBytes
//        {
//            get
//            {
//                if (_datagram != null && _datagram.Length > 0)
//                {
//#if DEBUG_MY
//                    UnityEngine.Debug.Log("发送数据:" + codeName + " length:" + _datagram.Length);
//#endif
//                    return _datagram;
//                }
//                else
//                {
//#if DEBUG_MY
//                    UnityEngine.Debug.Log("发送数据错误:");
//#endif
//                    return new byte[0];
//                }
//            }
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        public override byte[] recvBytes
//        {
//            set
//            {
//                if (value != null && value.Length > 0)
//                {
//#if DEBUG_MY
//                    UnityEngine.Debug.Log("收到数据:" + codeName + " length:" + value.Length);
//#endif
//                    S2C_ONE_MSG msgData = null;
//                    //var stream = GetHelpMS1(value);
//                    try
//                    {
//                        //msgData = Serializer.Deserialize<S2C_MSG_DATA>(stream);
//                        //msgData = S2C_MSG_DATA.Parser.ParseFrom(stream.GetBuffer());
//                        //msgData = S2C_MSG_DATA.Parser.ParseFrom(stream);
//                        msgData = S2C_ONE_MSG.Parser.ParseFrom(value);
//                    }
//                    catch (System.Exception ex)
//                    {
//                        UnityEngine.Debug.LogException(ex);
//                    }
//                    Handling(msgData);
//                }
//                else
//                {
//                    HandlingError(null);
//                }
//            }
//        }

//        /// <summary>Sets the recv error.</summary>
//        public override string recvError
//        {
//            set
//            {
//#if UNITY_EDITOR || DEBUG_MY
//                UnityEngine.Debug.Log("网络错误: " + code + " " + value);
//#endif
//                KServer.ProcessError(501);
//                if (_callback != null)
//                {
//                    _callback(501, "网络连接超时，请检查网络设置", value);
//                }
//            }
//        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public override Packet Init(int code, PacketCallback callback)
        {
            if (_msgData == null)
            {
                _msgData = new C2S_ONE_MSG();
            }

            _msgData.MsgCode = code;
            _callback = callback;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override void AddData(IMessage data)
        {
            Debuger.Log(string.Format("添加消息体：[{0}] {1}", data.Descriptor.Name, data.ToString()));
            _msgParam = data;
        }

        ///// <summary>Generates this instance.serial</summary>
        //public override void Generate(int serial)
        //{
        //    //_msgData.PlayerId = KUser.PlayerId;
        //    //_msgData.Token = KUser.PlayerToken;

        //    GenerateData(_msgParam);

        //    var stream = GetHelpMS2();
        //    //Serializer.Serialize(stream, _msgData);
        //    _msgData.WriteTo(stream);
        //    _datagram = stream.ToArray();
        //}

        ///// <summary>Handlings the specified table.</summary>
        ///// <param name="table">The table.</param>
        //public void Handling(S2C_ONE_MSG msgData)
        //{
        //    if (HandlingError(msgData))
        //    {
        //        return;
        //    }

        //    var dataList = new ArrayList();

        //    //var buffer = msgData.Data;
        //    var buffer = msgData.Data.ToByteArray();
        //    if (buffer != null && buffer.Length > 0)
        //    {
        //        var stream = GetHelpMS1(buffer);
        //        while (stream.Length - stream.Position >= 4)
        //        {
        //            ushort code = K.IO.BigEndianBinaryReader.ReadUInt16(stream);
        //            ushort count = K.IO.BigEndianBinaryReader.ReadUInt16(stream);

        //            if (stream.Length - stream.Position < count)
        //            {
        //                break;
        //            }

        //            var stream2 = GetHelpMS2(buffer, (int)stream.Position, count);
        //            stream.Position += count;
        //            try
        //            {
        //                //var data = MessageHelper.Deserialize(stream2, code);
        //                MessageParser parser = GameNetMgr.Instance.mGameServer.GetNetHandler(code).mMsgParser;
        //                var data = parser.ParseFrom(stream2);

        //                Debuger.Log(string.Format("收到数据消息体：[{0}] {1}", data.Descriptor.Name, data.ToString()));

        //                dataList.Add(data);
        //            }
        //            catch(System.Exception e)
        //            {
        //                Debuger.LogError("Protocol Deserialize Failed! \n" + e);
        //            }
        //        }
        //    }

        //    HandlingData(dataList);
        //}

        ///// <summary>Generates the data.</summary>
        ///// <param name="table">The table.</param>
        //protected virtual void GenerateData(IMessage data)
        //{
        //    if (data != null)
        //    {
        //        var stream = GetHelpMS2();
        //        //Serializer.Serialize(stream, data);
        //        data.WriteTo(stream);
        //        _msgData.Data = ByteString.CopyFrom(stream.ToArray());
        //    }

        //}

        ///// <summary>Handlings the data.</summary>
        ///// <param name="table">The table.</param>
        ///// <returns>true 自己处理回调函数 否则返回 false.</returns>
        //protected virtual void HandlingData(ArrayList dataList)
        //{
        //    KServer.ProcessData(dataList);
        //    HandlingSuccess(dataList);
        //}

        ///// <summary>Handlings the error.</summary>
        ///// <param name="errCode">The error code.</param>
        //protected virtual bool HandlingError(object data)
        //{
        //    var msgData = data as S2C_ONE_MSG;
        //    if (msgData == null)
        //    {
        //        KServer.ProcessError(1);
        //        K.Events.EventInvoker.Invoke(_callback, 1, null, null);
        //        return true;
        //    }
        //    if (msgData.ErrorCode < 0)
        //    {
        //        KServer.ProcessError(msgData.ErrorCode);
        //        //K.Events.EventInvoker.Invoke(_callback, msgData.ErrorCode, msgData.ErrorMsg, null);
        //        return true;
        //    }
        //    //if (msgData.Data == null)
        //    //{
        //    //    KServer.ProcessError(2);
        //    //    if (_callback != null)
        //    //    {
        //    //        _callback(2, "message数据为空", null);
        //    //    }
        //    //    return true;
        //    //}
        //    return false;
        //}

        ///// <summary>Handlings the success.</summary>
        ///// <param name="succCode">The succ code.</param>
        ///// <param name="succMessage">The succ message.</param>
        //protected virtual void HandlingSuccess(object data)
        //{
        //    K.Events.EventInvoker.Invoke(_callback, 0, null, data);
        //}
    }
}
