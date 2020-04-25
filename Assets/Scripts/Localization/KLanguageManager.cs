// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KLanguageManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using UnityEngine;

namespace Game
{
    public class KLanguageManager : KGameModule
    {
        #region Property

        public override int priority
        {
            get
            {
                return 100;
            }
        }

        public KLanguage defaultLanguage
        {
            get;
            private set;
        }

        public KLanguage currentLanguage
        {
            get;
            private set;
        }

        public KLanguage[] allLanguages
        {
            get;
            private set;
        }

        public SystemLanguage systemLanguage
        {
            get { return Application.systemLanguage; }
        }

        #endregion

        #region Method

        public KLanguage GetLanguage(string name)
        {
            if (allLanguages != null)
            {
                return System.Array.Find(allLanguages, l => l.name == name);
            }
            return null;
        }

        public void SetLanguage(string name)
        {
            currentLanguage = GetLanguage(name);
            if (currentLanguage == null)
            {
                currentLanguage = defaultLanguage;
            }
            PlayerPrefs.SetString("language", currentLanguage.name);
            PlayerPrefs.Save();
            LocationMgr.Instance.SwitchLanguage();
        }

        private void Load(Hashtable table)
        {
            var langList = table.GetArrayList("language");
            if (langList != null && langList.Count > 0)
            {
                var languages = new KLanguage[langList.Count - 1];

                var tmpT = new Hashtable();
                var tmpL0 = (ArrayList)langList[0];

                for (int i = 1; i < langList.Count; i++)
                {
                    var tmpLi = (ArrayList)langList[i];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpT[tmpL0[j]] = tmpLi[j];
                    }
                    var language = new KLanguage
                    {
                        name = tmpT.GetString("Name"),
                        iconName = tmpT.GetString("Icon"),
                        database = tmpT.GetString("Database"),
                    };
                    language.LoadEntries(table.GetArrayList(language.database));
                    languages[i - 1] = language;
                }

                allLanguages = languages;
                defaultLanguage = languages[0];
                defaultLanguage.SetAsDefault();
            }
        }

        private void ApplyLanguage()
        {
            var languageName = PlayerPrefs.GetString("language", string.Empty);
            if (string.IsNullOrEmpty(languageName))
            {
                var systemLanguage = Application.systemLanguage;
                switch (systemLanguage)
                {
                    case SystemLanguage.ChineseSimplified:
                        currentLanguage = GetLanguage("ChineseSimplified");
                        PlayerPrefs.SetString("language", "ChineseSimplified");
                        PlayerPrefs.Save();
                        break;
                    default:
                        currentLanguage = GetLanguage("ChineseSimplified");
                        PlayerPrefs.SetString("language", "ChineseSimplified");
                        PlayerPrefs.Save();
                        break;
                }
                if (currentLanguage == null)
                {
                    currentLanguage = defaultLanguage;
                }
            }
            else
            {
                currentLanguage = GetLanguage(languageName);
                if (currentLanguage == null)
                {
                    currentLanguage = defaultLanguage;
                }
            }
            LocationMgr.Instance.SwitchLanguage();
        }

        /// <summary>
        /// 默认词条Id
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public int GetStringId(string entry)
        {
            if (defaultLanguage != null)
            {
                int id;
                defaultLanguage.TryGetId(entry, out id);
                return id;
            }
            return 0;
        }

        /// <summary>
        /// 本地语言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetLocalString(int id)
        {
            if (currentLanguage != null)
            {
                string entry;
                currentLanguage.TryGet(id, out entry);
                return entry;
            }
            return string.Empty;
        }

        /// <summary>
        /// 用默认语言翻译本地语言
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public string GetLocalString(string entry)
        {
            if (defaultLanguage == null || currentLanguage == null || currentLanguage == defaultLanguage)
            {
                return entry;
            }

            int id;
            if (defaultLanguage.TryGetId(entry, out id))
            {
                string retEntry;
                currentLanguage.TryGet(id, out retEntry);
                return retEntry;
            }
            return string.Empty;
        }

        #endregion

        #region Unity

        public static KLanguageManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        // Use this for initialization
        public override void Load()
        {
            TextAsset textAsset;
            KAssetManager.Instance.TryGetExcelAsset("language", out textAsset);
            if (textAsset)
            {
                var table = textAsset.bytes.ToJsonTable();
                if (table != null)
                {
                    Load(table);
                }
            }

            ApplyLanguage();
        }

        #endregion
    }
}