

//using System.Collections;
//using System.Collections.Generic;
///** 
//*FileName:     FriendListWindow.cs 
//*Author:       LiMuChen 
//*Version:      1.0 
//*UnityVersion：5.6.3f1
//*Date:         2017-10-23 
//*Description:    
//*History: 
//*/
//namespace Game.UI
//{
//    public partial class FriendListWindow : KUIWindow
//    {
//        #region Constructor

//        public FriendListWindow()
//            : base(UILayer.kNormal, UIMode.kSequence)
//        {
//            uiPath = "FriendList";
//        }

//        #endregion

//        #region Method       

//        #endregion

//        #region Action

//        private void OnCloseBtnClick()
//        {
//            CloseWindow(this);

//        }
//        private void OnChooseCatBtnClick()
//        {
//            List<KCat> catList = new List<KCat>(KCatManager.Instance.allCats);

//            OpenWindow<ChooseCatWindow>(new ChooseCatWindow.Data()
//            {
//                idx = 1,
//                catsList = catList,
//                onConfirm = OnChooseCatConfim,
//                onCancel= OnChooseCancel,
//            });
//        }
       
//        private void OnChooseCancel()
//        {

//        }
//        private void OnChooseCatConfim(KCat cat,int idx)
//        {
//            _cat = cat;
//            RefreshView();
//        }
//        private void OnFosterBtnClick()
//        {
//            if (_firendData != null&& _cat != null)
//            {
//                KFoster.Instance.AddSelfCatInFriend(_firendData.PlayerId, _cat.catId, AddSelfCatInFriendCallBakc);
//            }
      
//        }
//      private void AddSelfCatInFriendCallBakc(int code,string message,object data)
//        {
//            if (code==0)
//            {
//                    KFoster.Instance.GetMixInfos(GetMiddleInfosCallBack);
//            }
//        }
//        private void GetMiddleInfosCallBack(int code, string message, object data)
//        {
//            KUIWindow.GetWindow<AdoptionFriendWindow>().RefreshView();
//            CloseWindow(this);
//        }


//        #endregion

//        #region Unity

//        public override void Awake()
//        {
//            //InitModel();
//            InitView();
//        }

//        public override void OnEnable()
//        {
//            //RefreshItem();
//            //RefreshModel();
//            GetFriendInfor();

//        }

//        #endregion
//    }
//}

