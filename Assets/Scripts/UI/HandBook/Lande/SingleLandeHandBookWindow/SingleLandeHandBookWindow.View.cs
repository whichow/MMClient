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
    partial class SingleLandeHandBookWindow
    {
        #region Field
        private Button _btn_close;

        private Text _txt_title;
        private Text _txt_totalNum;
        private Button _btn_get;

        private Image _img_btn;
        private Text _txt_btn;

        int getnum = 0;
        private KUIItemPool _layoutElementPool;
        #endregion

        #region Method

        public void InitView()
        {
            _btn_close = Find<Button>("close");
            _btn_close.onClick.AddListener(this.CloseUI);

            _txt_title = Find<Text>("single_lande/txt_title");
            _txt_totalNum = Find<Text>("single_lande/CatHave/txt_num");
            _btn_get = Find<Button>("single_lande/Button");

            _img_btn = Find<Image>("single_lande/Button");
            _txt_btn = Find<Text>("single_lande/Button/Text");

            _layoutElementPool = Find<KUIItemPool>("single_lande/Scroll View");
            if (_layoutElementPool && _layoutElementPool.elementTemplate)
            {
                _layoutElementPool.elementTemplate.gameObject.AddComponent<SingleLandeHandBookWindowItem>();
            }
        }

        public void RefreshView()
        {
            _txt_title.text = _data_suit.displayName;
            StartCoroutine(FillElements());
        }

        private IEnumerator FillElements()
        {
            _layoutElementPool.Clear();
            var Catdata = GetDatas();           
            for (int i = 0; i < Catdata.Length; i++)
            {
                if (Catdata[i].learned == 1)
                {
                    getnum++;
                }
            }
            _txt_totalNum.text = "激活总数：" + getnum.ToString() + "/" + Catdata.Length.ToString();
            ResetBtn();
            for (int i = 0; i < Catdata.Length; i++)
            {
                var element = _layoutElementPool.SpawnElement();
                var catItem = element.GetComponent<SingleLandeHandBookWindowItem>();
                catItem.ShowBuilding(Catdata, i);
            }
            yield return null;
        }

        private void ResetBtn() {
            _btn_get.onClick.RemoveAllListeners();
            if (getnum < GetDatas().Length)
            {
                _btn_get.gameObject.GetComponent<Image>().material = Resources.Load<Material>("Materials/UIGray");
                _btn_get.onClick.AddListener(() => {
                    ToastBox.ShowText("收集	满了才可以领取哦~");
                });
            }
            else if (KHandBookManager.Instance.AllFinishLande.Contains(_data_suit.itemID))
            {
                _btn_get.gameObject.GetComponent<Image>().material = Resources.Load<Material>("Materials/UIGray");
                _btn_get.onClick.AddListener(() => {
                    ToastBox.ShowText("已经领取过了~");
                });
            }
            else
            {
                _btn_get.gameObject.GetComponent<Image>().material = null;
                _btn_get.onClick.AddListener(() => {
                    KHandBookManager.Instance.GetLandeAward(_data_suit.itemID, GetAwardCallBack);
                });
            }
        }
        //领取动作的回调
        private void GetAwardCallBack(int cod,string str,object obj) {
           CloseWindow <SingleLandeHandBookWindow> ();
            OpenWindow <LandeHandBookWindow>();
        }
        #endregion
    }
}

