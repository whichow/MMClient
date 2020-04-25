using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public partial class SettingPushWindow
    {

        private Button _backBtn;
 



      


        public void InitView()
        {
            _backBtn = Find<Button>("Back/Quit");
            _backBtn.onClick.AddListener(this.OnBackBtnClick);
    


        }
 

        public void RefreshView()
        {


        }
    }
}
