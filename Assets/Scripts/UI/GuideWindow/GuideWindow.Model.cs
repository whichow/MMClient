/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/30 10:30:15
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using Game.UI;
using Game.DataModel;

namespace Game
{
    public partial class GuideWindow
    {
        private Data _actionData;

        public class Data
        {
            public GuideActionXDM action;
        }

        public void InitModel()
        {
            _actionData = new Data();
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData != null)
            {
                _actionData.action = passData.action;
            }
            else
            {
                _actionData.action = null;
            }
        }

    }
}
