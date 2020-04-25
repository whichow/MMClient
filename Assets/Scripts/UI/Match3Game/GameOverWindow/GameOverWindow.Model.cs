/** 
*FileName:     #SCRIPTFULLNAME# 
*Author:       #AUTHOR# 
*Version:      #VERSION# 
*UnityVersion：#UNITYVERSION#
*Date:         #DATE# 
*Description:    
*History: 
*/
using Game;
using Msg.ClientMessage;
using System.Collections.Generic;

namespace Game.UI
{
    public class GameOverWindowData
    {
        public int stageId;
        public int gameOverType = -1;
        public int score = 0;
        public int maxScore = 0;
        public int starCount = 0;
        public int coin = 0;
        public int addCoin = 0;
        public IList<ItemInfo> items;
        public IList<ItemInfo> firstItems;
        public IList<ItemInfo> threeStarItems;

        public IList<PlayerStageInfo> friendRank;
        public IList<CatInfo> cats;
        public IList<DepotBuildingInfo> buildings;
        public KCat cat;
        public int friendMyRank;

        public GameOverWindowData(int stage, int type, int s, int maxS, int star, int getCoin, IList<ItemInfo> getItems, IList<ItemInfo> getFirstItems, IList<ItemInfo> getThreeStarItems, IList<CatInfo> getCats, IList<DepotBuildingInfo> getBuildings, IList<PlayerStageInfo> fRank, int catCoin, KCat playerCat)
        {
            stageId = stage;
            gameOverType = type;
            score = s;
            maxScore = maxS;
            starCount = star;
            addCoin = catCoin;
            coin = getCoin;
            items = getItems;
            this.firstItems = getFirstItems;
            this.threeStarItems = getThreeStarItems;
            cats = getCats;
            buildings = getBuildings;
            friendRank = fRank;
            cat = playerCat;
            var myRank = new PlayerStageInfo()
            {
                Head = PlayerDataModel.Instance.mPlayerData.mHead,
                Lvl = PlayerDataModel.Instance.mPlayerData.mLevel,
                Name = PlayerDataModel.Instance.mPlayerData.mName,
                PlayerId = PlayerDataModel.Instance.mPlayerData.mPlayerID,
                Score = maxScore,
            };
            friendRank.Add(myRank);

        }
        public void SortFriendRankByScores()
        {
            //friendRank.Sort((y, x) => { return x.Score.CompareTo(y.Score); });
            Debuger.Log("Sort待处理");
        }
    }
    public partial class GameOverWindow
    {
        public enum RankType
        {
            Friends = 1,
            World = 2,
        }
        public enum RewardType
        {
            Normal,
            First,
            ThreeStar,
        }
        public RankType rankType;
        //public IList<RankingListItemInfo> CurrentRankContentLst { get; private set; }

        public bool IsWin
        {
            get
            {
                return windowData.gameOverType != 0;
            }
        }

        public void InitModel()
        {

        }

        public void RefreshModel()
        {
            windowData = (GameOverWindowData)data;
            windowData.SortFriendRankByScores();
            rankType = RankType.Friends;
        }
    }

    public class GameOverFriendRankData
    {
        public int Rank;
        public PlayerStageInfo friendRankInfo;
    }
}