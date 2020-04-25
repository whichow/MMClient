//// ***********************************************************************
//// Company          : 
//// Author           : KimCh
//// Copyright(c)     : KimCh
////
//// Last Modified By : KimCh
//// Last Modified On : KimCh
//// ***********************************************************************
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//namespace K.Network
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class HttpConnector : Connector
//    {
//        #region Field

//        public readonly Dictionary<string, string> headers = new Dictionary<string, string>();

//        #endregion

//        #region Method

//        /// <summary>
//        /// 发送数据包
//        /// </summary>
//        /// <param name="packet"></param>
//        public override void Send(Packet packet)
//        {
//            this.StartCoroutine(SendRoutine(packet));
//        }

//        private IEnumerator SendRoutine(Packet pack)
//        {
//            using (var www = new WWW(this.remoteAddress, pack.buffer, headers))
//            {
//                yield return www;
//                if (string.IsNullOrEmpty(www.error))
//                {
//                    this.OnReceive(new Packet(www.bytes));
//                }
//            }
//        }

//        #endregion  
//    }
//}
