using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class BagPowerBoxWindow : KUIWindow
    {
        #region Field


        #endregion

        #region Constructor

        public BagPowerBoxWindow() :
            base(UILayer.kNormal, UIMode.kSequence)
        {
            uiPath = "BagPowerBox";
        }

        #endregion

        #region Method

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
        private void OnBackBtnClick()
        {
            if (_bagPowerBoxData.onCancel != null)
            {
                K.Events.EventInvoker.Invoke(_bagPowerBoxData.onCancel);
            }
            CloseWindow(this);
        }
        private void OnPlusClick()
        {

            if (num < _bagPowerBoxData.itemdata.curCount)
            {
                num++;
                RefreshView();
            }
            else
            {
                Debug.Log("达到最大");
            }

        }
        private void OnLongPlusClick()
        {
            if (num < _bagPowerBoxData.itemdata.curCount)
            {
                Debug.Log("长按增加");
                num++;
                RefreshView();
            }

        }
        private void OnMinusClick()
        {
            if (num > 1)
            {
                num--;
                RefreshView();
            }
            else
            {
                num = 1;
                RefreshView();
                Debug.Log("最少为1");
            }
        }
        private void OnLongMinusClick()
        {
            if (num > 1)
            {
                Debug.Log("长按减少");
                num--;
                RefreshView();
            }
            else
            {
                num = 1;
                RefreshView();
            }

        }
        private void OnMaxClick()
        {
            num = _bagPowerBoxData.itemdata.curCount;
            RefreshView();
        }
        private void OnBtnOkClick()
        {
            KUser.UseItems(_bagPowerBoxData.itemdata.itemID, num, OkBtnCallBack);

        }
        private void OkBtnCallBack(int code, string message, object data)
        {
            if (code == 0)
            {
                var window = GetWindow<BagWindow>();
                if (window != null)
                    window.RefreshView(window._bagType);
                num = 1;
                RefreshView();
                if (_bagPowerBoxData.onConfirm != null)
                {
                    K.Events.EventInvoker.Invoke(_bagPowerBoxData.onConfirm);
                }
                CloseWindow(this);
            }
        }
        #endregion
    }
}
