using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public partial class SettingWindow
    {
        private UIList _languangeList;
        private GameObject _languangeObj;
        private Image _languangeImg;
        private Text _lang;
        private Button _languangeClose;
        private Button _close;
        private KUISwitch _buttonMusic;
        private KUISwitch _buttonSound;
        private Button _buttonSetLanguange;
        private Button _buttonUse;
        private Button _buttonYsz;
        private Button _buttonPushInfo;
        private Image _imageMusic;
        private Image _imageSound;

        public void InitView()
        {
            _languangeList = Find<UIList>("SetLanguage/List");
            _languangeList.SetRenderHandler(RenderHandler);
            _languangeList.SetPointerHandler(PointerHandler);
            _languangeObj = Find("SetLanguage");
            _languangeClose = Find<Button>("SetLanguage/Close");
            _languangeClose.onClick.AddListener(OnCloseLanguange);
            _close = Find<Button>("SettingBg/Close");
            _close.onClick.AddListener(OnBackBtnClick);
            _buttonMusic = Find<KUISwitch>("SettingBg/TextSound/MusicSwitch");
            _buttonMusic.onValueChanged.AddListener(OnMusicBtnClick);
            _buttonSound = Find<KUISwitch>("SettingBg/TextSound/AudioSwitch");
            _buttonSound.onValueChanged.AddListener(OnSoundBtnClick);
            _buttonSetLanguange = Find<Button>("SettingBg/TextLang/banner");
            _buttonSetLanguange.onClick.AddListener(OnSetLanguangeBtnClick);
            _buttonUse = Find<Button>("SettingBg/TextOther/UseItem");
            _buttonUse.onClick.AddListener(OnUseBtnClick);
            _buttonYsz = Find<Button>("SettingBg/TextOther/Ysz");
            _buttonYsz.onClick.AddListener(OnYszBtnClick);
            _imageMusic = Find<Image>("SettingBg/TextSound/MusicSwitch/Background");
            _imageSound = Find<Image>("SettingBg/TextSound/AudioSwitch/Background");
            _languangeImg = Find<Image>("SettingBg/TextLang/banner");
            _lang = Find<Text>("SettingBg/TextLang/LangBack/Lang");

            _buttonPushInfo = Find<Button>("SettingBg/TextOther/Other_01");
            _buttonPushInfo.onClick.AddListener(PushInformation);

            _languangeObj.SetActive(false);
        }

        private void PointerHandler(UIListItem item, int index)
        {
            KLanguage vo = item.dataSource as KLanguage;
            if (vo == null)
                return;
            KLanguageManager.Instance.SetLanguage(vo.name);
            _languangeImg.sprite = KIconManager.Instance.GetItemIcon(KLanguageManager.Instance.currentLanguage.iconName);
            _languangeList.DataArray = KLanguageManager.Instance.allLanguages;
            _lang.text = KLocalization.GetLocalString(KLanguageManager.Instance.currentLanguage.name);
        }

        private void RenderHandler(UIListItem item, int index)
        {
            KLanguage vo = item.dataSource as KLanguage;
            if (vo == null)
                return;
            item.GetComp<Text>("Text").text = KLocalization.GetLocalString(vo.name);
            item.GetComp<Image>("Image").overrideSprite = KIconManager.Instance.GetItemIcon(vo.iconName);
        }

        private void OnMusicBtnClick(bool isOn)
        {
            KSoundManager.MusicEnable = isOn;
            if (isOn == false)
                _imageMusic.material = Resources.Load<Material>("Materials/UIGray");
            else
                _imageMusic.material = null;
        }

        private void OnSoundBtnClick(bool isOn)
        {
            KSoundManager.AudioEnable = isOn;
            if (isOn == false)
                _imageSound.material = Resources.Load<Material>("Materials/UIGray");
            else
                _imageSound.material = null;
        }

        private void OnSetLanguangeBtnClick()
        {
            _languangeObj.SetActive(true);
            _languangeList.DataArray = KLanguageManager.Instance.allLanguages;
        }

        private void OnCloseLanguange()
        {
            _languangeObj.SetActive(false);
        }

        private void OnUseBtnClick()
        {
            Debug.Log("使用条款");
        }

        private void OnYszBtnClick()
        {
            Debug.Log("隐私政策");
        }

        private void PushInformation()
        {
            Debug.Log("推送消息开关");
            //OpenWindow<SettingPushWindow>();
        }

        public void RefreshView()
        {
            _languangeImg.sprite = KIconManager.Instance.GetItemIcon(KLanguageManager.Instance.currentLanguage.iconName);
            RefreshMusic(KSoundManager.MusicEnable);
            RefreshSound(KSoundManager.AudioEnable);
            _lang.text = KLocalization.GetLocalString(KLanguageManager.Instance.currentLanguage.name);
        }

        private void RefreshMusic(bool isMusic)
        {
            Debug.Log("音乐开关" + isMusic);
            if (isMusic == false)
                _imageMusic.material = Resources.Load<Material>("Materials/UIGray");
            else
                _imageMusic.material = null;
            _buttonMusic.isOn = isMusic;

        }

        private void RefreshSound(bool isSound)
        {
            Debug.Log("音效开关" + isSound);
            if (isSound == false)
                _imageSound.material = Resources.Load<Material>("Materials/UIGray");
            else
                _imageSound.material = null;
            _buttonSound.isOn = isSound;
        }

    }
}
