// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KShopFormula" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;

namespace Game
{
    /// <summary>
    /// 配方数据
    /// </summary>
    public class KItemFormula : KItem
    {
        public int buildingId
        {
            get;
            private set;
        }

        private int _buildingTag = -1;
        public int buildingTag
        {
            get
            {
                if (_buildingTag < 0)
                {
                    var building = KItemManager.Instance.GetBuilding(buildingId);
                    if (building != null)
                    {
                        _buildingTag = building.itemTag;
                    }
                    else
                    {
                        _buildingTag = 0;
                    }
                }
                return _buildingTag;
            }
        }

        public int costCoin
        {
            get;
            private set;
        }

        public int costStar
        {
            get;
            private set;
        }

        public int costTime
        {
            get;
            private set;
        }

        public int getExp
        {
            get;
            private set;
        }

        /// <summary>
        /// 消耗的材料
        /// </summary>
        public int[] costItems
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int sortValue
        {
            get;
            private set;
        }

        public int unlockLevel
        {
            get;
            private set;
        }

        public override bool isLock
        {
            get
            {
                return PlayerDataModel.Instance.mPlayerData.mCurMaxStage < unlockLevel;
            }
        }

        #region Method

        public string GetBuildindIcon()
        {
            var building = KItemManager.Instance.GetBuilding(buildingId);
            if (building != null)
            {
                return building.iconName;
            }
            return null;
        }

        public override void Load(Hashtable table)
        {
            base.Load(table);
            buildingId = table.GetInt("BuildingId");
            costCoin = table.GetInt("Cost");
            costStar = table.GetInt("Star");
            costTime = table.GetInt("Time");
            costCoin = table.GetInt("Cost");
            costItems = table.GetArray<int>("Group");

            unlockLevel = table.GetInt("UnlockLevel");
            getExp = table.GetInt("Exp");
            sortValue = table.GetInt("Sort");
        }

        #endregion
    }
}
