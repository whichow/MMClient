using UnityEngine;

namespace Game
{
    public class GameNetMgr : Singleton<GameNetMgr>
    {
        public GameServer mGameServer { get; private set; }
        public LoginServer mLoginServer { get; private set; }

        public string mDomain { get; private set; }

        public void Init(int svrEnum)
        {
            mGameServer = new GameServer();
            mLoginServer = new LoginServer();
        }

        public void Update()
        {
            if (mLoginServer != null)
                mLoginServer.Update();
            if (mGameServer != null && mGameServer.blEnable)
                mGameServer.Update();
            if (_count > 0)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable || _blNeedRelogin)
                {
                    //LoadingMgr.Instance.HideRechargeMask();
                    //NetReconnectMgr.Instance.ShowRecconect();
                }
            }
        }

        public void Dispose()
        {
            if (mGameServer != null)
            {
                mGameServer.Dispose();
                mGameServer = null;
            }
        }

        private int _count = 0;
        private bool _blNeedRelogin = false;
        public void AddNetErrorCount()
        {
            _count++;
            if (_count >= 3)
                _blNeedRelogin = true;
        }

        public void ResetNetErrorCount()
        {
            _blNeedRelogin = false;
            _count = 0;
        }

    }
}