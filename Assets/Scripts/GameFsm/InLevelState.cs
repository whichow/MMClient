// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "InLevelState" company=""></copyright>
// <summary></summary>
// ***********************************************************************
namespace Game
{
    using K.Fsm;
    public class InLevelState : GameState
    {
        public const int kFinishLevel = 1000;

        protected override void OnInit(IFsm<GameFsm> fsm)
        {
            SubscribeEvent(kFinishLevel, OnFinishLevel);
        }

        protected override void OnEnter(IFsm<GameFsm> fsm)
        {
            //KLevelManager.Instance.StartLevel();
        }

        protected override void OnExit(IFsm<GameFsm> fsm)
        {

        }

        private void OnFinishLevel(IFsm<GameFsm> fsm, object sender, object userData)
        {
            fsm.SetData("level", userData);
            ChangeState<InGameState>(fsm);
        }
    }
}

