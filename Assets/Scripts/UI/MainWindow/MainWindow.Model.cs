// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MainWindow.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Build;
namespace Game.UI
{
    partial class MainWindow
    {
        #region Data

        public string GetName()
        {
            return PlayerDataModel.Instance.mPlayerData.mName;
        }

        public float GetExpProgress()
        {
            float maxExp = KUser.SelfPlayer.maxExp;
            if (maxExp > 0f)
            {
                return PlayerDataModel.Instance.mPlayerData.mExp / maxExp;
            }
            else
            {
                return 1f;
            }
        }

        private BuildingManager.OtherPlayerInfo otherPlayerInfo;
        public BuildingManager.OtherPlayerInfo GetPlayerData()
        {
            otherPlayerInfo = BuildingManager.Instance.GetOtherPlayerInfo();
            return otherPlayerInfo;
        }

        public Sprite GetHeadIcon()
        {
            return KIconManager.Instance.GetHeadIcon(PlayerDataModel.Instance.mPlayerData.mHead);
        }

        public Sprite GetOtherHeadIcon()
        {
            return KIconManager.Instance.GetHeadIcon(otherPlayerInfo.PlayerHead);
        }

        #endregion
    }
}

