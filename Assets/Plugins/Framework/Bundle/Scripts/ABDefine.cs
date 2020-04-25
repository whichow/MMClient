// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using System.IO;
using UnityEngine;

namespace K.AB
{
    /// <summary>
    /// AB 打包及运行时路径
    /// </summary>
    public class ABDefine
    {
        #region Const

        public const string kVersionKey = "version";
        public const string kPackageKey = "package";

        public const string kAssetNameKey = "assetName";
        public const string kAssetPathKey = "assetPath";

        public const string kBuildHashKey = "buildHash";
        public const string kBuildTimeKey = "buildTime";
        public const string kBuildTypeKey = "buildType";

        public const string kFileNameKey = "fileName";
        public const string kFileHashKey = "fileHash";
        public const string kFilePathKey = "filePath";
        public const string kFileSizeKey = "fileSize";

        public const string kDependenciesKey = "dependencies";

        public const string kConfigFile = "ab_config.json";
        public const string kFolderPath = "/ab_res/";

        #endregion

        #region Static

        /// <summary>
        /// 获取ab扩展名
        /// </summary>
        /// <returns></returns>
        public static string GetABExtensionName()
        {
            return ".u3d";
        }

        /// <summary>
        /// 获取ab文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetABFileName(string path)
        {
#if DEBUG_MY
            //带目录的
            string dir = Path.GetDirectoryName(path).Replace("\\","/");
            if (!string.IsNullOrEmpty(dir)) dir += "/";
            string fileName = dir + Path.GetFileNameWithoutExtension(path) + ".u3d";
            fileName = fileName.ToLower();
            return fileName;
#else
            return ABUtils.GetHash(path) + ".u3d";
#endif
        }

        private static string _SavePath;
        /// <summary>
        /// 保存目录
        /// </summary>
        public static string SavePath
        {
            get
            {
                if (string.IsNullOrEmpty(_SavePath))
                {
                    _SavePath = Application.streamingAssetsPath + kFolderPath;
                }
                return _SavePath;
            }
        }

        private static string _CachePath;
        /// <summary>
        /// 缓存目录
        /// </summary>
        public static string CachePath
        {
            get
            {
                if (string.IsNullOrEmpty(_CachePath))
                {
#if UNITY_EDITOR
                    _CachePath = Application.persistentDataPath + kFolderPath;
#else
                    _CachePath = Application.persistentDataPath + kFolderPath;
#endif
                    if (!System.IO.Directory.Exists(_CachePath))
                    {
                        System.IO.Directory.CreateDirectory(_CachePath);
                    }
                }
                return _CachePath;
            }
        }

        /// <summary>
        /// 配置文件路径
        /// </summary>
        /// <returns></returns>
        public static string GetLocalConfigFile()
        {
            return kFolderPath + kConfigFile;
        }

        /// <summary>
        /// 获取 AB 源文件路径（打包进安装包的）
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetSourceFile(string fileName)
        {
            return SavePath + fileName;
        }

        /// <summary>
        /// 获取 AB 缓存文件路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetCachedFile(string fileName)
        {
            return CachePath + fileName;
        }

        #endregion
    }
}