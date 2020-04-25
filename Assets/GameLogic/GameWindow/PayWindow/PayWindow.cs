/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/6/3 10:58:58
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/


namespace Game.UI
{
    public partial class PayWindow : KUIWindow
    {

        public PayWindow() : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "PayWindow";
        }

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();

            // if (IAPSdk.Instance.IsInitialized())
            // {
            //     IAPSdk.Instance.BuyProductID(GetProductID(), PlayerDataModel.Instance.mPlayerData.mPlayerID, _payID, PayResultHandler);
            // }
            // else
            // {
            //     IAPSdk.Instance.Initialize();
            //     ToastBox.ShowText("Network error, please try again later!");
            //     CloseWindow<PayWindow>();
            // }
        }

        public override void UpdatePerSecond()
        {
            base.UpdatePerSecond();

            if (_state == -1)
            {
                CloseWindow<PayWindow>();
            }
        }

        private void PayResultHandler(int result, string productID, string receipt)
        {
            if (GetProductID() != productID)
            {
                Debuger.Log("支付成功，productID不一样:" + productID);
            }

            _state = result;
            if (result == 1)
            {
                RefreshView();
            }
            else
            {
                ShowMsg(receipt);
            }
        }

    }
}
