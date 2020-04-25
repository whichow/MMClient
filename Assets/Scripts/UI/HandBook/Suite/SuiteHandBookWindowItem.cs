// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CatBagItem" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.UI
{
	public class SuiteHandBookWindowItem : KUIItem, IPointerClickHandler
    {
        #region Field
		private KUIImage _img_icon;
        private KUIImage _img_icon_shadow;
        private Text _txt_infoName;
        private GameObject _go_infoPanel;
        private GameObject _go_btnPanel;

        private KItemSuit _suitData;
        private KHandBookManager.HandBookConfiger _hbData;
        [HideInInspector]
        public int _mailID { get; private set; }
        #endregion

        #region Unity
        private void Awake()
		{
            _img_icon = Find<KUIImage>("img_icon");
            _txt_infoName = Find<Text>("Panel/Text");
            _img_icon_shadow = Find<KUIImage>("img_icon_shadow");
        }
		#endregion

        #region Method
		public void ShowSuit(KHandBookManager.HandBookConfiger[] kcats, int index)
        {
            //SetSelect(false);
            if (kcats == null || kcats.Length == 0)
            {
                return;
            }
            _hbData = kcats[index];
            _suitData = KItemManager.Instance.GetSuit(_hbData.id);
            _txt_infoName.text = _suitData.itemName;
            SetIcon();
            Debug.Log("当前ID：" + _hbData.id + "获取状态：" + _hbData.learned);
            _img_icon_shadow.gameObject.SetActive(!(_hbData.learned == 1));
        }
        #endregion

        #region Interface
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            KUIWindow.OpenWindow<SingleSuiteHandBookWindow>(_suitData);
        }
        private void SetIcon() {
            _img_icon.overrideSprite = KIconManager.Instance.GetBackGroundIcon(_suitData.iconName);
            _img_icon_shadow.overrideSprite = KIconManager.Instance.GetBackGroundIcon(_suitData.iconName);
        }
        #endregion
    }
}

