/** 
 *FileName:     DiscoveryRetWindow.Model.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-25 
 *Description:    
 *History: 
*/

namespace Game.UI
{
    using Msg.ClientMessage;
    using System.Collections.Generic;

    partial class DiscoveryRetWindow
    {
        #region WindowData

        public class Data
        {
            public int type;
            public IList<IdNum> itemIdNum;
            public IList<IdNum> specialsItemIdNum;
            public KExplore.Task task;
        }

        #endregion

        #region Field

        private Data _discoveryRetData;

        #endregion

        #region Method

        public void InitModel()
        {
            _discoveryRetData = new Data();
        }

        public void RefreshModel()
        {
            _discoveryRetData.type = 0;
            _discoveryRetData.specialsItemIdNum = new List<IdNum>();
            _discoveryRetData.itemIdNum = new List<IdNum>();
            _discoveryRetData.task = new KExplore.Task();
            _discoveryRetData.itemIdNum.Clear();
            _discoveryRetData.specialsItemIdNum.Clear();
            if (data is Data)
            {
                var tmpData = (Data)data;
                _discoveryRetData.type = tmpData.type;
                _discoveryRetData.itemIdNum = tmpData.itemIdNum;
                _discoveryRetData.specialsItemIdNum = tmpData.specialsItemIdNum;
                _discoveryRetData.task = tmpData.task;
            }
        }

        #endregion
    }
}

