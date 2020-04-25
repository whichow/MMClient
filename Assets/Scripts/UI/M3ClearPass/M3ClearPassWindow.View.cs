
using UnityEngine;
/** 
*FileName:     M3ClearPassWindow.View.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-11-13 
*Description:    
*History: 
*/
using UnityEngine.UI;
using K.Extension;
namespace Game.UI
{
    partial class M3ClearPassWindow
    {
        #region Field

        private Button _btnClose;
        private Button _btnInviationFriends;
        private Image _imageStart;
        private Text _textStartNum;
        private Transform _transAlreadyStartNum;
        private Button _btnGetStart;
        private Text _textUnlockTime;
        private Button _btnUnlock;
        private Text _textUnlockDiamods;
        private Text _textNowUnlock;
        private Transform _transFriends;
        private Transform _transAlreadyTime;

        private Image _imageTime;
        #endregion

        #region Method

        public void InitView()
        {
            _btnClose = Find<Button>("Panel/Close");
            _btnClose.onClick.AddListener(OnCloseBtnClick);
            _btnInviationFriends = Find<Button>("Panel/BlackBack/Up/Button");
            _btnInviationFriends.onClick.AddListener(OnInviationBtnClick);
            _imageStart = Find<Image>("Panel/BlackBack/Down/Item/Image/Tiao");
            _textStartNum = Find<Text>("Panel/BlackBack/Down/Item/Image/Tiao/Text");
            _transAlreadyStartNum = Find<Transform>("Panel/BlackBack/Down/Item/Text");
            _btnGetStart = Find<Button>("Panel/BlackBack/Down/Item/Button1");
            _btnGetStart.onClick.AddListener(OnGetStartBtnClick);
            _textUnlockTime = Find<Text>("Panel/BlackBack/Down/Item1/Image/Tiao/Text");
            _btnUnlock = Find<Button>("Panel/BlackBack/Down/Item1/Button");
            _btnUnlock.onClick.AddListener(OnUnlockBtnClick);
            _textUnlockDiamods = Find<Text>("Panel/BlackBack/Down/Item1/Button/Icon/Text");
            _textNowUnlock = Find<Text>("Panel/BlackBack/Up/Button/Text");
            _transFriends = Find<Transform>("Panel/BlackBack/Up/Friends");
            _transAlreadyTime = Find<Transform>("Panel/BlackBack/Down/Item1/Text");
            _imageTime = Find<Image>("Panel/BlackBack/Down/Item1/Image/Tiao");
        }

        public void RefreshView()
        {


            RefreshBtn();
            _textStartNum.text = _clearPassData.nowStartNum + "/" + _clearPassData.needStartNum;
            _imageStart.fillAmount = _clearPassData.nowStartNum / _clearPassData.needStartNum;
            if (_clearPassData.nowStartNum >= _clearPassData.needStartNum)
            {
                _transAlreadyStartNum.gameObject.SetActive(true);
                _btnGetStart.gameObject.SetActive(false);
            }
            else
            {
                _transAlreadyStartNum.gameObject.SetActive(false);
                _btnGetStart.gameObject.SetActive(true);
            }
            _textUnlockDiamods.text = LevelDataModel.Instance.SpeedUpMoneyCost.ToString();
            if (LevelDataModel.Instance.CurUnlockChapterID != 0 && LevelDataModel.Instance.CurUnlockChapterTime <= 0)
            {
                _transAlreadyTime.gameObject.SetActive(true);
                _btnUnlock.gameObject.SetActive(false);
            }
            else
            {
                _transAlreadyTime.gameObject.SetActive(false);
                _btnUnlock.gameObject.SetActive(true);
            }


        }
        private void RefreshBtn()
        {
            if ((LevelDataModel.Instance.CurUnlockChapterID != 0 && LevelDataModel.Instance.CurUnlockChapterTime <= 0) || _clearPassData.nowStartNum >= _clearPassData.needStartNum)
            {
                _textNowUnlock.text = "立即解锁";
                _textNowUnlock.gameObject.SetActive(true);
                _transFriends.gameObject.SetActive(false);
                _btnInviationFriends.onClick.RemoveAllListeners();
                _btnInviationFriends.onClick.AddListener(OnNowUnlockBtnClick);
            }
            else
            {
                _textNowUnlock.text = "请好友帮忙";
                _textNowUnlock.gameObject.SetActive(false);
                _transFriends.gameObject.SetActive(true);
                _btnInviationFriends.onClick.RemoveAllListeners();
                _btnInviationFriends.onClick.AddListener(OnInviationBtnClick);
            }
        }



        #endregion

        public override void UpdatePerSecond()
        {
            if (nowChapter != null&& LevelDataModel.Instance.CurUnlockChapterID != 0)
            {
                _textUnlockTime.text = TimeExtension.ToTimeString(LevelDataModel.Instance.CurUnlockChapterTime) + "后解锁";
                _imageTime.fillAmount = (float)LevelDataModel.Instance.CurUnlockChapterTime / (float)nowChapter.UnlockTime;
                if (LevelDataModel.Instance.CurUnlockChapterTime < 0)
                {
                    RefreshBtn();
                }
            }

        }

    }
}

