///*******************************************************************************
// * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
// * 
// * Author:          Coamy
// * Created:	        2019/5/21 18:29:31
// * Description:     
// * 
// * Update History:  
// * 
// *******************************************************************************/
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;

//namespace Game
//{
//    public class AssetBundleTool
//    {
//        #region Plugins Menu

//        [MenuItem("Tools/Build/BuildAssetBundles [打包资源]", false, 1)]
//        static void mBuildAllAssetBundles()
//        {
//            BuildAllAssetBundles();
//        }

//        #endregion

//        private static string m_ABDirectory
//        {
//            get
//            {
//                return "AssetBundle/" + EditorUserBuildSettings.activeBuildTarget;
//            }
//        }

//        private static Dictionary<string, List<string>> m_abbDic;
//        private static Dictionary<string, int> m_pathDic;

//        /// <summary>
//        /// 编译所有资源包
//        /// </summary>
//        private static void BuildAllAssetBundles()
//        {
//            m_pathDic = new Dictionary<string, int>();
//            m_abbDic = new Dictionary<string, List<string>>();

//            SetAssetBundleNameForSinglePack("Assets/Res/Lua");
//            SetAssetBundleNameForSinglePack("Assets/Res/Mis");

//            SetAssetBundleNameForMultiplePack("Assets/Res/Table");
//            SetAssetBundleNameForMultiplePack("Assets/Res/Chessboard");
//            SetAssetBundleNameForMultiplePack("Assets/Res/Global");
//            SetAssetBundleNameForMultiplePack("Assets/Res/UI");
//            SetAssetBundleNameForMultiplePack("Assets/Res/Building");
//            SetAssetBundleNameForMultiplePack("Assets/Res/Match3");
//            SetAssetBundleNameForMultiplePack("Assets/Res/Sounds");
//            SetAssetBundleNameForMultiplePack("Assets/Res/Character");
//            SetAssetBundleNameForMultiplePack("Assets/Res/Pet");
//            SetAssetBundleNameForMultiplePack("Assets/Res/Pet2D");

//            SetAssetBundleNameForMultiplePackFromTopDir("Assets/Res/Icon");

//            if (EditorUtility.DisplayCancelableProgressBar("Wait BuildAssetBundle", "Wait Build Asset Bundle...", 0.9f))
//            {
//                EditorUtility.ClearProgressBar();
//                return;
//            }

//            var abbArr = new List<AssetBundleBuild>();
//            foreach (var item in m_abbDic)
//            {
//                var paths = item.Value;
//                for (int j = paths.Count - 1; j > 0; j--)
//                {
//                    //没有其它依赖则不单独打包
//                    if (m_pathDic[paths[j]] == 1)
//                    {
//                        paths.RemoveAt(j);
//                    }
//                }
//                if (paths.Count > 0)
//                {
//                    abbArr.Add(new AssetBundleBuild()
//                    {
//                        assetBundleName = item.Key,
//                        assetNames = paths.ToArray(),
//                    });
//                }
//            }

//            string dir = m_ABDirectory;
//            if (!Directory.Exists(dir))
//            {
//                Directory.CreateDirectory(dir);
//            }

//            //先删除再编?
//            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(dir, abbArr.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);
//            if (manifest != null)
//            {
//                File.Move(dir + "/" + Path.GetFileName(dir), dir + "/ABConfig");
//                File.Move(dir + "/" + Path.GetFileName(dir) + ".manifest", dir + "/ABConfig.manifest");
//                CopyBundles(m_ABDirectory, Application.streamingAssetsPath);
//                Debug.Log(">>> 编资源 - 完成!");
//            }
//            else
//            {
//                Debug.LogError("资源打包失败");
//            }

//            m_pathDic = null;
//            m_abbDic = null;
//            AssetDatabase.RemoveUnusedAssetBundleNames();
//            AssetDatabase.Refresh();
//            EditorUtility.ClearProgressBar();
//        }

//        /// <summary>
//        /// 设置资源包名 多个资源到单个包
//        /// </summary>
//        private static void SetAssetBundleNameForSinglePack(string path)
//        {
//            SetAssetBundleName(path, Path.GetFileNameWithoutExtension(path), Path.GetDirectoryName(path));
//        }

//        /// <summary>
//        /// 设置资源包名  多个资源包每个文件对应一个包
//        /// </summary>
//        private static void SetAssetBundleNameForMultiplePack(string path)
//        {
//            if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
//            {
//                string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
//                for (int i = 0; i < files.Length; i++)
//                {
//                    string file = files[i];

//                    if (EditorUtility.DisplayCancelableProgressBar("SetAssetBundleName", file, (float)i / files.Length))
//                    {
//                        EditorUtility.ClearProgressBar();
//                        return;
//                    }

//                    if (!file.Contains(".meta"))
//                    {
//                        SetAssetBundleName(file, Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file));
//                    }
//                }
//                EditorUtility.ClearProgressBar();
//            }
//        }

//        /// <summary>
//        /// 设置资源包名 多个资源包从目录顶层每个文件夹一个包
//        /// </summary>
//        private static void SetAssetBundleNameForMultiplePackFromTopDir(string path)
//        {
//            if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
//            {
//                string[] dirs = Directory.GetDirectories(path);
//                for (int i = 0; i < dirs.Length; i++)
//                {
//                    string dir = dirs[i];

//                    if (EditorUtility.DisplayCancelableProgressBar("SetAssetBundleName", dir, (float)i / dirs.Length))
//                    {
//                        EditorUtility.ClearProgressBar();
//                        return;
//                    }

//                    SetAssetBundleName(dir, Path.GetFileNameWithoutExtension(dir), Path.GetDirectoryName(dir));
//                }
//                EditorUtility.ClearProgressBar();
//            }
//        }

//        private static void SetAssetBundleName(string path, string name, string abDirectory = "")
//        {
//            path = path.Replace("\\", "/");
//            if (m_pathDic.ContainsKey(path))
//            {
//                m_pathDic[path]++;
//                return;
//            }

//            AssetImporter importer = AssetImporter.GetAtPath(path);
//            if (importer != null)
//            {
//                string bname = Path.Combine(abDirectory, name).Replace("Assets\\", "");
//                //importer.assetBundleName = bname;

//                if (importer is TextureImporter)
//                {
//                    var textureImporter = (TextureImporter)importer;
//                    if (!string.IsNullOrEmpty(textureImporter.spritePackingTag))
//                    {
//                        bname = "[atlas]" + textureImporter.spritePackingTag;
//                    }
//                }

//                m_pathDic.Add(path, 1);

//                if (m_abbDic.ContainsKey(bname))
//                {
//                    m_abbDic[bname].Add(path);
//                }
//                else
//                {
//                    m_abbDic.Add(bname, new List<string>() { path });
//                }

//                //依赖
//                string[] dependencies = AssetDatabase.GetDependencies(path);
//                foreach (var filepath in dependencies)
//                {
//                    if (path != filepath)
//                    {
//                        //不包含脚本对象
//                        var extension = Path.GetExtension(filepath);
//                        if (extension != ".cs")
//                        {
//                            //string filename = AssetDatabase.AssetPathToGUID(filepath);
//                            SetAssetBundleName(filepath, Path.GetFileNameWithoutExtension(filepath), Path.GetDirectoryName(filepath));
//                        }
//                    }
//                }
//            }
//            else
//            {
//                Debug.LogError("Not found AssetImporter! " + path);
//            }
//        }

//        private static void CopyBundles(string sourcePath, string savePath)
//        {
//            //if (Directory.Exists(savePath))
//            //{
//            //    Directory.Delete(savePath, true);
//            //}

//            //while (Directory.Exists(savePath))
//            //{
//            //}

//            Directory.CreateDirectory(savePath);

//            CopyDirectory(sourcePath, savePath, new string[] { "^.*.meta$", "^.*.manifest" });

//            //var sFiles = Directory.GetFiles(sourcePath, "*.u3d");
//            //foreach (var sFile in sFiles)
//            //{
//            //    var copyFile = Path.Combine(savePath, Path.GetFileName(sFile));
//            //    File.Copy(sFile, copyFile, true);
//            //}
//        }

//        #region File

//        public static void DelFile(string path)
//        {
//            if (File.Exists(path))
//            {
//                File.Delete(path);
//            }
//            else if (Directory.Exists(path))
//            {
//                Directory.Delete(path, true);
//            }
//        }

//        public static void CopyFile(string path, string targetPath)
//        {
//            string dir = Directory.GetParent(targetPath).FullName;
//            if (!Directory.Exists(dir))
//            {
//                Directory.CreateDirectory(dir);
//            }

//            FileInfo fi = new FileInfo(path);
//            fi.CopyTo(targetPath, true);
//            //File.Copy(sFile, copyFile, true);
//        }

//        public static void CopyDirectory(string fullpath, string targetPath, string[] exclude = null)
//        {
//            //CopyDirectory(path, Application.dataPath, new string[] { "^.*.meta$", "^.*.txt" });
//            string regexExclude = null;
//            if (exclude != null)
//            {
//                regexExclude = string.Format(@"{0}", string.Join("|", exclude));
//            }

//            if ((File.GetAttributes(fullpath) & FileAttributes.Directory) == FileAttributes.Directory)
//            {
//                string[] fileEntries = Directory.GetFiles(fullpath, "*.*", SearchOption.AllDirectories);
//                for (int i = 0; i < fileEntries.Length; i++)
//                {
//                    string filepath = fileEntries[i].Replace('\\', '/');

//                    if (EditorUtility.DisplayCancelableProgressBar("拷贝文件", filepath, (float)i / fileEntries.Length))
//                    {
//                        EditorUtility.ClearProgressBar();
//                        return;
//                    }

//                    if (regexExclude != null)
//                    {
//                        if (System.Text.RegularExpressions.Regex.IsMatch(filepath, regexExclude))
//                        {
//                            continue;
//                        }
//                    }

//                    string name = filepath.Substring(fullpath.Length);

//                    CopyFile(filepath, targetPath + name);
//                }
//                EditorUtility.ClearProgressBar();
//            }
//        }

//        #endregion

//    }
//}
