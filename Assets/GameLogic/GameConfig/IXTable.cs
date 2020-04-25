/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/23 15:31:46
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections;

namespace Game.DataModel
{
    public partial class XTable<TRow> : IXTable
    {
        public static string RootDir { get { return "Tables/"; } }
    }

    public interface IXTable
    {
        string ResourceName { get; }
        bool IsReady { get; }
        void Load();
        void Load(Action callback);
        void LoadFromHashtable(Hashtable table);
    }

    public interface IXDM
    {
        int ID { get; }

        void Parse(Hashtable table);

    }

}
