// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CheckState" company=""></copyright>
// <summary></summary>
// ***********************************************************************
namespace Game
{
    using K.Fsm;

    public class InitState : GameState
    {
        protected override void OnEnter(IFsm<GameFsm> fsm)
        {
            KAssetManager.Instance.Initialize();
        }

        protected override void OnExit(IFsm<GameFsm> fsm)
        {
            //KLaunch.LoadLevel("loginScene");
            KLaunch.LoadLevel(KLaunch.MainScene);
        }

        protected override void Update(IFsm<GameFsm> fsm, float delta)
        {
            base.Update(fsm, delta);
            if (KAssetManager.Instance.initialized)
            {
                if (KLaunch.MainScene == "loginScene")
                {
                    ChangeState<CheckState>(fsm);
                }
                else
                {
                    KAssetManager.Instance.LoadTables();
                    KGameModuleManager.Instance.Load();
                    ChangeState<M3EditorState>(fsm);
                }
            }
        }
    }
}

