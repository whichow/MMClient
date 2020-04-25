// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-11-11
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "ToastBox" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    /// <summary>
    /// 上浮渐隐提示条
    /// </summary>
    public partial class ToastBox : KUIWindow
    {
        #region Static Api

        public static void ShowText(string text)
        {
            OpenWindow<ToastBox>(text);
        }

        #endregion

        #region Constructor

        public ToastBox()
            : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "ToastBox";
        }

        #endregion

        #region Method

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            //RefreshModel();
            RefreshView();
        }

        public override void Update()
        {
            //RefreshView();
        }

        #endregion
    }
}
