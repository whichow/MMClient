using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LocalDataMgr : Singleton<LocalDataMgr>
    {
        class LocalDataKeys
        {
            public const string PlayerAccount = "PlayerAccout";
            public const string PlayerId = "playerId";
            public const string PasswordKey = "passwordKey";
            public const string LoginChannelKey = "loginChannelKey";

            public const string MusicSele = "musicSele";
            public const string SoundSele = "soundSele";

            public const string NewBieGuideIDKey = "newBieGuideIDKey";
            public const string NewBieGuideStepIDKey = "newBieGuideStepIDKey";

            public const string CurSpeedLevel = "curSpeedLevel";
            public const string LanguageKey = "languageKey";

        }

        public static void Init()
        {
            _playerAccount = PlayerPrefs.GetString(LocalDataKeys.PlayerAccount, SystemInfo.deviceUniqueIdentifier);
            _playerId = PlayerPrefs.GetInt(LocalDataKeys.PlayerId, 0);

            _password = PlayerPrefs.GetString(LocalDataKeys.PasswordKey, "");
            _loginChannel = PlayerPrefs.GetInt(LocalDataKeys.LoginChannelKey, 0);

            _musicSele = PlayerPrefs.GetInt(LocalDataKeys.MusicSele, 0);
            _soundSele = PlayerPrefs.GetInt(LocalDataKeys.SoundSele, 0);

            _guideID = PlayerPrefs.GetInt(LocalDataKeys.NewBieGuideIDKey, 1);
            _guideStepID = PlayerPrefs.GetInt(LocalDataKeys.NewBieGuideStepIDKey, 1);
            _speedLevel = PlayerPrefs.GetInt(LocalDataKeys.CurSpeedLevel, 1);

#if UNITY_IPHONE
#if NewVersion
        int tmpLanguage = PlayerPrefs.GetInt(LocalDataKeys.LanguageKey, (int)Application.systemLanguage);
        _curLanguage = (SystemLanguage)tmpLanguage;
#else
        //兼容第一个版本的ipa包，Application.systemLanguage在母包里被裁切了
        int tmpLanguage = PlayerPrefs.GetInt(LocalDataKeys.LanguageKey, 10);
        _curLanguage = (SystemLanguage)tmpLanguage;
#endif

#elif UNITY_ANDROID || UNITY_EDITOR
            int tmpLanguage = PlayerPrefs.GetInt(LocalDataKeys.LanguageKey, (int)Application.systemLanguage);
            _curLanguage = (SystemLanguage)tmpLanguage;
#endif

            if (_guideStepID < 1)
                _guideStepID = 1;
            if (_guideID < 1)
                _guideID = 1;
        }

        #region language

        private static SystemLanguage _curLanguage;
        public static SystemLanguage CurLanguage
        {
            get { return _curLanguage; }
            set
            {
                if (_curLanguage == value)
                    return;
                _curLanguage = value;
                int language = (int)_curLanguage;
                PlayerPrefs.SetInt(LocalDataKeys.LanguageKey, language);
                PlayerPrefs.SetInt("languageKey", language); //兼容老版本母包逻辑
                PlayerPrefs.Save();
                //GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HeroEvent.SwitchLanguage);
            }
        }

        public static bool IsChinese
        {
            get { return _curLanguage == SystemLanguage.ChineseSimplified || _curLanguage == SystemLanguage.Chinese || _curLanguage == SystemLanguage.ChineseTraditional; }
        }
        #endregion


        #region newbie guide local data
        private static int _guideID;
        public static int NewBieGuideID
        {
            get { return _guideID; }
            set
            {
                if (_guideID == value)
                    return;
                _guideID = value;
                PlayerPrefs.SetInt(LocalDataKeys.NewBieGuideIDKey, _guideID);
                PlayerPrefs.Save();
            }
        }

        private static int _guideStepID;
        public static int NewBieGuildStepID
        {
            get { return _guideStepID; }
            set
            {
                if (_guideStepID == value)
                    return;
                _guideStepID = value;
                PlayerPrefs.SetInt(LocalDataKeys.NewBieGuideStepIDKey, _guideStepID);
                PlayerPrefs.Save();
            }
        }
        #endregion

        #region login data
        private static int _loginChannel;
        public static int LoginChannel
        {
            get { return _loginChannel; }
            set
            {
                if (_loginChannel == value)
                    return;
                _loginChannel = value;
                PlayerPrefs.SetInt(LocalDataKeys.LoginChannelKey, _loginChannel);
                PlayerPrefs.Save();
            }
        }

        private static string _password;
        public static string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                PlayerPrefs.SetString(LocalDataKeys.PasswordKey, _password);
                PlayerPrefs.Save();
            }
        }

        private static string _playerAccount;
        public static string PlayerAccount
        {
            get { return _playerAccount; }
            set
            {
                if (_playerAccount == value)
                    return;
                DeleteLocalCache();
                _playerAccount = value;
                PlayerPrefs.SetString(LocalDataKeys.PlayerAccount, _playerAccount);
                PlayerPrefs.Save();
            }
        }

        private static int _playerId;
        public static int PlayerId
        {
            get { return _playerId; }
            set
            {
                if (_playerId == value)
                    return;
                DeleteLocalCache();
                _playerId = value;
                PlayerPrefs.SetInt(LocalDataKeys.PlayerId, _playerId);
                PlayerPrefs.Save();
            }
        }

        #endregion


        private static int _musicSele;
        public static bool IsMusic
        {
            get
            {
                return _musicSele == 0;
            }
            set
            {
                _musicSele = value ? 0 : 1;
                PlayerPrefs.SetInt(LocalDataKeys.MusicSele, _musicSele);
                PlayerPrefs.Save();
            }
        }

        private static int _soundSele;
        public static bool IsSound
        {
            get
            {
                return _soundSele == 0;
            }
            set
            {
                _soundSele = value ? 0 : 1;
                PlayerPrefs.SetInt(LocalDataKeys.SoundSele, _soundSele);
                PlayerPrefs.Save();
            }
        }

        private static int _speedLevel;
        public static int SpeedLevel
        {
            get { return _speedLevel; }
            set
            {
                if (_speedLevel == value)
                    return;
                _speedLevel = value;
                PlayerPrefs.SetInt(LocalDataKeys.CurSpeedLevel, _speedLevel);
                PlayerPrefs.Save();
            }
        }

        public static void DeleteLocalCache()
        {
            PlayerPrefs.DeleteKey(LocalDataKeys.MusicSele);
            PlayerPrefs.DeleteKey(LocalDataKeys.SoundSele);
            PlayerPrefs.DeleteKey(LocalDataKeys.NewBieGuideIDKey);
            PlayerPrefs.DeleteKey(LocalDataKeys.NewBieGuideStepIDKey);
            PlayerPrefs.DeleteKey(LocalDataKeys.CurSpeedLevel);

            Init();
        }


        public static Dictionary<int, OrderCacheData> mDictOrderDatas = new Dictionary<int, OrderCacheData>();
        private static int _orderKey = 10000;

        public static bool ParseOrderCache()
        {
            string orders = PlayerPrefs.GetString("ibordercache");
            //LogHelper.Log("[LocalDataMgr.ParseOrderCache() => read local order info data, value:" + orders + "]");
            if (!string.IsNullOrEmpty(orders))
            {
                int orderId = 0;
                string[] tmp = orders.Split('|');
                if (tmp.Length > 0)
                {
                    OrderCacheData orderData;
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        orderData = new OrderCacheData();
                        bool blInvalid = orderData.SetData(tmp[i]);
                        if (!blInvalid)
                            continue;
                        orderId = orderData.mCacheOrderID;//int.Parse(orderData.mCacheOrderID);
                        _orderKey = Mathf.Max(orderId, _orderKey);
                        mDictOrderDatas.Add(orderData.mCacheOrderID, orderData);
                    }
                    return mDictOrderDatas.Count > 0;
                }
            }
            return false;
        }

        public static void RemoveOrderByKey(int key)
        {
            if (!mDictOrderDatas.ContainsKey(key))
                return;
            mDictOrderDatas.Remove(key);
            SaveOrderToCache();
        }

        private static void SaveOrderToCache()
        {
            string value = "";
            if (mDictOrderDatas.Count > 0)
            {
                Dictionary<int, OrderCacheData>.KeyCollection keyColl = mDictOrderDatas.Keys;
                foreach (int key in keyColl)
                {
                    if (string.IsNullOrEmpty(value))
                        value = mDictOrderDatas[key].ToString();
                    else
                        value += ("|" + mDictOrderDatas[key].ToString());
                }
            }
            PlayerPrefs.SetString("ibordercache", value);
        }

        public static int AddOrderInfo(string payload, string value, int channel, string bundleid)
        {
            _orderKey += 1;
            OrderCacheData orderData = new OrderCacheData();
            orderData.mPayLoad = payload;
            orderData.mOrderData = value;
            orderData.mOrderChannel = channel;
            orderData.mCacheOrderID = _orderKey;
            orderData.mBundleId = bundleid;
            mDictOrderDatas.Add(orderData.mCacheOrderID, orderData);
            SaveOrderToCache();
            //LogHelper.LogWarning("[LocalDataMgr.AddOrderInfo() => sava order info, _orderKey:"+ _orderKey + ", payload:"+ payload + "]");
            return orderData.mCacheOrderID;
        }
    }

    public class OrderCacheData
    {
        public int mCacheOrderID;
        public int mOrderChannel;
        public string mOrderData;
        public string mBundleId;
        public string mPayLoad;

        private string _dataString;

        public bool SetData(string value)
        {
            string[] tmps = value.Split(':');
            if (tmps.Length != 5)
                return false;
            mCacheOrderID = int.Parse(tmps[0]);
            mOrderChannel = int.Parse(tmps[1]);
            mOrderData = tmps[2];
            mBundleId = tmps[3];
            mPayLoad = tmps[4];
            _dataString = value;
            return true;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_dataString))
                _dataString = mCacheOrderID + ":" + mOrderChannel + ":" + mOrderData + ":" + mBundleId + ":" + mPayLoad;
            return _dataString;
        }
    }
}