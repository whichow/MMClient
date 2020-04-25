// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace K.AB
{
    public class ABBuilder
    {
        public static string BUILD_PATH = "Bundles/";

        #region Property

        /// <summary>
        /// 资源编出后的存放路径
        /// </summary>
        public string BuildPath
        {
            get
            {
                return $"{BUILD_PATH}{PlayerSettings.bundleVersion}/{EditorUserBuildSettings.activeBuildTarget}";
            }
        }

        /// <summary>
        /// 热更资源保存目录
        /// </summary>
        public static string HotSavePath
        {
            get
            {
                return $"{Application.dataPath}/../Hot{BUILD_PATH}{PlayerSettings.bundleVersion}/{EditorUserBuildSettings.activeBuildTarget}{ABDefine.kFolderPath}";
            }
        }

        #endregion

        #region Method

        public void Begin()
        {
            ABBuildHelper.ClearCache();
            ABBuildHelper.LoadCache(EditorUserBuildSettings.activeBuildTarget);
        }

        public void End(bool isHot)
        {
            // 是否是热更资源，生成到热更目录
            if (isHot)
            {
                SaveHotBundles(HotSavePath);
            }
            else
            {
                SaveBundles(ABDefine.SavePath);
            }

            //ABBuildHelper.SaveCache();
            //ABBuildHelper.ClearCache();

            AssetDatabase.Refresh();
        }

        public virtual void Build()
        {
            this.Analyse();
            var all = ABBuildHelper.GetAll();
            for (int i = 0; i < all.Count; i++)
            {
                var target = all[i];
                if (EditorUtility.DisplayCancelableProgressBar("预处理分包", target.assetPath, (float)i / all.Count))
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
                target.PreBuild();
            }
            EditorUtility.ClearProgressBar();
        }

        public void Progress(float value, float min, float max)
        {
            if (ABBuildWindow.Instance)
            {
                ABBuildWindow.Instance.ShowProgress(value, min, max);
            }
        }

        /// <summary>
        /// 分析引用关系
        /// </summary>
        public void Analyse()
        {
            var all = ABBuildHelper.GetAll();
            for (int i = 0; i < all.Count; i++)
            {
                var target = all[i];
                if (EditorUtility.DisplayCancelableProgressBar("分析引用", target.assetPath, (float)i / all.Count))
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
                target.AnalyseDependencies();
            }

            all = ABBuildHelper.GetAll();
            for (int i = 0; i < all.Count; i++)
            {
                var target = all[i];
                if (EditorUtility.DisplayCancelableProgressBar("分引用 合并", target.assetPath, (float)i / all.Count))
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
                target.Merge();
            }
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="assetBuildInfos"></param>
        protected void SaveDependencies(List<AssetBuildInfo> assetBuildInfos)
        {
            if (assetBuildInfos != null && assetBuildInfos.Count > 0)
            {
                var dataWriter = new ABDataWriter();
                ABUtils.CreateDirectory(BuildPath);
                var filePath = BuildPath + "/" + ABDefine.kConfigFile;
                dataWriter.Write(filePath, assetBuildInfos.FindAll(ai => ai.needSelfExport).ToArray());
            }
        }

        protected void SaveBundles(string savePath)
        {
            if (Directory.Exists(savePath))
            {
                Directory.Delete(savePath, true);
            }
            while (Directory.Exists(savePath))
            {
            }
            Directory.CreateDirectory(savePath);

            var sFiles = Directory.GetFiles(BuildPath, "*" + ABDefine.GetABExtensionName(), SearchOption.AllDirectories);
            for (int i = 0; i < sFiles.Length; i++)
            {
                string sFile = sFiles[i].Replace('\\', '/');
                if (EditorUtility.DisplayCancelableProgressBar("Copy Files", sFile, (float)i / sFiles.Length))
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
                var targetPath = savePath + sFile.Substring(BuildPath.Length);

                string dir = Directory.GetParent(targetPath).FullName;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.Copy(sFile, targetPath, true);
            }

            File.Copy(BuildPath + "/" + ABDefine.kConfigFile, savePath + ABDefine.kConfigFile, true);

            EditorUtility.ClearProgressBar();

            System.Diagnostics.Process.Start(savePath);
        }

        /// <summary>
        /// 删除未使用的AB，可能是上次打包出来的，而这一次没生成的
        /// </summary>
        /// <param name="all"></param>
        protected void RemoveUnused(List<AssetBuildInfo> all)
        {
            //var usedSet = new HashSet<string>();
            //for (int i = 0; i < all.Count; i++)
            //{
            //    var target = all[i];
            //    if (target.needSelfExport)
            //    {
            //        usedSet.Add(target.hashName);
            //    }
            //}

            //var di = new DirectoryInfo(buildPath);
            //var abFiles = di.GetFiles("*.ab");
            //for (int i = 0; i < abFiles.Length; i++)
            //{
            //    var abFile = abFiles[i];
            //    if (!usedSet.Contains(abFile.Name))
            //    {
            //        Debug.Log("Remove unused AB : " + abFile.Name);

            //        abFile.Delete();
            //        //for U5X
            //        File.Delete(abFile.FullName + ".manifest");
            //    }
            //}
        }

        /// <summary>
        /// 热更资源
        /// </summary>
        protected void SaveHotBundles(string hotSavePath)
        {
            SaveBundles(hotSavePath);
        }

        #endregion

        public void AddRootTargets(string filter, string[] folders)
        {
            AssetDatabase.SaveAssets();

            var guids = AssetDatabase.FindAssets(filter, folders);
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ABBuildHelper.LoadAsRoot(assetPath);
            }
        }

    }
}
