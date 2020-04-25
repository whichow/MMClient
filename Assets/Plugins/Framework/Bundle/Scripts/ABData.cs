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
    using System.Collections;

    public class ABData
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get; private set; }
        /// <summary>
        /// (文件路径 用于网络)路径
        /// </summary>
        public string filePath { get; private set; }
        /// <summary>
        /// 文件的Hash值
        /// </summary>
        public string fileHash { get; private set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public int fileSize { get; private set; }
        /// <summary>
        /// (hash值)
        /// </summary>
        public string buildHash { get; private set; }
        ///// <summary>
        ///// build 时间
        ///// </summary>
        //public int buildTime { get; private set; }
        /// <summary>
        /// 资源导出类型
        /// </summary>
        public BuildType buildType { get; private set; }
        /// <summary>
        /// 资源名
        /// </summary>
        public string assetName { get; private set; }
        /// <summary>
        /// 包含的资源路径
        /// </summary>
        public string assetPath { get; private set; }
        /// <summary>
        /// 依赖项
        /// </summary>
        public string[] dependencies { get; private set; }
        /// <summary>
        /// 短名(不带扩展名)
        /// </summary>
        public string shortName { get; private set; }

        public int GetDependenceCount()
        {
            if (dependencies != null)
            {
                return dependencies.Length;
            }
            return 0;
        }

        public string GetDependence(int index)
        {
            if (dependencies != null)
            {
                return dependencies[index];
            }
            return string.Empty;
        }

        public void Load(Hashtable table)
        {
            if (table != null)
            {
                fileName = table.GetString(ABDefine.kFileNameKey);
                filePath = table.GetString(ABDefine.kFilePathKey);
                fileHash = table.GetString(ABDefine.kFileHashKey);
                fileSize = table.GetInt(ABDefine.kFileSizeKey);

                assetName = table.GetString(ABDefine.kAssetNameKey);
                assetPath = table.GetString(ABDefine.kAssetPathKey);

                buildHash = table.GetString(ABDefine.kBuildHashKey);
                //buildTime = table.GetInt(ABDefine.kBuildTimeKey);
                buildType = (BuildType)table.GetInt(ABDefine.kBuildTypeKey);

                dependencies = table.GetArray<string>(ABDefine.kDependenciesKey);

                shortName = ABUtils.GetFileNameWithoutExtension(assetName);
            }
        }
    }
}