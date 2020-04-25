// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BagWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class BagWindow : KUIWindow
    {
        private Button _closeBtn;
        private Toggle[] _toggles;
        public int _bagType { get; private set; }


        public BagWindow()
            : base(UILayer.kPop, UIMode.kSequenceRemove)
        {
            uiPath = "BagWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public override void Awake()
        {
            _closeBtn = Find<Button>("Right/Close");
            _closeBtn.onClick.AddListener(OnCloseBtnClick);
            _toggles = new Toggle[4];
            for (int i = 0; i < 4; i++)
                _toggles[i] = Find<Toggle>("Right/TabView/ToggleGroup/Tog" + (i + 1));
            foreach (Toggle tog in _toggles)
                tog.onValueChanged.AddListener((bool blSelect) => { if (blSelect) OnBagTypeChange(tog); });
            _bagType = ItemTagConst.AllItem;
            InitView();
        }

        public override void AddEvents()
        {
            base.AddEvents();
            BagDataModel.Instance.AddEvent(BagEvent.BagSellItem, OnSellItem);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            BagDataModel.Instance.RemoveEvent(BagEvent.BagSellItem, OnSellItem);
        }

        private void OnBagTypeChange(Toggle tog)
        {
            switch (tog.name)
            {
                case "Tog1":
                    _bagType = ItemTagConst.AllItem;
                    break;
                case "Tog2":
                    _bagType = ItemTagConst.Fragment;
                    break;
                case "Tog3":
                    _bagType = ItemTagConst.Material;
                    break;
                case "Tog4":
                    _bagType = ItemTagConst.Consumable;
                    break;
            }
            RefreshView(_bagType);
        }

        private void OnSellItem()
        {
            RefreshView(_bagType);
        }

        public void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        public override void OnEnable()
        {
            OnBagTypeChange(_toggles[_bagType]);
        }

        private void OnCompCatCallBack(int code, string message, object data)
        {
            KCat kCat = null;
            Debug.Log("合成猫咪成功");
            var list = data as ArrayList;
            if (list != null)
            {
                kCat = new KCat();
                foreach (var item in list)
                {
                    if (item is S2CComposeCatResult)
                    {
                        var result = (S2CComposeCatResult)item;
                        if (result.Cat != null)
                            kCat = KCatManager.Instance.GetCat(result.Cat.Id);
                    }
                }
            }
            if (kCat != null)
            {
                OpenWindow<PhotoShopGotBuildWindow>(new PhotoShopGotBuildWindow.Data
                {
                    cat = kCat,
                    item = null,
                    type = 1,
                });
            }
            else
            {
                Debug.Log("猫咪合成失败");
            }
        }
    }
}

