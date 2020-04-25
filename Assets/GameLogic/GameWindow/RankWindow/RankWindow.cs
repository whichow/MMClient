using Game.Match3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class RankWindow : KUIWindow
    {
        private Button _closeBtn;
        private Toggle[] _toggles;
        private int _rankType;

        public RankWindow()
           : base(UILayer.kNormal, UIMode.kSequenceRemove)
        {
            uiPath = "RankWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        public override void Awake()
        {
            _closeBtn = Find<Button>("Close");
            _closeBtn.onClick.AddListener(OnCloseBtnClick);
            _toggles = new Toggle[4];
            for (int i = 0; i < 4; i++)
                _toggles[i] = Find<Toggle>("TabTogGroup/Tog" + (i + 1));
            foreach (Toggle tog in _toggles)
                tog.onValueChanged.AddListener((bool blSelect) => { if (blSelect) OnRankTypeChange(tog); });
            _rankType = RankType.Checkpoint;
            InitView();
        }

        public override void AddEvents()
        {
            base.AddEvents();
            RankDataModel.Instance.AddEvent(RankEvent.RankData, OnRankData);
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendZan, OnZanPlayer);
        }


        public override void RemoveEvents()
        {
            base.RemoveEvents();
            RankDataModel.Instance.RemoveEvent(RankEvent.RankData, OnRankData);
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendZan, OnZanPlayer);
        }

        private void OnZanPlayer()
        {
            _zanImg.overrideSprite = _zanImg.sprites[0];
        }

        private void OnRankTypeChange(Toggle tog)
        {
            switch (tog.name)
            {
                case "Tog1":
                    _rankType = RankType.Checkpoint;
                    _back02.overrideSprite = _back02.sprites[0];
                    break;
                case "Tog2":
                    _rankType = RankType.Charm;
                    _back02.overrideSprite = _back02.sprites[1];
                    break;
                case "Tog3":
                    _rankType = RankType.OuQi;
                    _back02.overrideSprite = _back02.sprites[2];
                    break;
                case "Tog4":
                    _rankType = RankType.Fabulous;
                    _back02.overrideSprite = _back02.sprites[3];
                    break;
            }

            if (_rankType == RankType.OuQi)
            {
                List<CatDataVO> lstCat = new List<CatDataVO>();
                lstCat = CatDataModel.Instance.GetCatDataByType(0);
                int catScore = 0;
                int catId = 0;
                for (int i = 0; i < lstCat.Count; i++)
                {
                    if (lstCat[i].mCatScore > catScore)
                    {
                        catScore = lstCat[i].mCatScore;
                        catId = lstCat[i].mCatInfo.Id;
                    }
                }
                GameApp.Instance.GameServer.ReqRankData(_rankType, 1, 20, catId);
            }
            else
            {
                if (!RankDataModel.Instance.mDictRankData.ContainsKey(_rankType))
                    GameApp.Instance.GameServer.ReqRankData(_rankType, 1, 20);
                else
                    RefreshView(_rankType);
            }
        }

        public override void OnEnable()
        {
            OnRankTypeChange(_toggles[_rankType - 1]);
        }

        private void OnRankData()
        {
            RefreshView(_rankType);
        }
    }
}
