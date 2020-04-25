// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
namespace K.AB
{
    using Game;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using UnityEngine;
    using UnityEngine.Networking;

    public class ABDataManager
    {
        #region Field

        private static ABDataManager _Instance;

        public static ABDataManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ABDataManager();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// 路径对应 streamingAssets
        /// </summary>
        private Dictionary<string, ABData> _abDataStreamingDictionary;
        /// <summary>
        /// 路径对应 persistentData
        /// </summary>
        private Dictionary<string, ABData> _abDataPersistentDictionary;
        /// <summary>
        /// 路径对应 cloudServer
        /// </summary>
        private Dictionary<string, ABData> _abDataCloudDictionary;

        /// <summary>
        /// 路径对应 
        /// </summary>
        private Dictionary<string, ABData> _abDataDictionary;
        /// <summary>
        /// 资源名对应
        /// </summary>
        private Dictionary<string, string> _abNameDictionary;
        /// <summary>
        /// 文件名对应
        /// </summary>
        private Dictionary<string, string> _abFileDictionary;

        public Action<string> ShowMsgHandler;
        public Action<float> ProgressHandler;

        private Action<string> _initCompleteCallback;
        private bool m_needUpdateLocalVersionFile;
        private List<ABData> m_needDownFiles;
        private int m_totleSize;
        private int m_totleCount;
        private byte[] _cloudFile;

        #endregion

        #region Constructor

        private ABDataManager()
        {
        }

        #endregion

        #region Method 

        public void LoadConfig(Action<string> callback)
        {
            _initCompleteCallback = callback;

            LoadStreamingConfigFile();
        }

        public void LoadStreamingConfigFile()
        {
            string url = Application.streamingAssetsPath + AppConfig.CONFIG_FILE;
            ABManager.Instance.StartCoroutine(_DownLoad(url,
                delegate (byte[] data)
                {
                    string txt = Encoding.UTF8.GetString(data);
                    try
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(txt);
                        AppConfig.GameVersion = xml.SelectSingleNode("configuration/gameVersion").InnerText;
                        AppConfig.SvnVersion = xml.SelectSingleNode("configuration/svnVersion").InnerText;
                        AppConfig.HotUpdateRes = bool.Parse(xml.SelectSingleNode("configuration/hotUpdateRes").InnerText);
                        bool debugMod = bool.Parse(xml.SelectSingleNode("configuration/debugMod").InnerText);
                        AppConfig.IsDebugMod = debugMod;
                        if (debugMod)
                        {
                            AppConfig.ConfigFileUrl = xml.SelectSingleNode("configuration/debug/configFile").Attributes["url"].Value;
                            AppConfig.ResServerUrl = xml.SelectSingleNode("configuration/debug/resServer").Attributes["url"].Value;
                            AppConfig.RegServerUrl = xml.SelectSingleNode("configuration/debug/regServer").Attributes["url"].Value;
                            AppConfig.AppDownloadUrl = xml.SelectSingleNode("configuration/debug/appDownload").Attributes["url"].Value;
                        }
                        else
                        {
                            AppConfig.ConfigFileUrl = xml.SelectSingleNode("configuration/release/configFile").Attributes["url"].Value;
                            AppConfig.ResServerUrl = xml.SelectSingleNode("configuration/release/resServer").Attributes["url"].Value;
                            AppConfig.RegServerUrl = xml.SelectSingleNode("configuration/release/regServer").Attributes["url"].Value;
                            AppConfig.AppDownloadUrl = xml.SelectSingleNode("configuration/release/appDownload").Attributes["url"].Value;
                        }
                        AppConfig.ResServerUrl = AppConfig.ResServerUrl + "/" + AppConfig.GameVersion;
                    }
                    catch (Exception e)
                    {
                        ShowMsgHandler?.Invoke("Local config.xml parsing error!");
                        Debug.LogError("[LoadStreamingConfigFile] XML解析错误! " + e);
                        return;
                    }

                    LoadServerConfigFile();
                },
                delegate () {
                    ShowMsgHandler?.Invoke("Local not found config.xml! \n" + url);
                    Debug.LogError("Local not found config.xml! " + url);
                }
            ));
        }

        private void LoadServerConfigFile()
        {
            if (AppConfig.HotUpdateRes)
            {

                string url = AppConfig.CONFIG_FILE_URL;
                ABManager.Instance.StartCoroutine(_DownLoad(url,
                    delegate (byte[] data)
                    {
                        string txt = Encoding.UTF8.GetString(data);
                        try
                        {
                            XmlDocument xml = new XmlDocument();
                            xml.LoadXml(txt);
                            AppConfig.IsReview = bool.Parse(xml.SelectSingleNode("configuration/isReview").InnerText);
                            bool logEnable = bool.Parse(xml.SelectSingleNode("configuration/logEnable").InnerText);
                            Debuger.EnableLog = logEnable;

                            #region 版本检测
#if UNITY_STANDALONE
                            string platform = "StandaloneWindows";
#elif UNITY_IPHONE
                            string platform = "iPhone";
#elif UNITY_ANDROID
                            string platform = "Android";
#endif
                            string ver = xml.SelectSingleNode("configuration/version/" + platform).InnerText;
                            if (AppConfig.GameVersion != ver)
                            {
                                ShowMsgHandler?.Invoke("A new version has been detected, please upgrade to the latest version. v" + ver);
                                Debug.LogError("检测到有新版本，请升级到最新版本 v" + ver);
                                AppConfig.GameVersion = ver;
                                return;
                            }
                            #endregion
                            LoadStreaming();
                        }
                        catch (Exception e)
                        {
                            ShowMsgHandler?.Invoke("Server config.xml parsing error!");
                            Debug.LogError("[LoadServerConfigFile] XML解析错误! \n" + e);
                            return;
                        }
                    },
                    delegate ()
                    {
                        ShowMsgHandler?.Invoke("Failed to download server config.xml and try again in 3 seconds! \n" + url);
                        Debug.LogError("not found config.xml! " + url);
                    },
                    delegate ()
                    {
                        LoadServerConfigFile();
                    },
                    3
                ));
            }
            else
            {
                LoadStreaming();
            }
        }

        private void LoadStreaming()
        {
            string url = Application.streamingAssetsPath + ABDefine.GetLocalConfigFile();
            ABManager.Instance.StartCoroutine(_DownLoad(url,
                delegate (byte[] data)
                {
                    LoadABData(data, out _abDataStreamingDictionary);

                    if (AppConfig.HotUpdateRes)
                    {
                        if (File.Exists(Application.persistentDataPath + ABDefine.GetLocalConfigFile()))
                            LoadLocal();
                        else
                        {
                            _abDataPersistentDictionary = new Dictionary<string, ABData>();
                            LoadCloud();
                        }
                    }
                    else
                    {
                        Debug.Log("---未开启热更新资源---");
                        _abDataDictionary = _abDataStreamingDictionary;
                        _abDataStreamingDictionary = null;
                        if (Directory.Exists(AppConfig.LOCAL_RES_PATH))
                        {
                            Directory.Delete(AppConfig.LOCAL_RES_PATH, true);
                        }
                        InitCompleted();
                    }
                },
                delegate () { Debug.LogError("not found config.json! " + url); }
            ));
        }

        private void LoadLocal()
        {
            var bytes = File.ReadAllBytes(Application.persistentDataPath + ABDefine.GetLocalConfigFile());
            LoadABData(bytes, out _abDataPersistentDictionary);
            LoadCloud();
        }

        private void LoadCloud()
        {
            string url = AppConfig.ServerUrl + ABDefine.GetLocalConfigFile();
            ABManager.Instance.StartCoroutine(_DownLoad(url,
                delegate (byte[] data)
                {
                    _cloudFile = data;
                    LoadABData(data, out _abDataCloudDictionary);
                    CompareVersion();
                },
                delegate ()
                {
                    ShowMsgHandler?.Invoke("Failed to download config.json and try again in 3 seconds! \n" + url);
                    Debug.LogError("下载资源配置文件失败，3秒后重试！" + url);
                },
                delegate ()
                {
                    LoadCloud();
                },
                3
            ));
        }

        /// <summary>
        /// 计算出需要重新加载的资源 
        /// </summary>
        private void CompareVersion()
        {
            m_needDownFiles = new List<ABData>();
            m_needUpdateLocalVersionFile = false;

            //删除资源
            List<string> delFiles = new List<string>();
            var l = _abDataPersistentDictionary.GetEnumerator();
            while (l.MoveNext())
            {
                var version = l.Current;
                if (!_abDataCloudDictionary.ContainsKey(version.Key))
                {
                    delFiles.Add(version.Value.fileName);
                    m_needUpdateLocalVersionFile = true;
                }
            }

            var s = _abDataCloudDictionary.GetEnumerator();
            while (s.MoveNext())
            {
                var version = s.Current;
                string key = version.Key;
                ABData serverResInfo = version.Value;
                bool _streamingNeedUpdate = false;
                //先检测StreamingRes
                if (!_abDataStreamingDictionary.ContainsKey(key))
                {
                    _streamingNeedUpdate = true;
                }
                else if (!serverResInfo.buildHash.Equals(_abDataStreamingDictionary[key].buildHash))
                {
                    _streamingNeedUpdate = true;
                }
                if (_streamingNeedUpdate)
                {
                    //再检测LocalRes
                    if (!_abDataPersistentDictionary.ContainsKey(key))
                    {
                        m_needDownFiles.Add(serverResInfo);         //新增的资源  
                        m_totleSize += serverResInfo.fileSize;
                    }
                    else if (!serverResInfo.buildHash.Equals(_abDataPersistentDictionary[key].buildHash))
                    {
                        m_needDownFiles.Add(serverResInfo);         //需要替换的资源 
                        m_totleSize += serverResInfo.fileSize;
                    }
                }
                else
                {
                    //不需要更新，删除老版本的数据
                    if (_abDataPersistentDictionary.ContainsKey(key))
                    {
                        delFiles.Add(version.Value.fileName);
                        m_needUpdateLocalVersionFile = true;
                    }
                    else if(_abDataStreamingDictionary.ContainsKey(key))
                    {
                        delFiles.Add(version.Value.fileName);
                    }
                }
            }

            for (int i = 0; i < delFiles.Count; i++)
            {
                _abDataPersistentDictionary.Remove(delFiles[i]);
                //Debug.Log("删除文件："+AppConfig.LOCAL_RES_PATH + delFiles[i]);
                if (File.Exists(AppConfig.LOCAL_RES_PATH + delFiles[i]))
                {
                    File.Delete(AppConfig.LOCAL_RES_PATH + delFiles[i]);
                }
            }
            delFiles.Clear();

            //本次有更新，同时更新本地的version.txt  
            if (m_needDownFiles.Count > 0)
            {
                m_totleCount = m_needDownFiles.Count;
                m_needUpdateLocalVersionFile = true;
                ResourcePreUpdate(DownLoadRes);
            }
            else
            {
                UpdateLocalVersionFile();
            }
        }

        private void ResourcePreUpdate(Action<string> downLoadRes)
        {
            //if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            //{
            //    _ConfirmResourceUpdate(callback);
            //}
            //else
            //{
            //    m_ResUpdateBtn.onClick.AddListener(delegate ()
            //    {
            //        _ConfirmResourceUpdate(callback);
            //    });
            //}
            downLoadRes?.Invoke("");
        }

        /// <summary>
        /// 依次加载需要更新的资源
        /// </summary>
        private void DownLoadRes(string file = "")
        {
            if (string.IsNullOrEmpty(file))
            {
                if (m_needDownFiles.Count == 0)
                {
                    UpdateLocalVersionFile();
                    return;
                }

                file = m_needDownFiles[0].fileName;
                m_needDownFiles.RemoveAt(0);
            }

            string url = AppConfig.ServerUrl + AppConfig.RES_DIRECTORY + file;
            ShowMsgHandler?.Invoke("Download resources! \n" + url);
            ProgressHandler?.Invoke(1f - (m_needDownFiles.Count / (float)m_totleCount));
            ABManager.Instance.StartCoroutine(_DownLoad(url,
                delegate (byte[] data)
                {
                    ReplaceLocalRes(file, data);
                    DownLoadRes();
                },
                delegate ()
                {
                    ShowMsgHandler?.Invoke("Failed to download resource and is trying again! \n" + url);
                    Debug.Log("下载资源失败，正在重试... " + url);
                },
                delegate ()
                {
                    DownLoadRes(file);
                },
                5));
        }

        /// <summary>
        /// 将下载的资源替换本地旧的资源
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        private void ReplaceLocalRes(string fileName, byte[] data)
        {
            if (!Directory.Exists(AppConfig.LOCAL_RES_PATH))
            {
                Directory.CreateDirectory(AppConfig.LOCAL_RES_PATH);
            }

            string[] dirs = fileName.Split('/');
            string dir = AppConfig.LOCAL_RES_PATH;
            for (int i = 0; i < dirs.Length - 1; i++)
            {
                dir = dir + dirs[i] + "/";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }

            string path = AppConfig.LOCAL_RES_PATH + fileName;
            FileStream stream = new FileStream(path, FileMode.Create);
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
            if (!File.Exists(path))
            {
                Debug.LogError("---- 写入失败 " + path);
            }
            else
            {
                //Debug.Log("---- 写入成功 " + path);
            }
        }

        /// <summary>
        /// 更新本地的版本配置
        /// </summary>
        private void UpdateLocalVersionFile()
        {
            if (m_needUpdateLocalVersionFile)
            {
                FileStream stream = new FileStream(Application.persistentDataPath + ABDefine.GetLocalConfigFile(), FileMode.Create);
                byte[] data = _cloudFile;
                stream.Write(data, 0, data.Length);
                stream.Flush();
                stream.Close();

                _cloudFile = null;
            }

            ResourceUpdateComplete();
        }

        private void ResourceUpdateComplete()
        {
            ShowMsgHandler?.Invoke("Resource update complete!");

            _abDataDictionary = _abDataCloudDictionary;

            _abDataStreamingDictionary = null;
            _abDataPersistentDictionary = null;
            _abDataCloudDictionary = null;

            InitCompleted();

            //if (OnResourceUpdateCompleteEvent != null)
            //{
            //    OnResourceUpdateCompleteEvent();
            //}
        }

        private void InitCompleted()
        {
            Process();

            _initCompleteCallback?.Invoke(_abDataDictionary.Count > 0 ? "" : "error");
            _initCompleteCallback = null;
        }

        IEnumerator _DownLoad(string url, Action<byte[]> succHandler, Action failHandler = null, Action retryHandler = null, float retryTime = 0)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                yield return uwr.SendWebRequest();
                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    failHandler?.Invoke();
                    if (retryTime > 0)
                    {
                        yield return new WaitForSeconds(retryTime);
                        retryHandler?.Invoke();
                    }
                }
                else
                {
                    succHandler?.Invoke(uwr.downloadHandler.data);
                }
                uwr.Dispose();
            }
        }

        private void LoadABData(byte[] bytes, out Dictionary<string, ABData> dic)
        {
            dic = new Dictionary<string, ABData>();
            var abDataReader = new ABDataReader();
            var abDatas = abDataReader.Read(bytes);
            for (int i = 0; i < abDatas.Length; i++)
            {
                var abData = abDatas[i];
                var abPath = abData.assetPath;
                dic.Add(abPath, abData);
            }
        }

        private void LoadABData(Stream stream, out Dictionary<string, ABData> dic)
        {
            dic = new Dictionary<string, ABData>();
            var abDataReader = new ABDataReader();
            var abDatas = abDataReader.Read(stream);
            for (int i = 0; i < abDatas.Length; i++)
            {
                var abData = abDatas[i];
                var abPath = abData.assetPath;
                dic.Add(abPath, abData);
            }
        }

        private void Process()
        {
            //_abNameDictionary = new Dictionary<string, string>();
            _abFileDictionary = new Dictionary<string, string>();
            foreach (var item in _abDataDictionary)
            {
                //_abNameDictionary.Add(item.Value.assetName, item.Key);
                _abFileDictionary.Add(item.Value.fileName, item.Key);
            }
        }

        public string GetABPathByName(string assetName)
        {
            string assetPath;
            _abNameDictionary.TryGetValue(assetName, out assetPath);
            return assetPath;
        }

        public string GetABPathByFile(string fileName)
        {
            string assetPath;
            _abFileDictionary.TryGetValue(fileName, out assetPath);
            return assetPath;
        }

        public ABData GetABDataByPath(string assetPath)
        {
            if (!string.IsNullOrEmpty(assetPath))
            {
                ABData abData;
                _abDataDictionary.TryGetValue(assetPath, out abData);
                return abData;
            }
            return null;
        }

        public ABData GetABDataByName(string assetName)
        {
            string assetPath;
            _abNameDictionary.TryGetValue(assetName, out assetPath);
            if (!string.IsNullOrEmpty(assetPath))
            {
                ABData abData;
                _abDataDictionary.TryGetValue(assetPath, out abData);
                return abData;
            }
            return null;
        }

        public ABData GetABDataByFile(string fileName)
        {
            string assetPath;
            _abFileDictionary.TryGetValue(fileName, out assetPath);
            if (!string.IsNullOrEmpty(assetPath))
            {
                ABData abData;
                _abDataDictionary.TryGetValue(assetPath, out abData);
                return abData;
            }
            return null;
        }

        public ABData[] GetAllABData(string directory)
        {
            var list = ListPool<ABData>.Get();
            foreach (var item in _abDataDictionary)
            {
                if (item.Key.StartsWith(directory))
                {
                    list.Add(item.Value);
                }
            }
            return list.ToArray();
        }

        public string[] GetAllPath(string rootPath)
        {
            var gets = ListPool<string>.Get();
            foreach (var kp in _abDataDictionary)
            {
                if (kp.Value.buildType == BuildType.Root && kp.Key.StartsWith(rootPath))
                {
                    gets.Add(kp.Key);
                }
            }
            var ret = gets.ToArray();
            ListPool<string>.Release(gets);
            return ret;
        }

        #endregion
    }
}

