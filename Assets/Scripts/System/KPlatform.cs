// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
//#define ANDROID_MY
//#define IPHONE_MY
using Game.IAP;
using UnityEngine;
using System.Collections;
using PlatformCallback = System.Action<int, string, object>;

namespace Game
{
    /// <summary>
    /// Platform 相关内容
    /// </summary>
    public class KPlatform : KGameModule
    {
        private PlatformCallback _loginCallback;
        private PlatformCallback _shareCallback;
        private PlatformCallback _payCallback;
        private PlatformCallback _showAdsCallback;

        /// <summary>Gets a value indicating whether this instance is logined.</summary>
        public bool isLogined
        {
            get;
            set;
        }

        /// <summary>Gets any SDK channel identifier.</summary>
        public string sdkChannelID
        {
            get;
            set;
        }

        /// <summary>Gets the name of any SDK channel.</summary>
        public string sdkChannelName
        {
            get;
            set;
        }

        public static string appVersion
        {
            get
            {
#if (!UNITY_EDITOR) && (ANDROID_MY || IPHONE_MY)
#if IPHONE_MY
                return iOSGetVersionName();
#else
				return Application.version;			
#endif
#else
                return Application.version;
#endif
            }
        }

        public static string deviceID
        {
            get;
            set;
        }

        /// <summary>Initializes the platform.</summary>
        public void InitPlatform()
        {
#if UNITY_EDITOR
#else
            SdkInit();
#endif
        }

        private void TestLogin(PlatformCallback callback)
        {
            var index = PlayerPrefs.GetString("acc_index", "1");

            var dataTable = new Hashtable();
            dataTable.Add("openID", "test_OPENID_" + index + "_" + SystemInfo.deviceUniqueIdentifier);
            dataTable.Add("openToken", "test_OPENTOKEN");
            dataTable.Add("channel", "0");
            isLogined = true;
            if (callback != null)
            {
                callback(0, null, dataTable);
            }
        }

        /// <summary>登录</summary>
        public void LoginNormal(PlatformCallback callback)
        {
            _loginCallback = callback;

#if (!UNITY_EDITOR) && (ANDROID_MY || IPHONE_MY)
             SdkLogin(1);
#else
            TestLogin(callback);
#endif
        }

        /// <summary>Logins the guest.</summary>
        public void LoginGuest(PlatformCallback callback)
        {
            _loginCallback = callback;
#if (!UNITY_EDITOR) && (ANDROID_MY || IPHONE_MY)
             SdkLogin(2);
#else
            TestLogin(callback);
#endif
        }

        /// <summary>Automatics the login.</summary>
        public void AutoLogin(PlatformCallback callback)
        {
            _loginCallback = callback;

#if (!UNITY_EDITOR) && (ANDROID_MY || IPHONE_MY)
             SdkLogin(0);
#else
            TestLogin(callback);
#endif
        }

        /// <summary>注销</summary>
        /// <param name="callback">The callback.</param>
        public void Logout(PlatformCallback callback)
        {
        }

        /// <summary>Pays the specified item identifier.</summary>
        /// <param name="itemID">The item identifier.物品ID.</param>
        /// <param name="price">The price.物品价格(元).</param>
        /// <param name="display">The display.物品显示名称.</param>
        /// <param name="descrpition">The descrpition.物品显示功能.</param>
        /// <param name="callback">The callback.</param>
        public void Pay(int itemID, string price, string display, string descrpition, PlatformCallback callback)
        {
#if (!UNITY_EDITOR) && (ANDROID_MY || IPHONE_MY)
			_payCallback = callback;

			string productID = "";
			switch (itemID)
			{
			case 441101:
			productID = "_pay_6";
			break;
			case 441102:
			productID = "_pay_7";
			break;
			case 441103:
			productID = "_pay_8";
			break;
			case 441104:
			productID = "_pay_9";
			break;
			case 441201:
			productID = "_pay_10";
			break;
			case 441202:
			productID = "_pay_11";
			break;
			}

#if ANDROID_MY

            var table = new Hashtable();
            table.Add("productID", productID);
            table.Add("productName", display);
            table.Add("productBody", descrpition);
            table.Add("productPrice", price + "元");
            table.Add("serverID", "0");

            string json = table.ToJsonText();
            SdkPay(json);
#elif IPHONE_MY
			SdkPay(productID);
#endif
#else
            if (callback != null)
            {
                callback(0, null, null);
            }
#endif
        }

        public void Share(string title, string text, string imagePath, string imageURL, PlatformCallback callback)
        {
            _shareCallback = callback;
#if ANDROID_MY

            var table = new Hashtable();

            if (!string.IsNullOrEmpty(title))
            {
                table.Add("title", title);
            }
            if (!string.IsNullOrEmpty(text))
            {
                table.Add("text", text);
            }
            if (!string.IsNullOrEmpty(imagePath))
            {
                table.Add("imagePath", imagePath);
            }
            if (!string.IsNullOrEmpty(imageURL))
            {
                table.Add("imageURL", imageURL);
            }

            string json = table.ToJsonText();
            SdkShare(json);
#elif IPHONE_MY
            iOSShare(title, text, imagePath, imageURL);
#else
            if (callback != null)
            {
                callback(0, null, null);
            }
#endif
        }

        private void ShowUnityAds()
        {
#if IPHONE_MY
            if (UnityEngine.Advertisements.Advertisement.IsReady())
            {
                var options = new UnityEngine.Advertisements.ShowOptions()
                {
                    resultCallback = (result) =>
                    {
                        if (_showAdsCallback != null)
                        {
                            switch (result)
                            {
                                case UnityEngine.Advertisements.ShowResult.Finished:
                                    _showAdsCallback(0, null, null);
                                    break;
                                default:
                                    _showAdsCallback(632, null, null);
                                    break;
                            }
                        }
                    }
                };
                UnityEngine.Advertisements.Advertisement.Show(null, options);
            }
            else
            {
                if (_showAdsCallback != null)
                {
                    _showAdsCallback(631, null, null);
                }
            }
#endif
        }
        /// <summary>Shows the ads.</summary>
        /// <param name="type">The type. 1 video ,2 chaping</param>
        /// <param name="queue">The queue. chansi|fdf|dfdf</param>
        public void ShowAds(string type, string queue, PlatformCallback callback)
        {
            _showAdsCallback = callback;
#if IPHONE_MY
            if (string.IsNullOrEmpty(queue))
            {
                ShowUnityAds();
            }
            else
            {
                SdkShowAds(type, queue);
            }
#else
            Debug.Log("play ads success");
            if (_showAdsCallback != null)
            {
                _showAdsCallback(0, null, null);
            }
#endif
        }

        /// <summary>Statistics the specified event identifier.统计事件.</summary>
        public void Statistic(int eventID, string eventLabel, int eventCount, Hashtable eventInfo)
        {
#if !UNITY_EDITOR
#if ANDROID_MY
            AndroidJavaClass gameAPI = new AndroidJavaClass("com..manysdk.api.UnityGameAPI");
            gameAPI.CallStatic("statistic", eventID, eventLabel, eventCount, "");
#elif IPHONE_MY
#endif
#endif
        }


        #region SDK

#if IPHONE_MY
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private extern static string iOSGetVersionName();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private extern static string iOSGetVersionCode();
#endif

#if IPHONE_MY
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private extern static void iOSInit(string messager, string callback);
#endif
        private void SdkInit()
        {
#if UNITY_ANDROID
            AndroidJavaClass gameAPI = new AndroidJavaClass("com..manysdk.api.UnityGameAPI");

            gameAPI.CallStatic("setUnityMessagerName", "Platform");
            gameAPI.CallStatic("setUnityCallbackName", "OnManySDKCallback");
#elif IPHONE_MY
            iOSInit("Platform","OnManySDKCallback");
#endif
        }


#if IPHONE_MY
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private extern static void iOSLogin(int type);
#endif
        private void SdkLogin(int type)
        {
            isLogined = false;

#if UNITY_ANDROID
            AndroidJavaClass gameAPI = new AndroidJavaClass("com..manysdk.api.UnityGameAPI");
            gameAPI.CallStatic("lastLogin");
#elif IPHONE_MY
            iOSLogin(type);
#endif
        }

#if IPHONE_MY
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private extern static void iOSPay(string itemID);
#endif
        private void SdkPay(string json)
        {
#if ANDROID_MY
            AndroidJavaClass gameAPI = new AndroidJavaClass("com..manysdk.api.UnityGameAPI");
            gameAPI.CallStatic("pay", json);
#endif

#if IPHONE_MY
            iOSPay(json);
#endif
        }
#if IPHONE_MY
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private extern static void iOSShare(string title, string content, string imagePath, string targetUrl);
#endif

        private void SdkShare(string json)
        {
#if ANDROID_MY
            AndroidJavaClass gameAPI = new AndroidJavaClass("com..manysdk.api.UnityGameAPI");
            gameAPI.CallStatic("share", json);
#endif

#if IPHONE_MY
#endif
        }

#if IPHONE_MY
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private extern static void iOSShowAds(string type, string queue);
#endif
        private void SdkShowAds(string type, string queue)
        {
#if IPHONE_MY
            iOSShowAds(type, queue);
#endif
        }

        /// <summary>Called when [many SDK callback].</summary>
        /// <param name="message">The message.</param>
        public void OnManySDKCallback(string message)
        {
            var table = message.ToJsonTable();
            var method = table.GetString("method");
            if (method == "login")
            {
                var data = table.GetTable("data");
                if (data.Contains("err"))
                {
                    if (_loginCallback != null)
                    {
                        _loginCallback(611, data.GetString("msg"), null);
                    }
                }
                else
                {
                    isLogined = true;

                    var dataTable = new Hashtable();
                    dataTable.Add("openID", data.GetString("openID"));
                    dataTable.Add("openToken", data.GetString("openToken"));
                    dataTable.Add("headURL", data.GetString("headURL"));
                    dataTable.Add("nickName", data.GetString("nickName"));
                    dataTable.Add("channel", data.GetString("channel", "0"));

                    deviceID = data.GetString("deviceID");

                    if (_loginCallback != null)
                    {
                        _loginCallback(0, null, dataTable);
                    }
                }
                _loginCallback = null;
            }

            if (method == "pay")
            {
                var data = table.GetTable("data");
                if (data.Contains("err"))
                {
                    if (_payCallback != null)
                    {
                        _payCallback(621, data.GetString("msg"), null);
                    }
                }
                else
                {
                    if (_payCallback != null)
                    {
                        _payCallback(0, null, null);
                    }
                }
                _payCallback = null;
            }

            if (method == "share")
            {
                var data = table.GetTable("data");
                if (data.Contains("err"))
                {
                    var err = data.GetString("err");
                    if (_shareCallback != null)
                    {
                        _shareCallback((err != "16") ? 615 : 616, data.GetString("msg"), null);
                    }
                }
                else
                {
                    if (_shareCallback != null)
                    {
                        _shareCallback(0, null, null);
                    }
                }
                _shareCallback = null;
            }

            if (method == "ads")
            {
                var data = table.GetTable("data");
                if (data.Contains("err"))
                {
                    if (_showAdsCallback != null)
                    {
                        _showAdsCallback(631, data.GetString("msg"), null);
                    }
                }
                else
                {
                    if (_showAdsCallback != null)
                    {
                        _showAdsCallback(0, null, null);
                    }
                }
                _showAdsCallback = null;
            }

            Debug.Log("message:" + message);
        }


        private PlatformCallback _buyCallback;
        public void Buy(string goods, PlatformCallback callback)
        {
            _buyCallback = callback;
#if UNITY_IAP
        IAPWrapper.Instance.Buy(goods);
#else
            if (callback != null)
            {
                callback(0, null, "test");
            }
#endif
        }

        private void InitIAP()
        {
            var goodsBank = gameObject.AddComponent<IAPGoodsBank>();
            var rmbItems = Game.KShop.Instance.GetRmbItems();
            foreach (var item in rmbItems)
            {
                goodsBank.RegisterGoods(new IAPGoods
                {
                    id = item.itemID,
                    storeId = item.bundleId,
                    priceString = "¥" + item.Cost,
                });
            }
            //goodsBank.RegisterGoods(new IAPGoods
            //{
            //    id = "test01",
            //    storeID = "crgame.test01",
            //    priceV = 0.99f,
            //});
            //goodsBank.RegisterGoods(new IAPGoods
            //{
            //    id = "diamond01",
            //    storeID = "diamond01",
            //    priceV = 0.99f,
            //});
            //goodsBank.RegisterGoods(new IAPGoods
            //{
            //    id = "diamond02",
            //    storeID = "diamond02",
            //    priceV = 0.99f,
            //});
            //goodsBank.RegisterGoods(new IAPGoods
            //{
            //    id = "diamond03",
            //    storeID = "diamond03",
            //    priceV = 0.99f,
            //});
            //goodsBank.RegisterGoods(new IAPGoods
            //{
            //    id = "diamond04",
            //    storeID = "diamond04",
            //    priceV = 0.99f,
            //});
            //goodsBank.RegisterGoods(new IAPGoods
            //{
            //    id = "diamond05",
            //    storeID = "diamond05",
            //    priceV = 0.99f,
            //});
            //goodsBank.RegisterGoods(new IAPGoods
            //{
            //    id = "diamond06",
            //    storeID = "diamond06",
            //    priceV = 0.99f,
            //});

            var wrapper = gameObject.AddComponent<IAPUnityWrapper>();
            wrapper.onInitialize = OnIAPInit;
            wrapper.onPurchase = OnIAPPurchase;
        }

        private void InitSocial()
        {
            //gameObject.AddComponent<SocialManager>();
        }

        private void OnIAPInit(int code, string message, object data)
        {
            Debug.Log("OnIAPInit" + message);
        }

        private void OnIAPPurchase(int code, string message, object data)
        {
            if (_buyCallback != null)
            {
                _buyCallback(code, message, data);
            }
            _buyCallback = null;
            //if (code == 0)
            //{

            //    Debug.Log("receipt: " + data);
            //}
        }

        public override void LoadComplete()
        {
            InitIAP();
        }

        #endregion

        #region UNITY

        private void Awake()
        {
            _Instance = this;
        }

        private void Start()
        {
            //InitPlatform();
        }

        private void Update()
        {
        }

        #endregion

        #region STATIC

        private static KPlatform _Instance;
        public static KPlatform Instance { get { return _Instance; } }

        #endregion
    }
}