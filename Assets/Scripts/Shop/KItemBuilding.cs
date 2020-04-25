// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KItemBuilding" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 建筑
    /// </summary>
    public class KItemBuilding : KItem
    {
        /// <summary>
        /// 模型名字
        /// </summary>
        public string model
        {
            get;
            private set;
        }
        /// <summary>
        /// 旋转模型名字
        /// </summary>
        public string roModel
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string tent
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int maxGrade
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int type
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int charm
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int unlockType
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int unlockLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ItemInfo unlockCost
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int buildTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 水陆属性
        /// </summary>
        public int geography
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int rotatable
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int saleable
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int saleCoin
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int2 mapSize
        {
            get;
            private set;
        }

        /// <summary>
        /// 移动
        /// </summary>
        public int movable
        {
            get;
            private set;
        }

        /// <summary>
        /// 可移除
        /// </summary>
        public int removable
        {
            get;
            private set;
        }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int sort
        {
            get;
            private set;
        }
        /// <summary>
        /// 建筑配置ID
        /// </summary>
        public int cfgId
        {
            get;
            private set;
        }
        public string description
        {
            get;
            private set;
        }
        public int descriptionId
        {
            get;
            private set;
        }
        /// <summary>
        /// 可回收
        /// </summary>
        public int recovery
        {
            get;
            private set;
        }
        /// <summary>
        /// 模型缩放系数
        /// </summary>
        public float modelScale
        {
            get;
            private set;
        }

        /// <summary>
        /// 套装编号
        /// </summary>
        public int suitID
        {
            get;
            private set;
        }

        public ItemInfo removeCost
        {
            get;
            private set;
        }

        public int tab
        {
            get;
            private set;
        }
 
        public GameObject GetModel()
        {
            var modelName = model;
            GameObject modelPrefab;
            if (KAssetManager.Instance.TryGetBuildingPrefab(modelName, out modelPrefab))
            {
                var gameObj = Object.Instantiate(modelPrefab);
                return gameObj;
            }
            return null;
        }

        #region Method

        public override void Load(Hashtable table)
        {
            //string[] keys = { "Id", "Name", "Model", "Icon", "MaxLevel", "Type", "UnlockType", "UnlockLevel", "UnlockCost", "BuildTime", "Charm", "Geography", "Rotatable", "Saleable ", "SaleCoin", "MapSize", "Movable", "Removable" };

            base.Load(table);
            model = table.GetString("Model");
            if (!string.IsNullOrEmpty(model))
            {
                model = "Item/" + model;
            }
            roModel = table.GetString("RoModel");
            maxGrade = table.GetInt("MaxLevel");
            type = table.GetInt("Type");
            unlockType = table.GetInt("UnlockType");
            unlockLevel = table.GetInt("UnlockLevel");
            unlockCost = ItemInfo.Convert(table.GetArrayList("UnlockCost"));
            movable = table.GetInt("Movable");
            removable = table.GetInt("Removable");
            removeCost = ItemInfo.Convert(table.GetArrayList("Remove"));
            buildTime = table.GetInt("BuildTime");
            geography = table.GetInt("Geography");
            rotatable = table.GetInt("Rotatable");
            charm = table.GetInt("Charm");
            saleCoin = table.GetInt("SaleCoin");
            saleable = table.GetInt("Saleable");// saleCoin > 0 ? 1 : 0;
            recovery = table.GetInt("Callback");
            mapSize = new Int2(table.GetArray<int>("MapSize"));
            suitID = table.GetInt("SuitId");
            tab = table.GetInt("Tab");

            int scale = table.GetInt("Scale");

            if (scale == 0)
            {
                modelScale = 1f;
            }
            else
            {
                modelScale = scale * 0.001f;
            }
            sort = table.GetInt("Sort");
            cfgId = table.GetInt("ID");
            descriptionId = table.GetInt("DescriptionId");
            description = KLocalization.GetLocalString(descriptionId);
        }


        #endregion

        #region Unity  

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        #endregion
    }
}

