/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/23 13:14:00
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.Runtime.InteropServices;
using UnityEngine;

namespace Game
{
    public class CustomHeadImgPlugins
    {
#if UNITY_ANDROID
        private const string JAVA_CLASS = "com.gt.photo.CustomHeadImg";

        private static AndroidJavaObject m_AJO;
        private static AndroidJavaObject AJO
        {
            get
            {
                if (m_AJO == null)
                {
                    using (AndroidJavaClass cls = new AndroidJavaClass(JAVA_CLASS))
                    {
                        cls.CallStatic("Init", Application.persistentDataPath);
                        m_AJO = cls.CallStatic<AndroidJavaObject>("getInstance");
                    }
                }
                return m_AJO;
            }
        }

        private static void AndroidCall(string methodName, params object[] param)
        {
            AJO.Call(methodName, param);
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        public static void OpenCamera()
        {
            AndroidCall("takePhoto");
        }

        public static void OpenAlbum()
        {
            AndroidCall("pickFromAlbum");
        }
#elif UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void ex_OpenHeadImage();

        public static void OpenCamera()
        {
            ex_OpenHeadImage();
        }

        public static void OpenAlbum()
        {
            ex_OpenHeadImage();
        }
#else
        public static void OpenCamera()
        {
        }

        public static void OpenAlbum()
        {
        }
#endif

    }
}
