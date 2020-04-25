/** 
*FileName:     ChooseCatWindow.Model.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-10-23 
*Description:    
*History: 
*/
using System.Collections.Generic;

namespace Game.UI
{
    public partial class ChooseCatWindow
    {
        public class Data
        {
            public List<CatDataVO> catsList;
            public int idx;
            public System.Action<CatDataVO, int> onConfirm;
            public System.Action onCancel;
            public int type;// 1 = 产金 2 探索 3 消除
        }
        private Data _chooseCatData;

        public void InitModel()
        {
            _chooseCatData = new Data();
            _chooseCatData.catsList = new List<CatDataVO>();
        }

        public void RefreshModel()
        {
            _chooseCatData.catsList.Clear();
            _chooseCatData.onConfirm = null;
            _chooseCatData.onCancel = null;
            _indx = 0;
            if (data is Data)
            {
                var tmpData = (Data)data;
                _chooseCatData.catsList = tmpData.catsList;
                _chooseCatData.onCancel = tmpData.onCancel;
                _chooseCatData.onConfirm = tmpData.onConfirm;
                _chooseCatData.idx = tmpData.idx;
                _chooseCatData.type = tmpData.type;
            }
        }
    }
}

