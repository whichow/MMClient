
using Game;
using System;
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
using UnityEngine.UI;

namespace Game.UI
{
    public partial class GamePauseWindow
    {


        public Button backBtn;
        public Button continueBtn;
        public Button reLoadBtn;
        public Button quitBtn;
        private KUISwitch _buttonMusic;
        private KUISwitch _buttonSound;

        public void InitView()
        {
            backBtn = transform.Find("Close").GetComponent<Button>();
            continueBtn = transform.Find("ButtonPlay").GetComponent<Button>();
            reLoadBtn = transform.Find("ButtonReplay").GetComponent<Button>();
            quitBtn = transform.Find("ButtonQuit").GetComponent<Button>();
            quitBtn.onClick.AddListener(OnQuitBtnClick);
            continueBtn.onClick.AddListener(OnContinueClick);
            reLoadBtn.onClick.AddListener(OnReloadCLick);
            backBtn.onClick.AddListener(OnContinueClick);
            _buttonMusic = Find<KUISwitch>("MusicSwitch");
            _buttonMusic.onValueChanged.AddListener(OnMusicBtnClick);
            _buttonSound = Find<KUISwitch>("AudioSwitch");
            _buttonSound.onValueChanged.AddListener(OnSoundBtnClick);
        }


        public void RefreshView()
        {
            RefreshMusic(KSoundManager.MusicEnable);
            RefreshSound(KSoundManager.AudioEnable);
        }
        private void RefreshMusic(bool isMusic)
        {
            //Debug.Log("音乐开关" + isMusic);
            _buttonMusic.isOn = isMusic;
        }
        private void RefreshSound(bool isSound)
        {
            //Debug.Log("音效开关" + isSound);
            _buttonSound.isOn = isSound;
        }
    }
}