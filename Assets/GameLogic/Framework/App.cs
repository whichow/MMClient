/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/3/10 18:30:10
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Framework.Core;
using UnityEngine;

namespace Game
{
    public class App : MonoBehaviour
    {
        public delegate void UpdateDelegate();  // 游戏更新委托

        protected static App m_pInstance;

        public event UpdateDelegate OnUpdateEvent;
        public event UpdateDelegate OnLateUpdateEvent;

        public LoginServer LoginServer { get; private set; }
        public GameServer GameServer { get; private set; }


        public void InitGameNet()
        {
            GameNetMgr.Instance.Init(0);
            LoginServer = GameNetMgr.Instance.mLoginServer;
            GameServer = GameNetMgr.Instance.mGameServer;
        }

        protected virtual void Awake()
        {
            m_pInstance = this;
        }

        protected virtual void Start()
        {

        }

        protected virtual void Stop()
        {
 
        }

        protected virtual void Update()
        {
            GameComponent.Instance.Update();
            OnUpdateEvent?.Invoke();
            
        }


        protected virtual void LateUpdate()
        {
            GameNetMgr.Instance.Update();
            OnLateUpdateEvent?.Invoke();
        }

        protected virtual void InitManager()
        {
            //XLuaManager.Instance.Env.DoString("CS.UnityEngine.Debug.Log('Lua hello world')");//Test
            GameComponent.Instance.Init();
            //InitGameNet();

        }

    }
}