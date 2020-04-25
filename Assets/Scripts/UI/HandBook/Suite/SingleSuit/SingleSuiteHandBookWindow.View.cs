// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MessageBox.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    partial class SingleSuiteHandBookWindow
    {
        #region Field
        private Button _btn_close;

        private KUIImage _img_suitIcon;
        private Text _txt_suitName;
        private Text _txt_suitDesc;



        private KUIItemPool _layoutElementPool;
        private Text _txt_first;
        #endregion

        #region Method

        public void InitView()
        {
            _img_suitIcon = Find<KUIImage>("single_suite/img_suit");
            _txt_suitName = Find<Text>("single_suite/txt_title");
            _txt_suitDesc = Find<Text>("single_suite/txt_title/Text");


            _layoutElementPool = Find<KUIItemPool>("single_suite/Scroll View");
            if (_layoutElementPool && _layoutElementPool.elementTemplate)
            {
                _layoutElementPool.elementTemplate.gameObject.AddComponent<SingleSuiteHandBookWindowItem>();
            }
            _btn_close = Find<Button>("single_suite/close");
            _btn_close.onClick.AddListener(this.CloseUI);
        }

        public void RefreshView()
        {
            _img_suitIcon.overrideSprite = KIconManager.Instance.GetBackGroundIcon(_suitData.iconName);
            _txt_suitName.text = _suitData.displayName;
            _txt_suitDesc.text = _suitData.description;

            StartCoroutine(FillElements());
        }

        private IEnumerator FillElements()
        {
            _layoutElementPool.Clear();
            var Catdata = GetSuitDatas();
            for (int i = 0; i < Catdata.Length; i++)
            {
                var element = _layoutElementPool.SpawnElement();
                var catItem = element.GetComponent<SingleSuiteHandBookWindowItem>();
                catItem.ShowBuilding(Catdata, i);
                if (i > 7)
                    yield return null;                
            }
            yield return null;
        }
        #endregion
    }
}

