using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    class MakeItem : KUIItem
    {

        Text _makeCost;

        /// <summary>
        /// 猫槽 索引
        /// </summary>
        private int _makeSlotId;
        private FunctionWindow.ManualWorkShopFunEnum _makeType;
        private System.Action<int> _makeEvent;
        /// <summary>
        /// 猫的点击事件
        /// </summary>
        public System.Action<int> MakeEvent
        {
            get
            {
                if (_makeEvent == null)
                {
                    _makeEvent = new Action<int>((index) => { });
                }
                return _makeEvent;
            }
            set
            {
                _makeEvent = value;
            }
        }
        private System.Func<int> getmoneyCost { get; set; }
        public void MakeTypeSet(object makeType)
        {
            _makeType = (FunctionWindow.ManualWorkShopFunEnum)makeType;
        }
        protected override void OnCull(bool visible)
        {
            base.OnCull(visible);
        }
        protected override void Refresh()
        {
            base.Refresh();

            KWorkshop.Slot slot = data as KWorkshop.Slot;

            if (slot == null)
            {
                return;
            }
            Debug.Log("锻造槽索引+" + slot.slotId);
            _makeSlotId = slot.slotId;
            if (_makeType == FunctionWindow.ManualWorkShopFunEnum.UpSleep|| _makeType == FunctionWindow.ManualWorkShopFunEnum.ReadyMake)
            {
                Sprite sp = slot.GetFormulaIcon();
                GameObject iconTrans = Find("Icon").gameObject;
                Debug.Log("IconName" + sp.name);
                iconTrans.SetActive(true);
                if (sp != null)
                    iconTrans.GetComponent<Image>().overrideSprite = sp;
                if (_makeType == FunctionWindow.ManualWorkShopFunEnum.UpSleep)
                {
                    _makeCost = Find("Cost").GetComponent<Text>();
                }
                else
                {
                    _makeCost = null;
                }
            }
          


        }
        public void getmoneyCostSet(System.Func<int> getmoneyCost)
        {
            this.getmoneyCost = getmoneyCost;
        }
        private void MakeOnclick()
        {
            MakeEvent(_makeSlotId);
        }
        private void Update()
        {
            if (_makeCost != null&& getmoneyCost!=null)
            {
                _makeCost.text = getmoneyCost().ToString();
            }

        }
        private void Awake()
        {
            this.GetComponent<Button>().onClick.AddListener(MakeOnclick);
            //CatEvent;

        }
    }
}
