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

//using Msg.ClientMessage;
//using System.Collections;
//using UnityEngine;

//namespace Game.UI
//{
//    public partial class AdoptionCardWindow : KUIWindow
//    {


//        #region Constructor

//        public AdoptionCardWindow()
//                    : base(UILayer.kNormal, UIMode.kSequenceRemove)
//        {
//            uiPath = "AdoptionCard";
//        }

//        #endregion

//        #region Action



//        private void OnCloseBtnClick()
//        {
//            //KUIWindow.GetWindow<AdoptionWindow>().ShowModel(true);
//            KUIWindow.GetWindow<AdoptionWindow>().RefreshView();
//            CloseWindow(this);
//        }
//        private void OnToggleValueChanged(bool value)
//        {
//            RefreshPanel();
//        }
//        private void OnEnmpClick()
//        {
//            Debug.Log("激活");
//            if (nowCard != null)
//            {
//                KFoster.Instance.FosterEquipCard(KFoster.Instance.buildingId, nowCard.bindItemId, FosterEquipCardCallBack);
//            }

//        }
//        private void FosterEquipCardCallBack(int code, string message, object data)
//        {
//            if (code == 0)
//            {
//                RefreshView();
//                Debug.Log("装备成功");
//            }
//        }
//        private void OnCanel()
//        {
//            Debug.Log("取消");
//            OpenWindow<MessageBox>(new MessageBox.Data
//            {
//                content = KLocalization.GetLocalString(52090),
//                onConfirm = OnConfim,
//                onCancel = OnCancel,
//            });


//        }
//        private void OnCancel()
//        {
//            Debug.Log("取消卸载寄养卡");
//        }
//        private void OnConfim()
//        {
//            var card = KItemManager.Instance.GetFosterCardbyBindId(KFoster.Instance.cardId);
//            KFoster.Instance.FosterUnequipCard(KFoster.Instance.buildingId, FosterUnequipCardCallBack);
//        }
//        private void FosterUnequipCardCallBack(int code, string message, object data)
//        {
//            if (code == 0)
//            {
//                RefreshView();
//            }
//        }
//        private void OnCompClick()
//        {
//            Debug.Log("合成");
//            int cardlenth = 0;
//            for (int i = 0; i < cardDatas.Length; i++)
//            {
//                if (cardDatas[i] != null)
//                {
//                    cardlenth++;
//                }
//            }
//            int[] cardIdList = new int[cardlenth];
//            for (int i = 0; i < cardIdList.Length; i++)
//            {
//                cardIdList[i] = cardDatas[i].bindItemId;
//            }
//            if (cardIdList.Length >= 3)
//            {
//                OpenWindow<MessageBox>(new MessageBox.Data
//                {
//                    onConfirm = OnCompClickOnConfirm,
//                    content = string.Format(KLocalization.GetLocalString(52107), cardDatas[0].itemName, cardDatas[1].itemName, cardDatas[2].itemName),
//                    onCancel = OnCompClickOnCancel,
//                });
//            }
        
//        }
//        private void OnCompClickOnConfirm()
//        {
//            int cardlenth = 0;
//            for (int i = 0; i < cardDatas.Length; i++)
//            {
//                if (cardDatas[i] != null)
//                {
//                    cardlenth++;
//                }
//            }
//            int[] cardIdList = new int[cardlenth];
//            for (int i = 0; i < cardIdList.Length; i++)
//            {
//                cardIdList[i] = cardDatas[i].bindItemId;
//            }
//            if (cardIdList.Length > 0)
//            {
//                KFoster.Instance.FosterComposeCard(cardIdList, FosterComposeCard);
//            }
//        }
//        private void FosterComposeCard(int code, string message, object data)
//        {
//            if (code == 0)
//            {
//                InitCardList();
//                RefreshCompPanel();
//                var list = data as ArrayList;
//                if (list != null)
//                {
//                    foreach (var item in list)
//                    {
//                        if (item is S2CFosterCardComposeResult)
//                        {
//                            var result = (S2CFosterCardComposeResult)item;
//                            var card = KItemManager.Instance.GetFosterCardbyBindId(result.DestItemTableId);
//                            ShowCompCardReward(card);
//                            OpenWindow<MessageBox>(new MessageBox.Data {
//                                title = KLocalization.GetLocalString(28210),
//                                content = string.Format(KLocalization.GetLocalString(52108), card.itemName),
//                                onConfirm = FosterComposeCardOnConfim,
//                            });
//                        }
//                    }
//                }
//            }
//        }
//        private void FosterComposeCardOnConfim()
//        {
//            Debug.Log("确认");
//        }
//        private void OnCompClickOnCancel()
//        {

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

