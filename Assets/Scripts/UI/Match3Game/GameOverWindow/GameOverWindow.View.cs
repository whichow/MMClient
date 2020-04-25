using Game;
using Game.DataModel;
using Game.Match3;
using Msg.ClientMessage;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class GameOverWindow
    {

        private Button nextBtn;
        private Button backBtn;
        private GameObject winObj;
        private GameObject loseObj;

        private Toggle friendsRankToggle;
        private Toggle worldRankToggle;

        private Text nextBtnText;
        private Text levelText;
        private Text myRank;
        private Text scoreText;
        private Text catAdditionScore;
        private Text catAdditionGold;

        private KUIImage catIcon;

        private KUIGrid grid;
        private KUIList rewardList;

        private GameObject loseGo;

        private SkeletonGraphic winAnimator;
        private GameOverWindowData windowData;
        private string[] starCountStr = new string[] { "Bg_Sx_win1_0", "Bg_Sx_win1_1", "Bg_Sx_win1_2", "Bg_Sx_win1_3" };



        public void InitView()
        {
            nextBtn = transform.Find("VictGold/VicGoldBack01/Button").GetComponent<Button>();
            nextBtnText = nextBtn.transform.Find("Text").GetComponent<Text>();
            winObj = transform.Find("VictGold/Obj/Bg_Sx_win1").gameObject;
            loseObj = transform.Find("VictGold/Obj/Bg_Sx_lose").gameObject;
            backBtn = transform.Find("VictGold/backBtn").GetComponent<Button>();
            levelText = transform.Find("VictGold/levelID").GetComponent<Text>();
            grid = transform.Find("VictGold/VicGoldBack02/VicGoldBack02B/Scroll View").GetComponent<KUIGrid>();
            loseGo = transform.Find("VictGold/VicGoldBack01/lose").gameObject;
            friendsRankToggle = Find<Toggle>("VictGold/VicGoldBack02/VicGoldBack02B/ToggleGroup/Toggle1");
            worldRankToggle = Find<Toggle>("VictGold/VicGoldBack02/VicGoldBack02B/ToggleGroup/Toggle2");
            myRank = Find<Text>("VictGold/VicGoldBack02/Title");
            scoreText = Find<Text>("VictGold/VicGoldBack01/lose/MyScore");
            catAdditionGold = Find<Text>("VictGold/VicGoldBack01/lose/HighestScore01/Text");
            catAdditionScore = Find<Text>("VictGold/VicGoldBack01/lose/HighestScore02/Text");
            catIcon = Find<KUIImage>("VictGold/VicGoldBack01/lose/VicGoldBack01Back");
            rewardList = Find<KUIList>("VictGold/VicGoldBack01/lose/Prop");

            friendsRankToggle.onValueChanged.AddListener(OnRankToggleClick);
            worldRankToggle.onValueChanged.AddListener(OnRankToggleClick);

            if (grid != null)
            {
                grid.uiPool.itemTemplate.AddComponent<M3RankItem>();
            }

            winAnimator = winObj.GetComponent<SkeletonGraphic>();
            backBtn.onClick.AddListener(OnReturnBackClick);
            nextBtn.onClick.AddListener(OnNextBtnClick);
        }
        public void RefreshView()
        {
            if (windowData == null)
                return;
            ShowTitle();
            ShowButton();
            RefreshRank();
            if (windowData.gameOverType == 0)
            {
                loseGo.SetActive(false);
            }
            else
            {
                ShowCatAddition();
                ShowScore();
                ShowReward();
            }

        }

        private void ShowReward()
        {
            if (rewardList == null)
                return;
            //Debug.Log(windowData.firstItems.Count);
            //Debug.Log(windowData.threeStarItems.Count);
            //Debug.Log(windowData.items.Count);

            int firstCount = windowData.firstItems.Count;
            int threeCount = windowData.threeStarItems.Count;
            int noramlCount = windowData.items.Count;
            int totalCount = firstCount + noramlCount + threeCount;
            GameOverRewardItem item = null;
            for (int i = 0; i < totalCount; i++)
            {
                int index = i;
                item = (GameOverRewardItem)rewardList.GetItem(index);
                if (index < firstCount)
                {
                    item.ShowReward(windowData.firstItems[index], RewardType.First);
                }
                else if (index < threeCount + firstCount)
                {
                    item.ShowReward(windowData.threeStarItems[index - firstCount], RewardType.ThreeStar);
                }
                else if (index < threeCount + firstCount + noramlCount)
                {
                    item.ShowReward(windowData.items[index - firstCount - threeCount], RewardType.Normal);
                }
            }
        }

        private void OnRankToggleClick(bool arg0)
        {
            RankType type = rankType;
            if (friendsRankToggle.isOn)
            {
                type = RankType.Friends;
            }
            if (worldRankToggle.isOn)
            {
                type = RankType.World;
            }

            if (type != rankType)
            {
                rankType = type;
                RefreshRank();
            }
        }

        private void OnReturnBackClick()
        {
            if (M3Config.isEditor)
            {
                M3GameManager.Instance.OnExitGame(ExitGameType.Editor);
            }
            else
            {
                M3GameManager.Instance.OnExitGame(ExitGameType.None);
            }
        }

        private void ShowCatAddition()
        {
            if (windowData.cat != null)
            {
                if (catAdditionGold != null)
                    catAdditionGold.text = windowData.addCoin.ToString();
                if (catAdditionScore != null)
                    catAdditionScore.text = M3GameManager.Instance.modeManager.catAdditionScore.ToString();
                if (catIcon != null)
                {
                    //Debug.Log(windowData.cat.photo);
                    catIcon.overrideSprite = XTable.CatXTable.GetByID(windowData.cat.shopId).GetPhotoSprite();
                    catIcon.ShowGray(false);

                }
            }
            else
            {
                catIcon.ShowGray(true);
                if (catAdditionGold != null)
                    catAdditionGold.text = "0";
                if (catAdditionScore != null)
                    catAdditionScore.text = "0";
            }
        }

        private void ShowScore()
        {
            scoreText.text = string.Format(KLocalization.GetLocalString(51027), windowData.score);
        }
        private void ShowTitle()
        {
            levelText.text = XTable.LevelXTable.GetByID(windowData.stageId).Name.ToString();
        }

        private void ShowButton()
        {
            if (windowData.gameOverType == 1)
            {
                nextBtnText.text = KLocalization.GetLocalString(51051);
                winObj.SetActive(true);
                loseObj.SetActive(false);
                if (winAnimator != null)
                    winAnimator.AnimationState.SetAnimation(0, starCountStr[windowData.starCount], false);
                //    winAnimator.AnimationName = starCountStr[windowData.starCount];
                KSoundManager.StopMusic();
                M3GameManager.Instance.soundManager.PlayWin();
            }
            else
            {
                nextBtnText.text = KLocalization.GetLocalString(51052);
                winObj.SetActive(false);
                loseObj.SetActive(true);
                KSoundManager.StopMusic();
                M3GameManager.Instance.soundManager.PlayLose();
            }
        }

        public void RefreshRank()
        {
            switch (rankType)
            {
                case RankType.Friends:
                    ShowFriendsRankList();
                    break;
                case RankType.World:
                    ShowWorldRankList();
                    break;
                default:
                    break;
            }

        }

        private void GetWorldRankData(int arg1, string arg2, object arg3)
        {

            if (arg1 == 0)
            {
                //CurrentRankContentLst = new List<RankingListItemInfo>();
                //if (KRankManager.Instance.Dict_allRankLst.ContainsKey(2))
                //{
                //    CurrentRankContentLst = KRankManager.Instance.Dict_allRankLst[2];
                //    if (CurrentRankContentLst != null && (int)rankType == 2)
                //    {
                //        grid.uiPool.SetItemDatas(CurrentRankContentLst.ToArray());
                //        grid.RefillItems();
                //    }
                //}

                //var myRank = KRankManager.Instance.Dict_myRankIDInEveryRank[2].SelfRank;
                //ShowMySelfRank(myRank);
            }
        }

        private void ShowMySelfRank(int rank)
        {
            myRank.text = KLocalization.GetLocalString(51019) + rank.ToString();
        }
        public void ShowFriendsRankList()
        {
            if (windowData != null)
            {
                List<GameOverFriendRankData> list = new List<GameOverFriendRankData>();
                for (int i = 0; i < windowData.friendRank.Count; i++)
                {
                    int index = i;
                    list.Add(new GameOverFriendRankData()
                    {
                        friendRankInfo = windowData.friendRank[index],
                        Rank = index,
                    });
                }

                grid.uiPool.SetItemDatas(list);
                grid.RefillItems();

                for (int i = 0; i < list.Count; i++)
                {
                    if (PlayerDataModel.Instance.mPlayerData.mPlayerID == list[i].friendRankInfo.PlayerId)
                    {
                        ShowMySelfRank(list[i].Rank + 1);
                    }
                }
            }
        }

        public void ShowWorldRankList()
        {
            //KRankManager.Instance.PullOneRankingList(2, 1, KRankManager.int_const_eachTimeGetGameoverNum, windowData.stageId, GetWorldRankData);
        }
        private void OnNextBtnClick()
        {

            if (!M3Config.isEditor)
            {
                if (windowData.gameOverType == 1)
                    M3GameManager.Instance.OnExitGame(ExitGameType.NextLevel);
                else
                    M3GameManager.Instance.OnExitGame(ExitGameType.Reload);
            }
            else
            {
                M3GameManager.Instance.OnExitGame(ExitGameType.Editor);
            }
        }
    }
}