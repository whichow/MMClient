namespace Game.UI
{
    public partial class PhotoShopPickCardLowWindow : KUIWindow
    {
        public PhotoShopPickCardLowWindow() :
                base(UILayer.kNormal, UIMode.kSequence)
        {
            uiPath = "PhotoShopPickCardLow";
        }

        #region override

        public override void Awake()
        {
            //InitModel();
            InitView();
        }

        public override void OnDisable()
        {
            itemData = null;
        }

        public override void OnEnable()
        {
            RefreshView();
        }

        #endregion

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        private void OnAgainBtnClick()
        {
            Debuger.Log("再次抽奖");
            GetWindow<PhotoShopWindow>().SendBuyLowCat();
        }

        private void OnBtnNoClick()
        {
            OpenWindow<MessageBox>(new MessageBox.Data
            {
                content = "请先把牌翻开~",
            });
        }

    }
}
