// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace K.AB
{
    static class ABBuildHelper
    {
        #region Static API 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static AssetBuildInfo LoadAsRoot(string assetPath)
        {
            AssetBuildInfo assetBuildInfo;
            if (!_AssetPathDictionary.TryGetValue(assetPath, out assetBuildInfo))
            {
                var ext = Path.GetExtension(assetPath);

                if (ext == ".prefab")
                {

                }
                else if (ext == ".asset")
                {

                }

                assetBuildInfo = new AssetBuildInfo(assetPath)
                {
                    exportType = BuildType.Root
                };
                assetBuildInfo.SetMainAssetPath(assetPath);
                _AssetPathDictionary.Add(assetPath, assetBuildInfo);
            }
            else
            {
                Debug.LogError("build root failed");
            }

            return assetBuildInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static AssetBuildInfo LoadAsAsset(string assetPath)
        {
            string realAssetPath = assetPath;

            var assetImporter = AssetImporter.GetAtPath(assetPath);
            if (assetImporter is TextureImporter)
            {
                var textureImporter = (TextureImporter)assetImporter;
                if (!string.IsNullOrEmpty(textureImporter.spritePackingTag))
                {
                    realAssetPath = "[atlas]" + textureImporter.spritePackingTag;
                }
            }

            AssetBuildInfo assetBuildInfo;
            if (!_AssetPathDictionary.TryGetValue(realAssetPath, out assetBuildInfo))
            {
                assetBuildInfo = new AssetBuildInfo(realAssetPath)
                {
                    exportType = BuildType.Asset
                };

                assetBuildInfo.SetMainAssetPath(assetPath);
                _AssetPathDictionary[realAssetPath] = assetBuildInfo;
            }
            else
            {
                assetBuildInfo.AddSubAssetPath(assetPath);
            }

            return assetBuildInfo;
        }

        public static List<AssetBuildInfo> GetAll()
        {
            return new List<AssetBuildInfo>(_AssetPathDictionary.Values);
        }

        public static void LoadCache(BuildTarget buildTarget)
        {
            _AssetPathDictionary = new Dictionary<string, AssetBuildInfo>();
            _FileHashDictionary = new Dictionary<string, string>();
            _AssetCacheInfoDictionary = new Dictionary<string, AssetCacheInfo>();

            var cacheDirectory = Path.Combine(ABBuilder.BUILD_PATH, PlayerSettings.bundleVersion, buildTarget.ToString());
            if (!Directory.Exists(cacheDirectory))
            {
                Directory.CreateDirectory(cacheDirectory);
            }
            _CacheFilePath = cacheDirectory + "/bundleCache.txt";
            if (!File.Exists(_CacheFilePath))
            {
                return;
            }

            var text = File.ReadAllText(_CacheFilePath);
            var cacheTable = KJson.ToJsonTable(text);

            //版本比较
            string vString = cacheTable.GetString("version");
            bool wrongVer = false;
            try
            {
                var cacheVer = new Version(vString);
                wrongVer = cacheVer < ABManager.Version;
            }
            catch (Exception)
            {
                wrongVer = true;
            }

            if (wrongVer)
            {
                return;
            }

            var assetList = cacheTable["assets"] as ArrayList;

            //读取缓存的信息
            foreach (var item in assetList)
            {
                var assetTable = item as Hashtable;
                var cacheInfo = new AssetCacheInfo();
                cacheInfo.Load(assetTable);
                _AssetCacheInfoDictionary[cacheInfo.assetPath] = cacheInfo;
            }
        }

        public static void SaveCache()
        {
            var table = new Hashtable();
            table["version"] = ABManager.Version.ToString();
            var assetList = new ArrayList();
            table["assets"] = assetList;

            foreach (var target in _AssetPathDictionary.Values)
            {
                var tmpTable = new Hashtable();
                if (target.WriteCache(tmpTable))
                {
                    assetList.Add(tmpTable);
                }
            }

            File.WriteAllText(_CacheFilePath, KJson.ToJsonText(table));
        }

        public static void ClearCache()
        {
            _AssetPathDictionary = null;
            _FileHashDictionary = null;
            _AssetCacheInfoDictionary = null;
        }

        public static string GetFileHash(string path, bool force = false)
        {
            string _hexStr = null;
            if (_FileHashDictionary.ContainsKey(path) && !force)
            {
                _hexStr = _FileHashDictionary[path];
            }
            else if (File.Exists(path) == false)
            {
                _hexStr = "FileNotExists";
            }
            else
            {
                FileStream fs = new FileStream(path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);

                _hexStr = ABUtils.GetHash(fs);
                _FileHashDictionary[path] = _hexStr;
                fs.Close();
            }

            return _hexStr;
        }

        public static AssetCacheInfo GetCacheInfo(string path)
        {
            if (_AssetCacheInfoDictionary.ContainsKey(path))
            {
                return _AssetCacheInfoDictionary[path];
            }
            return null;
        }

        #endregion

        #region Static Field

        /// <summary>
        /// 
        /// </summary>
        private static string _CacheFilePath = "";

        /// <summary>
        /// 路径
        /// </summary>
        private static Dictionary<string, AssetBuildInfo> _AssetPathDictionary;
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<string, string> _FileHashDictionary;
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<string, AssetCacheInfo> _AssetCacheInfoDictionary;

        #endregion   
    }
}
