/** 
*FileName:     M3ClearPassWindow.Model.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-11-13 
*Description:    
*History: 
*/
using Game.DataModel;
using System;

namespace Game.UI
{
    partial class M3ClearPassWindow
    {
        #region WindowData

        public class Data
        {
            public float nowStartNum;
            public float needStartNum;
            public int charpterId;

            public Action unLockCallBack;
        }

        #endregion

        #region Field


        private Data _clearPassData;

        private ChapterUnlockXDM nowChapter;
        #endregion

        #region Method

        public void InitModel()
        {
            _clearPassData = new Data();
         
        }

        private void OnTiemLockBack(int arg1, string arg2, object arg3)
        {
            UnityEngine.Debug.Log(arg1 + "|" + arg2);
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData != null)
            {
                _clearPassData.nowStartNum = passData.nowStartNum;
                _clearPassData.needStartNum = passData.needStartNum;
                _clearPassData.charpterId = passData.charpterId;
                _clearPassData.unLockCallBack = passData.unLockCallBack;
            }
            else
            {
                _clearPassData.nowStartNum = 0f;
                _clearPassData.needStartNum = 0f;
                _clearPassData.charpterId = 0;
                _clearPassData.unLockCallBack = passData.unLockCallBack;
            }
            nowChapter = LevelDataModel.Instance.GetChapterByLevelID(LevelDataModel.Instance.CurrMaxLevelId);
            //if (nowChapter.unlockRemainTime < 0)
            //{
            //    KUser.ChapterUnlock(0, _clearPassData.charpterId, null, OnTiemLockBack);
            //}
            //Debug.Log(nowChapter.unlockRemainTime);
        }

        #endregion
    }
}

