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
    partial class SuiteHandBookWindow
    {
        #region Field
        //private GameObject _go_singleSuite;

        private KUIItemPool _layoutElementPool;
        private Text _txt_first;
        private Button _btn_close;
        private Text _txt_totalNum;
        #endregion

        #region Method

        public void InitView()
        {
            //_go_singleSuite = Find<Transform>("single_suite").gameObject;

            _layoutElementPool = Find<KUIItemPool>("go_suite/Scroll View");
            if (_layoutElementPool && _layoutElementPool.elementTemplate)
            {
                _layoutElementPool.elementTemplate.gameObject.AddComponent<SuiteHandBookWindowItem>();
            }
            _btn_close = Find<Button>("close");
            _btn_close.onClick.AddListener(this.CloseUI);
            _txt_totalNum = Find<Text>("go_suite/NumBack/Text");
        }

        public void RefreshView()
        {
            StartCoroutine(FillElements());
        }

        private IEnumerator FillElements()
        {
            _layoutElementPool.Clear();
            var Catdata = GetTorAData();
            for (int i = 0; i < Catdata.Length; i++)
            {
                var element = _layoutElementPool.SpawnElement();
                var catItem = element.GetComponent<SuiteHandBookWindowItem>();
                catItem.ShowSuit(Catdata, i);
            }
            yield return null;
            RefreshTotalNum(Catdata);
        }

        private void RefreshTotalNum(KHandBookManager.HandBookConfiger[] dt)
        {
            int getNum = 0;
            for (int i = 0; i < dt.Length; i++)
            {
                if (dt[i].learned == 1)
                {
                    getNum++;
                }
            }
            _txt_totalNum.text = "已激活：" + getNum.ToString() + "/" + dt.Length.ToString();
        }
        #endregion
    }
}

