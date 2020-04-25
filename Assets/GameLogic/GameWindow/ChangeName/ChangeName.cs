// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "ChangeNameBox" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

namespace Game.UI
{
    public class NmaeType
    {
        public const int CatName = 0;//猫咪名称
        public const int PlayerName = 1;//玩家名称
    }

    public class ChangeNameData
    {
        public object data;
        public int type;
    }

    public partial class ChangeName : KUIWindow
    {
        public ChangeName() :
            base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "CatNick";
        }

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            RefreshView();
        }

        private void OnBackBtnClick()
        {
            CloseWindow(this);
        }
    }
}
