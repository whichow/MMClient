/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/31 11:25:55
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/


namespace Game
{
    public class GTGameSdkCallbackMessage
    {
        public static void OnInitResult()
        {
            SdkCallbackApi.OnInitResult();
        }

        public static void OnLoginResult()
        {
            SdkCallbackApi.OnLoginResult();
        }

        public static void OnLogout()
        {
            SdkCallbackApi.OnLogout();
        }

        public static void OnExitSDK()
        {
            //UIConfirmWindow.ShowMsg("确认退出游戏？",
                //delegate () {
                    SdkCallbackApi.OnExitSDK();
                //},
                //delegate () { SdkCallbackApi.OnExitSDKCanceled(); }
            //);
        }

        public static void OnExitSDKCanceled()
        {
            SdkCallbackApi.OnExitSDKCanceled();
        }

        public static void OnPayCallback(string result)
        {
            if (int.Parse(result) == 0)
            {
                //成功
                //UIWindow.GetWindow("HyPayWindow").Hide();
            }
            else
            {
                //失败
                //UIWindow.GetWindow("HyPayFailWindow").Show();
            }

            SdkCallbackApi.OnPayCallback(result);
        }

    }
}
