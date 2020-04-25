/** 
 *FileName:     StopWindow.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2018-01-15 
 *Description:    
 *History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{


    public partial class StopWindow : KUIWindow
    {
        public class StopWindowData
        {
            public Dictionary<int, int> targetDic;
        }
        public StopWindow() : base(UILayer.kNormal, UIMode.kNone)
        {
            uiPath = "StopWindow";
        }
        public override void Awake()
        {
            base.Awake();
            InitModel();
            InitView();
        }
        public override void OnEnable()
        {
            base.OnEnable();
            RefreshModel();
            RefreshView();
        }
    }
}
