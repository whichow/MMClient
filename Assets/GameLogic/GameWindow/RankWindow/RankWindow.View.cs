using Game.Build;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class RankWindow
    {
        private UIList _rankList;
        private int _curRankType;
        private Text _rankText;
        private Text _playerName;
        private Text _playerScore;
        private Image _icon;
        private KUIImage _imgRankType;
        private GameObject _img1;
        private GameObject _img2;
        private GameObject _img3;
        private GameObject _img4;
        private RectTransform _rectFriend;
        private Button _friendBtn;
        private Button _addBtn;
        private Button _visiteBtn;
        private Button _roomBtn;
        private Button _zanBtn;
        private RankItemInfo _curRankItemInfo;
        private KUIImage _back02;
        private KUIImage _zanImg;


        public void InitView()
        {
            _rankList = Find<UIList>("RankObj/List");
            _rankList.SetRenderHandler(RenderHandler);
            _rankList.SetPointerHandler(PointerHandler);
            _rankText = Find<Text>("SelfItem/Ranking/Num");
            _playerName = Find<Text>("SelfItem/txt_name");
            _playerScore = Find<Text>("SelfItem/txt_score");
            _icon = Find<Image>("SelfItem/img_head/ImageMask/Iconhead");
            _imgRankType = Find<KUIImage>("SelfItem/img_type");
            _img1 = Find("SelfItem/Ranking/Img1");
            _img2 = Find("SelfItem/Ranking/Img2");
            _img3 = Find("SelfItem/Ranking/Img3");
            _img4 = Find("SelfItem/Ranking/Img4");
            _rectFriend = Find<RectTransform>("RankObj/AddFriendObj");
            _friendBtn = Find<Button>("RankObj/AddFriendObj/Panel");
            _friendBtn.onClick.AddListener(OnCloseBtn);
            _addBtn = Find<Button>("RankObj/AddFriendObj/ButtonGrid/btn_add");
            _visiteBtn = Find<Button>("RankObj/AddFriendObj/ButtonGrid/btn_visite");
            _roomBtn = Find<Button>("RankObj/AddFriendObj/ButtonGrid/btn_room");
            _zanBtn = Find<Button>("RankObj/AddFriendObj/ButtonGrid/btn_zan");
            _back02 = Find<KUIImage>("Back/Back02");
            _zanImg = Find<KUIImage>("RankObj/AddFriendObj/ButtonGrid/btn_zan/ImageIcon");

            _curRankType = 0;
        }

        private void RenderHandler(UIListItem item, int index)
        {
            RankItemInfo vo = item.dataSource as RankItemInfo;
            if (vo == null)
                return;
            item.GetComp<Text>("Ranking/Num").text = vo.Rank.ToString();
            item.GetComp<Text>("txt_name").text = vo.PlayerName;
            if (_curRankType == RankType.OuQi)
                item.GetComp<Text>("txt_score").text = vo.PlayerValue[1].ToString();
            else
                item.GetComp<Text>("txt_score").text = vo.PlayerValue[0].ToString();
            Image head = item.GetComp<Image>("img_head/ImageMask/Iconhead");
            HeadIconUtils.SetHeadIcon(vo.PlayerHead, vo.PlayerId, head);
            item.GetGameObject("Ranking/Img1").SetActive(vo.Rank == 1);
            item.GetGameObject("Ranking/Img2").SetActive(vo.Rank == 2);
            item.GetGameObject("Ranking/Img3").SetActive(vo.Rank == 3);
            item.GetGameObject("Ranking/Num").SetActive(vo.Rank > 3);
            item.GetGameObject("img_type").SetActive(_curRankType != RankType.Checkpoint);
            OnKuiImg(item.GetComp<KUIImage>("img_type"));
            _addBtn.onClick.RemoveAllListeners();
            _addBtn.onClick.AddListener(() => { OnAddBtn(); });
            _roomBtn.onClick.RemoveAllListeners();
            _roomBtn.onClick.AddListener(() => { OnRoomBtn(); });
            _visiteBtn.onClick.RemoveAllListeners();
            _visiteBtn.onClick.AddListener(OnVisiteBtn);
            _zanBtn.onClick.RemoveAllListeners();
            _zanBtn.onClick.AddListener(OnZanBtn);
        }

        private void OnKuiImg(KUIImage image)
        {
            switch (_curRankType)
            {
                case RankType.Checkpoint:
                    break;
                case RankType.Charm:
                    image.overrideSprite = image.sprites[1];
                    break;
                case RankType.OuQi:
                    image.overrideSprite = image.sprites[0];
                    break;
                case RankType.Fabulous:
                    image.overrideSprite = image.sprites[2];
                    break;
            }
        }

        private void PointerHandler(UIListItem item, int index)
        {
            _curRankItemInfo = item.dataSource as RankItemInfo;
            if (_curRankItemInfo == null)
                return;
            _rectFriend.gameObject.SetActive(_curRankItemInfo.PlayerId != PlayerDataModel.Instance.mPlayerData.mPlayerID);
            _rectFriend.position = item.transform.position;
            if (_curRankType == RankType.OuQi)
            {
                if (_curRankItemInfo.PlayerValue.Count > 2 && _curRankItemInfo.PlayerValue[2] > 0)
                    _zanImg.overrideSprite = _zanImg.sprites[0];
                else
                    _zanImg.overrideSprite = _zanImg.sprites[1];
            }
            else
            {
                if (_curRankItemInfo.PlayerValue.Count > 1 && _curRankItemInfo.PlayerValue[1] > 0)
                    _zanImg.overrideSprite = _zanImg.sprites[0];
                else
                    _zanImg.overrideSprite = _zanImg.sprites[1];
            }
        }

        private void RefreshView(int type)
        {
            if (!active)
                return;
            _rectFriend.gameObject.SetActive(false);
            _curRankType = type;
            _imgRankType.gameObject.SetActive(_curRankType != RankType.Checkpoint);
            OnKuiImg(_imgRankType);
            List<RankItemInfo> lstRankItem = RankDataModel.Instance.mDictRankData[_curRankType];
            lstRankItem.Sort((x, y) => x.Rank.CompareTo(y.Rank));
            _rankList.DataArray = lstRankItem;
            _rankText.text = RankDataModel.Instance.mDictSelfRank[_curRankType].ToString();
            _playerName.text = PlayerDataModel.Instance.mPlayerData.mName;
            if (_curRankType == RankType.OuQi)
                _playerScore.text = RankDataModel.Instance.mDictSelfValue2[_curRankType].ToString();
            else
                _playerScore.text = RankDataModel.Instance.mDictSelfValue1[_curRankType].ToString();
            HeadIconUtils.SetHeadIcon(PlayerDataModel.Instance.mPlayerData.mHead, PlayerDataModel.Instance.mPlayerData.mPlayerID, _icon);
            _img1.SetActive(RankDataModel.Instance.mDictSelfRank[_curRankType] == 1);
            _img2.SetActive(RankDataModel.Instance.mDictSelfRank[_curRankType] == 2);
            _img3.SetActive(RankDataModel.Instance.mDictSelfRank[_curRankType] == 3);
            _img4.SetActive(RankDataModel.Instance.mDictSelfRank[_curRankType] == 0);
            _rankText.gameObject.SetActive(RankDataModel.Instance.mDictSelfRank[_curRankType] > 3);
        }

        private void OnCloseBtn()
        {
            _rectFriend.gameObject.SetActive(false);
        }

        private void OnAddBtn()
        {
            GameApp.Instance.GameServer.ReqAddFriendById(_curRankItemInfo.PlayerId);
        }

        private void OnVisiteBtn()
        {
            BuildingManager.Instance.VisitPlayer(_curRankItemInfo.PlayerId, true);
            CloseWindow(this);
        }

        private void OnRoomBtn()
        {
            GameApp.Instance.GameServer.ReqSpaceOther(_curRankItemInfo.PlayerId);
        }

        private void OnZanBtn()
        {
            if (_curRankItemInfo.PlayerValue.Count > 1 && _curRankItemInfo.PlayerValue[1] <= 0)
                GameApp.Instance.GameServer.ReqZanPlayer(_curRankItemInfo.PlayerId);
        }
    }
}
