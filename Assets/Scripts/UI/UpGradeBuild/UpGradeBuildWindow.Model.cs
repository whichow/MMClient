

using UnityEngine;
/** 
*FileName:     UpGradeBuildWindow.Model.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-11-03 
*Description:    
*History: 
*/
namespace Game.UI
{
    partial class UpGradeBuildWindow
    {
        #region WindowData
        public class Data
        {
            public int BuildingId;
            public GameObject Model;
            public System.Action<int> CallBack;

        }

        #endregion

        #region Field
        Data dataReal;
      
        private void OnUpBtnClick()
        {
            Debug.Log("升级猫舍"+ catTery.buildingId);
            //dataReal = data as Data;
            KCattery.Instance.Upgrade(dataReal.BuildingId, OnUpCallBack);
        }
        private void OnUpCallBack(int code,string message,object data)
        {
            
            
            Debug.Log("猫舍升级成功"+code);
            dataReal.CallBack(code);
            CloseWindow(this);

        }
        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }


        #endregion

        #region Method

        public void InitModel()
        {
          
        }

        public void RefreshModel()
        {     
            
        }

        #endregion
    }
}

