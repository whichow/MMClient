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
	public class SingleSuiteHandBookWindowItem : KUIItem, IPointerClickHandler
    {
        #region Field
		private KUIImage _img_icon;
        private Text _txt_infoName;
        private GameObject _go_get;

        private KItemBuilding _hbData;
        [HideInInspector]
        public int _mailID { get; private set; }
        #endregion

        #region Unity
        private void Awake()
		{
            _img_icon = Find<KUIImage>("Item/Icon");
            _txt_infoName = Find<Text>("Item/Text");
            _go_get = Find<Transform>("Item/Image").gameObject;
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
		#endregion

        #region Method
		public void ShowBuilding(KItemBuilding[] kbuildings, int index)
        {
            if (kbuildings == null || kbuildings.Length == 0)
            {
                return;
            }
            _hbData = kbuildings[index];
            _txt_infoName.text = _hbData.displayName;
            _img_icon.overrideSprite = KIconManager.Instance.GetBuildingIcon(_hbData.iconName);
           
            _go_get.SetActive(KHandBookManager.Instance.HandBookConfigDictionary[_hbData.itemID].learned == 0);
            //_img_icon_shadow.gameObject.SetActive(!(_hbData.learned == 1));
        }
        #endregion

        #region Interface
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            UI.ItemGetPath.ShowGetPath(_hbData.itemID, typeof(KItemBuilding));
        }
        #endregion
    }
}

