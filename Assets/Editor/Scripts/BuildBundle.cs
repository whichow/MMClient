//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "BuildBundle" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using Game;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;

//public class BuildBundle : MonoBehaviour
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    //[MenuItem("Game/Build Bundles (发布)", false, 100)]
//    public static void BuildAllBundles0()
//    {
//        //BuildPipeline.BuildAssetBundles("Bundles/", BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
//        var buildTarget = EditorUserBuildSettings.activeBuildTarget;
//        BuildBundles(buildTarget);
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="compress"></param>
//    /// <param name="copy"></param>
//    /// <param name="buildTarget"></param>
//    private static void BuildBundles(BuildTarget buildTarget)
//    {
//        AssetDatabase.Refresh();

//        string packagePath = "Assets/Packages";
//        string targetPath = buildTarget.ToString().ToLower();

//        string bundlePath = "Bundles/" + targetPath;
//        string bundleVersion = PlayerSettings.bundleVersion;

//        if (Directory.Exists(bundlePath))
//        {
//            Directory.Delete(bundlePath, true);
//        }
//        Directory.CreateDirectory(bundlePath);

//        if (Directory.Exists(packagePath))
//        {
//            Directory.Delete(packagePath, true);
//        }
//        Directory.CreateDirectory(packagePath);

//        try
//        {
//            Debug.Log("Build Bundles Start");

//            var abbList = new List<AssetBundleBuild>();
//            abbList.Add(new AssetBundleBuild() { assetBundleName = "excel", assetNames = PackExcelAssets("excel", packagePath) });

//            var abNames = new string[] { "Global", /*"Character", "Building",*/ "Match3",/* "UI",*/ };
//            for (int i = 0; i < abNames.Length; i++)
//            {
//                var abName = abNames[i].ToLower();
//                var assetPaths = PackAssets(abName, packagePath);
//                if (assetPaths != null && assetPaths.Length > 0)
//                {
//                    abbList.Add(new AssetBundleBuild() { assetBundleName = abName, assetNames = assetPaths });
//                }
//            }

//            var abBuilds = abbList.ToArray();
//            var manifest = BuildPipeline.BuildAssetBundles(bundlePath, abBuilds, ((buildTarget == BuildTarget.Android) ?
//                BuildAssetBundleOptions.DeterministicAssetBundle : (BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle)), buildTarget);

//            if (manifest == null)
//            {
//                return;
//            }

//            var bundleNames = manifest.GetAllAssetBundles();
//            if (bundleNames == null || bundleNames.Length == 0)
//            {
//                return;
//            }

//            var abInfoList = new ArrayList();
//            foreach (var bundleName in bundleNames)
//            {
//                var dependencies = manifest.GetAllDependencies(bundleName);
//                if (dependencies != null && dependencies.Length > 0)
//                {
//                    var sb = new System.Text.StringBuilder(bundleName + " dependencies ");
//                    System.Array.ForEach(dependencies, (s) => sb.Append(s));
//                    Debug.LogError(sb.ToString());
//                }

//                var bundleFile = bundlePath + "/" + bundleName;

//                var copyPath = "Assets/StreamingAssets/Bundles/";
//                var copyPath2 = "Assets/Resources/Bundles/";

//                if (buildTarget == BuildTarget.Android)
//                {
//                    if (Directory.Exists(copyPath))
//                    {
//                        Directory.Delete(copyPath, true);
//                    }
//                    if (!Directory.Exists(copyPath2))
//                    {
//                        Directory.CreateDirectory(copyPath2);
//                    }

//                    var copyFile = copyPath2 + bundleName + ".bytes";
//                    File.Copy(bundleFile, copyFile, true);
//                }
//                else
//                {
//                    if (Directory.Exists(copyPath2))
//                    {
//                        Directory.Delete(copyPath2, true);
//                    }
//                    if (!Directory.Exists(copyPath))
//                    {
//                        Directory.CreateDirectory(copyPath);
//                    }

//                    var copyFile = copyPath + bundleName;
//                    File.Copy(bundleFile, copyFile, true);
//                }
//            }

//            //int timeStamp = (int)((System.DateTime.UtcNow - new System.DateTime(2000, 1, 1)).TotalMinutes);
//            //var jsonTable = new Hashtable();
//            //jsonTable["res_timestamp"] = timeStamp;
//            //File.WriteAllText("Assets/Resources/Configs/config.txt", jsonTable.ToJsonText());

//            Debug.Log("Build Bundles Finish");
//        }
//        finally
//        {
//            Directory.Delete("Assets/Packages", true);
//            File.Delete("Assets/Packages.meta");
//            AssetDatabase.Refresh();
//        }
//    }

//    private static string[] PackExcelAssets(string bundleName, string packagePath)
//    {
//        var paths = new string[] { "Assets/GameRes/Excel" };
//        var guids = AssetDatabase.FindAssets("t:TextAsset", paths);
//        if (guids == null || guids.Length == 0)
//        {
//            return null;
//        }

//        var objList = new List<Object>();
//        for (int i = 0; i < guids.Length; i++)
//        {
//            var file = AssetDatabase.GUIDToAssetPath(guids[i]);
//            if (!string.IsNullOrEmpty(Path.GetExtension(file)))
//            {
//                objList.Add(AssetDatabase.LoadAssetAtPath<Object>(file));
//            }
//        }

//        var package = ScriptableObject.CreateInstance<KResPackage>();
//        package.objects = objList.ToArray();

//        var packageFile = packagePath + "/" + bundleName + ".asset";
//        AssetDatabase.CreateAsset(package, packageFile);

//        return new string[] { packageFile };
//    }

//    private static string[] PackAssets(string bundleName, string packagePath)
//    {
//        var paths = new string[] { "Assets/GameRes/Prefabs/" + bundleName };
//        var guids = AssetDatabase.FindAssets("", paths);
//        if (guids == null || guids.Length == 0)
//        {
//            return null;
//        }

//        var objList = new List<Object>();
//        for (int i = 0; i < guids.Length; i++)
//        {
//            var file = AssetDatabase.GUIDToAssetPath(guids[i]);
//            if (!string.IsNullOrEmpty(Path.GetExtension(file)))
//            {
//                objList.Add(AssetDatabase.LoadAssetAtPath<Object>(file));
//            }
//        }

//        var package = ScriptableObject.CreateInstance<KResPackage>();
//        package.objects = objList.ToArray();

//        var packageFile = packagePath + "/" + bundleName + ".asset";
//        AssetDatabase.CreateAsset(package, packageFile);

//        return new string[] { packageFile };
//    }
//}


