/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/2 15:30:10
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using UnityEngine;

namespace Game
{
    public class GameApp : App
    {
        #region static
        public static GameApp Instance
        {
            get { return m_pInstance as GameApp; }
        }
        #endregion

        #region Property
        public KLanguageManager LanguageManager { get; private set; }
        #endregion

        #region override

        protected override void Start()
        {
            Debuger.Log("PersistentDataPath: " + Application.persistentDataPath);
            Debuger.Log("StreamingAssetsPath: " + Application.streamingAssetsPath);

            base.Start();

            Application.logMessageReceived += Debuger.LogCallback;

            GameObject.DontDestroyOnLoad(gameObject);

            SdkApi.InitSDK();
        }

        protected override void Stop()
        {
            base.Stop();
            SdkApi.ExitSDK();
        }

        protected override void Update()
        {
            base.Update();

            InputController.Instance.Update();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            InputController.Instance.LateUpdate();
        }

        protected override void InitManager()
        {
            base.InitManager();

            EventManager.Instance.Init();
            InputController.Instance.Init();

            //LanguageManager = new KLanguageManager();
            //ItemManager = new KItemManager();
            //CatManager = new KCatManager();
            //LevelManager = new KLevelManager();
            //HeadManager = new KHeadManager();
            //M3LevelConfigManager = new M3LevelConfigMgr();
            //IconManager = new KIconManager();

        }

        #endregion

        #region Application

        void OnApplicationQuit()
        {
            Stop();
            Debuger.Save();
        }

        void OnApplicationFocus(bool state)
        {
            Debug.Log("----------------- OnApplicationFocus : " + state);
            if (!state)
            {
                Debuger.Save();
            }
        }

        void OnApplicationPause(bool state)
        {
            SdkApi.Pause(state);
            if (state)
            {
                Debuger.Save();
            }
        }


        #endregion

        #region Public

        public void ResourceCheckUpdate()
        {
            GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/CheckResUpdateComp"));

        }

        public void ResourceUpdateCompleted()
        {
            InitManager();
        }

        #endregion

        public bool HotFixLuaSucc = false;    //Lua 热更是否启用成功

    }
}