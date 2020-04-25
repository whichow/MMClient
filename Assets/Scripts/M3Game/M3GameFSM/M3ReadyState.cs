using Game.UI;

namespace Game.Match3
{
    public class M3ReadyState : State<GameFSM>
    {
        private int tmpCount = 190;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            InitGame();
            M3GameManager.Instance.ScreenLock = true;
            M3GameManager.Instance.isReady = true;
            M3GameManager.Instance.soundManager.PlayPreGameMusic();
            M3GameManager.Instance.soundManager.PlayMainMusic();
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);
            tmpCount--;
            if (tmpCount <= 180)
            {
                if (tmpCount == 180)
                {
                    M3GameManager.Instance.gameUI.OpenWindow<GameGoalWindow>();
                }
                if (tmpCount <= 0)
                {
                    M3GameManager.Instance.isGoalSignFinish = true;
                    M3GameManager.Instance.ScreenLock = false;
                    M3GameManager.Instance.gameUI.CloseWindow<GameGoalWindow>();
                    M3GameManager.Instance.RunGame();
                }
            }
        }

        public override void Exit(GameFSM entityType)
        {
            base.Exit(entityType);
        }

        public void InitGame()
        {
            M3GameManager.Instance.isPause = false;
            if (!M3Config.isEditor)
            {
                M3GameManager.Instance.level = M3LevelConfigMgr.Instance.MapReader(LevelDataModel.Instance.CurrLevel.ChessboardID.ToString());
            }
            else
            {
                M3GameManager.Instance.level = M3LevelConfigMgr.Instance.MapReader(M3Config.levelId);
            }

            M3FxManager.Instance.Init();
            FrameScheduler.Instance().Clear();

            M3GameManager.Instance.specialHandler = new M3SpecialHandler();
            M3GameManager.Instance.comboManager = new M3ComboManager();
            M3GameManager.Instance.matcher = new M3Matcher();
            M3GameManager.Instance.bonusManager = new M3BonusManager();
            M3GameManager.Instance.propManager = new M3PropManager();
            M3GameManager.Instance.modeManager = new M3GameModeManager();
            M3GameManager.Instance.catManager = new M3CatManager();

            KCat cat = null;
            if (LevelDataModel.Instance.CurrentCatID > 0)
            {
                cat = KCatManager.Instance.GetCat(LevelDataModel.Instance.CurrentCatID);
            }
            if (M3Config.isEditor)
            {
                M3GameManager.Instance.catManager.Init(M3Config.editorCat);
            }
            else
            {
                M3GameManager.Instance.catManager.Init(cat);
            }

            M3GameManager.Instance.skillManager = new M3SkillManager();
            M3GameManager.Instance.conveyorManager = new M3ConveyorManager();
            M3GameManager.Instance.hiddenManager = new M3HiddenManager();


            M3ItemManager.Instance.InitView();
            M3GridManager.Instance.Init();
            M3GridManager.Instance.CreateGridMap();

            M3GameManager.Instance.venomManager = new M3VenomManager();
            M3GameManager.Instance.fishManager = new M3FishManager();

            EliminateManager.Instance.Init();

            M3Supporter.Instance.CheckMoveStateEnter();

            M3GameManager.Instance.soundManager = new M3SoundManager();

            M3GameManager.Instance.gameUI = new M3GameUIManager();
            M3GameManager.Instance.gameUI.EnterGame();

            M3GameManager.Instance.SetCurrentBackDate();
            M3GameManager.Instance.SetFirstBackDate();
        }

    }
}
