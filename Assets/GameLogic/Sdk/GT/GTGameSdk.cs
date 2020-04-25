/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/31 11:24:23
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/

namespace Game
{
    public class GTGameSdk
    {
        public static void InitSDK()
        {
            GTGameSdkCallbackMessage.OnInitResult();
        }

        public static void Login()
        {
            //UIWindow.GetWindow("HyFastLoginWindow").Show();
        }

        public static void Logout()
        {
            GTGameSdkCallbackMessage.OnLogout();
        }

        public static void ExitSDK()
        {
            GTGameSdkCallbackMessage.OnExitSDK();
        }
        public static void Pause(bool b)
        {

        }

        public static void Pay(PaySDKParams param)
        {

        }

        public static string GetSid()
        {
            //return GamePrefs.Instance.Account_Name;
            return "aaa";
        }

        public static void SubmitExtendData(string p, string datastr)
        {

        }


    }
}
