/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/30 17:49:41
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.UI;
using Google.Protobuf;
using Msg.ClientMessage;

namespace Game
{
    public partial class GameServer
    {
        #region 充值成功发货请求

        public void ReqCharge(int chargeId, string productId, string payload, string signature)
        {
            C2SChargeRequest req = new C2SChargeRequest();
#if UNITY_EDITOR
            req.Channel = 0;
#elif UNITY_ANDROID
            req.Channel = 1;
#elif UNITY_IPHONE
            req.Channel = 2;
#endif
            req.ItemId = chargeId;
            req.BundleId = productId;

            if (!string.IsNullOrEmpty(payload))
                req.PurchareData = ByteString.CopyFromUtf8(payload);
            if (!string.IsNullOrEmpty(signature))
                req.ExtraData = ByteString.CopyFromUtf8(signature);

            C2SRequest(req);
        }

        public void ExeCharge(S2CChargeResponse obj)
        {
            //Debuger.Log(obj.Channel);
            //Debuger.Log(obj.BundleId);

            //结单
            //IAPSdk.Instance.ConfirmPendingPurchase(obj.BundleId);
            KUIWindow.CloseWindow<PayWindow>();

            EventManager.Instance.GlobalDispatcher.DispatchEvent(GlobalEvent.PAY_SUCC, new EventData() {
                Integer = obj.ItemId
            });
        }

        #endregion

    }
}
