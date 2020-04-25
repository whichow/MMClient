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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Game.Build;

namespace Game.UI
{
	public class DecoratetHandBookWindowItem : KUIItem, IPointerClickHandler
    {
        #region Field
		private KUIImage _img_icon;
		private Text _txt_infoName;
        private Text _txt_num;
        private GameObject _go_openPanel;
        private GameObject _go_unlockedPanel;

        private KItemBuilding _buildingData;
        private KHandBookManager.HandBookConfiger _hbData;

        private Text _txt_openCharm;
        private Text _txt_unlockedCharm;
        #endregion

        #region Unity
        private void Awake()
		{
			_img_icon = Find<KUIImage>("img_icon");
			_txt_infoName = Find<Text>("Text");
            _txt_num = Find<Text>("open/Shu/Text");
            _go_openPanel = Find<Transform>("open").gameObject;
            _go_unlockedPanel = Find<Transform>("unlocked").gameObject;

            _txt_openCharm = Find<Text>("open/Shu/Icon/Text (1)");
            _txt_unlockedCharm = Find<Text>("unlocked/Icon/Text (1)");
        }
		#endregion

        #region Method
		public void ShowCat(KHandBookManager.HandBookConfiger[] kcats, int index)
        {
            if (kcats == null || kcats.Length == 0)
            {
                return;
            }
            _hbData = kcats[index];
            _buildingData = KItemManager.Instance.GetBuilding(_hbData.id);
			_txt_infoName.text = KLocalization.GetLocalString (_buildingData.itemName);
            _txt_openCharm.text = _buildingData.charm.ToString();
            //_txt_unlockedCharm.text = _buildingData.charm.ToString();
            _img_icon.overrideSprite = KIconManager.Instance.GetBuildingIcon(_buildingData.iconName);
            SetSelect(Convert.ToBoolean(_hbData.learned));
            _txt_num.text = "数量：" + (BuildingManager.Instance.BuildingTypeCountGet(_hbData.id) + KItemManager.Instance.GetBuilding(_hbData.id).curCount).ToString();
        }
        #endregion

        #region Interface
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            UI.ItemGetPath.ShowGetPath(_buildingData.itemID, typeof(KItemBuilding));
        }
        public void SetSelect(bool visable) {
            _go_openPanel.SetActive(visable);
            _go_unlockedPanel.SetActive(!visable);
        }
        private void RefreshRight() {
        }
        #endregion
    }
}

