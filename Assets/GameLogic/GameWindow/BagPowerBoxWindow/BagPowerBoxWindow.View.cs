using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Game.UI
{
    public partial class BagPowerBoxWindow
    {

        private Button _btnBack;
        private UIButtonExtension _btnPlus;
        private UIButtonExtension _btnMinus;
        private Button _btnMax;

        private Text _textName;
        private Text _textNum;
        private Text _textCurNum;
        private Image _imageIcon;
        private Button _btnOk;
        private int num=1;



        public void InitView()
        {
            _btnBack = Find<Button>("Close");
            _btnBack.onClick.AddListener(OnBackBtnClick);

            _btnPlus = Find<UIButtonExtension>("ImageBack/Choose/ButtonPlus");
            _btnPlus.onClick.AddListener(OnPlusClick);
            _btnPlus.onLongClick.AddListener(OnLongPlusClick);

            _btnMinus = Find<UIButtonExtension>("ImageBack/Choose/ButtonMinus");
            _btnMinus.onClick.AddListener(OnMinusClick);
            _btnMinus.onLongClick.AddListener(OnLongMinusClick);

            _btnMax = Find<Button>("ImageBack/Choose/ButtonMax");
            _btnMax.onClick.AddListener(OnMaxClick);

            _textNum = Find<Text>("ImageBack/Choose/Text");
            _textName = Find<Text>("ImageBack/Text");
            _textCurNum = Find<Text>("ImageBack/Have/Text");
            _imageIcon = Find<Image>("ImageBack/ItemBack/Item");
            _btnOk = Find<Button>("ImageBack/ButtonOK");
            _btnOk.onClick.AddListener(OnBtnOkClick);


        }

        public override void OnDisable()
        {
            base.OnDisable();
            num = 1;
        }

        public void RefreshView()
        {
 
            if (_bagPowerBoxData.itemdata.curCount==0)
            {
                num = 0;
            }
            _textNum.text = num +"/"+ _bagPowerBoxData.itemdata.curCount;
            if (_bagPowerBoxData.itemdata != null)
            {
                _imageIcon.overrideSprite = KIconManager.Instance.GetItemIcon(_bagPowerBoxData.itemdata.itemID);
                _textCurNum.text = _bagPowerBoxData.itemdata.curCount.ToString();
                _textName.text = _bagPowerBoxData.itemdata.itemName;
            }
        }
    }
}
