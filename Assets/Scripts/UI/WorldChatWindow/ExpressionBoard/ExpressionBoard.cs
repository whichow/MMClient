

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/** 
* 作用：世界聊天的控制类
* 作者：wsy
*/
namespace Game.UI
{
    public partial class ExpressionBoard : KUIWindow
    {
        #region Constructor
        public ExpressionBoard()
        : base(UILayer.kNormal, UIMode.kNone)
        {
            uiPath = "ExpressionBoard";
            uiAnim = UIAnim.kAnim1;
        }
        #endregion
        #region Unity
        public override void Awake()
        {
            InitializationModel();
            InitView();
        }
        public override void OnEnable()
        {
            ResetModel();
            RefreshView();
        }
        private void OnPageChange(bool value)
        {
            var page = GetOnToggle();
            if (page != CurrentPageType)
            {
                CurrentPageType = page;
                KEmojiManager.Instance.RecordLastPageType(CurrentPageType);
                RefreshView();
            }
        }
        private void CloseUI()
        {
            CloseWindow<ExpressionBoard>();
        }
        #endregion
    }
}

