
//using Msg.ClientMessage;
//using System.Collections.Generic;
///** 
//*FileName:     FriendListWindow.Model.cs 
//*Author:       LiMuChen 
//*Version:      1.0 
//*UnityVersion：5.6.3f1
//*Date:         2017-10-23 
//*Description:    
//*History: 
//*/
//namespace Game.UI
//{
//    public partial class FriendListWindow
//    {
//        private List<FriendInfo> lstFrdData;
//        //public class Data
//        //{

//        //}
//        //private Data _windowData;
//        //#region Method

//        //public void InitModel()
//        //{
//        //    _chooseCatData = new Data();
//        //    _chooseCatData.catsList = new List<KCat>();
//        //}

//        //public void RefreshModel()
//        //{
//        //    _chooseCatData.catsList.Clear();
//        //    _chooseCatData.onConfirm = null;
//        //    _chooseCatData.onCancel = null;
//        //    _indx = 0;
//        //    if (data is Data)
//        //    {
//        //        var tmpData = (Data)data;
//        //        _chooseCatData.catsList = tmpData.catsList;
//        //        _chooseCatData.onCancel = tmpData.onCancel;
//        //        _chooseCatData.onConfirm = tmpData.onConfirm;
//        //        _chooseCatData.idx = tmpData.idx;
//        //        _chooseCatData.type = tmpData.type;
//        //    }

//        private void GetFriendInfor()
//        {
//            KFriendManager.Instance.C2SFosterGetEmptySlotFriends(GetFriendInfoCallBack);
//        }
//        private void GetFriendInfoCallBack(int code, string str, object obj)
//        {
//             lstFrdData = KFriendManager.Instance.Lst_frdInfoHaseEmptyPosition;
//            RefreshView();
//        }
//    }
//}

