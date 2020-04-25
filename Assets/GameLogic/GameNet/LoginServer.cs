using Game;
using Google.Protobuf;
using Msg.ClientMessage;

namespace Game
{
    public class LoginServer : ServerBase
    {
        protected override void InitNetMsgHandle()
        {
            base.InitNetMsgHandle();
            RegisterNetMsgType<S2CLoginResponse>(S2CLoginResponse.Descriptor, OnLoginResponse);
        }

        #region 账号登陆

        public void ReqLogin(string acc, string psd, int channel)
        {
            C2SLoginRequest req = new C2SLoginRequest();
            req.Acc = acc;
            req.Password = psd;
            //req.ClientOS = FileConst.RunPlatform;
            //req.Channel = GameLoginType.GetChannelValue(channel);
            //req.IsAppleVerifyUse = !GameDriver.Instance.mBlRunHot;
            Send(ProtocolHelper.GetMsgID(C2SLoginRequest.Descriptor), req);
        }

        private void OnLoginResponse(S2CLoginResponse obj)
        {
            string gameLogicIp;
            string[] ips = obj.GameIP.Split(':');
            if (!string.IsNullOrEmpty(ips[0]))
            {
                gameLogicIp = obj.GameIP;
            }
            else
            {
                string[] ips2 = KConfig.ServerURL.Split(':');
                gameLogicIp = ips2[0] + ":" + ips[1];
            }

            KUser.AccountID = obj.Acc;
            KUser.AccountToken = obj.Token;
            KConfig.ConnectURL = gameLogicIp;

            LocalDataMgr.PlayerAccount = obj.Acc;
            LocalDataMgr.Password = obj.Token;
            //LocalDataMgr.LoginChannel = _channel;
            GameApp.Instance.GameServer.EnterGameRequest();
        }

        #endregion

        #region override

        protected override void DoRequestSend()
        {
            var url = string.Format("http://{0}/client", KConfig.ServerURL);
#if DEBUG_MY
            Debuger.Log("[HTTP_URL] " + url);
#endif

            _curRequest = new LoginPostRequest(url, OnReceiveData, OnDataError);

            C2S_ONE_MSG pbData = _postDataPools.Dequeue();
            _curRequest.InitPostData(pbData.ToByteArray());
            _curRequest.Send();
        }

        protected override void OnReceiveData(object value)
        {
            S2C_ONE_MSG pbMsg = value as S2C_ONE_MSG;
            if (DictNetHandle.ContainsKey(pbMsg.MsgCode))
            {
                NetMsgEventHandle handle = DictNetHandle[pbMsg.MsgCode];
                NetMsgRecvData data = NetPacketHelper.Read(pbMsg.MsgCode, pbMsg.Data.ToByteArray(), handle, _lstRequestID[0], true);
                if (data != null)
                {
                    _lstRecvDatas.Add(data);

#if DEBUG_MY
                    MessageParser parser = GameApp.Instance.LoginServer.GetNetHandler(pbMsg.MsgCode).mMsgParser;
                    var data2 = data.mData as IMessage;
                    Debuger.Log(string.Format("[收到数据:{0}-{1}] {2}", data2.Descriptor.Name, pbMsg.MsgCode, data2.ToString()));
#endif
                }
            }
            else
            {
                Debuger.LogError("[ServerBase.ProccessOneMsg() <= 消息号:" + pbMsg.MsgCode + "未注册]");
            }
            //ProccessOneMsg(data as S2C_ONE_MSG);
        }

        protected override void OnDataError()
        {
            base.OnDataError();
            if (_lstRequestID != null)
                _lstRequestID.Clear();
            //LoginHelper.DoLoginNetError(-1);
        }

        #endregion

    }
}