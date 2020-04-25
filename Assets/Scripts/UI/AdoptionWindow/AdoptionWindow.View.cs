//// ***********************************************************************
//// Assembly         : Unity
//// Author           : LiMuChen
//// Created          : #DATA#
////
//// Last Modified By : LiMuChen
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "AdoptionWindow.View" company="moyu"></copyright>
//// <summary></summary>
//// ***********************************************************************
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    partial class AdoptionWindow
//    {
//        #region Field


//        private Button _confirm;
//        private Button _cancel;
//        private Button _btnFosterageCat;
//        private Button _btnFosterCard;
//        private Button _btnFriendFoster;
//        private Transform[] _transCatParent;
//        private Text _textFosterName;
//        private Transform _transCatGroup;
//        private Image _cardIcon;
//        private Text _cardName;
//        private KUIImage _imageRaity;
//        private GameObject _goCard;
//        private Text _textAdopName;
//        #endregion





//        #region Method

//        public void InitView()
//        {

//            _confirm = Find<Button>("Panel/ButtonHarvest");
//            _confirm.onClick.AddListener(this.OnConfirmClick);
//            _cancel = Find<Button>("Panel/ButtonBack");
//            _cancel.onClick.AddListener(this.OnCancelClick);
//            _btnFosterageCat = Find<Button>("Panel/CardGroup/Card1");
//            _btnFosterageCat.onClick.AddListener(OnFosterageCatBtnClick);
//            _btnFosterCard = Find<Button>("Panel/CardGroup/Card2");

//            _btnFriendFoster = Find<Button>("Panel/CardGroup/Card3");
//            _btnFriendFoster.onClick.AddListener(OnFriendFosterBtnClick);
//            _textFosterName = Find<Text>("Panel/FriendName/Text");
//            _transCatGroup = Find<Transform>("Panel/CatGroup");
//            _cardIcon = Find<Image>("Panel/CardGroup/Card2/CardIcon/Icon");
//            _cardName = Find<Text>("Panel/CardGroup/Card2/Item/Text");
//            _imageRaity = Find<KUIImage>("Panel/CardGroup/Card2/CardIcon/ImageLight");
//            _goCard = Find("Panel/CardGroup/Card2/CardIcon");
//            _transCatParent = new Transform[_transCatGroup.childCount];
//            for (int i = 0; i < _transCatParent.Length; i++)
//            {
//                _transCatParent[i] = Find<Transform>("Panel/CatGroup/Cat" + (i + 1));
//            }
//            _textAdopName = Find<Text>("Panel/FriendName/Text");
//        }

//        public void RefreshView()
//        {


//            StartCoroutine(FillElements());

//            RefreshPanel();
//        }
//        private void RefreshPanel()
//        {
//            if (_windowdata.playerId == KUser.SelfPlayer.id)
//            {
//                _textAdopName.text = KUser.SelfPlayer.nickName+"的寄养所";
//                _btnFosterageCat.gameObject.SetActive(true);
//                _btnFosterCard.onClick.RemoveAllListeners();
//                _btnFosterCard.onClick.AddListener(OnFosterCardBtnClcik);
//                _confirm.gameObject.SetActive(true);
//                var cardData = KItemManager.Instance.GetFosterCardbyBindId(KFoster.Instance.cardId);
//                if (cardData != null)
//                {
//                    _cardIcon.overrideSprite = KIconManager.Instance.GetItemIcon(cardData.iconName);
//                    _cardName.text = cardData.itemName;
//                    ShowRarity(cardData.rarity);
//                    _goCard.SetActive(true);
//                }
//                else
//                {
//                    _goCard.SetActive(false);
//                }

//            }
//            else
//            {
//                _confirm.gameObject.SetActive(false);
//                _btnFosterageCat.gameObject.SetActive(false);
//                _btnFosterCard.onClick.RemoveAllListeners();
//                _btnFosterCard.onClick.AddListener(OnFriendFosterCardBtnClcik);
//                //_btnFriendFoster.onClick.AddListener(OnFriendFosterBtnClick);
//                Debug.Log(KFoster.Instance.friendCardId + "!!!!!");
//                var cardData = KItemManager.Instance.GetFosterCardbyBindId(KFoster.Instance.friendCardId);
//                if (cardData != null)
//                {
//                    _cardIcon.overrideSprite = KIconManager.Instance.GetItemIcon(cardData.iconName);
//                    _cardName.text = cardData.itemName;
//                    ShowRarity(cardData.rarity);
//                    _goCard.SetActive(true);
//                }
//                else
//                {
//                    _goCard.SetActive(false);
//                }

//            }
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
//        public void ShowModel(bool isActive)
//        {
//            _transCatGroup.gameObject.SetActive(isActive);
//        }
//        private IEnumerator FillElements()
//        {
//            RefreshCatModel();

//            for (int i = 0; i < _transCatParent.Length; i++)
//            {
//                if (i < GetFosterInfor().Count)
//                {
//                    var cat = GetFosterInfor()[i].cat;
//                    var modelObj = cat.model;
//                    if (modelObj != string.Empty)
//                    {
//                        GameObject modelPrefab = cat.GetUIModel();
//                        if (modelPrefab != null)
//                        {
//                            //modelPrefab.layer = _transCatParent[i].gameObject.layer;
//                            modelPrefab.transform.SetParent(_transCatParent[i], false);
//                            //modelPrefab.GetComponent<Renderer>().sortingOrder = _textFosterName.canvas.sortingOrder + 1;
//                            modelPrefab.AddComponent<Button>().onClick.AddListener(OnCatBtnClick);
//                        }
//                        else
//                        {
//                            Debug.Log("加载猫咪失败" + modelObj);
//                            continue;
//                        }
//                    }
//                }
//                else
//                {

//                }
//            }
//            _transCatGroup.gameObject.SetActive(true);
//            yield return null;
//        }
//        private void OnCatBtnClick()
//        {
//            Debug.Log("猫咪被点击");
//        }
//        public override void OnDisable()
//        {
//            RefreshCatModel();
//        }

//        private void RefreshCatModel()
//        {
//            for (int i = 0; i < _transCatParent.Length; i++)
//            {
//                if (_transCatParent[i].childCount > 0)
//                {
//                    Object.Destroy(_transCatParent[i].GetChild(0).gameObject);

//                }
//            }

//        }

//        #endregion
//    }
//}

