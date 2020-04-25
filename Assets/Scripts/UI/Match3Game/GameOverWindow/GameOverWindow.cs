
namespace Game.UI
{
    public partial class GameOverWindow : KUIWindow
    {
        public GameOverWindow() :
            base(UILayer.kNormal, UIMode.kSequence)
        {
            uiPath = "Settle";
        }

        public override void Awake()
        {
            base.Awake();
            InitModel();
            InitView();
        }
        public override void OnEnable()
        {
            base.OnEnable();
            RefreshModel();
            RefreshView();
        }


    }
}