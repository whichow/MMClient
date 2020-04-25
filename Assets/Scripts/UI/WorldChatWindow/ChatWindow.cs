

//using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
///** 
//* 作用：世界聊天的控制类
//* 作者：wsy
//*/
//namespace Game.UI
//{
//    public partial class ChatWindow : KUIWindow
//    {
//        #region Constructor
//        public ChatWindow()
//        : base(UILayer.kNormal, UIMode.kSequence)
//        {
//            uiPath = "ChatWindow";
//            uiAnim = UIAnim.kAnim_ChatEnable;
//            //hasMask = true;
//        }
//        #endregion

//        #region Method
//        #endregion

//        #region Action
//        private void OnCloseBtnClick()
//        {
//            if (_bl_isAcceptClick)
//            {
//                StartCoroutine(CloseAfterMove());
//            }
//            _bl_isAcceptClick = false;
//        }
//        /// <summary>
//        /// 关闭动画,即向右滑
//        /// </summary>
//        /// <returns></returns>
//        private IEnumerator CloseAfterMove()
//        {
//            transform.DOLocalMove(new Vector3(825, 0, 0), KChatManager.PanelMoveTime);
//            yield return new WaitForSeconds(KChatManager.PanelMoveTime);
//            CloseWindow(this);
//        }
//        #endregion

//        #region Unity
//        public override void Awake()
//        {
//            InitializationModel();
//            InitView();
//        }
//        public override void OnEnable()
//        {
//            _bl_isAcceptClick = true;
//            KEmojiManager.Instance.event_AddSpecialExpression = AddSpecialExpression;
//            KEmojiManager.Instance.event_AddCommonExpression = AddCommonExpression;

//            ResetModel();
//            RefreshView();
//        }
//        private void OnPageChange(bool value)
//        {
//            var page = GetOnToggle();
//            if (page != CurrentPageType)
//            {
//                CurrentPageType = page;
//                KChatManager.Instance.RecordLastPageType(CurrentPageType);
//                RefreshView();
//            }
//        }
//        #endregion
//    }
//}

