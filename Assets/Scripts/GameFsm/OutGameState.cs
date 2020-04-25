// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "OutGameState" company=""></copyright>
// <summary></summary>
// ***********************************************************************
namespace Game
{
    using K.Fsm;

    public class OutGameState : GameState
    {
        protected override void OnEnter(IFsm<GameFsm> fsm)
        {
            KUser.PullBaseInfo(OnPullBaseInfoCallback);
            //KUser.PullItemInfo(OnPullItemInfoCallback);
            GameApp.Instance.GameServer.ReqGetItemInfos();
            KUser.PullBuildingInfo(OnPullBuildingInfoCallback);
            Debuger.Log("----------LoadLevel-buildingScene");
            KLaunch.LoadLevel("buildingScene");
        }

        private void OnPullBaseInfoCallback(int code, string message, object data)
        {

        }
        private void OnPullItemInfoCallback(int code, string message, object data)
        {

        }
        private void OnPullBuildingInfoCallback(int code, string message, object data)
        {

        }
    }
}

