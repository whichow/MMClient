using Game.DataModel;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class ActivityWindow : KUIWindow
    {
        private Button _closeBtn;
        private Toggle[] _toggles;
        private int _ActivityType;


        public ActivityWindow()
            : base(UILayer.kNormal, UIMode.kSequenceRemove)
        {
            uiPath = "ActivityWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public override void AddEvents()
        {
            base.AddEvents();
            ActivityDataModel.Instance.AddEvent(ActivityEvent.ChargeData, RefreshView);
            ActivityDataModel.Instance.AddEvent(ActivityEvent.ChargeReward, OnChargeReward);
            EventManager.Instance.GlobalDispatcher.AddEvent(GlobalEvent.PAY_SUCC, OnPayReward);
            ActivityDataModel.Instance.AddEvent(ActivityEvent.SignReward, OnChargeReward);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            ActivityDataModel.Instance.RemoveEvent(ActivityEvent.ChargeData, RefreshView);
            ActivityDataModel.Instance.RemoveEvent(ActivityEvent.ChargeReward, OnChargeReward);
            EventManager.Instance.GlobalDispatcher.RemoveEvent(GlobalEvent.PAY_SUCC, OnPayReward);
            ActivityDataModel.Instance.RemoveEvent(ActivityEvent.SignReward, OnChargeReward);
        }

        private void OnPayReward(IEventData value)
        {
            GameApp.Instance.GameServer.ReqChargeData();
            int itemId = int.Parse((value as EventData).Integer.ToString());
            PayXDM payXDM = XTable.PayXTable.GetByID(itemId);
            List<ItemInfo> itemInfos = new List<ItemInfo>();
            ItemInfo itemInfo = new ItemInfo();
            itemInfo.ItemCfgId = ItemIDConst.Diamond;
            itemInfo.ItemNum = payXDM.GemReward;
            itemInfos.Add(itemInfo);
            KUIWindow.OpenWindow<GetItemTipsWindow>(itemInfos);
        }

        private void OnChargeReward(IEventData value)
        {
            RefreshView();
            KUIWindow.OpenWindow<GetItemTipsWindow>((value as EventData).Data as List<ItemInfo>);
        }

        public override void Awake()
        {
            _closeBtn = Find<Button>("Close");
            _closeBtn.onClick.AddListener(OnCloseBtnClick);
            _toggles = new Toggle[4];
            for (int i = 0; i < 4; i++)
                _toggles[i] = Find<Toggle>("ToggleGroup/Tog" + (i + 1));
            foreach (Toggle tog in _toggles)
                tog.onValueChanged.AddListener((bool blSelect) => { if (blSelect) OnActivityTypeChange(tog); });
            _ActivityType = ActivityTypeConst.FirstCharge;
            InitView();
        }

        private void OnActivityTypeChange(Toggle tog)
        {
            switch (tog.name)
            {
                case "Tog1":
                    _ActivityType = ActivityTypeConst.FirstCharge;
                    break;
                case "Tog2":
                    _ActivityType = ActivityTypeConst.EverydayReward;
                    break;
                case "Tog3":
                    _ActivityType = ActivityTypeConst.MonthlyCard;
                    break;
            }
            RefreshView();
        }

        public override void OnEnable()
        {
            // if (!IAPSdk.Instance.IsInitialized())
            // {
            //     IAPSdk.Instance.Initialize();
            // }
            OnActivityTypeChange(_toggles[_ActivityType]);
        }

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        private void OnTab1PayBtnClick()
        {
            OpenWindow<ShopWindow>(ShopIDConst.Diamond);
        }
    }
}
