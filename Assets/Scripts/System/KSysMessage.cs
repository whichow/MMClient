// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-10-24
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
//作用：通告消息使用的表
// ***********************************************************************
using System.Collections;

namespace Game
{
    public class KSysMessage
    {
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
        public int description
        {
            get;
            private set;
        }
        public int mainViewDescriptionID {
            get;
            private set;
        }
        public int newsTicker
        {
            get;
            private set;
        }
        #endregion
        #region Method
        //   ["Id","Type","DescriptionId","NewsTicker"],
        public void Load(Hashtable table)
        {
            id = table.GetInt("Id");
            type = table.GetInt("Type");
            description = table.GetInt("DescriptionId");
            mainViewDescriptionID = table.GetInt("MainViewDescriptionID");
            newsTicker = table.GetInt("NewsTicker");
        }
        #endregion       
    }
}
