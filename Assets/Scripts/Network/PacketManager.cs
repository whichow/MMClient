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
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    using PacketCallback = System.Action<int, string, object>;

    public class PacketManager// : MonoBehaviour
    {
        #region Field

        //private UnityWebRequest _sendRequest;
        //private UnityWebRequest _keepRequest;

        #endregion

        #region Property

        //public string connectURL
        //{
        //    get { return KConfig.ConnectURL; }
        //}

        //public bool encryptText
        //{
        //    get { return _encryptText; }
        //}

        #endregion

        //private enum Status
        //{
        //    WAIT,//
        //    SEND,
        //    RECV,
        //}

        ///// <summary>The _status</summary>
        //private Status _status;
        ///// <summary>The _status timer</summary>
        //private float _statusTimer;
        ///// <summary>The _last ping ping时间.</summary>
        //private float _lastPing;
        ///// <summary>The _resend count automatic 自动重试次数.</summary>
        //private int _autoResendCount;
        ///// <summary>The _auto resend count maximum</summary>
        //private int _autoResendCountMax = 0;
        ///// <summary>The _timeout timer 超时时间.</summary>
        //private float _timeoutTimer;
        ///// <summary>The _timeout duration</summary>
        //private float _timeoutDuration = 5f;

        //private bool _encryptText = false;

        ///// <summary>The _live cd</summary>
        //private float _keepTimer = float.MinValue;
        ///// <summary>The _live duration</summary>
        //private float _keepDuration = 90f;
        ///// <summary>The _live packet 心跳包.</summary>
        //private PacketLive _keepPacket = new PacketLive();

        ///// <summary>The _serial</summary>
        //private int _serial;
        ///// <summary>The _curr packet</summary>
        //private Packet _currPacket;
        ///// <summary>The _send packets</summary>
        //private Queue<Packet> _sendPackets = new Queue<Packet>();
        ///// <summary>The _sending packets</summary>
        //private Queue<Packet> _sendingPackets = new Queue<Packet>();
        /// <summary>The _all packet table</summary>
        private Dictionary<int, Packet> _allPacketTable = new Dictionary<int, Packet>();


        ///// <summary>Gets the last ping.</summary>
        //public string lastPing
        //{
        //    get { return _lastPing.ToString("N3"); }
        //}

        public Packet GetPacketAndRemove(int packetCode)
        {
            Packet val;
            if (!_allPacketTable.TryGetValue(packetCode, out val))
            {
                Debug.LogWarning("找不到MSGID: " + packetCode);
            }
            else
            {
                _allPacketTable.Remove(packetCode);
            }
            return val;
        }


        /// <summary>Creates the packet.</summary>
        /// <param name="code">The code.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public Packet CreatePacket(int code, PacketCallback callback)
        {
            Packet packet;
            if (!_allPacketTable.TryGetValue(code, out packet))
            {
                //switch (code)
                //{
                //    case (int)PacketCode.Login:
                //        packet = new PacketLogin();
                //        break;
                //    default:
                        packet = new PacketBinary();
                //        break;
                //}
                _allPacketTable.Add(code, packet);
            }
            return packet.Init((int)code, callback);
        }

        public Packet CreatePacket(Type tt, PacketCallback callback)
        {
            int msgID = ProtocolHelper.GetMsgID(tt);
            return CreatePacket(msgID, callback);
        }

        /// <summary>Sends the post packet.发送协议包.</summary>
        public void SendPostPacket(Packet packet)
        {
            //if (packet != null)
            //{
            //    packet.Generate(_serial++);
            //    _sendPackets.Enqueue(packet);
            //}
            Debug.Log("-------SendPostPacket: " + packet.MsgParam.Descriptor.Name);
            GameApp.Instance.GameServer.C2SRequest(packet.MsgParam.Descriptor, packet.MsgParam);

        }

        public void ResendPostPacket()
        {
            //if (_currPacket != null)
            //{
            //    _sendPackets.Enqueue(_currPacket);
            //}
        }

        //private UnityWebRequest Send(Packet packet)
        //{
        //    var request = new UnityWebRequest(connectURL, UnityWebRequest.kHttpVerbPOST)
        //    {
        //        //timeout = 5
        //    };
        //    if (encryptText)
        //    {
        //        string ivKey;
        //        var sendBytes = KSecurity.EncryptBytes(packet.sendBytes, out ivKey);
        //        request.uploadHandler = new UploadHandlerRaw(sendBytes);
        //        request.downloadHandler = new DownloadHandlerBuffer();
        //        //request.SetRequestHeader("Content-Type", "application/json");
        //        //sendHeader["CONTENT-VKEY"] = ivKey;
        //    }
        //    else
        //    {
        //        var sendBytes = packet.sendBytes;
        //        request.uploadHandler = new UploadHandlerRaw(sendBytes);
        //        request.downloadHandler = new DownloadHandlerBuffer();
        //    }
        //    return request;
        //}

        //private void Recv(Packet packet, UnityWebRequest request)
        //{
        //    if (encryptText)
        //    {
        //        var ivKey = request.GetResponseHeader("CONTENT-VKEY");
        //        packet.recvBytes = KSecurity.DecryptBytes(request.downloadHandler.data, ivKey);
        //    }
        //    else
        //    {
        //        packet.recvBytes = request.downloadHandler.data;
        //    }
        //}

        ///// <summary>
        ///// 独立通道心跳
        ///// </summary>
        ///// <param name="packet"></param>
        //public void KeepPacket()
        //{
        //    _keepPacket.Init();
        //    _keepPacket.Generate(_serial++);
        //    StartCoroutine(KeepSchedule());
        //}

        ///// <summary>Keep the schedule.</summary>
        ///// <returns></returns>
        //private IEnumerator KeepSchedule()
        //{
        //    _keepRequest = Send(_keepPacket);

        //    yield return _keepRequest.Send();

        //    if (string.IsNullOrEmpty(_keepRequest.error))
        //    {
        //        Recv(_keepPacket, _keepRequest);
        //    }
        //    else
        //    {
        //        _keepPacket.recvError = _keepRequest.error;
        //    }

        //    _keepRequest.Dispose();
        //    _keepRequest = null;
        //}

        #region UNITY 

        ///// <summary>Updates this instance.</summary>
        //private void Update()
        //{
        //    switch (_status)
        //    {
        //        case Status.WAIT:
        //            if (_sendPackets.Count > 0)
        //            {
        //                _keepTimer = 0f;
        //                _status = Status.SEND;
        //            }
        //            else
        //            {
        //                if (_keepRequest == null)
        //                {
        //                    _keepTimer += Time.deltaTime;
        //                    if (_keepTimer > _keepDuration)
        //                    {
        //                        _keepTimer = 0f;
        //                        KeepPacket();
        //                    }
        //                }
        //            }
        //            break;
        //        case Status.SEND:
        //            if (_sendPackets.Count > 0)
        //            {
        //                _currPacket = _sendPackets.Dequeue();
        //                _sendRequest = Send(_currPacket);
        //                _sendRequest.Send();
        //                _timeoutTimer = 0f;
        //                _status = Status.RECV;
        //            }
        //            break;
        //        case Status.RECV:
        //            if (_sendRequest.isDone)
        //            {
        //                var error = _sendRequest.error;
        //                if (string.IsNullOrEmpty(error))
        //                {
        //                    Recv(_currPacket, _sendRequest);
        //                }
        //                else
        //                {
        //                    _currPacket.recvError = error;
        //                }

        //                _autoResendCount = 0;// 成功后清空自动重试次数.

        //                _sendRequest.Dispose();
        //                _sendRequest = null;
        //                _status = Status.WAIT;
        //            }
        //            else
        //            {
        //                _timeoutTimer += Time.deltaTime;
        //                if (_timeoutTimer > _timeoutDuration)
        //                {
        //                    if (_autoResendCount < _autoResendCountMax)
        //                    {
        //                        _autoResendCount++;
        //                        _timeoutTimer = 0f;
        //                        _sendRequest.Abort();
        //                        _sendRequest.Dispose();
        //                        _sendRequest = Send(_currPacket);
        //                        _sendRequest.Send();
        //                    }
        //                    else
        //                    {
        //                        _currPacket.recvError = string.Empty;
        //                        _timeoutTimer = 0f;
        //                        _sendRequest.Abort();
        //                        _sendRequest.Dispose();
        //                        _sendRequest = null;
        //                        _status = Status.WAIT;
        //                    }
        //                }
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //private void OnApplicationPause(bool pauseStatus)
        //{
        //    //if (pauseStatus)
        //    //{
        //    //    if (this._status == Status.WAIT)
        //    //    {
        //    //        if (KUser.isLogin)
        //    //        {
        //    //            SendPostPacket(_keepPacket);
        //    //        }

        //    //        if (_sendPackets.Count > 0)
        //    //        {
        //    //            this.SetSendStatus();
        //    //        }
        //    //    }
        //    //}
        //}

        private void Awake()
        {
            _Instance = this;
        }

        #endregion

        private static PacketManager _Instance;
        public static PacketManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    //_Instance = new GameObject(typeof(PacketManager).Name).AddComponent<PacketManager>();
                    //_Instance.transform.parent = GameObject.Find("Launch").transform;
                    _Instance = new PacketManager();
                }
                return _Instance;
            }
        }
    }
}
