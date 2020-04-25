/** 
 *FileName:     LackHintWindow.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2018-01-22 
 *Description:    
 *History: 
*/ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.UI
{
    public partial class LackHintWindow : KUIWindow
    {
        public LackHintWindow() : base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "LackHintWindow";
        }

        public override void Awake()
        {
            base.Awake();
            InitView();
        }
        public override void OnEnable()
        {
            base.OnEnable();
            RefreshView();
        }
    }
}
