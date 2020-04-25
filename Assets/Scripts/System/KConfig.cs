// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System.Collections;
using UnityEngine;

namespace Game
{
    public class KConfig : SingletonUnity<KConfig>
    {
        #region FIELD

        public TextAsset shopText;
        public TextAsset lvupText;
        public TextAsset eventText;
        public TextAsset levelText;
        public TextAsset otherText;

        public TextAsset codeText;
        public TextAsset guideText;
        public TextAsset sencitiveText;

        public TextAsset rankingText;

        private object _initConfig;

        #endregion

        #region PROPERTY 

        #endregion

        #region METHOD  

        public object initConfig
        {
            get { return _initConfig; }
        }

        #endregion

        #region METHOD

        private void ReadLocal()
        {
            //var vkeyText = Resources.Load<TextAsset>("Configs/vkeys");
            //if (vkeyText)
            //{
            //    KSecurity.SetIVKey(vkeyText.bytes);
            //}

            //var configText = Resources.Load<TextAsset>("Configs/config");
            //if (configText)
            //{
            //    var cTable = configText.text.ToJsonTable();
            //    KConfig.ResTimestamp = cTable.GetInt("res_timestamp");
            //    KConfig.ResVersion = KConfig.ResTimestamp.ToString();
            //}
        }

        private void WriteLocal()
        {

        }

        private void LoadData()
        {

        }

        #endregion

        #region UNITY

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            if (this != Instance)
            {
                LoadData();
                Destroy(this);
                return;
            }

            ReadLocal();
            LoadData();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable()
        {

        }

        #endregion

        #region STATIC API

        private static Hashtable _OnlineConfig = new Hashtable();

        public static Hashtable OnlineConfig
        {
            get { return _OnlineConfig; }
            set
            {
                _OnlineConfig = value;
                var reviewVersion = value.GetString("review");
                if (!string.IsNullOrEmpty(reviewVersion))
                {
                    KConfig.ReviewFlag = true;
                    KConfig.EncryptText = false;
                    KConfig.ConnectURL = value.GetString("review_url");
                }
                else
                {
                    KConfig.ReviewFlag = false;
                    KConfig.EncryptText = true;
                    KConfig.ConnectURL = value.GetString("normal_url");
                }

                KConfig.UpdateURL = value.GetString("update_url");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [encrypt text].
        /// </summary>
        public static bool EncryptText
        {
            get;
            set;
        }

        private static string _AppVersion;
        /// <summary>Gets the application version.</summary>
        public static string AppVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_AppVersion))
                {
                    _AppVersion = KPlatform.appVersion;
                }
                return _AppVersion;
            }
        }

        /// <summary>Gets the application version hint.</summary>
        public static string AppVersionHint
        {
            get
            {
                return AppVersion.Replace('.', '_');
            }
        }

        /// <summary>Gets the resource version.</summary>
        public static string ResVersion
        {
            get;
            set;
        }

        /// <summary>Gets the resource timestamp.</summary>
        public static int ResTimestamp
        {
            get;
            set;
        }

        public static string ServerURL
        {
            get;
            set;
        }

        /// <summary>Gets or sets the ip URL.</summary>
        public static string ConnectURL
        {
            get;
            set;
        }

        public static string UpdateURL
        {
            get;
            set;
        }

        /// <summary>Gets the official URL.</summary>
        public static string OfficialURL
        {
            get { return "http://www..cc"; }
        }

        /// <summary>Gets or sets the review flag.</summary>
        public static bool ReviewFlag
        {
            get;
            set;
        }

        /// <summary>Gets the share image path.</summary>
        public static string ShareImagePath
        {
            get { return Application.persistentDataPath + "/screenshot.png"; }
        }

        /// <summary>Gets the share icon path.</summary>
        public static string ShareIconPath
        {
            get { return Application.persistentDataPath + "/icon.png"; }
        }

        /// <summary>Gets the share icon URL.</summary>
        public static string ShareIconURL
        {
            get { return "http://cdn..net/share_icon.png"; }
        }

        #endregion
    }
}