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
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// ABLoadManager AB包加载管理器
    /// </summary>
    public class ABManager : MonoBehaviour
    {
        #region Const

        /// <summary>
        /// 同时最大的加载数
        /// </summary>
        private const int kMAX_REQUEST = 64;

        #endregion

        #region Static

        /// <summary>
        /// 
        /// </summary>
        public static ABManager Instance;

        /// <summary>
        /// 版本
        /// </summary>
        public static Version Version = new Version(1, 0, 1);

        #endregion

        #region Field

        /// <summary>
        /// 检查时间
        /// </summary>
        private float _checkTime;
        /// <summary>
        /// 可再次申请的加载数
        /// </summary>
        private int _requestRemain = kMAX_REQUEST;
        /// <summary>
        /// 请求加载的队列
        /// </summary>
        private List<ABLoader> _requestQueue = new List<ABLoader>();
        /// <summary>
        /// 开始加载队列
        /// </summary>
        private List<ABLoader> _loadingQueue = new List<ABLoader>();
        /// <summary>
        /// 等待加载集合
        /// </summary>
        private HashSet<ABLoader> _waitingLoaderSet = new HashSet<ABLoader>();
        /// <summary>
        /// 当前所有加载集合(加载完成之后统一设置)
        /// </summary>
        private HashSet<ABLoader> _currentLoaderSet = new HashSet<ABLoader>();
        /// <summary>
        /// 已创建的所有加载列表
        /// </summary>
        private Dictionary<string, ABLoader> _allABLoaders = new Dictionary<string, ABLoader>();
        /// <summary>
        /// 已加载完成的缓存列表
        /// </summary>
        private Dictionary<string, ABInfo> _allABInfos = new Dictionary<string, ABInfo>();

        #endregion

        #region Properties

        /// <summary>
        /// 当前是否在加载状态
        /// </summary>
        public bool isLoading { get; private set; }

        #endregion

        #region API

        /// <summary>
        /// 初始化 加载资源清单文件
        /// </summary>
        /// <param name="callback"></param>
        public void Initialize(Action<string> callback)
        {
            ABDataManager.Instance.LoadConfig(callback);
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public ABInfo LoadAssetAtPath(string assetPath)
        {
            if (!string.IsNullOrEmpty(assetPath))
            {
                var abInfo = GetABInfo(assetPath);
                if (abInfo != null)
                {
                    return abInfo;
                }
                var abLoader = CreateABLoader(assetPath);
                if (abLoader != null)
                {
                    return abLoader.Load();
                }
                else
                {
                    Debug.LogError("[ABManager.LoadAssetAtPath] ABLoad is null! " + assetPath);
                }
            }
            return null;
        }

        /// <summary>
        /// 用默认优先级为0的值加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="handler">回调</param>
        /// <returns></returns>
        public ABLoader LoadAsync(string path, Action<ABInfo> callback = null)
        {
            return LoadBundleAsync(path, 0, callback);
        }

        /// <summary>
        /// 通过一个路径加载ab
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="prority">优先级</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public ABLoader LoadAsync(string path, int prority, Action<ABInfo> callback = null)
        {
            return LoadBundleAsync(path, prority, callback);
        }

        public ABInfo GetABInfo(string assetPath)
        {
            ABInfo abInfo;
            _allABInfos.TryGetValue(assetPath, out abInfo);
            return abInfo;
        }

        public void RemoveABInfo(string assetPath)
        {
            this.RemoveABInfo(this.GetABInfo(assetPath));
        }

        public void RemoveAll()
        {
            this.StopAllCoroutines();

            _loadingQueue.Clear();
            _requestQueue.Clear();
            _allABLoaders.Clear();

            foreach (var abInfo in _allABInfos.Values)
            {
                abInfo.Dispose();
            }
            _allABInfos.Clear();
        }

        /// <summary>
        /// 卸载不用的
        /// </summary>
        public void UnloadUnused(bool force = false)
        {
            if (force || !isLoading)
            {
                var keys = ListPool<string>.Get();
                keys.AddRange(_allABInfos.Keys);

                //一次卸载(太卡)
                int unloadLimit = 16;
                int unloadCount = 0;

                var hasUnused = false;
                do
                {
                    hasUnused = false;
                    for (int i = keys.Count - 1; i >= 0 && unloadCount < unloadLimit; i--)
                    {
                        var abInfo = _allABInfos[keys[i]];
                        if (abInfo.isUnused)
                        {
                            this.RemoveABInfo(abInfo);
                            keys.RemoveAt(i);
                            hasUnused = true;
                            if (++unloadCount >= unloadLimit)
                            {
                                _checkTime = 0f;
                                break;
                            }
                        }
                    }
                } while (hasUnused && unloadCount < unloadLimit);

                ListPool<string>.Release(keys);
            }
        }

        #endregion

        #region Method      

        /// <summary>
        /// 统一创建加载器
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        internal ABLoader CreateABLoader(string assetPath)
        {
            ABLoader abLoader;
            if (!_allABLoaders.TryGetValue(assetPath, out abLoader))
            {
                var abData = ABDataManager.Instance.GetABDataByPath(assetPath);
                if (abData != null)
                {
                    abLoader = new ABLoader(this, abData);
                    _allABLoaders.Add(assetPath, abLoader);
                }
            }
            return abLoader;
        }

        /// <summary>
        /// 统一创建资源信息
        /// </summary>
        /// <param name="abInfo"></param>
        /// <returns></returns>
        internal ABInfo CreateABInfo(ABData abData)
        {
            var abInfo = new ABInfo
            {
                abData = abData,
            };
            _allABInfos[abInfo.assetPath] = abInfo;
            return abInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abInfo"></param>
        internal void RemoveABInfo(ABInfo abInfo)
        {
            if (abInfo != null)
            {
                abInfo.Dispose();
                _allABInfos.Remove(abInfo.assetPath);
            }
        }

        /// <summary>
        /// 通过一个路径加载ab
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="prority">优先级</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        private ABLoader LoadBundleAsync(string path, int prority, Action<ABInfo> callback = null)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var abLoader = this.CreateABLoader(path);
                if (abLoader != null)
                {
                    if (abLoader.completed)
                    {
                        if (callback != null)
                        {
                            callback(abLoader.abInfo);
                        }
                    }
                    else
                    {
                        if (abLoader.prority < prority)
                        {
                            abLoader.prority = prority;
                        }
                        _currentLoaderSet.Add(abLoader);
                        _waitingLoaderSet.Add(abLoader);
                        abLoader.onComplete += callback;
                        isLoading = true;
                    }
                    return abLoader;
                }
            }
            if (callback != null)
            {
                callback(null);
            }
            return null;
        }

        /// <summary>
        /// 请求加载，这里统一分配加载时机，防止加载太卡
        /// </summary>
        /// <param name="abLoader"></param>
        internal void Enqueue(ABLoader abLoader)
        {
            _requestQueue.Add(abLoader);
        }

        /// <summary>
        /// 异步加载完成
        /// </summary>
        /// <param name="abLoader"></param>
        internal void LoadAsyncComplete(ABLoader abLoader)
        {
            _requestRemain = Mathf.Min(_requestRemain + 1, kMAX_REQUEST);
            _loadingQueue.Remove(abLoader);

            if (_loadingQueue.Count == 0 && _waitingLoaderSet.Count == 0)
            {
                isLoading = false;

                foreach (var item in _currentLoaderSet)
                {
                    if (item.abInfo != null)
                    {
                        item.abInfo.ResetLifeTime();
                    }
                }

                _currentLoaderSet.Clear();
            }
        }

        /// <summary>
        /// 异步加载失败
        /// </summary>
        /// <param name="abLoader"></param>
        internal void LoadAsyncError(ABLoader abLoader)
        {
            LoadAsyncComplete(abLoader);
        }

        /// <summary>
        /// 检查加载
        /// </summary>
        private void CheckLoader()
        {
            if (_waitingLoaderSet.Count > 0)
            {
                var abLoaders = ListPool<ABLoader>.Get();
                abLoaders.AddRange(_waitingLoaderSet);
                _waitingLoaderSet.Clear();
                foreach (var abLoader in abLoaders)
                {
                    if (abLoader != null)
                    {
                        abLoader.LoadAsync();
                        _loadingQueue.Add(abLoader);
                    }
                }
                ListPool<ABLoader>.Release(abLoaders);
            }
        }

        /// <summary>
        /// 检查队列
        /// </summary>
        private void CheckQueue()
        {
            if (_requestRemain > 0 && _requestQueue.Count > 0)
            {
                _requestQueue.Sort();
            }

            while (_requestRemain > 0 && _requestQueue.Count > 0)
            {
                var abLoader = _requestQueue[0];
                _requestQueue.RemoveAt(0);

                if (abLoader != null && !abLoader.completed)
                {
                    _requestRemain--;
                    abLoader.LoadBundleAsync();
                }
            }
        }

        private void CheckUnusedBundle()
        {
            this.UnloadUnused();
            //Resources.UnloadUnusedAssets();
            //System.GC.Collect();
        }

        #endregion

        #region Unity

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (isLoading)
            {
                CheckLoader();
                CheckQueue();
            }
            else
            {
                _checkTime -= Time.deltaTime;
                if (_checkTime < 0f)
                {
                    _checkTime = 6f;
                    CheckUnusedBundle();
                }
            }
        }

        private void OnDestroy()
        {
            this.RemoveAll();
        }

        #endregion
    }
}
