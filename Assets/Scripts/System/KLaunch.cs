// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
//#define LIMITTIME
namespace Game
{
    using UnityEngine;

    public class KLaunch : MonoBehaviour
    {
        #region Static 

        public static string MainScene;

        public static int Timestamp
        {
            get;
            private set;
        }

        #endregion

        #region LoadLevel

        public static string CurrLevelName
        {
            get;
            private set;
        }

        public static string LastLevelName
        {
            get;
            private set;
        }

        public static void LoadLevel(string levelName)
        {
            LastLevelName = CurrLevelName;
            CurrLevelName = levelName;
#if UNITY_5_3_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelName, UnityEngine.SceneManagement.LoadSceneMode.Single);
#else
			Application.LoadLevel(levelName);
#endif
        }

        public static AsyncOperation LoadLevelAsync(string levelName)
        {
            LastLevelName = CurrLevelName;
            CurrLevelName = levelName;
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = Color.black;
#if UNITY_5_3_OR_NEWER
            var asynOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelName, UnityEngine.SceneManagement.LoadSceneMode.Single);
#else
			var asynOp = Application.LoadLevelAsync(levelName);
#endif
            return asynOp;
        }

#endregion


        #region Field

        public string mainScene;

        #endregion

        #region Unity

        private void Awake()
        {
            MainScene = mainScene;

#if UNITY_STANDALONE_WIN
            Screen.SetResolution(1334, 750, false);
#else
            //SetDesignContentScale();
#endif
            DontDestroyOnLoad(gameObject);

            Random.InitState((int)System.DateTime.Now.Ticks);

            Application.targetFrameRate = 30;
            Time.fixedDeltaTime = 0.04f;

            //Application.logMessageReceived += KLog.LogCallback;
        }

        private void Update()
        {
            Timestamp = (int)((System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            //KPlatform.SessionChange(!pauseStatus);
            //KPlatform.ProfileSign(!pauseStatus);
            if (pauseStatus)
            {
                //KLog.Save();
                //PlayerPrefs.Save();
                //if (KNGUITemp.Instance != null)
                //{
                //    KNGUITemp.Instance.SendMessage();
                //}
            }
            else
            {
                SetDesignContentScale();
            }
        }

        private int _scaleWidth = 0;
        private int _scaleHeight = 0;
        public void SetDesignContentScale()
        {
#if UNITY_ANDROID
            if (_scaleWidth + _scaleHeight == 0)
            {
                int designWidth = 1334;
                int designHeight = 750;

                int screenWidth = Screen.width;
                int screenHeight = Screen.height;

                float s1 = (float)designWidth / (float)designHeight;
                float s2 = (float)screenWidth / (float)screenHeight;

                if (s1 < s2)
                {
                    designWidth = Mathf.RoundToInt(designHeight * s2);
                }
                else if (s1 > s2)
                {
                    designHeight = Mathf.RoundToInt(designWidth / s2);
                }

                float contentScale = (float)designWidth / (float)screenWidth;
                if (contentScale < 1.0f)
                {
                    _scaleWidth = designWidth;
                    _scaleHeight = designHeight;
                }
            }

            if (_scaleWidth + _scaleHeight > 0)
            {
                //if (_scaleWidth % 2 == 0)
                //{
                //    _scaleWidth += 1;
                //}
                //else
                //{
                //    _scaleWidth -= 1;
                //}
                Screen.SetResolution(_scaleWidth, _scaleHeight, true);
            }
#endif
        }

        #endregion

#if DEBUG_MY
        #region Fps 

        public bool showFps = true;

        private int _fpsCount = 0;
        private float _fpsValue = 0f;
        private string _fpsString = "";

        private void OnGUI()
        {
            if (!showFps)
            {
                return;
            }

            _fpsCount += 1;
            _fpsValue += Time.unscaledDeltaTime;
            if (_fpsValue > 0.5f)
            {
                _fpsString = (_fpsCount / _fpsValue).ToString("f");
                _fpsCount = 0;
                _fpsValue = 0f;
            }
            GUI.Label(new Rect(Screen.width - 33f, Screen.height - 22f, 30f, 20f), _fpsString);
        }
        #endregion
#endif

    }
}
