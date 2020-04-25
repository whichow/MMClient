using Game;
using Msg.ClientMessage;
using System;

namespace Game
{
    public class LoginPostRequest : GameRequestBase
    {
        public LoginPostRequest(string url, Action<object> onComplete, Action onError)
            : base(url, onComplete, onError)
        {

        }

        protected override void DoParseData()
        {
            if (string.IsNullOrEmpty(_webRequestAsync.webRequest.downloadHandler.text))
            {
                Debuger.LogError("[LoginPostRequest.DoParseData() => get data was empty!!!]");
                CheckReSend();
                return;
            }
            byte[] data = _webRequestAsync.webRequest.downloadHandler.data;
            S2C_ONE_MSG pbMsgData = S2C_ONE_MSG.Parser.ParseFrom(data);
            _status = HttpsStatus.None;
            //if (pbMsgData.ErrorCode < NetErrorCode.None)
            if (pbMsgData.ErrorCode < 0)
            {
                Debuger.LogError("ErrCode = " + pbMsgData.ErrorCode);
                KServer.ProcessError(pbMsgData.ErrorCode);
                //LoginHelper.DoLoginNetError(pbMsgData.ErrorCode);
                ////if (_onErrMethod != null)
                ////_onErrMethod.Invoke();
            }
            else
            {
                if (_onCompleteMethod != null)
                    _onCompleteMethod.Invoke(pbMsgData);
            }
        }
    }
}