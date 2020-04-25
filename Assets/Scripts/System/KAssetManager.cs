// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
#if UNITY_EDITOR
#define USE_EDITOR_RES
#endif

using Game.DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class KAssetManager : MonoBehaviour
    {

        #region STATIC

        private static KAssetManager _Instance;

        public static KAssetManager Instance
        {
            get { return _Instance; }
        }

        #endregion

        #region FIELD

        /// <summary>
        /// 异步加载回调
        /// </summary>
        private AsyncR _asyncR = new AsyncR();

        /// <summary>
        /// 公共资源
        /// </summary>
        private Dictionary<string, Object> _globalAssets = new Dictionary<string, Object>(System.StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 角色模型 猫
        /// </summary>
        private Dictionary<string, Object> _characterAssets = new Dictionary<string, Object>(System.StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 宠物模型2D
        /// </summary>
        private Dictionary<string, Object> _pet2DAssets = new Dictionary<string, Object>(System.StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 宠物模型
        /// </summary>
        private Dictionary<string, Object> _petAssets = new Dictionary<string, Object>(System.StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 建筑资源
        /// </summary>
        private Dictionary<string, Object> _buildingAssets = new Dictionary<string, Object>(System.StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 三消资源
        /// </summary>
        private Dictionary<string, Object> _match3Assets = new Dictionary<string, Object>(System.StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// UI资源
        /// </summary>
        private Dictionary<string, Object> _uiAssets = new Dictionary<string, Object>(System.StringComparer.OrdinalIgnoreCase);

        #endregion

        #region API

        /// <summary>
        /// 已经初始化
        /// </summary>
        public bool initialized
        {
            get;
            private set;
        }

        public void Initialize()
        {
#if USE_EDITOR_RES
            OnInitAssetsCallback("");
#else
            KGameRes.Initialize(OnInitAssetsCallback);
#endif
        }

        private void OnInitAssetsCallback(string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                initialized = true;
                GameApp.Instance.ResourceUpdateCompleted();
            }
            else
            {
                Debuger.LogError("Init Assets Error");
            }
        }

        public void LoadTables()
        {
            XTable.GlobalXTable.Load();
            XTable.ErrorXTable.Load();
            XTable.GuideXTable.Load();
            XTable.ItemXTable.Load();
            XTable.MissionXTable.Load();
            XTable.CatXTable.Load();
            XTable.SkillXTable.Load();
            XTable.ElementXTable.Load();
            XTable.LevelXTable.Load();
            XTable.ChapterUnlockXTable.Load();
            XTable.ShopXTable.Load();
            XTable.MailXTable.Load();
            XTable.PayXTable.Load();
            XTable.SignXTable.Load();
        }

        /// <summary>
        /// 加载配置文件并解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        public bool LoadExcelAsset(string name, IXTable[] tables)
        {
            TextAsset tmpText;
            if (TryGetExcelAsset(name, out tmpText))
            {
                if (tmpText)
                {
                    var tmpTable = tmpText.bytes.ToJsonTable();
                    foreach (var table in tables)
                    {
                        table.LoadFromHashtable(tmpTable);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 加载公共资源
        /// </summary>
        /// <returns></returns>
        public IAsyncR LoadGlobalAssets()
        {
            this.AsyncProcess(0f);
#if USE_EDITOR_RES
            StartCoroutine(LoadAssets("Assets/Res/Global", "t:GameObject", _globalAssets));
#else
        if (_globalAssets.Count == 0)
        {
            StartCoroutine(LoadAssetsRoutine("Res/Global/", _globalAssets));
        }
        else
        {
            this.AsyncSuccess(null);
        }
#endif
            return this.GetAsyncR();
        }

        /// <summary>
        /// 加载三消资源
        /// </summary>
        /// <returns></returns>
        public IAsyncR LoadMatch3Assets()
        {
            this.AsyncProcess(0f);
#if USE_EDITOR_RES
            StartCoroutine(LoadAssets("Assets/Res/Match3", "t:GameObject", _match3Assets));
#else
            if (_match3Assets.Count == 0)
        {
            StartCoroutine(LoadAssetsRoutine("Res/Match3/", _match3Assets));
        }
        else
        {
            this.AsyncSuccess(null);
        }
#endif
            return this.GetAsyncR();
        }

        /// <summary>
        /// 卸载三消资源
        /// </summary>
        public void UnloadMatch3Assets(bool immediately = false)
        {
            //foreach (var item in _match3Assets)
            //{
            //    if (item.Value)
            //    {
            //        Object.Destroy(item.Value);
            //    }
            //}
            _match3Assets.Clear();
            if (immediately)
            {
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
        }

        /// <summary>
        /// 卸载建筑资源
        /// </summary>
        /// <param name="immediately"></param>
        public void UnloadBuildingAssets(bool immediately = false)
        {
            //foreach (var item in _buildingAssets)
            //{
            //    if (item.Value)
            //    {
            //        Object.Destroy(item.Value);
            //    }
            //}
            _buildingAssets.Clear();

            if (immediately)
            {
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
        }

        /// <summary>
        /// 卸载角色资源
        /// </summary>
        /// <param name="immediately"></param>
        public void UnloadCharacterAssets(bool immediately = false)
        {
            //foreach (var item in _characterAssets)
            //{
            //    if (item.Value)
            //    {
            //        Object.Destroy(item.Value);
            //    }
            //}
            _characterAssets.Clear();

            if (immediately)
            {
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
        }

        #endregion

        #region GetAsset

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        public bool TryGetExcelAsset(string name, out TextAsset asset)
        {
#if USE_EDITOR_RES
            var paths = new List<string> { "Assets/Res/Table" };
            paths.AddRange(System.IO.Directory.GetDirectories(paths[0]));
            var guids = UnityEditor.AssetDatabase.FindAssets(System.IO.Path.GetFileName(name) + " t:TextAsset", paths.ToArray());
            if (guids.Length == 0)
            {
                Debug.LogError("找不到配置表：" + name);
                asset = null;
                return false;
            }
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
#else
            asset = KGameRes.LoadAssetAtPath("Res/Table/" + name) as TextAsset;
#endif
            return asset;
        }

        /// <summary>
        /// 加载棋盘配置文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        public bool TryGetChessboardAsset(string name, out TextAsset asset)
        {
#if USE_EDITOR_RES
            var paths = new List<string> { "Assets/Res/Chessboard" };
            paths.AddRange(System.IO.Directory.GetDirectories(paths[0]));
            var guids = UnityEditor.AssetDatabase.FindAssets(System.IO.Path.GetFileName(name) + " t:TextAsset", paths.ToArray());
            if (guids.Length == 0)
            {
                Debug.LogError("找不到棋盘配置表：" + name);
                asset = null;
                return false;
            }
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
#else
            asset = KGameRes.LoadAssetAtPath("Res/Chessboard/" + name) as TextAsset;
#endif
            return asset;
        }

        /// <summary>
        /// 读取声音资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        public bool TryGetSoundAsset(string name, out AudioClip asset)
        {
#if USE_EDITOR_RES
            var paths = new List<string>
        {
            "Assets/Res/Sounds"
        };
            paths.AddRange(System.IO.Directory.GetDirectories(paths[0]));
            var guids = UnityEditor.AssetDatabase.FindAssets(System.IO.Path.GetFileName(name), paths.ToArray());
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            asset = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
#else
            asset = KGameRes.LoadAssetAtPath("Res/Sounds/" + name) as AudioClip;
#endif
            return asset;
        }

        /// <summary>
        /// 公共资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public bool TryGetGlobalPrefab(string name, out GameObject prefab)
        {
            Object asset;
            if (_globalAssets.TryGetValue(name, out asset))
            {
                prefab = asset as GameObject;
                return true;
            }

#if USE_EDITOR_RES
            var assetPath = "Assets/Res/Global/" + name + ".prefab";
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
#else
            prefab = KGameRes.LoadAssetAtPath("Res/Global/" + name) as GameObject;
#endif
            _globalAssets.Add(name, prefab);
            return prefab;
        }

        /// <summary>
        /// 角色模型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public bool TryGetCharacterPrefab(string name, out GameObject prefab)
        {
            Object asset;
            if (_characterAssets.TryGetValue(name, out asset))
            {
                prefab = asset as GameObject;
                return true;
            }

#if USE_EDITOR_RES
            var assetPath = "Assets/Res/Character/" + name + ".prefab";
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
#else
        prefab = KGameRes.LoadAssetAtPath("Res/Character/" + name) as GameObject;
#endif
            _characterAssets.Add(name, prefab);
            return prefab;
        }

        public void TryGetCharacterPrefab(string name, Action<GameObject> callback)
        {
            Object asset;
            if (_characterAssets.TryGetValue(name, out asset))
            {
                if (callback != null)
                {
                    callback(asset as GameObject);
                }
                return;
            }
#if USE_EDITOR_RES
            if (callback != null)
            {
                var assetPath = "Assets/Res/Character/" + name + ".prefab";
                asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                callback(asset as GameObject);
                _characterAssets.Add(name, asset);
            }
#else
        KGameRes.LoadAsync("Res/Character/" + name, (obj) =>
        {
            _characterAssets.Add(name, obj);
            if (callback != null)
            {
                callback(obj as GameObject);
            }
        });
#endif
        }


        /// <summary>
        /// 宠物模型2D
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public bool TryGetPet2DPrefab(string name, out GameObject prefab)
        {
            Object asset;
            if (_pet2DAssets.TryGetValue(name, out asset))
            {
                prefab = asset as GameObject;
                return true;
            }

#if USE_EDITOR_RES
            var assetPath = "Assets/Res/Pet2D/" + name + ".prefab";
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
#else
            prefab = KGameRes.LoadAssetAtPath("Res/Pet2D/" + name) as GameObject;
#endif
            _pet2DAssets.Add(name, prefab);
            return prefab;
        }

        public void TryGetPet2DPrefab(string name, Action<GameObject> callback)
        {
            Object asset;
            if (_pet2DAssets.TryGetValue(name, out asset))
            {
                if (callback != null)
                {
                    callback(asset as GameObject);
                }
                return;
            }
#if USE_EDITOR_RES
            if (callback != null)
            {
                var assetPath = "Assets/Res/Pet2D/" + name + ".prefab";
                asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                callback(asset as GameObject);
                _pet2DAssets.Add(name, asset);
            }
#else
            KGameRes.LoadAsync("Res/Pet2D/" + name, (obj) =>
        {
            _pet2DAssets.Add(name, obj);
            if (callback != null)
            {
                callback(obj as GameObject);
            }
        });
#endif
        }

        /// <summary>
        /// 宠物模型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public bool TryGetPetPrefab(string name, out GameObject prefab)
        {
            Object asset;
            if (_petAssets.TryGetValue(name, out asset))
            {
                prefab = asset as GameObject;
                return true;
            }

#if USE_EDITOR_RES
            var assetPath = "Assets/Res/Pet/" + name + ".prefab";
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
#else
        prefab = KGameRes.LoadAssetAtPath("Res/Pet/" + name) as GameObject;
#endif
            _petAssets.Add(name, prefab);
            return prefab;
        }

        public void TryGetPetPrefab(string name, Action<GameObject> callback)
        {
            Object asset;
            if (_petAssets.TryGetValue(name, out asset))
            {
                if (callback != null)
                {
                    callback(asset as GameObject);
                }
                return;
            }
#if USE_EDITOR_RES
            if (callback != null)
            {
                var assetPath = "Assets/Res/Pet/" + name + ".prefab";
                asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                callback(asset as GameObject);
                _petAssets.Add(name, asset);
            }
#else
        KGameRes.LoadAsync("Res/Pet/" + name, (obj) =>
        {
            _petAssets.Add(name, obj);
            if (callback != null)
            {
                callback(obj as GameObject);
            }
        });
#endif
        }


        /// <summary>
        /// 建筑资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public bool TryGetBuildingPrefab(string name, out GameObject prefab)
        {
            Object asset;
            if (_buildingAssets.TryGetValue(name, out asset))
            {
                prefab = asset as GameObject;
                return true;
            }

#if USE_EDITOR_RES
            var assetPath = "Assets/Res/Building/" + name + ".prefab";
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
#else
            prefab = KGameRes.LoadAssetAtPath("Res/Building/" + name) as GameObject;
#endif
            _buildingAssets.Add(name, prefab);
            return prefab;
        }

        /// <summary>
        /// UI资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public bool TryGetUIPrefab(string name, out GameObject prefab)
        {
            //Object asset;
            //if (_uiAssets.TryGetValue(name, out asset))
            //{
            //    prefab = asset as GameObject;
            //    return true;
            //}
#if USE_EDITOR_RES
            var assetPath = "Assets/Res/UI/" + name + ".prefab";
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
#else
            prefab = KGameRes.LoadAssetAtPath("Res/UI/" + name) as GameObject;
        //if (prefab)
        //{
        //    var ret = Instantiate(prefab);
        //    ret.name = prefab.name;
        //    ret.SetActive(false);
        //    DontDestroyOnLoad(ret);
        //    ret.hideFlags = HideFlags.HideAndDontSave;
        //    _uiAssets.Add(name, ret);
        //}
#endif
            return prefab;
        }

        public void TryGetUIPrefab(string name, Action<GameObject> callback)
        {
            //Object asset;
            //if (_uiAssets.TryGetValue(name, out asset))
            //{
            //    if (callback != null)
            //    {
            //        callback(asset as GameObject);
            //    }
            //    return;
            //}
#if USE_EDITOR_RES
            if (callback != null)
            {
                var assetPath = "Assets/Res/UI/" + name + ".prefab";
                callback(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath));
            }
#else
            KGameRes.LoadAsync("Res/UI/" + name, (prefab) =>
        {
            //var ret = prefab as GameObject;
            //if (ret)
            //{
            //    ret = Instantiate(ret);
            //    ret.name = prefab.name;
            //    ret.SetActive(false);
            //    DontDestroyOnLoad(ret);
            //    ret.hideFlags = HideFlags.HideAndDontSave;
            //    _uiAssets.Add(name, ret);
            //}
            if (callback != null)
            {
                callback(prefab as GameObject);
            }
        });
#endif
        }

        /// <summary>
        /// 三消资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public bool TryGetMatchPrefab(string name, out GameObject prefab)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Object asset;
                if (_match3Assets.TryGetValue(name, out asset))
                {
                    prefab = asset as GameObject;
                    return true;
                }
            }
            prefab = null;
            return false;
        }
        #endregion

        #region METHOD

        /// <summary>
        /// 编辑下资源读取
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filter"></param>
        /// <param name="assetDictionary"></param>
        /// <returns></returns>
        private IEnumerator LoadAssets(string path, string filter, Dictionary<string, Object> assetDictionary)
        {
#if UNITY_EDITOR

            if (assetDictionary.Count == 0)
            {
                var guids = UnityEditor.AssetDatabase.FindAssets(filter, new string[] { path });

                float totalMilliseconds = 0f;
                for (int i = 0; i < guids.Length; i++)
                {
                    var startT = DateTime.UtcNow;
                    var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                    var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                    if (asset && !assetDictionary.ContainsKey(asset.name))
                    {
                        assetDictionary.Add(asset.name, asset);
                    }
                    else
                    {
                        Debug.LogError(string.Format("[{0}].[{1}]: {2} 有问题", path, filter, assetPath));
                    }
                    this.AsyncProcess((float)i / guids.Length);
                    var currMilliseconds = (float)((DateTime.UtcNow - startT).TotalMilliseconds);
                    //Debug.Log(assetPath + " load time " + currMilliseconds);
                    totalMilliseconds += currMilliseconds;
                    if (totalMilliseconds > 33f)
                    {
                        totalMilliseconds = 0;
                        yield return null;
                    }
                }
            }

#endif
            yield return null;
            this.AsyncSuccess(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadAssetsRoutine(string rootPath, Dictionary<string, Object> assetDictionary)
        {
            var paths = KGameRes.GetAllAssetPath(rootPath);
            int length = paths.Length;

            float totalMilliseconds = 0f;
            for (int i = 0; i < length; i++)
            {
                var startT = DateTime.UtcNow;

                var path = paths[i];
                var obj = KGameRes.LoadAssetAtPath(path);
                assetDictionary.Add(obj.name, obj);
                this.AsyncProcess((float)i / length);

                totalMilliseconds += (float)((DateTime.UtcNow - startT).TotalMilliseconds);
                if (totalMilliseconds > 30f)
                {
                    totalMilliseconds = 0f;
                    yield return null;
                }
            }

            yield return null;
            this.AsyncSuccess(null);
        }

        private GameObject LoadUI(string path)
        {
            GameObject ret;
            TryGetUIPrefab(path, out ret);
            return ret;
        }

        public void LoadUIAsync(string path, Action<GameObject> callback)
        {
            TryGetUIPrefab(path, callback);
        }

        public IAsyncR LoadAll()
        {
#if UNITY_EDITOR
            LoadMatch3Assets();
#endif
            return GetAsyncR();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private AsyncR GetAsyncR()
        {
            return _asyncR;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progress"></param>
        private void AsyncProcess(float progress)
        {
            _asyncR.AsyncProcess(progress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void AsyncSuccess(object data)
        {
            _asyncR.AsyncSuccess(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        private void AsyncFailure(string error)
        {
            _asyncR.AsyncFailure(error);
        }

        #endregion

        #region UNITY

        /// <summary>Awakes this instance.</summary>
        private void Awake()
        {
            _Instance = this;

            KUIBind.Bind(LoadUI, LoadUIAsync);
        }

        #endregion

        #region Model

        private class AsyncR : IAsyncR
        {
            #region Field

            /// <summary>
            /// 
            /// </summary>
            private float _progress;
            /// <summary>
            /// 
            /// </summary>
            private string _error;
            /// <summary>
            /// 
            /// </summary>
            private object _data;

            #endregion

            #region Property

            bool IAsyncR.done
            {
                get
                {
                    return _progress > 1f;
                }
            }

            string IAsyncR.error
            {
                get
                {
                    return _error;
                }
            }

            float IAsyncR.progress
            {
                get
                {
                    return _progress;
                }
            }

            object IAsyncR.asyncData
            {
                get
                {
                    return _data;
                }
            }

            AssetBundle IAsyncR.assetBundle
            {
                get
                {
                    return _data as AssetBundle;
                }
            }

            #endregion

            #region Method

            public void AsyncProcess(float progress)
            {
                _progress = progress;
                _error = null;
                _data = null;
            }

            public void AsyncSuccess(object data)
            {
                _progress = 1.0001f;
                _error = null;
                _data = data;
            }

            public void AsyncFailure(string error)
            {
                _progress = 1.0001f;
                _error = error;
                _data = null;
            }

            #endregion
        }

        #endregion

    }

    #region INTERFACE

    public interface IAsyncR
    {
        /// <summary>Gets a value indicating whether this <see cref="IAsyncR"/> is done.</summary>
        bool done
        {
            get;
        }

        /// <summary>Gets the error.</summary>
        string error
        {
            get;
        }

        /// <summary>Gets or sets the progress.</summary>
        float progress
        {
            get;
        }

        /// <summary>Gets or sets the data.</summary>
        object asyncData
        {
            get;
        }

        /// <summary>Gets the asset bundle.</summary>        
        AssetBundle assetBundle
        {
            get;
        }
    }

    #endregion

}