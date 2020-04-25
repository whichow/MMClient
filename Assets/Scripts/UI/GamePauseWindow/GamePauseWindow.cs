/** 
 *FileName:     GamePauseWindow.cs 
 *Author:       Hejunjie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-11-06 
 *Description:    
 *History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using Game;
using Game.Match3;
using System;

namespace Game.UI
{
    public partial class GamePauseWindow : KUIWindow
    {
        public GamePauseWindow() :base(UILayer.kPop, UIMode.kSequenceHide)
        {
            uiPath = "PauseWindow";
        }

        public override void Awake()
        {
            base.Awake();
            InitModel();
            InitView();
        }
        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
            PauseGame();
        }

        public override void AddEvents()
        {
            base.AddEvents();
            LevelDataModel.Instance.AddEvent(LevelEvent.LEVEL_FINISHED, OnLevelFinishedHandler);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            LevelDataModel.Instance.RemoveEvent(LevelEvent.LEVEL_FINISHED, OnLevelFinishedHandler);
        }

        private void OnLevelFinishedHandler(IEventData args)
        {
            ContinueGame();
            M3GameManager.Instance.OnExitGame(ExitGameType.Reload);
        }

        private void OnMusicBtnClick(bool isOn)
        {
            KSoundManager.MusicEnable = isOn;
        }

        private void OnSoundBtnClick(bool isOn)
        {
            KSoundManager.AudioEnable = isOn;
        }

        public void PauseGame()
        {
            M3GameManager.Instance.isPause = true;
            Time.timeScale = 0;
        }

        public void ContinueGame()
        {
            M3GameManager.Instance.isPause = false;
            Time.timeScale = 1;

        }

        private void OnContinueClick()
        {
            ContinueGame();
            CloseWindow(this);
        }

        private void OnReloadCLick()
        {
            if (M3Config.isEditor)
            {
                ContinueGame();
                M3GameManager.Instance.OnExitGame(ExitGameType.Reload);

            }
            else
            {
                M3GameManager.Instance.modeManager.SendGameOverMessage();
                //M3GameManager.Instance.modeManager.SendGameOverMessage(delegate (object data)
                //{
                //    ContinueGame();
                //    M3GameManager.Instance.OnExitGame(ExitGameType.Reload);
                //});
            }
        }

        private void OnQuitBtnClick()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            var targetDic = M3GameManager.Instance.modeManager.GameModeCtrl.targetDic;
            if (targetDic != null)
            {
                foreach (var item in targetDic)
                {
                    if (item.Value > 0)
                    {
                        dic.Add(item.Key, item.Value);
                    }
                }
            }

            KUIWindow.OpenWindow<StopWindow>(new StopWindow.StopWindowData() { targetDic = dic });
            CloseWindow(this);
            //QuitGame();
        }


        public void QuitGame()
        {
            ContinueGame();
            if (M3Config.isEditor)
            {
                M3Config.isEditor = false;
                M3GameManager.Instance.OnExitGame(ExitGameType.Editor);
            }
            else
            {
                M3Config.isEditor = false;
                M3GameManager.Instance.OnExitGame(ExitGameType.None);
            }
        }

    }
}