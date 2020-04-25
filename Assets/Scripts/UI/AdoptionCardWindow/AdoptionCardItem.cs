//// ***********************************************************************
//// Assembly         : Unity
//// Author           : LiMuChen
//// Created          : #DATA#
////
//// Last Modified By : LiMuChen
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "AdoptionMyCatItem" company="moyu"></copyright>
//// <summary></summary>
//// ***********************************************************************
//using K.Extension;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//namespace Game.UI
//{
//    public class AdoptionCardItem : KUIItem, IPointerClickHandler
//    {
//        #region Field
//        private Image _imageCardIcon;
//        private Text _textTime;
//        private Text _textExp;
//        private Text _textDima;
//        private Text _textCardName;
//        private KUIImage _imageRaity;
//        private KItemFosterCard _cardData;
//        private Transform _transOkBlack;
//        private Text _textCardNum;
//        #endregion

//        #region Interface
//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        {
//            if (KUIWindow.GetWindow<AdoptionCardWindow>()._toggleEquipment.isOn)
//            {
//                if (KFoster.Instance.cardId != 0)
//                {
//                    return;
//                } 
//                if (_transOkBlack.gameObject.activeSelf)
//                {
               
//                    _transOkBlack.gameObject.SetActive(false);
//                    KUIWindow.GetWindow<AdoptionCardWindow>().RefreshCard(null, this);
//                }
//                else
//                {
                
//                    _transOkBlack.gameObject.SetActive(true);
//                    KUIWindow.GetWindow<AdoptionCardWindow>().RefreshCard(_cardData, this);
//                }
//            }
//            else 
//            {
//                if (!_transOkBlack.gameObject.activeSelf)
//                {
//                    KUIWindow.GetWindow<AdoptionCardWindow>().AddCard(_cardData, this);
//                }
//                else
//                {
                 
//                    KUIWindow.GetWindow<AdoptionCardWindow>().RemoveCard(_cardData, this);
//                }
//            }
       
         
//        }
//        #endregion

//        #region Method
//        public void ShowFosterCard(KItemFosterCard cardData)
//        {
//            _cardData = cardData;
//            _textCardNum.text = cardData.curCount.ToString();
//            _imageCardIcon.overrideSprite = KIconManager.Instance.GetItemIcon(cardData.iconName);
//            _textCardName.text = cardData.itemName;
//            _textExp.text = KItemManager.Instance.GetItem(cardData.rewards[0].itemID).itemName + "+" + cardData.rewards[0].itemCount + "/h";
//            if (cardData.rewards.Length >= 2)
//            {
//                _textDima.text = KItemManager.Instance.GetItem(cardData.rewards[1].itemID).itemName + "+" + cardData.rewards[1].itemCount + "/h";
//                _textDima.gameObject.SetActive(true);
//            }
//            else
//            {
//                _textDima.gameObject.SetActive(false);
//            }
//            _textTime.text = TimeExtension.ToTimeString(cardData.fosterTime);
//            ShowRarity(cardData.rarity);
//        }
//        public void RefreshOk(bool isActivity)
//        {
//            _transOkBlack.gameObject.SetActive(isActivity);
//        }
//        public void ShowRarity(int rarity)
//        {
//            if (rarity == 2)
//            {
//                _imageRaity.gameObject.SetActive(true);
//                _imageRaity.ShowSprite(1);
//            }
//            else if (rarity == 3)
//            {
//                _imageRaity.gameObject.SetActive(true);
//                _imageRaity.ShowSprite(2);
//            }
//            else if (rarity == 4)
//            {
//                _imageRaity.gameObject.SetActive(true);
//                _imageRaity.ShowSprite(3);
//            }
//            else if (rarity == 5)
//            {
//                _imageRaity.gameObject.SetActive(true);
//                _imageRaity.ShowSprite(4);
//            }
//            else
//            {
//                _imageRaity.gameObject.SetActive(false);
//                _imageRaity.ShowSprite(0);
//            }
//        }
//        #endregion




//        #region Unity
//        void Awake()
//        {
//            _imageCardIcon = Find<Image>("Item/CardIcon/Icon");
//            _textTime = Find<Text>("Item/Time2");
//            _textExp = Find<Text>("Item/EXP/exp1");
//            _textDima = Find<Text>("Item/Item/item1");
//            _textCardName = Find<Text>("Item/CardName");
//            _imageRaity = Find<KUIImage>("Item/CardIcon/ImageLight");
//            _transOkBlack = Find<Transform>("Item/OK");
//            _textCardNum = Find<Text>("Item/CardIcon/Count");
//        }


//        void Update()
//        {

//        }

//        #endregion
//    }


//}

