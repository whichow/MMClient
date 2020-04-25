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
    public class KItemGetPath
    {
        #region Enum

        #endregion

        #region Properties

        public int id
        {
            get;
            private set;
        }

        public int description
        {
            get;
            private set;
        }

        public int[] getPathSequence
        {
            get;
            private set;
        }

        #endregion

        #region Method
        public void Load(Hashtable table)
        {
            id = table.GetInt("Id");
            description = table.GetInt("DescriptionId");
            getPathSequence = table.GetArray<int>("Sequence");
        }
        #endregion       
    }
}
