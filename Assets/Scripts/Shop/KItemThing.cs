// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KItemThing" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;

namespace Game
{
    /// <summary>
    /// 背包物品
    /// </summary>
    public class KItemThing : KItem
    {
        public int type
        {
            get;
            private set;
        }

        public override void Load(Hashtable table)
        {
            base.Load(table);
            type = table.GetInt("Type");
        }
    }
}

