using UnityEngine.UI;
namespace Game.UI
{
    partial class ShopWindow : KUIWindow
    {
        private Button _closeBtn;
        private Toggle[] _toggles;
        public int _shopType { get; private set; }
        private int _shopId = 0;

        public ShopWindow()
            : base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "ShopWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public override void Awake()
        {
            _closeBtn = Find<Button>("Close");
            _closeBtn.onClick.AddListener(OnCloseBtnClick);
            _toggles = new Toggle[4];
            for (int i = 0; i < 4; i++)
                _toggles[i] = Find<Toggle>("ToggleGroup/Tog" + (i + 1));
            foreach (Toggle tog in _toggles)
                tog.onValueChanged.AddListener((bool blSelect) => { if (blSelect) OnShopTypeChange(tog); });
            _shopType = ShopTypeConst.Prop;
            InitView();
        }

        public override void OnEnable()
        {
            // if (!IAPSdk.Instance.IsInitialized())
            // {
            //     IAPSdk.Instance.Initialize();
            // }

            int shopId = int.Parse(data.ToString());
            switch (shopId)
            {
                case 1:
                    OnShopTypeChange(_toggles[ShopTypeConst.Prop]);
                    _toggles[ShopTypeConst.Prop].isOn = true;
                    break;
                case 2:
                    OnShopTypeChange(_toggles[ShopTypeConst.Prop]);
                    _toggles[ShopTypeConst.Prop].isOn = true;
                    break;
                case 3:
                    OnShopTypeChange(_toggles[ShopTypeConst.Prop]);
                    _toggles[ShopTypeConst.Prop].isOn = true;
                    break;
                case 4:
                    OnShopTypeChange(_toggles[ShopTypeConst.Diamond]);
                    _toggles[ShopTypeConst.Diamond].isOn = true;
                    break;
                case 5:
                    OnShopTypeChange(_toggles[ShopTypeConst.SoulStone]);
                    _toggles[ShopTypeConst.SoulStone].isOn = true;
                    break;
                case 6:
                    break;
                case 7:
                    OnShopTypeChange(_toggles[ShopTypeConst.Attire]);
                    _toggles[ShopTypeConst.Attire].isOn = true;
                    break;
                case 8:
                    break;
            }
        }

        public void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        private void OnShopTypeChange(Toggle tog)
        {
            switch (tog.name)
            {
                case "Tog1":
                    _shopType = ShopTypeConst.Prop;
                    break;
                case "Tog2":
                    _shopType = ShopTypeConst.SoulStone;
                    data = ShopIDConst.SoulStone;
                    break;
                case "Tog3":
                    _shopType = ShopTypeConst.Diamond;
                    data = ShopIDConst.Diamond;
                    break;
                case "Tog4":
                    _shopType = ShopTypeConst.Attire;
                    data = ShopIDConst.Attire;
                    break;
            }
            RefreshView();
        }

        private void OnStoneInforBtnClick()
        {
            MessageBox.ShowMessage(null, KLocalization.GetLocalString(58151), OnConfim);
        }

        private void OnConfim()
        {

        }

    }
}

