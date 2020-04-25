// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "InfomationBox.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

using Game.Build;
using Game.DataModel;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    partial class ItemGetPath
    {
        #region 物品表中获取途径所指向的面板
        public enum ItemGetPathWindow {
            //商店
            ShopWindow = 1,
            //拍照小摊
            PhotoShopWindow = 2,
            //手工作坊
            OrnamentShopWindow = 3,
            //探索地带
            DiscoveryWindow = 4,
            //寄养所:功能尚未开始
            Jiyangsuo = 5,
            //关卡
            MapSelectWindow = 6,
        }
        #endregion

        #region OulineData 
        public class WindowData
        {
            public int itemID;
            public Type itemClass;
        }

        public class AccurateData {
            public string itemName;
            public int itemNum;
            public int itemQuality;
            //激活：是否曾经获取过 。1：是
            public int learned = 1;
            public int[] getPaths;
            public Sprite _spt_icon;
            public string _str_className;
            public string _str_titleOne;
            public string _str_titleTwo;
            public string _str_titleThree;
            public string _str_titleFour;
            public string _str_attributeOne;
            public string _str_attributeTwo;
            public string _str_attributeThree;
            public string _str_attributeFour;
        }
        #endregion
        #region Field
        public readonly static WindowData DefaultData = new WindowData();
        private WindowData _infomationData;
        //数据类型与对应的处理方式
        private Dictionary<Type, Action> ClassAndOperation = new Dictionary<Type, Action>();
        private AccurateData accurateData;
        #endregion
        #region Method
        public void InitModel()
        {
            _infomationData = new WindowData();
            ClassAndOperation.Clear();
            ClassAndOperation.Add(typeof(CatXDM), CatDataOperate);
            ClassAndOperation.Add(typeof(KItemBuilding), BuildingDataOperate);
            ClassAndOperation.Add(typeof(KItemFormula), FormulaDataOperate);
            ClassAndOperation.Add(typeof(KItemThing), ThingDataOperate);
        }
        public void RefreshModel()
        {
            _infomationData = data as WindowData;
            Type datatp = _infomationData.itemClass;
            ClassAndOperation[datatp]();
        }

        private int[] GetShowGetPathsData() {
            return accurateData.getPaths;
        }
        private void CatDataOperate() {
            CatXDM openData = new CatXDM();
            openData = XTable.CatXTable.GetByID(_infomationData.itemID);
            accurateData = new AccurateData
            {
                itemName = openData.Name,
                itemNum = KCatManager.Instance.GetCatCountByShopID(openData.ID),
                itemQuality = openData.Rarity,
                learned = KHandBookManager.Instance.HandBookConfigDictionary[openData.ID].learned,
                getPaths = openData.Getinformation.ToArray(),
                _spt_icon = KIconManager.Instance.GetCatIcon(openData.Icon),
                _str_className = typeof(CatXDM).FullName,
                _str_titleOne = string.Format(KLocalization.GetLocalString(54010),"") + openData.CoinAbilityRange[0].ToString() + "~" + openData.CoinAbilityRange[1].ToString(),
                _str_titleTwo = string.Format(KLocalization.GetLocalString(54011), "") + openData.ExploreAbilityRange[0].ToString() + "~" + openData.ExploreAbilityRange[1].ToString(),
                _str_titleThree = string.Format(KLocalization.GetLocalString(54012), "") + openData.MatchAbilityRange[0].ToString() + "~" + openData.MatchAbilityRange[1].ToString(),
                //_str_attributeOne = openData.CoinAbilityRange[0].ToString() + "~" + openData.CoinAbilityRange[1].ToString(),                
                //_str_attributeTwo = openData.ExploreAbilityRange[0].ToString() + "~" + openData.ExploreAbilityRange[1].ToString(),
                //_str_attributeThree = openData.MatchAbilityRange[0].ToString() + "~" + openData.MatchAbilityRange[1].ToString(),
            };
        }
        private void BuildingDataOperate()
        {
            KItemBuilding openData = new KItemBuilding();
            openData = KItemManager.Instance.GetBuilding(_infomationData.itemID);
            accurateData = new AccurateData
            {
                itemName = openData.itemName,
                itemNum = openData.curCount + BuildingManager.Instance.BuildingTypeCountGet(openData.itemID),
                itemQuality = openData.rarity,
                learned = KHandBookManager.Instance.HandBookConfigDictionary[openData.itemID].learned,
                getPaths = openData.getInformation,
                _spt_icon = KIconManager.Instance.GetBuildingIcon(openData.iconName),
                _str_className = typeof(KItemBuilding).FullName,
                _str_titleOne = KLocalization.GetLocalString(55023) + openData.mapSize.x.ToString() + "*" + openData.mapSize.y.ToString(),
                _str_titleTwo = KLocalization.GetLocalString(55024) + openData.charm.ToString(),
                //_str_attributeOne = openData.mapSize.x.ToString() + "*" + openData.mapSize.y.ToString(),
                 //_str_attributeTwo = openData.charm.ToString(),
             };
        }
        private void FormulaDataOperate()
        {
            KItemFormula openData = new KItemFormula();
            openData = KItemManager.Instance.GetFormula(_infomationData.itemID);
            string consumptiveMaterial = "";
            for (int i = 0; i < openData.costItems.Length; i++)
            {
                if (KItemManager.Instance.GetItem(openData.costItems[i]) != null)
                {
                    string itemName = KLocalization.GetLocalString(KItemManager.Instance.GetItem(openData.costItems[i]).displayName);
                    consumptiveMaterial = itemName + ",";
                }
            }
            if (consumptiveMaterial != string.Empty)
            {
                consumptiveMaterial = consumptiveMaterial.TrimEnd(',');
            }
            accurateData = new AccurateData
            {
                itemName = openData.itemName,
                itemNum = openData.curCount,
                itemQuality = openData.rarity,
                learned = KHandBookManager.Instance.HandBookConfigDictionary[openData.itemID].learned,
                getPaths = openData.getInformation,
                _spt_icon = KIconManager.Instance.GetItemIcon(openData.iconName),
                _str_className = typeof(KItemFormula).FullName,
                _str_titleOne = KLocalization.GetLocalString(55025) + openData.costTime.ToString(),
                _str_titleTwo = KLocalization.GetLocalString(55026) + openData.costCoin.ToString(),
                _str_titleThree = KLocalization.GetLocalString(55027) + consumptiveMaterial,
                //_str_attributeOne = openData.costTime.ToString(),
                //_str_attributeTwo = openData.costCoin.ToString(),
                //_str_attributeThree = consumptiveMaterial,
            };
        }

        private void ThingDataOperate() {
            KItemThing openData = new KItemThing();
            openData = KItemManager.Instance.GetThing(_infomationData.itemID);
            accurateData = new AccurateData
            {
                itemName = openData.itemName,
                itemNum = openData.curCount,
                itemQuality = openData.rarity,
                learned = KHandBookManager.Instance.HandBookConfigDictionary[openData.itemID].learned,
                getPaths = openData.getInformation,
                _spt_icon = KIconManager.Instance.GetItemIcon(openData.iconName),
                _str_className = typeof(KItemThing).FullName,
                _str_titleFour = KLocalization.GetLocalString(55028),
                _str_attributeFour = KLocalization.GetLocalString(openData.description),
            };
        }
        #endregion
    }
}
