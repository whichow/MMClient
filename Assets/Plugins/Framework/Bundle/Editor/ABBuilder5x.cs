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
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class ABBuilder5x : ABBuilder
    {
        public override void Build()
        {
            Debug.Log("编资源");

            base.Build();

            ClearAssetBundlesName();

            var abbList = new List<AssetBundleBuild>();

            //标记所有 asset bundle name
            var abiList = ABBuildHelper.GetAll();
            abiList.Sort();
            for (int i = 0; i < abiList.Count; i++)
            {
                var target = abiList[i];
                if (EditorUtility.DisplayCancelableProgressBar("Set AssetBundle Name", target.assetPath, (float)i / abiList.Count))
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
                if (target.needSelfExport)
                {
                    var build = new AssetBundleBuild()
                    {
                        assetBundleName = target.fileName,
                        assetNames = target.assetPaths,
                    };
                    abbList.Add(build);
                }
            }
            EditorUtility.ClearProgressBar();


            if (EditorUtility.DisplayCancelableProgressBar("Start BuildAssetBundles", "Wait BuildAssetBundles...", 0.9f))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            //开始打包
            var manifest = BuildPipeline.BuildAssetBundles(BuildPath, abbList.ToArray(),
                BuildAssetBundleOptions.ChunkBasedCompression |
                //BuildAssetBundleOptions.ForceRebuildAssetBundle |
                BuildAssetBundleOptions.DeterministicAssetBundle,
                EditorUserBuildSettings.activeBuildTarget);

            if (manifest == null)
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            //var timeStamp = (int)((System.DateTime.UtcNow - new System.DateTime(2000, 1, 1)).TotalMinutes);
            //hash
            for (int i = 0; i < abiList.Count; i++)
            {
                var target = abiList[i];
                if (EditorUtility.DisplayCancelableProgressBar("Set AssetBundle File HashCode", target.assetPath, (float)i / abiList.Count))
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
                if (target.needSelfExport)
                {
                    var hash = manifest.GetAssetBundleHash(target.fileName);
                    target.buildHash = hash.ToString();

                    using (var stream = System.IO.File.OpenRead(ABUtils.CombinPath(BuildPath, target.fileName)))
                    {
                        target.fileSize = (int)(stream.Length >> 10);
                        target.fileHash = ABUtils.GetHash(stream);
                    }

                    //target.buildTime = timeStamp;
                }
            }
            EditorUtility.ClearProgressBar();

            this.SaveDependencies(abiList);
            this.RemoveUnused(abiList);

            ClearAssetBundlesName();
            AssetDatabase.RemoveUnusedAssetBundleNames();

            Debug.Log($"编资源>>>完成!  版本:{PlayerSettings.bundleVersion}");
        }

        /// <summary>
        /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包
        /// 工程中只要设置了AssetBundleName的，都会进行打包
        /// </summary>
        static void ClearAssetBundlesName()
        {
            string[] abNames = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < abNames.Length; i++)
            {
                if (EditorUtility.DisplayCancelableProgressBar("Clear AssetBundlesName", abNames[i], (float)i / abNames.Length))
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
                AssetDatabase.RemoveAssetBundleName(abNames[i], true);
            }
            EditorUtility.ClearProgressBar();
        }

    }
}