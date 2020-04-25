// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "GameInState" company=""></copyright>
// <summary></summary>
// ***********************************************************************
namespace Game
{
    using K.Fsm;
    public class InGameState : GameState
    {
        public const int kStartLevel = 100;

        private int _delay;

        protected override void OnInit(IFsm<GameFsm> fsm)
        {
            SubscribeEvent(kStartLevel, OnStartLevel);
        }

        protected override void OnEnter(IFsm<GameFsm> fsm)
        {
            _delay = 3;
            KLaunch.LoadLevel("buildingScene");
        }

        protected override void OnExit(IFsm<GameFsm> fsm)
        {
        }

        protected override void Update(IFsm<GameFsm> fsm, float delta)
        {
            if (_delay-- < 0)
            {
                var levelData = fsm.GetData("level");
                if (levelData != null)
                {
                    fsm.RemoveData("level");
                    KUIWindow.OpenWindow<Game.UI.MapSelectWindow>(levelData);
                }
            }
        }

        private void OnStartLevel(IFsm<GameFsm> fsm, object sender, object userData)
        {
            ChangeState<InLevelState>(fsm);
        }
    }
}

