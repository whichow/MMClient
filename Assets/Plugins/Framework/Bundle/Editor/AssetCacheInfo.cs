// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using System.Collections;

namespace K.AB
{
    class AssetCacheInfo
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public string assetPath;
        /// <summary>
        /// 源文件的hash，比较变化
        /// </summary>
        public string assetHash;
        /// <summary>
        /// 源文件meta文件的hash，部分类型的素材需要结合这个来判断变化
        /// </summary>
        public string metaHash;
        /// <summary>
        /// 上次打好的AB的Hash值，用于增量判断
        /// </summary>
        public string buildHash;
        /// <summary>
        /// 所依赖的那些文件
        /// </summary>
        public string[] dependencies;

        public void Load(Hashtable table)
        {
            this.assetPath = table.GetString("assetPath");
            this.assetHash = table.GetString("assetHash");
            this.metaHash = table.GetString("metaHash");
            this.buildHash = table.GetString("buildHash");
            this.dependencies = table.GetArray<string>("dependencies");
        }
    }
}

