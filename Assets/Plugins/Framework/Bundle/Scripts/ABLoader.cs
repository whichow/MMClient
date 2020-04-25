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
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Loader 父类
    /// </summary>
    public class ABLoader : IComparable<ABLoader>
    {
        public enum State
        {
            kNone = 0,
            kDepLoading = 1,
            kLoading = 2,
            kComplete = 3,
            kError = 4,
        }

        /// <summary>
        /// id 计数器
        /// </summary>
        private static int _IdCounter = 0;

        #region Field

        public Action<ABInfo> onComplete;

        /// <summary>
        /// 加载依赖项的数量
        /// </summary>
        private int _loadingDepLoaderCount;
        /// <summary>
        /// 依赖项Loader
        /// </summary>
        private ABLoader[] _depLoaders;
        /// <summary>
        /// 缓存文件
        /// </summary>
        private string _abCachedFile;
        /// <summary>
        /// 来源文件
        /// </summary>
        private string _abSourceFile;
        /// <summary>
        /// 优先级
        /// </summary>
        private int _prority;

        #endregion

        #region Property

        /// <summary>
        /// 
        /// </summary>
        public int id { get; private set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int prority
        {
            get { return _prority; }
            set
            {
                if (_prority != value)
                {
                    _prority = value;
                    RefreshPrority();
                }
            }
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public bool asyncMethod { get; private set; }

        /// <summary>
        /// 完成
        /// </summary>
        public bool completed
        {
            get { return state == State.kError || state == State.kComplete; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public State state { get; private set; }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string stateMessage { get; private set; }

        public string abName { get; private set; }

        public ABManager abManager { get; private set; }

        public ABData abData { get; private set; }

        public ABInfo abInfo { get; private set; }

        public AssetBundle assetBundle { get; private set; }

        #endregion

        #region Constructor

        public ABLoader(ABManager manager, ABData data)
        {
            id = _IdCounter++;
            abManager = manager;
            abData = data;
            abName = data.fileName;
        }

        int IComparable<ABLoader>.CompareTo(ABLoader other)
        {
            if (other != null)
            {
                int compare = other.prority.CompareTo(prority);
                if (compare == 0)
                {
                    return id.CompareTo(other.id);
                }
                return compare;
            }
            return 1;
        }

        #endregion

        #region Method 

        private void CheckState()
        {
            if (abInfo != null && !abInfo.isValid)
            {
                state = State.kNone;
            }
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <returns></returns>
        public ABInfo Load()
        {
            CheckState();

            switch (state)
            {
                case State.kNone:
                    state = State.kLoading;
                    this.LoadDependencies();
                    this.LoadBundle();
                    break;
                case State.kComplete:
                    this.Complete();
                    break;
                case State.kError:
                    this.Error("Load Error");
                    break;
            }
            return abInfo;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public void LoadAsync()
        {
            CheckState();

            switch (state)
            {
                case State.kNone:
                    state = State.kLoading;
                    this.LoadDependenciesAsync();
                    break;
                case State.kComplete:
                    this.CompleteAsync();
                    break;
                case State.kError:
                    this.Error("Load Error");
                    break;
            }
        }

        /// <summary>
        /// 同步加载AssetBundle
        /// </summary>
        public void LoadBundle()
        {
            _abCachedFile = ABDefine.GetCachedFile(abName);
            _abSourceFile = ABDefine.GetSourceFile(abName);
            if (ABUtils.ExistsFile(_abCachedFile))
            {
                LoadFromCaching();
            }
            else
            {
                LoadFromPackage();
            }
        }
        /// <summary>
        /// 异步加载AssetBundle
        /// </summary>
        public void LoadBundleAsync()
        {
            _abCachedFile = ABDefine.GetCachedFile(abName);
            _abSourceFile = ABDefine.GetSourceFile(abName);
            if (ABUtils.ExistsFile(_abCachedFile))
            {
                abManager.StartCoroutine(LoadFromCachingAsync());
            }
            else
            {
                abManager.StartCoroutine(LoadFromPackageAsync());
            }
        }

        /// <summary>
        /// 先加载依赖项
        /// </summary>
        private void LoadDependencies()
        {
            if (_depLoaders == null)
            {
                var dependencies = abData.dependencies;
                if (dependencies != null && dependencies.Length > 0)
                {
                    _depLoaders = new ABLoader[dependencies.Length];
                    for (int i = _depLoaders.Length - 1; i >= 0; i--)
                    {
                        _depLoaders[i] = abManager.CreateABLoader(abData.GetDependence(i));
                    }
                }
                else
                {
                    _depLoaders = new ABLoader[0];
                }
            }

            for (int i = _depLoaders.Length - 1; i >= 0; i--)
            {
                var depLoader = _depLoaders[i];
                if (!depLoader.completed)
                {
                    depLoader.Load();
                }
            }
        }

        /// <summary>
        /// 先加载依赖项
        /// </summary>
        private void LoadDependenciesAsync()
        {
            if (_depLoaders == null)
            {
                int depCount = abData.GetDependenceCount();
                _depLoaders = new ABLoader[depCount];
                for (int i = 0; i < depCount; i++)
                {
                    _depLoaders[i] = abManager.CreateABLoader(abData.GetDependence(i));
                }
                RefreshPrority();
            }

            _loadingDepLoaderCount = 0;
            for (int i = _depLoaders.Length - 1; i >= 0; i--)
            {
                var depLoader = _depLoaders[i];
                if (!depLoader.completed)
                {
                    _loadingDepLoaderCount++;
                    depLoader.onComplete += OnDepCompleted;
                    depLoader.LoadAsync();
                }
            }

            this.CheckDependencies();
        }

        /// <summary>
        /// 依赖项结束，加载自身
        /// </summary>
        private void CheckDependencies()
        {
            if (_loadingDepLoaderCount == 0)
            {
                abManager.Enqueue(this);
            }
        }

        /// <summary>
        /// 从已缓存的文件里加载(后面做crc校验)
        /// </summary>
        /// <returns></returns>
        private void LoadFromCaching()
        {
            if (!completed)
            {
                assetBundle = AssetBundle.LoadFromFile(_abCachedFile);
                this.Complete();
            }
        }

        /// <summary>
        /// 从已缓存的文件里加载(后面做crc校验)
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadFromCachingAsync()
        {
            if (!completed)
            {
                var request = AssetBundle.LoadFromFileAsync(_abCachedFile);
                yield return request;
                assetBundle = request.assetBundle;
                this.CompleteAsync();
            }
        }

        /// <summary>
        /// 从源文件(安装包里)加载
        /// </summary>
        /// <returns></returns>
        private void LoadFromPackage()
        {
            if (!completed)
            {
                assetBundle = AssetBundle.LoadFromFile(_abSourceFile);
                this.Complete();
            }
        }

        /// <summary>
        /// 从源文件(安装包里)加载
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadFromPackageAsync()
        {
            if (!completed)
            {
                var request = AssetBundle.LoadFromFileAsync(_abSourceFile);
                yield return request;
                assetBundle = request.assetBundle;
                this.CompleteAsync();
            }
        }

        private void CreateABInfo()
        {
            if (abInfo == null)
            {
                this.abInfo = abManager.CreateABInfo(abData);
                this.abInfo.Ready(assetBundle);
                this.abInfo.onUnloaded = OnAssetBundleUnload;
                this.assetBundle = null;

                foreach (var depLoader in _depLoaders)
                {
                    abInfo.AddDependencies(depLoader.abInfo);
                }
            }
        }

        private void Complete()
        {
            CreateABInfo();
            this.state = State.kComplete;
            OnCompleted();
        }

        private void CompleteAsync()
        {
            CreateABInfo();
            this.state = State.kComplete;
            OnCompleted();
            abManager.LoadAsyncComplete(this);
        }

        private void Error(string message)
        {
            this.abInfo = null;
            this.state = State.kError;
            this.stateMessage = message;
            OnCompleted();
            abManager.LoadAsyncError(this);
        }

        protected void RefreshPrority()
        {
            if (_depLoaders != null)
            {
                for (int i = _depLoaders.Length - 1; i >= 0; i--)
                {
                    var depLoader = _depLoaders[i];

                    if (depLoader.prority <= prority)
                    {
                        depLoader.prority = prority + 1;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abInfo"></param>
        private void OnDepCompleted(ABInfo abInfo)
        {
            _loadingDepLoaderCount--;
            this.CheckDependencies();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnCompleted()
        {
            if (onComplete != null)
            {
                var handler = onComplete;
                onComplete = null;
                handler(abInfo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abi"></param>
        private void OnAssetBundleUnload(ABInfo abi)
        {
            this.abInfo = null;
            this.state = State.kNone;
            this.prority = 0;
        }

        #endregion

    }
}
