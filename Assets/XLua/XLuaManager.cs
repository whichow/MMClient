/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/4 10:21:19
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using K.AB;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using XLua;

namespace Game
{
    public class XLuaManager : Singleton<XLuaManager>
    {
        #region Member

        //private const string LUA_PATH = "http://192.168.0.102/lua";
        private AssetBundle m_LuaAB;
        private LuaEnv m_Env;

        public bool HotFixComplete { get; private set; }

        public LuaEnv Env
        {
            get { return m_Env; }
        }

        #endregion

        #region C&D
        public XLuaManager()
        {
            m_Env = new LuaEnv();
        }
        #endregion

        #region Public

        public void Init()
        {
#if UNITY_EDITOR
            m_Env.AddLoader(CustomLoaderFormFile);
            StartHotfix();
#else
            m_Env.AddLoader(CustomLoaderFromAB);
            //GameApp.Instance.StartCoroutine(LoadFromUnityWebRequest(StartHotfix));
            StartHotfix();
#endif
        }

        public void Dispose()
        {
            m_Env.Dispose();
            m_Env = null;
        }

        #endregion

        #region Private

        private byte[] CustomLoaderFromAB(ref string fileName)
        {
            TextAsset textAsset = KGameRes.LoadAssetAtPath("Res/Lua/"+ fileName + ".lua") as TextAsset;
            //fileName = fileName.Replace('.', '/') + ".lua";
            //TextAsset textAsset = m_LuaAB.LoadAsset<TextAsset>(fileName);
            if (textAsset == null)
            {
                Debug.LogError("Not found Lua file from AB! " + fileName);
                return null;
            }
            else
            {
                return textAsset.bytes;
            }
        }

        private byte[] CustomLoaderFormFile(ref string fileName)
        {
            fileName = Application.dataPath + "/Res/Lua/" + fileName.Replace('.', '/') + ".lua.txt";
            if (File.Exists(fileName))
            {
                return File.ReadAllBytes(fileName);
            }
            else
            {
                Debug.LogError("Not found Lua file! " + fileName);
                return null;
            }
        }

        //private IEnumerator LoadFromUnityWebRequest(Action callback)
        //{
        //    string url = LUA_PATH;
        //    UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        //    yield return request.SendWebRequest();
        //    m_LuaAB = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
        //    request.Dispose();
        //    if (m_LuaAB != null)
        //    {
        //        callback?.Invoke();
        //        m_LuaAB.Unload(true);
        //        m_LuaAB = null;
        //    }
        //}

        private void StartHotfix()
        {
            try
            {
            	m_Env.DoString("require 'Main'");
            }
            catch (System.Exception ex)
            {
                Debuger.LogError("Hotfix error! \n" + ex);
            }
            HotFixComplete = true;
        }

        #endregion

    }
}
