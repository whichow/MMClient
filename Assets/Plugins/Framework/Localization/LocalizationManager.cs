// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************

//using GameFramework.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace K.Localization
{
    /// <summary>
    /// 本地化管理器。
    /// </summary>
    internal sealed class LocalizationManager : MonoBehaviour, ILocalizationManager
    {
        private readonly Dictionary<string, string> m_Dictionary;
        //private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        //private IResourceManager m_ResourceManager;
        private ILocalizationHelper m_LocalizationHelper;
        private Language m_Language;

        /// <summary>
        /// 初始化本地化管理器的新实例。
        /// </summary>
        public LocalizationManager()
        {
            m_Dictionary = new Dictionary<string, string>();
            //m_ResourceManager = null;
            m_LocalizationHelper = null;
            m_Language = Language.Unspecified;
            //m_LoadDictionarySuccessEventHandler = null;
            //m_LoadDictionaryFailureEventHandler = null;
            //m_LoadDictionaryUpdateEventHandler = null;
            //m_LoadDictionaryDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取或设置本地化语言。
        /// </summary>
        public Language language
        {
            get
            {
                return m_Language;
            }
            set
            {
                if (value == Language.Unspecified)
                {
                    throw new Exception("Language is invalid.");
                }

                m_Language = value;
            }
        }

        /// <summary>
        /// 获取系统语言。
        /// </summary>
        public Language systemLanguage
        {
            get
            {
                if (m_LocalizationHelper == null)
                {
                    throw new Exception("You must set localization helper first.");
                }

                return m_LocalizationHelper.SystemLanguage;
            }
        }

        /// <summary>
        /// 获取字典条数。
        /// </summary>
        public int dictionaryCount
        {
            get
            {
                return m_Dictionary.Count;
            }
        }   

        /// <summary>
        /// 设置本地化辅助器。
        /// </summary>
        /// <param name="localizationHelper">本地化辅助器。</param>
        public void SetLocalizationHelper(ILocalizationHelper localizationHelper)
        {
            if (localizationHelper == null)
            {
                throw new Exception("Localization helper is invalid.");
            }

            m_LocalizationHelper = localizationHelper;
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        public void LoadDictionary(string dictionaryAssetName)
        {
            LoadDictionary(dictionaryAssetName, null);
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDictionary(string dictionaryAssetName, object userData)
        {
            //if (m_ResourceManager == null)
            //{
            //    throw new Exception("You must set resource manager first.");
            //}

            //if (m_LocalizationHelper == null)
            //{
            //    throw new Exception("You must set localization helper first.");
            //}

            //m_ResourceManager.LoadAsset(dictionaryAssetName, m_LoadAssetCallbacks, userData);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseDictionary(string text)
        {
            return ParseDictionary(text, null);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseDictionary(string text, object userData)
        {
            if (m_LocalizationHelper == null)
            {
                throw new Exception("You must set localization helper first.");
            }

            return m_LocalizationHelper.ParseDictionary(text, userData);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="args">字典参数。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, params object[] args)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("Key is invalid.");
            }

            string value = null;
            if (!m_Dictionary.TryGetValue(key, out value))
            {
                return string.Format("<NoKey>{0}", key);
            }

            try
            {
                return string.Format(value, args);
            }
            catch (Exception exception)
            {
                string errorString = string.Format("<Error>{0},{1}", key, value);
                if (args != null)
                {
                    foreach (object arg in args)
                    {
                        errorString += "," + arg.ToString();
                    }
                }

                errorString += "," + exception.Message;
                return errorString;
            }
        }

        /// <summary>
        /// 是否存在字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否存在字典。</returns>
        public bool HasRawString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("Key is invalid.");
            }

            return m_Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 根据字典主键获取字典值。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>字典值。</returns>
        public string GetRawString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("Key is invalid.");
            }

            string value = null;
            if (m_Dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            return string.Format("<NoKey>{0}", key);
        }

        /// <summary>
        /// 增加字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="value">字典内容。</param>
        /// <returns>是否增加字典成功。</returns>
        public bool AddRawString(string key, string value)
        {
            if (HasRawString(key))
            {
                return false;
            }

            m_Dictionary.Add(key, value ?? string.Empty);
            return true;
        }

        /// <summary>
        /// 移除字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否移除字典成功。</returns>
        public bool RemoveRawString(string key)
        {
            if (!HasRawString(key))
            {
                return false;
            }

            return m_Dictionary.Remove(key);
        }  
    }
}
