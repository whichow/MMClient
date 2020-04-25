/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/23 14:00:53
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
#if UNITY_ANDROID
using UnityEngine;

namespace Game
{
    public class AndroidPlugins
    {

        public static void CallCurrentActivityMethod(string methodName, params object[] param)
        {
            var m_JavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var m_JvavObject = m_JavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            m_JvavObject.Call(methodName, param);
        }

        public static void CallStaticMethod(string className, string methodName, params object[] param)
        {
            var jc = new AndroidJavaClass(className);
            jc.CallStatic(methodName, param);
        }

    }
}
#endif
