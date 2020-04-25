/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/20 10:22:02
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class GlobalXTable : XTable<GlobalXDM>
    {
        /// <summary>
        /// 照片服务器上传地址
        /// </summary>
        public string PhotoServerUploadUrl
        {
            get { return GetByID(1).stringVal; }
        }

        /// <summary>
        /// 照片服务器下载地址
        /// </summary>
        public string PhotoServerDownloadUrl
        {
            get { return GetByID(2).stringVal; }
        }

        /// <summary>
        /// 服务器列表
        /// </summary>
        public List<string> ServerList
        {
            get { return GetByID(4).stringVal.Split('|').ToList(); }
        }

        protected override void Parseed(IXDM xdm)
        {
            base.Parseed(xdm);

        }


    }
}
