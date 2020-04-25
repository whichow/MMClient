/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/23 17:11:29
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AppConfig
    {
        #region path

        /// <summary>
        /// 服务器资源地址
        /// </summary>
        public static string ServerUrl
        {
            get
            {
#if UNITY_STANDALONE
                string SERVER_RES_DIRECTORY = "/StandaloneWindows/";
#elif UNITY_IPHONE
                string SERVER_RES_DIRECTORY = "/iPhone/";
#elif UNITY_ANDROID
                string SERVER_RES_DIRECTORY = "/Android/";
#endif
                return SERVER_RES_URL + SERVER_RES_DIRECTORY;
            }
        }

        public static readonly string CHANGE_LOG = "changelog.txt";
        public static readonly string CONFIG_FILE = "/config.xml";
        public static readonly string RES_DIRECTORY = "/ab_res/";
        public static readonly string LOCAL_RES_PATH = Application.persistentDataPath + RES_DIRECTORY;

        public static string CHANGE_LOG_URL { get { return ServerUrl + RES_DIRECTORY + CHANGE_LOG; } }   //更新公告地址
        public static string CONFIG_FILE_URL { get { return ConfigFileUrl; } }              //配置文件地址
        public static string SERVER_RES_URL { get { return ResServerUrl; } }                //资源服地址

        #endregion


        #region 是否需要资源热更新   // 从本地config.xml读取

        public static string ChangedVersion;        //热更版本号
        public static string GameVersion;           //游戏版本号
        public static string BundleVersion;         //iOS 包版本号
        public static string BundleVersionCode;     //Anroid 版本的内部版本号 仅用于应用市场、程序内部识别版本，判断新旧等用途
        public static string SvnVersion;            //Svn 版本号

        public static bool  IsDebugMod;
        public static bool  HotUpdateRes;           // 是否需要开启热更新资源

        public static string ResServerUrl;          //资源服务器
        public static string RegServerUrl;          //账号服务器
        public static string ConfigFileUrl;         //配置表地址
        public static string AppDownloadUrl;        //更新包地址

        #endregion

        public static bool IsReview = false;        //包是否在审核中

    }
}
