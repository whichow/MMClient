
using System;
/** 
*FileName:     StopWindow.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2018-01-15 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class StopWindow
    {
        private Button closeBtn;
        private Button continueBtn;
        private Button quitBtn;
        private KUIList goalListParent;



        public void InitView()
        {
            closeBtn = Find<Button>("Close");
            continueBtn = Find<Button>("ButtonReplay");
            quitBtn = Find<Button>("ButtonQuit");
            goalListParent = Find<KUIList>("ImageBack/Grid");

            closeBtn.onClick.AddListener(OnCloseBtnClick);
            continueBtn.onClick.AddListener(OnContinueBtnClick);
            quitBtn.onClick.AddListener(OnQuitBtnClick);
            goalListParent.itemTemplate.AddComponent<StopWindowTargetItem>();
        }

        public void RefreshView()
        {
            goalListParent.Clear();
            foreach (var item in windowData.targetDic)
            {
                var tmp = goalListParent.GetItem() as StopWindowTargetItem;
                tmp.ShowInfo(item.Key, item.Value);
            }
        }
        private void OnQuitBtnClick()
        {
            CloseWindow(this);
            KUIWindow.GetWindow<GamePauseWindow>().QuitGame();
        }

        private void OnContinueBtnClick()
        {
            CloseWindow(this);
            KUIWindow.GetWindow<GamePauseWindow>().ContinueGame();
        }

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
            KUIWindow.GetWindow<GamePauseWindow>().ContinueGame();
        }
    }
}
