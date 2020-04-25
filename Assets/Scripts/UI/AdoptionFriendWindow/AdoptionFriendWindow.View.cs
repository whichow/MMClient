//// ***********************************************************************
//// Assembly         : Unity
//// Author           : LiMuChen
//// Created          : #DATA#
////
//// Last Modified By : LiMuChen
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "AdoptionCatWindow.View" company="moyu"></copyright>
//// <summary></summary>
//// ***********************************************************************
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using K.Extension;
//namespace Game.UI
//{
//    partial class AdoptionFriendWindow
//    {
//        #region Field


//        private Button _btnClose;
//        private Dropdown _sortDropdown;
//        private List<Toggle> _rarityToggles;
//        private KUIGrid _layoutElementPool;
//        private KFoster.Slot[] _myFostFriend;
//        private KFoster.Slot[] _friendsByMyFost;
//        private Transform[] _transCats;
//        private Text[] _textTime;
//        private GameObject _myCatGroup;
//        private Transform FriendCat;
//        private KCat _addMyCatInFriend;
//        #endregion

//        #region Method

//        public void InitView()
//        {
//            _sortDropdown = Find<Dropdown>("Panel/Myself/Dropdown");
//            _sortDropdown.onValueChanged.AddListener(this.OnSortTypeChange);
//            _btnClose = Find<Button>("Panel/Close");
//            _btnClose.onClick.AddListener(this.OnCancelClick);
//            var toggleGroup = Find<ToggleGroup>("Panel/Myself/Tab View/ToggleGroup");
//            _rarityToggles = new List<Toggle>(toggleGroup.GetComponentsInChildren<Toggle>());
//            for (int i = 0; i < _rarityToggles.Count; i++)
//            {
//                _rarityToggles[i].onValueChanged.AddListener(this.OnPageChange);
//            }
//            _myCatGroup = Find("Panel/Myself");
//            _layoutElementPool = Find<KUIGrid>("Panel/Myself/Scroll View");
//            if (_layoutElementPool)
//            {
//                _layoutElementPool.uiPool.itemTemplate.AddComponent<AdoptionFriendCatItem>();
//            }
//            FriendCat = Find<Transform>("Panel/Friends/FriendCat");
//            var MyCatByFriend = Find<Transform>("Panel/Friends/MyCatFriend");
//            _transCats = new Transform[FriendCat.childCount + MyCatByFriend.childCount];
//            _myFostFriend = new KFoster.Slot[MyCatByFriend.childCount];
//            _textTime = new Text[_transCats.Length];
//            _friendsByMyFost = new KFoster.Slot[FriendCat.childCount];
//            for (int i = 0; i < _transCats.Length; i++)
//            {
//                if (i < 3)
//                {
//                    _transCats[i] = Find<Transform>("Panel/Friends/FriendCat/CardBig" + (i + 1));

//                }
//                else
//                {
//                    _transCats[i] = Find<Transform>("Panel/Friends/MyCatFriend/CardBig" + (i + 1));
//                }
//            }
//            for (int i = 0; i < _textTime.Length; i++)
//            {
//                _textTime[i] = _transCats[i].Find("Cat/Time").GetComponent<Text>();
//            }
//        }
//        public void RefreshTransCats()
//        {
//            InitCatList();
//            if (_friendsByMyFost != null)
//            {
//                for (int i = 0; i < _friendsByMyFost.Length; i++)
//                {
//                    if (_friendsByMyFost[i] != null)
//                    {
//                        _transCats[i].GetComponent<Button>().onClick.RemoveAllListeners();
//                        ShowCat(_transCats[i], _friendsByMyFost[i]);
//                        _transCats[i].Find("Cat").gameObject.SetActive(true);
//                        _transCats[i].Find("Empty").gameObject.SetActive(false);
//                        //_transCats[i].Find("Cat/Del").GetComponent<Button>().onClick.AddListener(OnePointColliderObject);
//                    }
//                    else
//                    {
          
//                        _transCats[i].Find("Cat").gameObject.SetActive(false);
//                        _transCats[i].Find("Empty").gameObject.SetActive(true);
//                        _transCats[i].Find("TopImage/PlayerName").GetComponent<Text>().text = string.Empty;
//                    }

//                }
//            }
//            if (_myFostFriend != null)
//            {
//                for (int i = 0; i < _myFostFriend.Length; i++)
//                {
//                    if (_myFostFriend[i] != null)
//                    {
//                        ShowCat(_transCats[i + 3], _myFostFriend[i]);
//                        _transCats[i + 3].Find("Cat").gameObject.SetActive(true);
//                        _transCats[i + 3].Find("Empty").gameObject.SetActive(false);
//                    }
//                    else
//                    {
//                        _transCats[i + 3].Find("Cat").gameObject.SetActive(false);
//                        _transCats[i + 3].Find("Empty").gameObject.SetActive(true);
//                        _transCats[i + 3].Find("TopImage/PlayerName").GetComponent<Text>().text=string.Empty;
//                    }
//                }
//            }

//        }


//        public void InitCatList()
//        {
//            for (int i = 0; i < _friendsByMyFost.Length; i++)
//            {
//                _friendsByMyFost[i] = null;
//            }
//            for (int i = 0; i < _myFostFriend.Length; i++)
//            {
//                _myFostFriend[i] = null;
//            }
//            if (_windowdata.playerId == KUser.SelfPlayer.id)
//            {
//                for (int i = 0; i < KFoster.Instance.mixSlots.Count; i++)
//                {
//                    if (KFoster.Instance.mixSlots[i].slotOwner != null)
//                    {
//                        AddFriendByMyFost(KFoster.Instance.mixSlots[i]);
//                    }
//                    else if (KFoster.Instance.mixSlots[i].catOwner != null)
//                    {
//                        AddSoltMyFostFriend(KFoster.Instance.mixSlots[i]);
//                    }
//                }
//            }
//            else
//            {
//                for (int i = 0; i < KFoster.Instance.friendMixSlots.Count; i++)
//                {
//                    if (KFoster.Instance.friendMixSlots[i] != null)
//                    {
//                        AddSoltMyFostFriend(KFoster.Instance.friendMixSlots[i]);

//                    }
//                }
//            }

//        }
//        private void AddSoltMyFostFriend(KFoster.Slot slot)
//        {
//            for (int i = 0; i < _myFostFriend.Length; i++)
//            {
//                if (_myFostFriend[i] == null)
//                {
//                    _myFostFriend[i] = slot;
//                    return;
//                }
//            }
//        }
//        private void AddFriendByMyFost(KFoster.Slot slot)
//        {

//            for (int i = 0; i < _friendsByMyFost.Length; i++)
//            {
//                if (_friendsByMyFost[i] == null)
//                {
//                    _friendsByMyFost[i] = slot;
//                    return;
//                }
//            }
//        }
//        public void ShowCat(Transform trans, KFoster.Slot slot)
//        {
//            Image _imageIcon;
//            Text _textGrad;
//            KUIImage _imageFrame;
//            KUIImage _imageFlag;
//            KUIImage[] _starImages;

//            KUIImage _imageColor;
//            Text _textName;
//            Text _textPlayerName;
//            var fish = trans.Find("Cat/Fish");
//            _starImages = new KUIImage[fish.childCount];
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i] = fish.GetChild(i).GetComponent<KUIImage>();
//            }
//            _textPlayerName = trans.Find("Cat/TopImage/PlayerName").GetComponent<Text>();
//            _textName = trans.Find("Cat/Name").GetComponent<Text>();
//            _imageIcon = trans.Find("Cat").GetComponent<Image>();
//            _textGrad = trans.Find("Cat/Level/Text").GetComponent<Text>();
//            _imageFlag = trans.Find("Cat/Level").GetComponent<KUIImage>();
//            _imageFrame = trans.Find("Cat/Light").GetComponent<KUIImage>();
//            _imageColor = trans.Find("Cat/Fish").GetComponent<KUIImage>();
//            if (slot.slotOwner != null)
//            {
//                _textPlayerName.text = slot.slotOwner.nickName;
//            }
//            else if (slot.catOwner != null)
//            {
//                _textPlayerName.text = slot.catOwner.nickName;
//            }
//            ShowRarity(_imageFlag, _imageFrame, slot.cat.rarity);
//            _textName.text = slot.cat.name;
//            _textGrad.text = slot.cat.grade.ToString();
//            _imageIcon.overrideSprite = KIconManager.Instance.GetCatFull(slot.cat.photo);
//            ShowStar(_starImages, slot.cat.star);
//            ShowColor(_imageColor, slot.cat.mainColor);
//        }
//        public void AddCat(KCat cat)
//        {
//            //KFoster.Instance.AddSelfCat(cat.catId,AddSelfCatCallBack);
//            if (_windowdata.playerId == KUser.SelfPlayer.id)
//            {

//            }
//            else
//            {
//                _addMyCatInFriend = cat;
//                int catNum = 0;
//                for (int i = 0; i < _myFostFriend.Length; i++)
//                {
//                    if (_myFostFriend[i] != null)
//                    {
//                        catNum++;
//                    }
//                }
//                if (KFoster.Instance.friendFosteredSlotCount > 0 && KFoster.Instance.friendFosteredSlotCount - catNum > 0)
//                {
//                    OpenWindow<MessageBox>(new MessageBox.Data
//                    {
//                        content = KLocalization.GetLocalString(52113),
//                        onConfirm = OnAddSelfCatInFriendConfirm,
//                        onCancel = OnCanel,
//                    });
//                }
//                else
//                {
//                    ToastBox.ShowText(KLocalization.GetLocalString(52114));
//                }
//            }
//        }
//        private void OnCanel()
//        {

//        }
//        private void OnAddSelfCatInFriendConfirm()
//        {
//            KFoster.Instance.AddSelfCatInFriend(_windowdata.playerId, _addMyCatInFriend.catId, AddSelfCatInFriendCallBack);
//        }
//        private void AddSelfCatInFriendCallBack(int code, string message, object data)
//        {
//            if (code == 0)
//            {
//                RefreshView();

//            }
//        }
//        public void RemoveCat(KCat cat)
//        {
//            KFoster.Instance.RemoveSelfCat(cat.catId, RemoveSelfCatCallBack);
//        }
//        private void RemoveSelfCatCallBack(int code, string message, object data)
//        {
//            if (code == 0)
//            {
//                RefreshView();

//            }
//        }
//        //public bool isHaveNull()
//        //{
//        //    for (int i = 0; i < _catsList.Length; i++)
//        //    {
//        //        if (_catsList[i] == null)
//        //        {
//        //            return true;
//        //        }
//        //    }
//        //    return false;
//        //}
//        public void ShowStar(KUIImage[] _starImages, int star)
//        {
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i].ShowGray(i >= star);
//            }
//        }
//        public void ShowColor(KUIImage _colorImage, int color)
//        {
//            if (color == (int)KCat.Color.fRed)
//            {
//                _colorImage.ShowSprite(0);
//            }
//            else if (color == (int)KCat.Color.fYellow)
//            {
//                _colorImage.ShowSprite(1);
//            }
//            else if (color == (int)KCat.Color.fBlue)
//            {
//                _colorImage.ShowSprite(3);
//            }
//            else if (color == (int)KCat.Color.fGreen)
//            {
//                _colorImage.ShowSprite(2);
//            }
//            else if (color == (int)KCat.Color.fPurple)
//            {
//                _colorImage.ShowSprite(5);
//            }
//            else if (color == (int)KCat.Color.fBrown)
//            {
//                _colorImage.ShowSprite(4);
//            }
//        }
//        public void ShowRarity(KUIImage _specialFlagImage, KUIImage _specialFrameImage, int rarity)
//        {
//            if (rarity == 2)
//            {
//                _specialFlagImage.ShowSprite(1);
//                _specialFrameImage.gameObject.SetActive(true);
//                _specialFrameImage.ShowSprite(1);
//            }
//            else if (rarity == 3)
//            {
//                _specialFlagImage.ShowSprite(2);
//                _specialFrameImage.gameObject.SetActive(true);
//                _specialFrameImage.ShowSprite(2);
//            }
//            else if (rarity == 4)
//            {
//                _specialFlagImage.ShowSprite(3);
//                _specialFrameImage.gameObject.SetActive(true);
//                _specialFrameImage.ShowSprite(3);
//            }
//            else
//            {
//                _specialFlagImage.ShowSprite(0);
//                _specialFrameImage.gameObject.SetActive(false);
//                _specialFrameImage.ShowSprite(0);
//            }
//        }
//        public void RefreshView()
//        {

//            RefreshTransCats();

//            RefreshPanel();
//            RefreshTransSolt();
//        }
//        private void RefreshTransSolt()
//        {
//            int topCount = 0;
//            int btmCount = 0;
//            if (_windowdata.playerId == KUser.SelfPlayer.id)
//            {
//                topCount = KFoster.Instance.friendInSelfSlotCount;
//                btmCount = KFoster.Instance.selfInFriendSlotCount;
//                for (int i = 0; i < 3; i++)
//                {
//                    if (i < btmCount)
//                    {
//                        _transCats[i].GetComponent<Button>().onClick.AddListener(OnOpenFriendClick);
//                        _transCats[i].Find("Black").gameObject.SetActive(false);
//                    }
//                    else
//                    {
//                        _transCats[i].GetComponent<Button>().onClick.RemoveAllListeners();
//                        _transCats[i].Find("Black").gameObject.SetActive(true);
//                    }

//                }
//                for (int i = 0; i < 3; i++)
//                {
//                    if (i < topCount)
//                    {
//                        _transCats[i + 3].Find("Black").gameObject.SetActive(false);
//                    }
//                    else
//                    {
//                        _transCats[i + 3].Find("Black").gameObject.SetActive(true);
//                    }
//                }
//            }
//            else
//            {
//                topCount = KFoster.Instance.friendFosteredSlotCount;
//                for (int i = 0; i < 3; i++)
//                {
//                    if (i < topCount)
//                    {
//                        _transCats[i + 3].Find("Black").gameObject.SetActive(false);
//                    }
//                    else
//                    {
//                        _transCats[i + 3].Find("Black").gameObject.SetActive(true);
//                    }

//                }
//            }

//        }
//        private void RefreshPanel()
//        {
//            if (_windowdata.playerId == KUser.SelfPlayer.id)
//            {
//                _myCatGroup.gameObject.SetActive(false);
//                FriendCat.gameObject.SetActive(true);
//            }
//            else
//            {
//                _sortDropdown.value = (int)sortType;
//                StartCoroutine(FillElements());
//                _myCatGroup.gameObject.SetActive(true);
//                FriendCat.gameObject.SetActive(false);
//            }
//        }
//        private IEnumerator FillElements()
//        {
//            _layoutElementPool.ClearItems();
//            var cats = GetCatInfos();
//            var datas = new ArrayList(cats.Length);
//            for (int i = 0; i < cats.Length; i++)
//            {
//                //datas.Add(new CatInfoWindow.WindowData
//                //{
//                //    catIndex = i,
//                //    catArray = cats,
//                //});
//            }
//            _layoutElementPool.uiPool.SetItemDatas(datas);
//            _layoutElementPool.RefillItems(0);
//            yield return null;
//        }
//        private string GetOnToggle()
//        {
//            foreach (var toggle in _rarityToggles)
//            {
//                if (toggle.isOn)
//                {
//                    return toggle.name;
//                }
//            }
//            return null;
//        }
//        public override void UpdatePerSecond()
//        {
//            if (_friendsByMyFost != null)
//            {
//                for (int i = 0; i < _friendsByMyFost.Length; i++)
//                {
//                    if (_friendsByMyFost[i] != null)
//                    {
//                        _textTime[i].text = TimeExtension.ToTimeString(_friendsByMyFost[i].remainTime);
//                    }
//                }
//            }
//            if (_myFostFriend != null)
//            {
//                for (int i = 0; i < _myFostFriend.Length; i++)
//                {
//                    if (_myFostFriend[i] != null)
//                    {
//                        _textTime[i + 3].text = TimeExtension.ToTimeString(_myFostFriend[i].remainTime);
//                    }
//                }
//            }

//        }




//        #endregion
//    }


//}

