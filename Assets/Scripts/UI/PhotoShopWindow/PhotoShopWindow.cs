using Game.DataModel;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Callback = System.Action<int, string, object>;

namespace Game.UI
{
    public partial class PhotoShopWindow : KUIWindow
    {
        public enum BuyType
        {
            Low,
            Middle,
            Hight,
        }

        #region Constructor

        private List<PhotoShopPickCardLowWindow.PickCardData> kItemDatas;

        public PhotoShopWindow() : base(UILayer.kNormal, UIMode.kSequenceRemove)
        {
            uiPath = "PhotoShop";
        }

        #endregion

        #region Action

        private void OnQuitBtnClick()
        {
            CloseWindow(this);
        }

        public void SendBuyLowCat()
        {
            Debuger.Log("发送抽取低级猫");
            Callback middle = (code, message, data) =>
            {
                kItemDatas = new List<PhotoShopPickCardLowWindow.PickCardData>();
                var list = data as ArrayList;
                if (list != null)
                {
                    foreach (var s2cItem in list)
                    {
                        if (s2cItem is S2CDrawResult)
                        {
                            var result = (S2CDrawResult)s2cItem;
                            foreach (var item in result.Cats)
                            {
                                var xdm = XTable.CatXTable.GetByID(item.CatCfgId);
                                kItemDatas.Add(new PhotoShopPickCardLowWindow.PickCardData()
                                {
                                    ID = xdm.ID,
                                    Type = PhotoShopPickCardLowWindow.EPickCardType.Cat,
                                    Icon = xdm.Icon,
                                    Rarity = xdm.Rarity,
                                });
                            }
                            foreach (var item in result.Buildings)
                            {
                                var xdm = KItemManager.Instance.GetItem(item.CfgId);
                                kItemDatas.Add(new PhotoShopPickCardLowWindow.PickCardData()
                                {
                                    ID = item.CfgId,
                                    Type = PhotoShopPickCardLowWindow.EPickCardType.Building,
                                    Icon = xdm.iconName,
                                    Rarity = xdm.rarity,
                                });
                            }
                            foreach (var item in result.Items)
                            {
                                var xdm = KItemManager.Instance.GetItem(item.ItemCfgId);
                                kItemDatas.Add(new PhotoShopPickCardLowWindow.PickCardData()
                                {
                                    ID = item.ItemCfgId,
                                    Type = PhotoShopPickCardLowWindow.EPickCardType.Item,
                                    Icon = xdm.iconName,
                                    Rarity = xdm.rarity,
                                });
                            }
                        }
                    }
                }

                if (kItemDatas.Count > 0)
                {
                    OpenWindow<PhotoShopPickCardLowWindow>(kItemDatas);
                }
                else
                {
                    Debuger.Log("低级抽卡失败");
                }
            };
            if (BagDataModel.Instance.GetItemCountById(ItemIDConst.LowCard) >= 5)
            {
                KUser.DrawCard(1, 5, middle);
            }
            else
            {
                if (BagDataModel.Instance.GetItemCountById(ItemIDConst.LowCard) > 0)
                {
                    KUser.DrawCard(1, BagDataModel.Instance.GetItemCountById(ItemIDConst.LowCard), middle);
                }
                else
                {
                    LackHintBox.ShowLackHintBox(ItemIDConst.LowCard);
                }
            }
        }

        private void SendBuyMiddleCat()
        {
            Debuger.Log("发送抽取中级猫");
            if(BagDataModel.Instance.GetItemCountById(ItemIDConst.MidCard) > 0)
            {
                OpenWindow<PhotoShopPickCardHighWindow>(new PhtotShopPickCardWindowData(1));
            }
            else
            {
                LackHintBox.ShowLackHintBox(ItemIDConst.LowCard);
            }
        }

        private void SendBuyHightCat()
        {
            Debuger.Log("发送抽取高级猫");
            bool blEnough = PlayerDataModel.Instance.mPlayerData.mDiamond >=
                            XTable.ItemXTable.GetByID(ItemIDConst.MidCard).Cost;
            if (blEnough)//BagDataModel.Instance.GetItemCountById(ItemIDConst.Diamond) > 0)
            {
                OpenWindow<PhotoShopPickCardHighWindow>(new PhtotShopPickCardWindowData(2));
            }
            else
            {
                LackHintBox.ShowLackHintBox(ItemIDConst.Diamond);
            }
        }

        #endregion

        #region Unity  

        public override void OnEnable()
        {
            RefreshView();
        }
        public override void Awake()
        {
            //InitModel();
            InitView();
        }

        public override void UpdatePerSecond()
        {
            RefreshView();
        }

        #endregion
    }

}