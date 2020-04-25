// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "AreaUnlockBox" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
namespace Game.UI
{
    public partial class AreaUnlockBox : KUIWindow
    {
        #region Static

        public static void ShowBox(int grade, int star, int cost, int costType, Action onConfirm)
        {
            DefaultData.grade = grade;
            DefaultData.star = star;
            DefaultData.cost = cost;
            DefaultData.costType = costType;
            DefaultData.onConfirm = onConfirm;
            DefaultData.onCancel = null;
            OpenWindow<AreaUnlockBox>(DefaultData);
        }

        #endregion

        #region Constructor

        public AreaUnlockBox()
            : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "AreaUnlock";
            hasMask = true;
        }

        #endregion

        #region Method

        private void OnCloseClick()
        {
            CloseWindow(this);
        }

        private void OnConfirmClick()
        {
            CloseWindow(this);
            if (_myData.onConfirm != null)
            {
                _myData.onConfirm();
            }
        }

        public override void Awake()
        {
            InitModel();
            InitView();
        }

        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
        }

        #endregion
    }
}
