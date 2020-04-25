namespace Game.UI
{
    public  partial class SettingWindow : KUIWindow
    {
        public SettingWindow() :
            base(UILayer.kPop, UIMode.kSequence)
        {
            uiPath = "SettingWindow";
            uiAnim = UIAnim.kAnim1;
        }

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            RefreshView();
        }

        private void OnBackBtnClick()
        {
            CloseWindow(this);
        }
    }
}
