/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/31 11:15:53
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using IgaworksUnityAOS;
using UnityEngine;

namespace Game
{
    public class SdkCallbackApi
    {
        public static void OnInitResult()
        {
            //IGA统计
            IgaworksUnityPluginAOS.Common.startSession();

            if (string.IsNullOrEmpty(AppConfig.SvnVersion))
            {
                //LoadStreamingConfigFile();
                GameApp.Instance.ResourceCheckUpdate();
            }
            else
            {
                Debuger.LogError("[SdkCallbackApi.OnInitResult] 重复的调用");
            }
        }

        public static void OnLoginResult()
        {
            if (AppConfig.HotUpdateRes)
            {
                //UIWindow.CloseAll();
                //UIWindow.GetWindow("ServerSelectWindow").Show();
            }
            else
            {
                //CApp.Instance.SessionManager._SetGFWInfo(GamePrefs.Instance.Server_IP, GamePrefs.Instance.Server_Port);
                //CApp.Instance.AccountManager.Login(GamePrefs.Instance.Account_Name, GamePrefs.Instance.Account_Pwd);
            }
        }

        public static void OnLogout()
        {
//            if (CApp.Instance.SessionManager != null && CApp.Instance.SessionManager.C2GRequestSession != null)
//            {
//                CApp.Instance.AccountManager.Logout();
//                CApp.Instance.SessionManager._BreakFromGS();
//            }
        }

        public static void OnExitSDK()
        {
            //SdkApi.Logout();
            OnLogout();

            //IGA统计
            IgaworksUnityPluginAOS.Common.endSession();

            Application.Quit();
        }

        public static void OnExitSDKCanceled()
        {
            //SdkApi.IsShowExitWindow_OPPO = false;

            ////退出失败，取消退出
            //if (ReleaseNotes.Instance.Changed)
            {
                SdkApi.ExitSDK();
            }
        }

        public static void OnPayCallback(string result)
        {
            //支付结束，可刷新充值商城
            //if (CApp.Instance.UnitManager.MainPlayer != null)
            //{
            //    ((CApp.Instance.UnitManager.MainPlayer as CMainPlayer).ShopManagerNew as CShopManagerNew).CShopUIUpdate();
            //}

            //商品购买时，调用的 API            
            //IgaworksUnityPluginAOS.Adbrix.purchase(
            //    "orerdID_1",                                     //String orderID
            //    "productID_1",                                   //String productID
            //    "ProudctName_1",                                 //String productName
            //    10000.00,                                        //double price
            //    1,                                               //int quantity
            //    IgaworksUnityPluginAOS.Adbrix.Currency.KR_KRW,                          //Currency
            //    "cat1"                                           //商品分类的定义
            //);
        }

    }
}
