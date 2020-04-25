// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "FormulaShopWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
namespace Game.UI
{
    public partial class OrnamentShopWindow : KUIWindow
    {
        #region Constructor

        public OrnamentShopWindow()
            : base(UILayer.kNormal, UIMode.kSequenceRemove)
        {
            uiPath = "OrnamentShop";
        }

        #endregion

        #region Method       

        #endregion

        #region Action

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        private void OnOkBtnClick()
        {
            //Debug.Log("确定");
            if (PlayerDataModel.Instance.mPlayerData.Vigour < 1)
            {
                //行动力不足
                UI.MessageBox.ShowMessage("提示", KLanguageManager.Instance.GetLocalString(57132));
                return;
            }

            if (shopItem == null)
            {
                if (GuideManager.Instance.IsGuideing)
                {
                    var item = _itemPool.GetElement(0).GetComponent<OrnamentShopItem>();
                    item.PointerClick();
                }
                else
                {
                    Debug.Log("请选择制造配方");
                    return;
                }
            }

            KWorkshop.Instance.Make(_ornamentShopData.indx, shopItem.itemID, OkCallBack);
        }
        private void OkCallBack(int code, string message, object data)
        {
            CloseWindow(this);
            if (_ornamentShopData.onConfirm != null)
            {
                K.Events.EventInvoker.Invoke(_ornamentShopData.onConfirm, code, message, data);
            }
        }


        private void OnPageChange(bool value)
        {
            var page = GetOnToggle();

            if (page == "Toggle2")
            {
                pageType = PageType.kTag2;
            }
            else if (page == "Toggle3")
            {
                pageType = PageType.kTag3;
            }
            else if (page == "Toggle4")
            {
                pageType = PageType.kTag4;
            }
            else if (page == "Toggle5")
            {
                pageType = PageType.kTag5;
            }
            else if (page == "Toggle6")
            {
                pageType = PageType.kTag6;
            }
            else
            {
                pageType = PageType.kAll;
            }

            if (isChanged)
            {
                RefreshView();
            }
        }

        #endregion

        #region Unity

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

        #endregion
    }
}

