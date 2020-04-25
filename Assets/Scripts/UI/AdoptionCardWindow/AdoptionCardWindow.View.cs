//// ***********************************************************************
//// Assembly         : Unity
//// Author           : LiMuChen
//// Created          : #DATA#
////
//// Last Modified By : LiMuChen
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "AdoptionCardWindow.View" company="moyu"></copyright>
//// <summary></summary>
//// ***********************************************************************
//using K.Extension;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    partial class AdoptionCardWindow
//    {
//        #region Field
//        private Button _btnClose;
//        public  Toggle _toggleEquipment;
//        private Toggle _toggleComp;
//        private Transform _transCardPanel;
//        private Transform _transCompPanel;
//        private KUIItemPool _layoutElementPool;
//        private Text _textBtn;
//        private Button _btnOkBtn;
//        //激活寄养卡
//        private Transform _transCard;
//        private Image _imageCardIcon;
//        private KUIImage _imageRaity;
//        private Text _textTime;
//        private Text _textExp;
//        private Text _textDima;
//        private Text _textCardName;
//        private KItemFosterCard cardData;
//        private AdoptionCardItem oldCardItem;
//        private KItemFosterCard nowCard;
//        //合成寄养卡
//        private Transform[] _transcardList;
//        private KItemFosterCard[] cardDatas;
//        private AdoptionCardItem[] items;
//        private Transform _transCardReward;
//        private Image _imageRewardIcon;
//        private KUIImage _imageRewardLight;
//        #endregion

//        #region Method

//        public void InitView()
//        {
//            _btnClose = Find<Button>("Close");
//            _btnClose.onClick.AddListener(OnCloseBtnClick);
//            _toggleEquipment = Find<Toggle>("Left/Tab View/ToggleGroup/Toggle1");
//            _toggleEquipment.onValueChanged.AddListener(OnToggleValueChanged);
//            _toggleComp = Find<Toggle>("Left/Tab View/ToggleGroup/Toggle2");
//            _toggleComp.onValueChanged.AddListener(OnToggleValueChanged);
//            _transCardPanel = Find<Transform>("Right/CardImage");
//            _transCompPanel = Find<Transform>("Right/ActPanel");
//            _textBtn = Find<Text>("Right/ButtonAct/Text");
//            _btnOkBtn = Find<Button>("Right/ButtonAct");
//            _transCard = Find<Transform>("Right/CardImage/Card");
//            _imageCardIcon = Find<Image>("Right/CardImage/Card/CardIcon/Icon");
//            _imageRaity = Find<KUIImage>("Right/CardImage/Card/CardIcon/ImageLight");
//            _textTime = Find<Text>("Right/CardImage/Card/Time2");
//            _textExp = Find<Text>("Right/CardImage/Card/EXP/exp1");
//            _textDima = Find<Text>("Right/CardImage/Card/Item/item1");
//            _textCardName = Find<Text>("Right/CardImage/Card/CardName");
//            //合成寄养卡
//            var cardGroup = Find<Transform>("Right/ActPanel/CardGroup");
//            _transcardList = new Transform[cardGroup.childCount];
//            cardDatas = new KItemFosterCard[cardGroup.childCount];
//            items = new AdoptionCardItem[cardGroup.childCount];
//            _transCardReward = Find<Transform>("Right/ActPanel/ImageBack1/CardReward");
//            _imageRewardIcon = Find<Image>("Right/ActPanel/ImageBack1/CardReward/Icon");
//            _imageRewardLight = Find<KUIImage>("Right/ActPanel/ImageBack1/CardReward/ImageLight");
//            for (int i = 0; i < _transcardList.Length; i++)
//            {
//                _transcardList[i] = Find<Transform>("Right/ActPanel/CardGroup/Image"+(i+1));
//            }
//            _layoutElementPool = Find<KUIItemPool>("Left/Compose/Scroll View");
//            if (_layoutElementPool && _layoutElementPool.elementTemplate)
//            {
//                _layoutElementPool.elementTemplate.gameObject.AddComponent<AdoptionCardItem>();
//            }
//        }

//        public void RefreshView()
//        {
//            RefreshCard();
//            StartCoroutine(FillElements());
//            RefreshPanel();

//        }
//        private IEnumerator FillElements()
//        {
//            _layoutElementPool.Clear();
//            var cards = KItemManager.Instance.GetFosterCards();
//            for (int i = 0; i < cards.Length; i++)
//            {
//                if (cards[i].curCount>0)
//                {
//                    var element = _layoutElementPool.SpawnElement();
//                    var cardItem = element.GetComponent<AdoptionCardItem>();
//                    cardItem.ShowFosterCard(cards[i]);
//                }
//            }
//            yield return null;
//        }
//        private void RefreshPanel()
//        {
//            if (_toggleEquipment.isOn)
//            {
//                ShowCompCardReward(null);
//                InitCardList();
//                RefreshCompPanel();
//                _transCardPanel.gameObject.SetActive(true);
//                _transCompPanel.gameObject.SetActive(false);
//                if (KFoster.Instance.cardId!=0)
//                {
//                    var cardata = KItemManager.Instance.GetFosterCardbyBindId(KFoster.Instance.cardId);
//                    RefreshCard(cardata, null);
//                    _textBtn.text = KLocalization.GetLocalString(58214);
//                    _btnOkBtn.onClick.RemoveAllListeners();
//                    _btnOkBtn.onClick.AddListener(OnCanel);
//                }
//                else
//                {
//                    RefreshCard(null, oldCardItem);
//                    _textBtn.text = KLocalization.GetLocalString(58211);
//                    _btnOkBtn.onClick.RemoveAllListeners();
//                    _btnOkBtn.onClick.AddListener(OnEnmpClick);
//                }
//            }
//            else
//            {
//                InitCardList();
//                RefreshCompPanel();
//                if (oldCardItem!=null)
//                {
//                    RefreshCard(null, oldCardItem);
//                }
//                _transCardPanel.gameObject.SetActive(false);
//                _transCompPanel.gameObject.SetActive(true);
//                _textBtn.text = KLocalization.GetLocalString(58212);
//                _btnOkBtn.onClick.RemoveAllListeners();
//                _btnOkBtn.onClick.AddListener(OnCompClick);
//            }
//        }
//        private void RefreshCompPanel()
//        {
//            for (int i = 0; i < cardDatas.Length; i++)
//            {
//                if (cardDatas[i]!=null)
//                {
//                    Image _imageIcon;
//                    KUIImage _imageLight;
//                    _imageIcon = _transcardList[i].Find("CardIcon/Icon").GetComponent<Image>();
//                    _imageLight = _transcardList[i].Find("CardIcon/ImageLight").GetComponent<KUIImage>();
//                    ShowRarity(_imageLight, cardDatas[i].rarity);
//                    _imageIcon.overrideSprite = KIconManager.Instance.GetItemIcon(cardDatas[i].iconName);
//                    _transcardList[i].Find("CardIcon").gameObject.SetActive(true);
//                }
//                else
//                {
//                    _transcardList[i].Find("CardIcon").gameObject.SetActive(false);
//                }
//            }
//        }
//        public void AddCard(KItemFosterCard card,AdoptionCardItem item)
//        {
       
//            ShowCompCardReward(null);
//            item.RefreshOk(true);
//            int indx = 0;
//            for (int i = 0; i < cardDatas.Length; i++)
//            {
//                if (isHaveNull())
//                {
//                    if (cardDatas[i] == null)
//                    {
//                        cardDatas[i] = card;
//                        items[i] = item;
//                        indx = i;
//                        break;
//                    }
//                    else
//                    {
//                        continue;
//                    }
//                }
//                else
//                {
//                    items[items.Length - 1].RefreshOk(false);
//                    cardDatas[cardDatas.Length - 1] = card;
//                    items[items.Length - 1] = item;
//                    indx = cardDatas.Length - 1;
//                    break;
//                }

//            }
//            RefreshCompPanel();
//        }
//        public void InitCardList()
//        {
//            for (int i = 0; i < cardDatas.Length; i++)
//            {
//                cardDatas[i] = null;
//            }
//            for (int i = 0; i < items.Length; i++)
//            {
//                if (items[i] != null)
//                {
//                    items[i].RefreshOk(false);
//                }
//                items[i] = null;
//            }
//        }
//        private void ShowCompCardReward(KItemFosterCard card)
//        {
//            if (card==null)
//            {
//                _transCardReward.gameObject.SetActive(false);
//                return;
//            }
//            _imageRewardIcon.overrideSprite = KIconManager.Instance.GetItemIcon(card.iconName);
//            ShowRarity(_imageRewardLight, card.rarity);
//            _transCardReward.gameObject.SetActive(true);
//        }
//        public bool isHaveNull()
//        {
//            for (int i = 0; i < cardDatas.Length; i++)
//            {
//                if (cardDatas[i] == null)
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
//        public void RemoveCard(KItemFosterCard card, AdoptionCardItem item)
//        {
      
//            ShowCompCardReward(null);
//            item.RefreshOk(false);
//            int indx = 0;
//            for (int i = 0; i < cardDatas.Length; i++)
//            {
//                if (cardDatas[i] != null && card != null)
//                {
//                    if (card.bindItemId == cardDatas[i].bindItemId)
//                    {
//                        indx = i;
//                        items[i] = null;
//                        cardDatas[i] = null;
//                    }
//                }
//            }
//            RefreshCompPanel();
//        }
    
//        public void RefreshCard(KItemFosterCard cardData,AdoptionCardItem item)
//        {
      
//            if (cardData==null)
//            {
//                _transCard.gameObject.SetActive(false);
//                if (item!=null)
//                {
//                    item.RefreshOk(false);
//                }
//                return;
//            }
//            nowCard = cardData;
//            _textTime.text = TimeExtension.ToTimeString(cardData.fosterTime);
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
//            if (oldCardItem!=null)
//            {
//                oldCardItem.RefreshOk(false);
//            }
//            if (item!=null)
//            {
//                item.RefreshOk(true);
//            }
//            oldCardItem = item;
//            ShowRarity(_imageRaity,cardData.rarity);
//            _transCard.gameObject.SetActive(true);
//        }
//        private void RefreshCard()
//        {
//            if (KFoster.Instance.cardId != 0)
//            {
//                cardData = KItemManager.Instance.GetFosterCardbyBindId(KFoster.Instance.cardId);
//                _imageCardIcon.overrideSprite = KIconManager.Instance.GetItemIcon(cardData.iconName);
//                _textCardName.text = cardData.itemName;
//                _textExp.text = KItemManager.Instance.GetItem(cardData.rewards[0].itemID).itemName + "+" + cardData.rewards[0].itemCount + "/h";
//                if (cardData.rewards.Length >= 2)
//                {
//                    _textDima.text = KItemManager.Instance.GetItem(cardData.rewards[1].itemID).itemName + "+" + cardData.rewards[1].itemCount + "/h";
//                    _textDima.gameObject.SetActive(true);
//                }
//                else
//                {
//                    _textDima.gameObject.SetActive(false);
//                }

//                ShowRarity(_imageRaity,cardData.rarity);
//                _transCard.gameObject.SetActive(true);
//            }
//            else
//            {
//                _transCard.gameObject.SetActive(false);
//            }
//        }
//        public void ShowRarity(KUIImage _imageRaity, int rarity)
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
//        public override void UpdatePerSecond()
//        {
//            if (cardData != null)
//            {
//                _textTime.text = TimeExtension.ToTimeString(cardData.fosterTime);
//            }
//        }
//    }
//}

