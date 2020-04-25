
//// ***********************************************************************
//// Assembly         : Unity
//// Author           : LiMuChen
//// Created          : #DATA#
////
//// Last Modified By : LiMuChen
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "AdoptionWindow" company="moyu"></copyright>
//// <summary></summary>
//// ***********************************************************************

//using UnityEngine;

//namespace Game.UI
//{
//    public partial class AdoptionWindow : KUIWindow
//    {
//        #region Static
//        /// <summary>
//        /// 打开寄养所 
//        /// </summary>
//        /// <param name="id">玩家Id</param>
//        public static void OpenAdoption(int id)
//        {
//            if (id==KUser.SelfPlayer.id)
//            {
//                KFoster.Instance.GetSelfInfos(false, OpenFoseterCallBack);
//            }
//            else
//            {
//                KFoster.Instance.GetFriendInfos(id, OpenFoseterCallBack);
//            }
//            KUIWindow.OpenWindow<AdoptionWindow>(new AdoptionWindow.Data()
//            {
//                playerId = id,
//            });
//        }
//        private static  void OpenFoseterCallBack(int code, string message, object data)
//        {
//            if (code == 0)
//            {
//                KUIWindow.GetWindow<AdoptionWindow>().RefreshView();
//            }
//        }

//        #endregion

//        #region Constructor

//        public AdoptionWindow()
//                    : base(UILayer.kBackground, UIMode.kSequenceRemove)
//        {
//            uiPath = "AdoptionWindow";
//        }

//        #endregion

//        #region Action

//        private void OnConfirmClick()
//        {

//            Debug.Log("收取");
//            KFoster.Instance.GetSelfInfos(true, GetSelfFoseterCallBack);
//        }
//        private void GetSelfFoseterCallBack(int code,string message,object data)
//        {
//            if (code==0)
//            {
//                RefreshView();
//            }
//        }
//        private void OnFosterageCatBtnClick()
//        {
//            Debug.Log("猫咪寄养");
//            _transCatGroup.gameObject.SetActive(false);
//            OpenWindow<AdoptionCatWindow>();
//        }
//        private void OnFosterCardBtnClcik()
//        {
//            Debug.Log("寄养卡");
//            _transCatGroup.gameObject.SetActive(false);
//            OpenWindow<AdoptionCardWindow>();
//        }
//        private void OnFriendFosterCardBtnClcik()
//        {
//            Debug.Log("好友中点击寄养卡");
//            //_transCatGroup.gameObject.SetActive(false);
//            //OpenWindow<AdoptionCardWindow>();
//        }
//        private void OnFriendFosterBtnClick()
//        {
//            Debug.Log("好友寄养");
//            _transCatGroup.gameObject.SetActive(false);
//            if (_windowdata.playerId==KUser.SelfPlayer.id)
//            {
//                KFoster.Instance.GetMixInfos(GetMiddleInfosCallBack);
//            }
//            OpenWindow<AdoptionFriendWindow>(new AdoptionFriendWindow.Data
//            {
//                playerId = _windowdata.playerId,
//            });
//        }
     
//        private void GetMiddleInfosCallBack(int code,string message,object  data)
//        {
//            KUIWindow.GetWindow<AdoptionFriendWindow>().RefreshView();
//        }
//        private void OnCancelClick()
//        {
//            CloseWindow(this);
//        }

//        #endregion

//        #region Unity  

//        // Use this for initialization
//        public override void Awake()
//        {
//            InitModel();
//            InitView();
//        }

//        public override void OnEnable()
//        {
//            RefreshModel();
//            RefreshView();
//        }

//        #endregion
//    }
//}

