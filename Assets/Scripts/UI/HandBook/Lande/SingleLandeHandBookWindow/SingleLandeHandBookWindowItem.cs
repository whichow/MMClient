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
using Game.Build;

namespace Game.UI
{
	public class SingleLandeHandBookWindowItem : KUIItem, IPointerClickHandler
    {
        #region Field
		private KUIImage _img_icon;
        private Text _txt_openName;
        private Text _txt_unlockedName;
        private Text _txt_openCharm;
        private Text _txt_unlockedCharm;
        private Text _txt_num;

        private KItemBuilding _bdData;
        private KHandBookManager.HandBookConfiger _hbData;

        private GameObject _go_open;
        private GameObject _go_unlocked;
        #endregion

        #region Unity
        private void Awake()
		{
            _img_icon = Find<KUIImage>("img_icon");
            _txt_openName = Find<Text>("Open/Name");
            _txt_unlockedName = Find<Text>("Unlocked/Name");
            _txt_openCharm = Find<Text>("Unlocked/Icon/Text (1)");
            _txt_unlockedCharm = Find<Text>("Open/Shu/Icon/Text (1)");
            _txt_num = Find<Text>("Open/Shu/Text");
            _go_open = Find<Transform>("Open").gameObject;
            _go_unlocked = Find<Transform>("Unlocked").gameObject;
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
		#endregion

        #region Method
		public void ShowBuilding(KHandBookManager.HandBookConfiger[] kbuildings, int index)
        {
            if (kbuildings == null || kbuildings.Length == 0)
            {
                return;
            }
            _hbData = kbuildings[index];
            _bdData = KItemManager.Instance.GetBuilding(_hbData.id);
            _txt_openName.text = _hbData.name;
            _txt_unlockedName.text = _hbData.name;
            _txt_openCharm.text = _bdData.charm.ToString();
            _txt_unlockedCharm.text = _bdData.charm.ToString();
            _img_icon.overrideSprite = KIconManager.Instance.GetBuildingIcon(_bdData.iconName);
            if (_hbData.learned == 1)
            {
                _go_open.SetActive(true);
                _go_unlocked.SetActive(false);
                _txt_num.text = (KItemManager.Instance.GetBuilding(_hbData.id).curCount + BuildingManager.Instance.BuildingTypeCountGet(_hbData.id)).ToString();
            }
            else {
                _go_open.SetActive(false);
                _go_unlocked.SetActive(true);
            }
        }
        #endregion

        #region Interface
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            UI.ItemGetPath.ShowGetPath(_bdData.itemID, typeof(KItemBuilding));
        }
        #endregion
    }
}

