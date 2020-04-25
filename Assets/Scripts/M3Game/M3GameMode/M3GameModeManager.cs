using Game.UI;
using Msg.ClientMessage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public enum GameModeEnum
    {
        None = 0,
        TimeMode = 1,//计时
        StepMode = 2,//步数
    }

    public enum GameTargetEnum
    {
        None = 0,
        Collection = 1,//收集玩法 
        Score = 2,//积分玩法
    }

    public enum ReburnType
    {
        None = 470,
        Reburn_1 = 471,
        Reburn_2 = 472,
        Reburn_3 = 473,
        Reburn_4 = 474,
        Reburn_5 = 475,
        Reburn_6 = 476,
        Reburn_7 = 477,
        Reburn_8 = 478,
        Reburn_9 = 479,
        Reburn_10 = 480,
    }

    public class M3GameModeManager
    {
        public GameModeEnum mode = GameModeEnum.None;

        public GameTargetEnum target = GameTargetEnum.None;

        private M3GameMode gameModeCtrl;

        private ReburnType reburnCount = 0;

        private bool finishTask;

        public bool isGameEnd;

        public bool timeOut;

        public bool isBonusEnd = false;

        public int score;

        public int[] starScore;

        public int star;

        public int superTimeRate = 1;

        public int roundScore = 0;

        public List<int> roundElement = new List<int>();

        public int catAdditionScore = 0;

        public int catAdditionGold = 0;

        public M3GameModeManager()
        {
            Init();
        }

        public M3GameMode GameModeCtrl
        {
            get
            {
                return gameModeCtrl;
            }
            set
            {
                gameModeCtrl = value;
            }
        }

        public void Init()
        {
            M3LevelData data = M3GameManager.Instance.level;
            score = 0;
            star = 0;
            finishTask = false;
            isGameEnd = false;
            timeOut = false;
            reburnCount = ReburnType.Reburn_1;
            if (data.gameMode == 1)
            {
                mode = GameModeEnum.TimeMode;
                GameModeCtrl = new M3GameTimeMode();
            }
            else if (data.gameMode == 2)
            {
                mode = GameModeEnum.StepMode;
                GameModeCtrl = new M3GameStepMode();
            }
            else
                return;
            if (data.gameTarget == 1)
                target = GameTargetEnum.Collection;
            else if (data.gameTarget == 2)
                target = GameTargetEnum.Score;
            GameModeCtrl.Init(this, data, target);

            starScore = data.star;
        }

        public void Update()
        {
            GameModeCtrl.Execute(Time.fixedDeltaTime);
        }

        public void EnterSuperTime(int rate)
        {
            superTimeRate = rate;
            gameModeCtrl.OnEnterSuperTime();
        }

        public void AddReburnCount()
        {
            reburnCount = (reburnCount >= ReburnType.Reburn_10) ? (ReburnType.Reburn_10) : (reburnCount + 1);
        }

        public ReburnType GetReburnCount()
        {
            return reburnCount;
        }

        public void AddScore(int value, int type, Vector3 position)
        {
            if (gameModeCtrl.isSuperTime)
            {
                value *= superTimeRate;
            }
            score += value;

            roundScore += value;
            CheckStar();
            UpdateScore();
            if (value <= 0)
                return;
            gameModeCtrl.ProcessScore(score);
            if (!M3GameManager.Instance.isAutoAI)
                M3FxManager.Instance.ShowScoreText(value, type, position);
        }

        public int ProcessComboScore(int sourceScore, ItemSpecial special, int comboCount, bool doComboEffect = false)
        {
            float score = sourceScore;
            float ratio = 1;
            int pet_bonus = 0;
            if (M3GameManager.Instance.catManager != null)
                pet_bonus = M3GameManager.Instance.catManager.GetMatchBonus(); //宠物加成，之后需要传参
            switch (special)
            {

                case ItemSpecial.fNormal:
                    ratio = 1;
                    break;
                case ItemSpecial.fRow:
                    ratio = 1.5f;
                    break;
                case ItemSpecial.fColumn:
                    ratio = 1.5f;
                    break;
                case ItemSpecial.fArea:
                    ratio = 2.0f;
                    break;
                case ItemSpecial.fColor:
                    ratio = 2.5f;
                    break;
                case ItemSpecial.fDoubleCol:
                    ratio = 3f;
                    break;
                case ItemSpecial.fDoubleRow:
                    ratio = 3f;
                    break;
                case ItemSpecial.fRowAndCol:
                    ratio = 3f;
                    break;
                case ItemSpecial.fRow2Area:
                    ratio = 3.5f;
                    break;
                case ItemSpecial.fCol2Area:
                    ratio = 3.5f;
                    break;
                case ItemSpecial.fDoubleArea:
                    ratio = 4f;
                    break;
                case ItemSpecial.fDoubleColor:
                    ratio = 5f;
                    break;
                default:
                    ratio = 1;
                    break;
            }
            //Debug.Log(comboCount + "_" + pet_bonus);
            if (doComboEffect)
            {
                //score = sourceScore * (1.0f + 0.2f * (ratio - 1.0f + Mathf.Max(comboCount - 1, 0)) + pet_bonus * 0.01f);
                score = sourceScore * (1.0f + pet_bonus * 0.01f);
                catAdditionScore += (int)(sourceScore * (pet_bonus * 0.01f));
            }
            else
            {
                //score = sourceScore * (1.0f + 0.2f * (ratio - 1.0f) + pet_bonus * 0.01f);
                score = sourceScore * (1.0f + pet_bonus * 0.01f);
                catAdditionScore += (int)(sourceScore * (pet_bonus * 0.01f));
            }
            return (int)score;
        }

        private void UpdateScore()
        {
            M3GameEvent.DispatchEvent(M3FightEnum.UpdateScore, score, star);
        }

        private void CheckStar()
        {
            if (star < 3 && score >= starScore[star])
            {
                star++;
                if (!M3GameManager.Instance.isAutoAI)
                {
                    KUIWindow.GetWindow<M3GameUIWindow>().ShowStarGet(star);
                }
            }
        }

        public int GetStep()
        {
            return gameModeCtrl.totalSetps;
        }

        public int GetScore()
        {
            return gameModeCtrl.targetScore;
        }

        public bool IsLevelFinish()
        {
            if (mode == GameModeEnum.StepMode)
                return gameModeCtrl.isTargetFinish;
            else if (mode == GameModeEnum.TimeMode)
                return timeOut;
            return false;
        }

        public bool IsLevelEnd()
        {
            if (mode == GameModeEnum.StepMode)
                return gameModeCtrl.isEnd;
            else if (mode == GameModeEnum.TimeMode)
                return timeOut;
            return false;
        }

        /// <summary>
        /// 是否已完成 目标为积分时，达到积分也不算完成，待步数耗尽为止（算星数）
        /// </summary>
        /// <returns></returns>
        public bool IsStepModeLevelEnd()
        {
            if (GameModeCtrl.gameTarget == GameTargetEnum.Collection)
                return gameModeCtrl.isEnd || gameModeCtrl.isTargetFinish;
            else if (GameModeCtrl.gameTarget == GameTargetEnum.Score)
                return gameModeCtrl.isEnd;
            return gameModeCtrl.isEnd || gameModeCtrl.isTargetFinish;
        }

        public bool IsTimeModeLevelEnd()
        {
            return timeOut /*|| gameModeCtrl.isFinish*/;
        }

        public void EndLevel()
        {
            isGameEnd = true;
            finishTask = gameModeCtrl.isTargetFinish;
            if (finishTask && M3GameManager.Instance.isAutoAI)
            {
                Debug.Log("目标完成" + gameModeCtrl.currentStep);
                return;
            }
            if (finishTask && mode == GameModeEnum.StepMode)
            {
                FrameScheduler.Instance().Add(30, delegate
                {
                    M3GameManager.Instance.bonusManager.StartBonus();
                });
            }
            else if (!finishTask)
            {
                M3GameManager.Instance.ScreenLock = true;
                KUIWindow.OpenWindow<M3PayStepWindow>();
                //ShowGameOver();
            }
        }

        public void AddStepCallBack(int count)
        {
            if (count > 0)
            {
                isGameEnd = false;
                gameModeCtrl.isTargetFinish = false;
                gameModeCtrl.isEnd = false;
                gameModeCtrl.AddStep(count);
                M3GameManager.Instance.ScreenLock = false;
            }
        }

        public void ShowGameOver()
        {
            if (M3GameManager.Instance.isAutoAI)
                return;
            M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.GameOver);

            if (M3Config.isEditor)
            {
                M3GameManager.Instance.OnExitGame(ExitGameType.Editor);
                return;
            }

            M3GameManager.Instance.isGameOver = true;

            if (M3GameManager.Instance.isGameoverNeedTutorial)
                return;
            M3GameManager.Instance.isReady = false;

            SendGameOverMessage();
            //SendGameOverMessage(delegate (object data) { ProcessData(data); });
        }

        public void SendGameOverMessage(/*Action<object> action*/)
        {
            //var levelData = new C2SStagePass
            //{
            //    StageId = LevelDataModel.Instance.CurrLevelID,
            //    Stars = Mathf.Max(1, this.star),
            //    Score = this.score,
            //    Result = (finishTask ? 1 : 0),
            //};
            //var propList = M3GameManager.Instance.propManager.GetUsedPropID();
            //for (int i = 0; i < propList.Count; i++)
            //{
            //    levelData.Items.Add(new ItemInfo
            //    {
            //        ItemCfgId = propList[i],
            //        ItemNum = 1,
            //    });
            //}
            //Debug.Log("FinishLevel  " + star);
            //KUser.FinishLevel(levelData, (code, message, data) =>
            //{
            //    if (code == 0)
            //    {
            //        if (action != null)
            //            action(data);
            //    }
            //    else
            //    {
            //        M3GameManager.Instance.OnExitGame(ExitGameType.None);
            //    }
            //});

            List<ItemInfo> items = new List<ItemInfo>();
            var propList = M3GameManager.Instance.propManager.GetUsedPropID();
            for (int i = 0; i < propList.Count; i++)
            {
                items.Add(new ItemInfo
                {
                    ItemCfgId = propList[i],
                    ItemNum = 1,
                });
            }
            GameApp.Instance.GameServer.ReqStagePass(LevelDataModel.Instance.CurrLevelID, Mathf.Max(1, this.star), score, (finishTask ? 1 : 0), items);
        }

        //private void ProcessData(object data)
        //{
        //    var arrList = data as ArrayList;
        //    Debug.Log(arrList.Count);
        //    if (arrList != null)
        //    {
        //        for (int i = 0; i < arrList.Count; i++)
        //        {
        //            if (arrList[i] is S2CStagePass)
        //            {
        //                var message = (S2CStagePass)arrList[i];
        //                KUIWindow.OpenWindow<GameOverWindow>(new GameOverWindowData(
        //                    message.StageId,
        //                    message.Result,
        //                    message.Score,
        //                    message.TopScore,
        //                    message.Stars,
        //                    message.GetCoin,
        //                    message.GetItems,
        //                    message.GetItemsFirst,
        //                    message.GetItems3Star,
        //                    message.GetCats,
        //                    message.GetBuildings,
        //                    message.FriendItems,
        //                    message.CatExtraAddCoin,
        //                    M3GameManager.Instance.catManager.GameCat
        //                    ));
        //            }
        //        }
        //    }
        //}

        public Dictionary<int, int> CloneTarget()
        {
            if (GameModeCtrl.targetDic == null)
                return null;
            Dictionary<int, int> targetDic = new Dictionary<int, int>();
            foreach (var item in GameModeCtrl.targetDic)
            {
                targetDic.Add(item.Key, item.Value);
            }
            return targetDic;
        }

        public void ResetVars()
        {
            roundScore = 0;
            roundElement.Clear();
        }

    }
}