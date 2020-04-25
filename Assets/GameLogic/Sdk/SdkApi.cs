/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/31 11:11:59
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using IgaworksUnityAOS;
using UnityEngine;

namespace Game
{
    public class SdkApi
    {
        /// <summary>
        /// 调用初始化
        /// </summary>
        public static void InitSDK()
        {
            GameObject callbackGO = new GameObject("SdkCallbackMessage");
            callbackGO.AddComponent<SdkCallbackMessage>();
            GameObject.DontDestroyOnLoad(callbackGO);

#if UNITY_ANDROID

#endif

#if UNITY_IPHONE

#endif
            //Bugly
            BuglyApi.InitBuglySDK();

            //统计
            IgaworksUnityPluginAOS.InitPlugin();

            GTGameSdk.InitSDK();
        }

        /// <summary>
        /// 登陆 该方法需要在初始化成功后调用
        /// </summary>
        public static void Login()
        {
            GTGameSdk.Login();
        }

        /// <summary>
        /// 退出账号
        /// </summary>
        public static void Logout()
        {
            //UIWindow.CloseAll();
            GTGameSdk.Logout();
        }

        /// <summary>
        /// 退出SDK
        /// 当游戏退出前 必须调用该方法，进行清理工作。如果游戏直接退出，而不调用该方法，可能会出现未知错误，导致程序崩溃。
        /// </summary>
        public static void ExitSDK()
        {
            GTGameSdk.ExitSDK();
        }

        public static void Pause(bool b)
        {
            GTGameSdk.Pause(b);
            if (b)
            {
                IgaworksUnityPluginAOS.Common.endSession();
            }
            else
            {
                IgaworksUnityPluginAOS.Common.startSession();
            }
        }

        /// <summary>
        /// 获取当前登陆用户的SID
        /// </summary>
        /// <returns></returns>
        public static string GetSID()
        {
            return GTGameSdk.GetSid();
        }

        /// <summary>
        /// 调用支付接口
        /// </summary>
        public static void Pay(PaySDKParams param)
        {
            GTGameSdk.Pay(param);
        }

        /// <summary>
        /// 提交游戏扩展数据
        /// 游戏 SDK 要求游戏在运行过程中提交一些用于运营需要的扩展数据，这些数据通过扩展数据提交方法进行提交。
        /// </summary>
        public static void SubmitExtendData(EReportDataType type, string eventName = "", string param = "")
        {
            if (type == EReportDataType.Custom)
            {
                if (!string.IsNullOrEmpty(eventName))
                {
                    IgaworksUnityPluginAOS.Adbrix.retention(eventName, param);
                }
            }
            else
            {
                IgaworksUnityPluginAOS.Adbrix.firstTimeExperience("TutorialComplete");
                switch (type)
                {
                    case EReportDataType.Custom:
                        if (!string.IsNullOrEmpty(eventName))
                        {
                            IgaworksUnityPluginAOS.Adbrix.retention(eventName, param);
                        }
                        break;
                    case EReportDataType.CreateRole:
                        IgaworksUnityPluginAOS.Adbrix.firstTimeExperience("CharacterCreate");
                        break;
                    case EReportDataType.LoginRole:
                        IgaworksUnityPluginAOS.Adbrix.firstTimeExperience("loginComplete");
                        IgaworksUnityPluginAOS.Adbrix.retention("LoginRole");
                        break;
                    case EReportDataType.LogoutRole:
                        IgaworksUnityPluginAOS.Adbrix.retention("LogoutRole");
                        break;
                    case EReportDataType.LevelUp:
                        IgaworksUnityPluginAOS.Adbrix.firstTimeExperience("LevelUp");
                        break;
                    case EReportDataType.Recharge:
                        
                        break;
                    case EReportDataType.EnterServer:
                        IgaworksUnityPluginAOS.Adbrix.retention("EnterServer");
                        break;
                    default:
                        break;
                }
                //string roleID = CApp.Instance.UnitManager.MainPlayer.ID.ToString();
                //string roleName = CApp.Instance.UnitManager.MainPlayer.Name;
                //long roleLev = (long)CApp.Instance.UnitManager.MainPlayer.Level;
                ////long ticks = new DateTime((CApp.Instance.UnitManager.MainPlayer as CMainPlayer).CreateTime).ToUniversalTime().Ticks;
                ////long epoch = (ticks - 621355968000000000) / 10000000;
                //long epoch = CApp.Instance.UnitManager.MainPlayer.CreateTime;

                //JsonData json = new JsonData();		    //字段类型	是否为空	字段描述			    提交时机
                //json["roleId"] = roleID;			    //String	必填		角色ID				用户登录游戏成功后
                //json["roleName"] = roleName;	        //String	必填		角色昵称			    用户登录游戏成功后
                //json["roleLevel"] = roleLev;	        //String	必填		角色等级			    用户登录游戏成功后／当用户的角色等级发生变化后
                //json["roleCTime"] = epoch;		        //long		必填		角色创建时间(单位：秒)  用户登录游戏成功后
                //json["zoneId"] = serverID;			    //String	必填		区服ID				用户登录游戏成功后
                //json["zoneName"] = serverName;		    //String	必填		区服名称			    用户登录游戏成功后
                //string datastr = json.ToJson();
                //HYGameSdk.SubmitExtendData("createGameRole", datastr);
            }
        }

    }

    public enum EReportDataType
    {
        Custom      = 0,    // 自定义事件
        CreateRole  = 1,    // 创建角色
        LoginRole   = 2,    // 角色登录
        LogoutRole  = 3,    // 角色退出
        LevelUp     = 4,    // 角色升级
        Recharge    = 5,    // 充值(下单时)
        EnterServer = 6,
    }

    public class PaySDKParams
    {
        public int shopId;       // 商城商品ID
        public string productId;    // AppStore产品ID
        public string accountId;    // 账号
        public string cpOrderId;    // cp订单
        public string amount;       // 金额
        public string callbackInfo;
        public string notifyUrl;
        public string signType;
        public string sign;
    }

}
