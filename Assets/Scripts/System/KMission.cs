// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-10-24
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KMission" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;

namespace Game
{
    public class KMission
    {
        #region Enum

        public enum Status
        {
            None = 0,
            Complete = 1,
            Reward = 2,
        }

        public enum Type
        {
            Daily = 1,
            Achievement = 2,
        }

        #endregion

        #region Properties

        public int id
        {
            get;
            private set;
        }

        public int type
        {
            get;
            private set;
        }

        public int displayName
        {
            get;
            private set;
        }

        public int description
        {
            get;
            private set;
        }

        public KItem.ItemInfo[] bonusItems
        {
            get;
            private set;
        }

        public int maxValue
        {
            get;
            set;
        }

        public int curValue
        {
            get;
            set;
        }

        public int status
        {
            get;
            set;
        }

        public bool dynamic
        {
            get;
            private set;
        }

        #endregion

        #region Method

        public void Load(Hashtable table)
        {
            id = table.GetInt("Id");
            type = table.GetInt("Type");
            displayName = table.GetInt("Title");
            description = table.GetInt("DescriptionId");
            maxValue = table.GetInt("CompleteNum");
            dynamic = maxValue < 1;

            var bonusArray = table.GetArray<int>("Reward");
            if (bonusArray != null)
            {
                bonusItems = new KItem.ItemInfo[bonusArray.Length / 2];
                for (int i = 0; i < bonusItems.Length; i++)
                {
                    bonusItems[i] = new KItem.ItemInfo
                    {
                        itemID = bonusArray[i + i],
                        itemCount = bonusArray[i + i + 1],
                    };
                }
            }
        }

        #endregion       
    }
}
