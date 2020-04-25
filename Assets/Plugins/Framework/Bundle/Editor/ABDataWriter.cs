// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using System.Collections;
using System.IO;

namespace K.AB
{
    public class ABDataWriter
    {
        private string GetResPath(string assetPath)
        {
            int startIndex = assetPath.IndexOf("GameRes") + 8;
            int lastIndex = assetPath.LastIndexOf(".");
            return assetPath.Substring(startIndex, lastIndex - startIndex);
        }

        private ArrayList GetDepList(AssetBuildInfo target)
        {
            var retList = new ArrayList();

            var depSet = HashSetPool<AssetBuildInfo>.Get();
            target.GetDependencies(depSet);
            foreach (var item in depSet)
            {
                retList.Add(GetResPath(item.assetPath));
            }
            HashSetPool<AssetBuildInfo>.Release(depSet);
            return retList;
        }

        public void Write(string path, AssetBuildInfo[] targets)
        {
            var fs = new FileStream(path, FileMode.OpenOrCreate);
            fs.SetLength(0);
            Write(fs, targets);
            fs.Flush();
            fs.Close();
        }

        protected virtual void Write(FileStream stream, AssetBuildInfo[] targets)
        {
            var assetList = new ArrayList
            {
                new ArrayList
                {
                    ABDefine.kFileNameKey,
                    ABDefine.kFileSizeKey,
                    ABDefine.kFileHashKey,
                    ABDefine.kBuildHashKey,
                    //ABDefine.kBuildTimeKey,
                    ABDefine.kBuildTypeKey,
                    ABDefine.kAssetNameKey,
                    ABDefine.kAssetPathKey,
                    ABDefine.kDependenciesKey,
                }
            };

            foreach (var target in targets)
            {
                assetList.Add(new ArrayList
                {
                    target.fileName,
                    target.fileSize,
                    target.fileHash,
                    target.buildHash,
                    //target.buildTime,
                    target.buildType,
                    target.assetName,
                    GetResPath(target.assetPath),
                    GetDepList(target),
                });
            }

            var table = new Hashtable()
            {
                { ABDefine.kVersionKey, UnityEditor.PlayerSettings.bundleVersion },
                { ABDefine.kPackageKey, assetList }
            };

            var bytes = KJson.ToJsonBytes(table);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}