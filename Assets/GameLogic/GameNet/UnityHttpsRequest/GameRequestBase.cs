using Framework;
using Framework.Core;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Game
{
    public enum HttpsStatus
    {
        None,
        Send,
        Wait,
        Recived,
    }

    public abstract class GameRequestBase : UpdateBase
    {
        protected Action<object> _onCompleteMethod { get; private set; }
        protected Action _onErrMethod { get; private set; }

        protected string _url;
        protected HttpsStatus _status;

        protected float _flTimeOut = 3f;
        protected int _connectCount = 2;

        protected UnityWebRequestAsyncOperation _webRequestAsync;
        protected byte[] _pbDatas;
        protected float _flSendTime;
        protected bool _blEnd;

        public bool mBlNeedCheckMask { get; protected set; }
        protected List<int> _lstMsgId;

        #region getter
        public bool BlEnd
        {
            get { return _blEnd; }
        }
        #endregion

        public GameRequestBase(string url, Action<object> onCompleteMethod, Action onErrMethod = null)
        {
            _url = url;
            _onCompleteMethod = onCompleteMethod;
            _onErrMethod = onErrMethod;
            Initialize();
            _status = HttpsStatus.None;
            _blEnd = false;
        }

        public bool CheckIncludeID(int msgId)
        {
            return _lstMsgId.Contains(msgId);
        }

        public virtual void AddMsgID(int msgId)
        {

        }

        public virtual void InitPostData(byte[] data)
        {
            _pbDatas = data;
        }

        public void Send()
        {
            _status = HttpsStatus.Send;
            _connectCount = 2;
        }

        public override void Update()
        {
            base.Update();
            switch (_status)
            {
                case HttpsStatus.Send:
                    _flTimeOut = 5f;
                    if (_webRequestAsync != null && _webRequestAsync.webRequest != null)
                    {
                        _webRequestAsync.webRequest.Abort();
                        _webRequestAsync = null;
                    }
                    _webRequestAsync = DoSend();
                    _status = HttpsStatus.Wait;
                    break;
                case HttpsStatus.Wait:
                    DoCheckTimeout();
                    break;
                case HttpsStatus.Recived:
                    DoParseData();
                    _blEnd = true;
                    break;
            }
        }


        protected abstract void DoParseData();
        protected virtual UnityWebRequestAsyncOperation DoSend()
        {
            UnityWebRequest req = new UnityWebRequest(_url, "POST");
            req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            req.uploadHandler = new UploadHandlerRaw(_pbDatas);
            req.downloadHandler = new DownloadHandlerBuffer();
            _flSendTime = UnityEngine.Time.realtimeSinceStartup;
            return req.SendWebRequest();
        }

        private void DoCheckTimeout()
        {
            if (_webRequestAsync.isDone)
            {
                if (_webRequestAsync.webRequest.isNetworkError || _webRequestAsync.webRequest.isHttpError)
                    CheckReSend();
                else
                    _status = HttpsStatus.Recived;
            }
            else
            {
                if (_flTimeOut <= 0.01f)
                    CheckReSend();
                else
                    _flTimeOut -= UnityEngine.Time.deltaTime;
            }
        }

        protected void CheckReSend()
        {
            if (_connectCount <= 0)
            {
                OnNetError();
                return;
            }
            Debuger.LogWarning("[GameRequestBase.CheckResSend() => count:" + _connectCount + "]");
            _connectCount--;
            _status = HttpsStatus.Send;
        }

        private void OnNetError()
        {
            Debuger.LogWarning("[GameRequestBase.OnNetError() => connnect error！！！]");
            if (_onErrMethod != null)
                _onErrMethod.Invoke();
            Dispose();
        }

        protected override void Remove()
        {
            base.Remove();
            if (_webRequestAsync != null && _webRequestAsync.webRequest != null)
                _webRequestAsync.webRequest.Dispose();
            _webRequestAsync = null;
            _onCompleteMethod = null;
            _onErrMethod = null;
        }
    }
}