//// ***********************************************************************
//// Assembly         : Unity
//// Author           : LiMuChen
//// Created          : #DATA#
////
//// Last Modified By : LiMuChen
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "AdoptionWindow.Model" company="moyu"></copyright>
//// <summary></summary>
//// ***********************************************************************

//using System.Collections.Generic;

//namespace Game.UI
//{
//    partial class AdoptionWindow
//    {
//        #region WindowData
//       public class Data
//        {
//           public int playerId;
//        }


//        #endregion

//        #region Field
//        private Data _windowdata;

//        #endregion

//        #region Method
         
//        public void InitModel()
//        {
//            _windowdata = new Data();
//        }

//        public void RefreshModel()
//        {
//            var passData = data as Data;
//            if (passData!=null)
//            {
//                _windowdata.playerId = passData.playerId;
//            }
//        }
//        private List<KFoster.Slot> GetFosterInfor()
//        {
//            if (_windowdata.playerId==KUser.SelfPlayer.id)
//            {
//                return KFoster.Instance.selfSlots;
//            }
//            else
//            {
//                return KFoster.Instance.friendSlots;
//            }
//        }
//        #endregion
//    }
//}

