// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace K.AB
{
    public class AssetBuildInfo : System.IComparable<AssetBuildInfo>
    {
        #region Fields

        /// <summary>
        /// 上次打好的AB的Hash值（用于增量打包）
        /// </summary>
        private string _buildHash;

        /// <summary>
        /// 主资源路径
        /// </summary>
        private string _assetPath;
        /// <summary>
        /// 补充资源路径
        /// </summary>
        private HashSet<string> _subAssetPaths = new HashSet<string>();
        /// <summary>
        /// 我依赖的项
        /// </summary>
        private HashSet<AssetBuildInfo> _parentDependencies = new HashSet<AssetBuildInfo>();
        /// <summary>
        /// 依赖我的项
        /// </summary>
        private HashSet<AssetBuildInfo> _childrenDependencies = new HashSet<AssetBuildInfo>();

        /// <summary>
        /// 已分析过依赖
        /// </summary>
        private bool _analysed = false;
        /// <summary>
        /// 目标文件是否已改变
        /// </summary>
        private bool _fileChanged = false;
        /// <summary>
        /// 依赖树是否改变（用于增量打包）
        /// </summary>
        private bool _depTreeChanged = false;

        /// <summary>
        /// 资源类型
        /// </summary>
        public AssetType assetType = AssetType.Asset;
        /// <summary>
        /// 导出类型
        /// </summary>
        public BuildType exportType = BuildType.Asset;
        /// <summary>
        /// 
        /// </summary>
        private bool _preExportProcess;
        /// <summary>
        /// 上次打包的信息（用于增量打包） 
        /// </summary>
        private AssetCacheInfo _cacheInfo;

        #endregion

        #region Properties 

        /// <summary>
        /// 包名(hash)
        /// </summary>
        public string fileName
        {
            get;
            private set;
        }
        /// <summary>
        /// 短名(text)
        /// </summary>
        public string assetName
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int fileSize
        {
            get;
            set;
        }
        /// <summary>
        /// 文件的hash值
        /// </summary>
        public string fileHash
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string metaHash
        {
            get;
            private set;
        }
        public int buildTime
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string buildHash
        {
            get { return _buildHash; }
            set
            {
                if (value != _buildHash)
                {
                    //Debug.Log("Build AB : " + fileName);
                }
                _buildHash = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int buildType
        {
            get
            {
                var tmpType = exportType;
                if (tmpType == BuildType.Root && _childrenDependencies.Count > 0)
                {
                    tmpType |= BuildType.Asset;
                }
                return (int)tmpType;
            }
        }

        /// <summary>
        /// 是不是自己的原因需要重编的，有的可能是因为被依赖项的原因需要重编
        /// </summary>
        public bool needSelfRebuild
        {
            get
            {
                if (_fileChanged || _depTreeChanged)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 是不是自身的原因需要导出如指定的类型prefab等，有些情况下是因为依赖树原因需要单独导出
        /// </summary>
        public bool needSelfExport
        {
            get
            {
                if (assetType == AssetType.Builtin)
                {
                    return false;
                }
                return exportType == BuildType.Root || exportType == BuildType.Standalone;
            }
        }

        /// <summary>
        /// 相对路径
        /// </summary>
        public string assetPath
        {
            get { return _assetPath; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] assetPaths
        {
            get
            {
                return _subAssetPaths.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] dependPaths
        {
            get
            {
                var retList = new List<string>();
                var assetSet = new HashSet<AssetBuildInfo>();
                this.GetDependencies(assetSet);
                foreach (var asset in assetSet)
                {
                    retList.AddRange(asset.assetPaths);
                }
                return retList.ToArray();
            }
        }

        /// <summary>
        /// 我依赖的项
        /// </summary>
        public AssetBuildInfo[] parentDependencies
        {
            get { return _parentDependencies.ToArray(); }
        }

        /// <summary>
        /// 依赖我的项
        /// </summary>
        public AssetBuildInfo[] childrenDependencies
        {
            get { return _childrenDependencies.ToArray(); }
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public int priority
        {
            get
            {
                int ret = _childrenDependencies.Count;
                if (_parentDependencies.Count == 0)
                {
                    return int.MaxValue - ret;
                }
                foreach (var child in _childrenDependencies)
                {
                    if (this == child)
                    {
                        UnityEngine.Debug.LogError("[资源循环引用]" + assetName);
                        break;
                    }
                    ret += child.priority;
                }
                return ret;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public AssetBuildInfo(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                this.fileName = ABDefine.GetABFileName(path);
                this.assetName = ABUtils.GetFileName(path);
            }
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        int System.IComparable<AssetBuildInfo>.CompareTo(AssetBuildInfo other)
        {
            return other.priority.CompareTo(priority);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(this.assetName + " ");
            foreach (var child in childrenDependencies)
            {
                sb.Append(child.assetName + " ");
            }
            return sb.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 设置主资源
        /// </summary>
        /// <param name="value"></param>
        public void SetMainAssetPath(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _assetPath = value;
                _subAssetPaths.Add(value);
            }
        }

        /// <summary>
        /// 增加补充资源
        /// </summary>
        /// <param name="asset"></param>
        public void AddSubAssetPath(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _subAssetPaths.Add(value);
            }
        }

        /// <summary>
        /// 分析引用关系
        /// </summary>
        public void AnalyseDependencies()
        {
            if (_analysed)
            {
                return;
            }
            _analysed = true;

            //_cacheInfo = ABBuildHelper.GetCacheInfo(assetPath);
            //_isFileChanged = _cacheInfo == null || _cacheInfo.assetHash != GetAssetHash() || _cacheInfo.metaHash != _metaHash;
            //if (_cacheInfo != null)
            //{
            //    _buildHash = _cacheInfo.buildHash;
            //    if (_isFileChanged)
            //    {
            //        Debug.Log("File changed: " + assetPath);
            //    }
            //}

            if (!string.IsNullOrEmpty(assetPath))
            {
                var validDependencies = new HashSet<string>();

                var dependencies = AssetDatabase.GetDependencies(assetPath, true);
                foreach (var dependence in dependencies)
                {
                    if (string.IsNullOrEmpty(dependence))
                    {
                        continue;
                    }

                    var extension = ABUtils.GetFileExtensionName(dependence);
                    // 移除Unity对象
                    //不包含脚本对象
                    //不包含LightingDataAsset对象
                    if (extension == ".cs")
                    {
                        continue;
                    }

                    if (validDependencies.Add(dependence))
                    {

                    }
                }

                foreach (var dependence in validDependencies)
                {
                    var abBuildInfo = ABBuildHelper.LoadAsAsset(dependence);
                    if (abBuildInfo != null)
                    {
                        this.AddParentDependencies(abBuildInfo);
                        abBuildInfo.AnalyseDependencies();
                    }
                }
            }
        }

        /// <summary>
        /// 判断是否依赖树变化了
        /// 如果现在的依赖和之前的依赖不一样了则改变了，需要重新打包
        /// </summary>
        public void AnalyzeIfDepTreeChanged()
        {
            _depTreeChanged = false;
            if (_cacheInfo != null)
            {
                var deps = new HashSet<AssetBuildInfo>();
                GetDependencies(deps);

                if (deps.Count != _cacheInfo.dependencies.Length)
                {
                    _depTreeChanged = true;
                }
                else
                {
                    foreach (var dep in deps)
                    {
                        if (!ArrayUtility.Contains(_cacheInfo.dependencies, dep.assetPath))
                        {
                            _depTreeChanged = true;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// (作为AssetType.Asset时)是否需要单独导出
        /// </summary>
        public void Merge()
        {
            if (_childrenDependencies.Count > 1)
            {
                var copyDependencies = _childrenDependencies.ToArray();
                _childrenDependencies.Clear();

                foreach (var dependence in copyDependencies)
                {
                    dependence.RemoveParentDependencies(this, false);
                }

                foreach (var dependence in copyDependencies)
                {
                    dependence.AddParentDependencies(this);
                }
            }
        }

        /// <summary>
        /// 在导出之前执行
        /// </summary>
        public void PreBuild()
        {
            if (_preExportProcess)
            {
                return;
            }
            _preExportProcess = true;

            foreach (var item in _childrenDependencies)
            {
                item.PreBuild();
            }

            if (this.exportType == BuildType.Asset)
            {
                var rootSet = new HashSet<AssetBuildInfo>();
                this.GetRoot(rootSet);
                if (rootSet.Count > 1)
                {
                    this.exportType = BuildType.Standalone;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootSet"></param>
        private void GetRoot(HashSet<AssetBuildInfo> rootSet)
        {
            switch (this.exportType)
            {
                case BuildType.Root:
                case BuildType.Standalone:
                    rootSet.Add(this);
                    break;
                default:
                    foreach (var inversion in _childrenDependencies)
                    {
                        inversion.GetRoot(rootSet);
                    }
                    break;
            }
        }

        public string GetAssetHash()
        {
            if (assetType == AssetType.Builtin)
            {
                return "0";
            }
            else
            {
                return ABBuildHelper.GetFileHash(assetPath);
            }
        }

        /// <summary>
        /// 获取所有依赖项
        /// </summary>
        /// <param name="assetSet"></param>
        public void GetDependencies(HashSet<AssetBuildInfo> assetSet)
        {
            foreach (var item in _parentDependencies)
            {
                if (item.needSelfExport)
                {
                    assetSet.Add(item);
                }
                else
                {
                    item.GetDependencies(assetSet);
                }
            }
        }

        /// <summary>
        /// 增加我依赖的项
        /// </summary>
        /// <param name="target"></param>
        private void AddParentDependencies(AssetBuildInfo target)
        {
            if (target == this || this.ContainsDependencies(target))
            {
                return;
            }
            _parentDependencies.Add(target);

            target.AddChildrenDependencies(this);
            this.TrimDependencies(target);
        }

        /// <summary>
        /// 增加依赖我的项
        /// </summary>
        /// <param name="target"></param>
        private void AddChildrenDependencies(AssetBuildInfo target)
        {
            _childrenDependencies.Add(target);
        }

        /// <summary>
        /// 移除我依赖的项
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        private void RemoveParentDependencies(AssetBuildInfo target, bool recursive = true)
        {
            _parentDependencies.Remove(target);
            target.RemoveChildrenDependencies(this);

            if (recursive)
            {
                var copyDependencies = _childrenDependencies.ToArray();
                foreach (var child in copyDependencies)
                {
                    child.RemoveParentDependencies(target);
                }
            }
        }

        /// <summary>
        /// 移除依赖我的项
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        private void RemoveChildrenDependencies(AssetBuildInfo target)
        {
            _childrenDependencies.Remove(target);
        }

        /// <summary>
        /// 我依赖了这个项，那么依赖我的项不需要直接依赖这个项了
        /// </summary>
        private void TrimDependencies(AssetBuildInfo target = null)
        {
            if (target != null)
            {
                var copyDependencies = _childrenDependencies.ToArray();
                foreach (var child in copyDependencies)
                {
                    child.RemoveParentDependencies(target, true);
                }
            }
            else
            {
                var copyParentDependencies = _parentDependencies.ToArray();
                var copyChildrenDependencies = _childrenDependencies.ToArray();

                foreach (var parent in copyParentDependencies)
                {
                    foreach (var child in copyChildrenDependencies)
                    {
                        child.RemoveParentDependencies(child, true);
                    }
                }
            }
        }

        /// <summary>
        /// 是否已经包含了这个依赖
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        private bool ContainsDependencies(AssetBuildInfo target, bool recursive = true)
        {
            if (_parentDependencies.Contains(target))
            {
                return true;
            }

            if (recursive)
            {
                foreach (var dependence in _parentDependencies)
                {
                    if (dependence.ContainsDependencies(target, true))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        public bool WriteCache(Hashtable table)
        {
            if (!string.IsNullOrEmpty(assetPath))
            {
                table["assetHash"] = GetAssetHash();
                table["metaHash"] = metaHash;
                table["buildHash"] = buildHash;

                table["assetPath"] = assetPath;
                var dependencies = new ArrayList();
                table["dependencies"] = dependencies;

                var depSet = new HashSet<AssetBuildInfo>();
                this.GetDependencies(depSet);
                foreach (var abi in depSet)
                {
                    dependencies.Add(abi.assetPath);
                }
                return true;
            }
            return false;
        }

        #endregion

    }
}
