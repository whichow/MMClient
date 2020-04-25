/** 
*FileName:     M3ClearPassWindow.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-11-13 
*Description:    
*History: 
*/
using UnityEngine;

namespace Game.UI
{
    public partial class M3ClearPassWindow : KUIWindow
    {
        #region Constructor

        public M3ClearPassWindow() : base(UILayer.kNormal, UIMode.kNone)
        {
            uiPath = "ClearPass";
        }

        #endregion

        #region override  

        public override void Awake()
        {
            InitModel();
            InitView();
        }

        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
        }

        public override void AddEvents()
        {
            base.AddEvents();
            LevelDataModel.Instance.AddEvent(LevelEvent.CHAPTER_UNLOCKED, OnChapterUnlockedHandler);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            LevelDataModel.Instance.RemoveEvent(LevelEvent.CHAPTER_UNLOCKED, OnChapterUnlockedHandler);
        }

        #endregion

        #region Action

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        private void OnInviationBtnClick()
        {
            Debug.Log("邀请好友");
        }

        private void OnGetStartBtnClick()
        {
            OpenWindow<HaveStarWindow>();
        }

        private void OnUnlockBtnClick()
        {
            if (PlayerDataModel.Instance.mPlayerData.mDiamond >= LevelDataModel.Instance.SpeedUpMoneyCost)
            {
                GameApp.Instance.GameServer.ReqChapterUnlock(2, _clearPassData.charpterId, null);
            }
            else
            {
                CloseWindow(this);
                OpenWindow<ShopWindow>(ShopIDConst.Special);
            }
        }

        private void OnNowUnlockBtnClick()
        {
            if (_clearPassData.nowStartNum >= _clearPassData.needStartNum)
            {
                GameApp.Instance.GameServer.ReqChapterUnlock(1, _clearPassData.charpterId, null);
            }
        }

        #endregion

        private void OnChapterUnlockedHandler(IEventData args)
        {
            CloseWindow(this);
            _clearPassData.unLockCallBack?.Invoke();
        }

    }
}

