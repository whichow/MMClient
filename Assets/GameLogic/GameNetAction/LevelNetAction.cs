/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.5
 * 
 * Author:          Coamy
 * Created:	        2019/3/20 11:58:10
 * Description:     关卡
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.Match3;
using Game.UI;
using Msg.ClientMessage;
using System.Collections.Generic;

namespace Game
{
    public partial class GameServer
    {
        
        #region 关卡开始
        public void StageBeginRequest(int levelID, int catID, int[] itemIDs)
        {
            C2SStageBegin req = new C2SStageBegin();
            req.StageId = levelID;
            req.CatId = catID;
            if (itemIDs != null)
            {
                req.ItemIds.AddRange(itemIDs);
            }
            C2SRequest(req);
        }

        public void StageBeginResultHandler(S2CStageBeginResult obj)
        {
            LevelDataModel.Instance.EnterLevelScene();
        }
        #endregion

        #region 关卡通关
        public void ReqStagePass(int levelID, int star, int score, int result, List<ItemInfo> items)
        {
            C2SStagePass req = new C2SStagePass()
            {
                StageId = levelID,
                Stars = star,
                Score = score,
                Result = result,
            };
            if (items != null)
            {
                req.Items.AddRange(items);
            }
            C2SRequest(req);
        }

        public void ExeStagePass(S2CStagePass obj)
        {
            KUIWindow.OpenWindow<GameOverWindow>(new GameOverWindowData(
                obj.StageId,
                obj.Result,
                obj.Score,
                obj.TopScore,
                obj.Stars,
                obj.GetCoin,
                obj.GetItems,
                obj.GetItemsFirst,
                obj.GetItems3Star,
                obj.GetCats,
                obj.GetBuildings,
                obj.FriendItems,
                obj.CatExtraAddCoin,
                M3GameManager.Instance.catManager.GameCat
            ));
            LevelDataModel.Instance.SetLevelResult(obj.Result == 1);
        }
        #endregion

        #region 解锁章节
        /// <summary>
        /// 请求解锁三消关卡
        /// </summary>
        /// <param name="type">// 解锁方式 0时间解锁 1星星解锁 2钻石解锁 3请求好友</param>
        /// <param name="chapterId">章节ID</param>
        /// <param name="firendId">好友Id</param>
        public void ReqChapterUnlock(int type, int chapterId, int[] firendId)
        {
            C2SChapterUnlock req = new C2SChapterUnlock();
            req.ChapterId = chapterId;
            req.UnLockType = type;
            if (firendId != null)
            {
                req.FriendIds.AddRange(firendId);
            }
            C2SRequest(req);
        }

        public void ExeChapterUnlock(S2CChapterUnlock obj)
        {
            LevelDataModel.Instance.SetChapterUnlock(obj.MaxUnlockStageId);
        }
        #endregion
    }
}