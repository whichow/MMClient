using Game.DataModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    class CatItem : KUIItem
    {
        Button _catBtn;
        Image _catImage;
        Text _catName;
        Text _grade ;

        KUIImage[] _fishs =new KUIImage[5];
        /// <summary>
        /// 猫槽 索引
        /// </summary>
        private int _catPosIndex;
        //private ItemType;

        private System.Action<int> _catEvent;

        public FunctionWindow.CatFunEnum _catType;
        //public int _catIndex;
        /// <summary>
        /// 猫的点击事件
        /// </summary>
        public System.Action<int> CatEvent
        {
            get
            {

                if (_catEvent == null)
                {
                    _catEvent = new Action<int>((indx)=> { });
                }
                return _catEvent;
            }
            set
            {
                _catEvent = value;
            }
        }
        public void setCatType(object catType)
        {
            _catType = (FunctionWindow.CatFunEnum)catType;
        }
        protected override void OnCull(bool visible)
        {
            base.OnCull(visible);
        }
        protected override void Refresh()
        {
            base.Refresh();
           

 
            if (_catType == FunctionWindow.CatFunEnum.CatInfo)
            {
                KCat kCat = data as KCat;
                if (kCat == null)
                {
                    return;
                }
                if (_catType == FunctionWindow.CatFunEnum.CatInfo)
                {
                    _catImage = this.transform.Find("Image").GetComponent<Image>();
                    _catName = this.transform.Find("Text").GetComponent<Text>();
                    _grade = this.transform.Find("Grade").GetComponent<Text>();
                    Transform fish = this.transform.Find("Fish");

                    for (int i = 0; i < _fishs.Length; i++)
                    {
                        _fishs[i] = fish.GetChild(i).GetComponent<KUIImage>();
                    }
                }

                for (int i = 0; i < _fishs.Length; i++)
                { 
                        if(i< kCat.star)
                            _fishs[i].ShowGray(false);
                        else
                            _fishs[i].ShowGray(true);
                }

                _catPosIndex = kCat.catId;
                Debug.Log("catId" + _catPosIndex);

                this.transform.SetAsLastSibling();
                Debug.Log("catId" + _catPosIndex);
                _catImage.overrideSprite = XTable.CatXTable.GetByID(kCat.shopId).GetIconSprite();
                _catName.text = kCat.name;
                _grade.text = CatDataModel.Instance.GetCatDataVOById(kCat.catId).mCatInfo.CoinAbility.ToString();
                this.gameObject.SetActive(true);
            }

        }

        private void Awake()
        {
            _catBtn = this.GetComponent<Button>();
            _catBtn.onClick.AddListener(() => CatEvent(_catPosIndex));
        }
    }
}
