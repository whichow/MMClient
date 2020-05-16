/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/6/10 13:51:44
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using K.AB;
using System;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class BuildPlayer
    {
        private static string configPath = "file:///" + Application.streamingAssetsPath + "/config.xml";

        #region Plugins Menu

        [MenuItem("Tools/BuildPlayer [发布]", false, 0)]
        private static void _BuildPlayer()
        {
            if (!EditorApplication.isCompiling)
            {
                EditorPrefs.SetBool("Build_Player", true);
                EditorApplication.update += EditorApplicationUpdate;
                GenXLua();
            }
        }

        [MenuItem("Tools/Build AssetBundle [打包资源]", false, 0)]
        private static void BuildAllAssetBundles()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(configPath);
            PlayerSettings.bundleVersion = xml.SelectSingleNode("configuration/gameVersion").InnerText;
            ABBuildWindow.BuildAssetBundles(false);
        }

        [MenuItem("Tools/Build Hot AssetBundle [打包热更资源]", false, 0)]
        private static void BuildAllHotAssetBundles()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(configPath);
            PlayerSettings.bundleVersion = xml.SelectSingleNode("configuration/gameVersion").InnerText;
            ABBuildWindow.BuildAssetBundles(true);
        }
        
        #endregion

        public static void StartBuildPlayer()
        {
            if (EditorUtility.DisplayCancelableProgressBar("StartBuildPlayer", "Start Build Player...", 0.1f))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            string dataPath = Directory.GetParent(Application.dataPath).ToString() + "/";

            XmlDocument xml = new XmlDocument();
            xml.Load(configPath);
            PlayerSettings.applicationIdentifier = xml.SelectSingleNode("configuration/bundleIdentifier").InnerText;
            PlayerSettings.bundleVersion = xml.SelectSingleNode("configuration/gameVersion").InnerText;
            PlayerSettings.iOS.buildNumber = xml.SelectSingleNode("configuration/bundleVersion").InnerText;
            PlayerSettings.Android.bundleVersionCode = int.Parse(xml.SelectSingleNode("configuration/versionCode").InnerText);
            string gameName = xml.SelectSingleNode("configuration/gameName").InnerText;
            string subpacket = xml.SelectSingleNode("configuration/subpacket").InnerText;

            string[] levels = new string[] {
                "Assets/Scenes/launchScene.unity",
                "Assets/Scenes/loginScene.unity",
                "Assets/Scenes/buildingScene.unity",
                "Assets/Scenes/loadingScene.unity",
                "Assets/Scenes/matchScene.unity",
            };

#if UNITY_ANDROID
            PlayerSettings.statusBarHidden = true;
            PlayerSettings.productName = gameName;

            //设置签名文件
            string keystoreFile = $"{dataPath}keystore/mm.keystore";
            string keystoreDesc = $"{dataPath}keystore/keystore.txt";
            if (!File.Exists(keystoreFile) || !File.Exists(keystoreDesc))
            {
                Debug.LogError("Not find keyStore file");
                return;
            }

            StreamReader sr = File.OpenText(keystoreDesc);
            string aliasname = sr.ReadLine().Trim();
            string password = sr.ReadLine().Trim();

            PlayerSettings.Android.keystoreName = keystoreFile;
            PlayerSettings.Android.keystorePass = password;
            PlayerSettings.Android.keyaliasName = aliasname;
            PlayerSettings.Android.keyaliasPass = password;

            string apkName = "mm.apk";

            DelFile(dataPath + apkName);

            string pathName = dataPath + apkName;
            BuildPipeline.BuildPlayer(levels, pathName, BuildTarget.Android, BuildOptions.None);
#elif UNITY_IPHONE
            PlayerSettings.statusBarHidden = false;
            PlayerSettings.productName = gameName;

            string apkName = "mm_xcode";

            DelFile(dataPath + apkName);

            string pathName = dataPath + apkName;
            BuildPipeline.BuildPlayer(levels, pathName, BuildTarget.iPhone, BuildOptions.None);
#else
            System.DateTime now = System.DateTime.Now;
            PlayerSettings.productName = string.Format("{0} {1}.{2} {3}:{4}", gameName, now.Month, now.Day, now.Hour, now.Minute);

            DelFile(dataPath + "mm.pc");

            string pathName = dataPath + "mm.pc/Client.exe";
            BuildPipeline.BuildPlayer(levels, pathName, BuildTarget.StandaloneWindows, BuildOptions.Development);
#endif

            Debug.Log("发布完成: " + pathName);
            System.Diagnostics.Process.Start(dataPath);
        }

        private static void GenXLua()
        {
            if (EditorUtility.DisplayCancelableProgressBar("XLua Generator", "Generator.GenAll", 0.1f))
            {
                EditorUtility.ClearProgressBar();
                return;
            }
        }

        private static float t = 0;
        private static void EditorApplicationUpdate()
        {
            t += 0.0005f;
            EditorUtility.DisplayCancelableProgressBar("Code Compiling", "Compiling...", t);
            if (!EditorApplication.isCompiling)
            {
                EditorApplication.update -= EditorApplicationUpdate;
                EditorPrefs.DeleteKey("Build_Player");
                EditorUtility.ClearProgressBar();
                StartBuildPlayer();
            }
        }

        public static void DelFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

    }

    public class UnityScriptCompiling
    {
        private static DateTime dt;

        [UnityEditor.Callbacks.DidReloadScripts]
        static void AllScriptsReloaded()
        {
            if (EditorPrefs.GetBool("Build_Player"))
            {
                dt = DateTime.Now;
                EditorApplication.update += Update;
            }
        }

        private static void Update()
        {
            if ((DateTime.Now - dt).TotalSeconds > 1)
            {
                EditorApplication.update -= Update;

                EditorPrefs.DeleteKey("Build_Player");
                EditorUtility.ClearProgressBar();
                BuildPlayer.StartBuildPlayer();
            }
        }
    }

}
