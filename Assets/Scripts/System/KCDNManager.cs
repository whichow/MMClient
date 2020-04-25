//// ***********************************************************************
//// Company          : Kunpo
//// Author           : KimCh
//// Created          : 
////
//// Last Modified By : KimCh
//// Last Modified On : 
//// ***********************************************************************
//#if UNITY_ANDROID
//#define USE_WEB_REQUEST
//#endif

//using UnityEngine;
//using System.Collections;
//using UnityEngine.Networking;
//using K.AB;

//namespace Game
//{
//    public class KCDNManager : MonoBehaviour, IAsyncR
//    {
//        private const string BASE_URL_IOS = "http://47.74.186.77/mm_res/ios/";
//        private const string BASE_URL_ANDROID = "http://47.74.186.77/mm_res/android/";

//        #region MODEL 

//        public class BundleInfo
//        {
//            public string fileName;
//            public string fileHash;
//            public string filePath;
//            public int fileTime;
//            public int fileSize;

//            public string localName;
//            public string localHash;
//            public string localPath;
//            public int localTime;
//            public int localSize;

//            public bool isCached;
//            public bool isUpdate;
//            public AssetBundle assetBundle;
//        }

//        #endregion

//        #region FIELD

//        /// <summary>The _config URL</summary>
//        private string _configURL;
//        /// <summary>The _assets URL</summary>
//        private string _assetsURL;

//        private int _timestamp;
//        private WWW _www;
//        private BundleInfo[] _bundleInfos = new BundleInfo[0];

//        #endregion

//        #region UPDATE

//        /// <summary>Gets a value indicating whether [force update].强制更新.</summary>
//        /// <value><c>true</c> if [force update]; otherwise, <c>false</c>.</value>
//        public bool forceUpdate
//        {
//            get;
//            private set;
//        }

//        /// <summary>Gets a value indicating whether this instance has update.增量更新.</summary>
//        /// <value>
//        /// <c>true</c> if this instance has update; otherwise, <c>false</c>.
//        /// </value>
//        public bool littleUpdate
//        {
//            get;
//            private set;
//        }

//        /// <summary>Gets the size of the file.增量更新大小 (kb).</summary>
//        /// <value>The size of the file.</value>
//        public int littleUpdateSize
//        {
//            get;
//            private set;
//        }

//        /// <summary>Gets the size of the force update.强制更新大小 (kb)</summary>
//        /// <value>The size of the force update.</value>
//        public int forceUpdateSize
//        {
//            get;
//            private set;
//        }

//        #endregion

//        #region DOWN

//        /// <summary>Downs the configuration.</summary>
//        /// <returns></returns>
//        public IAsyncR DownConfig()
//        {
//            AsyncProcess(0f);
//            this.StartCoroutine(this.DownConfigSchedule());
//            return this;
//        }

//        /// <summary>Downs the resources.</summary>
//        /// <returns></returns>
//        public IAsyncR DownResources()
//        {
//#if UNITY_EDITOR
//            AsyncSuccess(null);
//#else
//            AsyncProcess(0f);
//            this.StartCoroutine(this.DownResourcesSchedule());
//#endif
//            return this;
//        }

//        /// <summary>Downs the new application.</summary>
//        /// <returns></returns>
//        public IAsyncR DownNewApplication()
//        {
//#if UNITY_EDITOR
//            AsyncSuccess(null);
//#else
//            AsyncProcess(0f);
//            this.StartCoroutine(this.DownApplicationSchedule());
//#endif
//            return this;
//        }

//        /// <summary>Downs the configuration schedule.</summary>
//        private IEnumerator DownConfigSchedule()
//        {
//#if USE_WEB_REQUEST
//            using (var www = UnityWebRequest.Get(_assetsURL))
//            {
//                yield return www.Send();
//                if (!www.isNetworkError)
//                {
//                    var wwwText = www.downloadHandler.text;
//#else

//            using (var www = new WWW(_assetsURL))
//            {
//                yield return www;
//                if (string.IsNullOrEmpty(www.error))
//                {
//                    var wwwText = www.text;
//#endif
//                    AsyncProcess(0.2f);

//                    var table = wwwText.ToJsonTable();
//                    if (table != null)
//                    {
//                        forceUpdate = table.GetInt("force_update") != 0;

//                        if (forceUpdate)
//                        {
//                            AsyncSuccess(null);
//                            yield break;
//                        }

//                        var reader = new ABDataReader();
//                        var newABDatas = reader.Read(table);

//                        var bundleList = table.GetArrayList("bundle_list");
//                        if (bundleList != null)
//                        {
//                            int resTime = KConfig.ResTimestamp;

//                            _bundleInfos = new BundleInfo[bundleList.Count];
//                            for (int i = 0; i < bundleList.Count; i++)
//                            {
//                                var bundleT = (Hashtable)bundleList[i];

//                                var tmpBI = new BundleInfo();
//                                tmpBI.fileName = bundleT.GetString("name");
//                                tmpBI.fileHash = bundleT.GetString("hash");
//                                tmpBI.filePath = bundleT.GetString("path");
//                                tmpBI.fileTime = bundleT.GetInt("time");
//                                tmpBI.fileSize = bundleT.GetInt("size");

//                                tmpBI.isUpdate = tmpBI.fileTime > resTime;
//                                if (tmpBI.isUpdate)
//                                {
//                                    tmpBI.isCached = Caching.IsVersionCached(tmpBI.filePath, Hash128.Parse(tmpBI.fileHash));
//                                }
//                                _bundleInfos[i] = tmpBI;
//                            }

//                            littleUpdate = false;
//                            littleUpdateSize = 0;
//                            for (int i = 0; i < _bundleInfos.Length; i++)
//                            {
//                                var tmpBI = _bundleInfos[i];
//                                resTime = Mathf.Max(resTime, tmpBI.fileTime);
//                                if (tmpBI.isUpdate && (!tmpBI.isCached))
//                                {
//                                    littleUpdate = true;
//                                    littleUpdateSize += tmpBI.fileSize;
//                                }
//                            }
//                            KConfig.ResVersion = resTime.ToString();
//                        }
//                    }
//                }
//                else
//                {
//                    //                    Debug.Log("error" + www.error);
//                    AsyncFailure("网络连接异常，请检查网络设置");
//                    yield break;
//                }
//            }

//            AsyncSuccess(null);
//        }

//        /// <summary>Downs the resources schedule.</summary>
//        /// <returns></returns>
//        private IEnumerator DownResourcesSchedule()
//        {
//            for (int i = 0; i < _bundleInfos.Length; i++)
//            {
//                var tmpBI = _bundleInfos[i];
//                if (tmpBI.isUpdate && !tmpBI.isCached)
//                {
//                    _www = WWW.LoadFromCacheOrDownload(tmpBI.filePath, Hash128.Parse(tmpBI.fileHash));
//                    yield return _www;
//                    if (!string.IsNullOrEmpty(_www.error))
//                    {
//                        AsyncFailure("网络连接异常，请检查网络设置");
//                        yield break;
//                    }
//                    _www.Dispose();
//                    _www = null;
//                }
//            }
//            AsyncSuccess(null);
//        }

//        /// <summary>Downs the application schedule.</summary>
//        /// <returns></returns>
//        private IEnumerator DownApplicationSchedule()
//        {
//            //Caching.CleanCache();
//            yield return new WaitForSeconds(1f);
//#if ANDROID_MY
//            var cid = KPlatform.Instance.sdkChannelID;
//            Application.OpenURL(KConfig.UpdateURL + cid);
//#else
//            Application.OpenURL(KConfig.UpdateURL);
//#endif
//            yield return new WaitForSeconds(1f);
//            Application.Quit();
//        }

//        #endregion

//        #region LOAD

//        /// <summary>Loads the bundle.</summary>
//        /// <param name="bundleName">Name of the bundle.</param>
//        /// <returns></returns>
//        public IAsyncR LoadBundle(string bundleName)
//        {
//            AsyncProcess(0f);
//            for (int i = 0; i < _bundleInfos.Length; i++)
//            {
//                if (_bundleInfos[i].fileName == bundleName && _bundleInfos[i].isUpdate)
//                {
//                    StartCoroutine(LoadBundleSchedule(_bundleInfos[i]));
//                    return this;
//                }
//            }
//            return null;
//        }

//        /// <summary>Loads the bundle schedule.</summary>
//        /// <param name="bundleInfo">The bundle information.</param>
//        /// <returns></returns>
//        private IEnumerator LoadBundleSchedule(BundleInfo bundleInfo)
//        {
//            if (bundleInfo.assetBundle)
//            {
//                AsyncSuccess(bundleInfo.assetBundle);
//            }
//            else
//            {
//                _www = WWW.LoadFromCacheOrDownload(bundleInfo.filePath, Hash128.Parse(bundleInfo.fileHash));
//                yield return _www;
//                if (string.IsNullOrEmpty(_www.error))
//                {
//                    bundleInfo.assetBundle = _www.assetBundle;
//                    AsyncSuccess(bundleInfo.assetBundle);
//                }
//                else
//                {
//                    AsyncFailure("网络连接异常，请检查网络设置");
//                }
//                _www.Dispose();
//                _www = null;
//            }
//        }

//        #endregion

//        #region UNITY

//        private void Awake()
//        {
//            _Instance = this;

//#if UNITY_ANDROID
//            _assetsURL = BASE_URL_ANDROID + "ab_config.json";
//#else
//        _assetsURL = BASE_URL_IOS + "ab_config.json";
//#endif
//        }

//        private void Update()
//        {
//            if (_www != null)
//            {
//                AsyncProcess(_www.progress);
//            }
//        }

//        #endregion


//        #region STATIC

//        private static KCDNManager _Instance;

//        public static KCDNManager Instance
//        {
//            get
//            {
//                //if (!_Instance)
//                //{
//                //    _Instance = new GameObject("CDNManager").AddComponent<KCDNManager>();
//                //    _Instance.transform.parent = GameObject.Find("Launch").transform;
//                //}
//                return _Instance;
//            }
//        }

//        #endregion

//        #region INTERFACE

//        float _progress;
//        string _error;
//        object _asyncData;

//        bool IAsyncR.done
//        {
//            get
//            {
//                return _progress > 1f;
//            }
//        }

//        string IAsyncR.error
//        {
//            get
//            {
//                return _error;
//            }
//        }

//        float IAsyncR.progress
//        {
//            get
//            {
//                return _progress;
//            }
//        }

//        object IAsyncR.asyncData
//        {
//            get
//            {
//                return _asyncData;
//            }
//        }

//        AssetBundle IAsyncR.assetBundle
//        {
//            get
//            {
//                return _asyncData as AssetBundle;
//            }
//        }

//        private void AsyncProcess(float progress)
//        {
//            _progress = progress;
//            _error = null;
//            _asyncData = null;
//        }

//        private void AsyncSuccess(object data)
//        {
//            _progress = 1.0001f;
//            _error = null;
//            _asyncData = data;
//        }

//        private void AsyncFailure(string error)
//        {
//            _progress = 1.0001f;
//            _error = error;
//            _asyncData = null;
//        }

//        #endregion
//    }
//}