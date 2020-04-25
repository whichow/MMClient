//// ***********************************************************************
//// Company          : 
//// Author           : KimCh
//// Created          : 
////
//// Last Modified By : KimCh
//// Last Modified On : 
//// ***********************************************************************

//namespace Game
//{
//    using Google.Protobuf;
//    using Msg.ClientMessage;
//    using System;
//    using System.Collections;

//    /// <summary>
//    /// 心跳包
//    /// </summary>
//    public class PacketLive : PacketBinary
//    {
//        public void Init()
//        {
//            //base.Init(HeartBeat.MessageType, null);
//            base.Init(ProtocolHelper.GetMsgID(typeof(S2CHeartbeat)), null);
//        }

//        protected override void GenerateData(IMessage data)
//        {
//            var liveRequest = new C2SHeartbeat();
//            base.GenerateData(liveRequest);
//        }
//    }
//}
