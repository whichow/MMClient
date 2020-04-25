// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************

namespace Game.IAP
{
    using System;
    using UnityEngine;

    public enum IAPPlatform
    {
        Unknow,
        GooglePlay,
        AppleAppStore,
        WindowsStore,
        TizenStore,
        MacAppStore
    }

    public abstract class IAPWrapper : MonoBehaviour
    {
        #region Static

        public static IAPWrapper Instance
        {
            get;
            protected set;
        }

        public static void Purchase(string product)
        {
            if (Instance != null)
            {
                Instance.Buy(product);
            }
            else
            {
                Debug.LogError("Store not initialized ");
            }
        }

        public static IAPPlatform GetCurrentPlatform()
        {
#if UNITY_ANDROID
            return IAPPlatform.GooglePlay;
#elif UNITY_IOS
            return IAPPlatform.AppleAppStore;
#elif UNITY_WSA
            return IAPPlatform.WindowsStore;
#elif UNITY_TIZEN
            return IAPPlatform.TizenStore;
#elif UNITY_STANDALONE_OSX
            return IAPPlatform.MacAppStore;
#else
            return IAPPlatform.Unknow;
#endif
        }

        #endregion

        #region Event 

        public Action<int, string, object> onInitialize;
        public Action<int, string, object> onPurchase;

        #endregion

        #region Method

        public abstract void Buy(string product);

        #endregion
    }
}

