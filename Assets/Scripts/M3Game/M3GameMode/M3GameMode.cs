using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class M3GameMode
    {
        /// <summary>
        /// 收集目标
        /// </summary>
        public Dictionary<int, int> targetDic;
        /// <summary>
        /// 目标积分
        /// </summary>
        public int targetScore;
        public int totalSetps = 0;
        public GameTargetEnum gameTarget;
        public M3GameModeManager manager;

        public bool isSuperTime = false;
        /// <summary>
        /// 目标是否完成
        /// </summary>
        public bool isTargetFinish;
        /// <summary>
        /// 步数是否用完
        /// </summary>
        public bool isEnd;


        /// <summary>
        /// 能量值
        /// </summary>
        private int energyValue = -1;
        /// <summary>
        /// 步数
        /// </summary>
        private int energyStepCount = -1;
        /// <summary>
        /// 个数
        /// </summary>
        private int energyCreateCount = 0;

        private int currentValue = 0;

        public int currentStep = 0;

        public float currentTime = 0;

        public int CurrentEnergyStepCount;

        public virtual void Init(M3GameModeManager m, M3LevelData data, GameTargetEnum target)
        {
            gameTarget = target;
            manager = m;
            isTargetFinish = false;
            isEnd = false;
            isSuperTime = false;
            switch (target)
            {
                case GameTargetEnum.None:
                    break;
                case GameTargetEnum.Collection:
                    targetDic = new Dictionary<int, int>();
                    foreach (var item in data.LevelTaskElementDic.Keys)
                    {
                        targetDic.Add(item, data.LevelTaskElementDic[item]);
                    }
                    break;
                case GameTargetEnum.Score:
                    targetScore = data.score;
                    break;
                default:
                    break;
            }
            energyValue = data.energyRate;
            energyStepCount = data.energyStepCount;
            energyCreateCount = data.energyCreateCount;
            currentStep = 0;
            currentTime = 0;
        }

        /// <summary>
        /// 增加移动步数
        /// </summary>
        /// <param name="step"></param>
        public virtual void ProcessSteps(int step)
        {
            if (!isEnd || !isTargetFinish)
            {
                CurrentEnergyStepCount++;
            }
            currentStep += step;
        }

        /// <summary>
        /// 散发能量符到元素上
        /// </summary>
        /// <returns></returns>
        public int CheckSendEnergyElement()
        {
            if (energyStepCount == 0) return 0;

            bool flag = (CurrentEnergyStepCount >= energyStepCount);
            int count = 0;
            if (flag && M3GameManager.Instance.catManager != null)
            {
                //M3GameManager.Instance.catManager.AddEnergy(energyValue2);
                var list = M3GameManager.Instance.GetAllBaseElementWithoutEnergy();
                count = Mathf.Min(energyCreateCount, list.Count);
                for (int i = 0; i < count; i++)
                {
                    int index = i;
                    M3Item item = list[M3Supporter.Instance.GetRandomInt(0, list.Count)];
                    item.itemInfo.GetElement().AddEnergy(energyValue);
                    list.Remove(item);
                }
                ResetVars();
                if (M3GameManager.Instance.soundManager != null)
                    M3GameManager.Instance.soundManager.PlaySendEnergy();
            }
            return count; ;
        }

        public virtual void AddStep(int num)
        {
        }
        public virtual void AddTime(int num)
        {
        }
        public virtual void OnEnterSuperTime()
        {

        }
        public virtual void AddTarget(List<int> eleIdList, Vector3 pos, Element ele, int count = 1)
        {
            if (gameTarget != GameTargetEnum.Collection)
                return;
            int id;
            for (int i = 0; i < eleIdList.Count; i++)
            {
                id = eleIdList[i];
                if (targetDic.ContainsKey(id))
                {
                    if (targetDic[id] > 0)
                    {
                        targetDic[id] = Mathf.Max(0, targetDic[id] - count);
                        manager.roundElement.Add(id);
                        M3GameEvent.DispatchEvent(M3FightEnum.EliminateElement, id, targetDic[id], pos, ele);
                        if (targetDic[id] == 0)
                            targetDic.Remove(id);
                        if (targetDic.Count == 0)
                        {
                            if (!isTargetFinish)
                                isTargetFinish = true;
                        }
                    }
                    else
                        continue;
                }
            }
        }

        public void SetTargetDic(Dictionary<int, int> dic)
        {
            if (targetDic != null)
            {
                targetDic.Clear();
                foreach (var item in dic)
                {
                    M3GameEvent.DispatchEvent(M3FightEnum.EliminateElement, item.Key, item.Value);
                    targetDic.Add(item.Key, item.Value);
                }
            }
        }

        public virtual void ProcessScore(int score)
        {
            if (gameTarget != GameTargetEnum.Score)
                return;
            if (score >= targetScore)
            {
                if (!isTargetFinish)
                    isTargetFinish = true;
            }
        }

        public virtual void Execute(float time)
        {
            currentTime += time;
        }

        public virtual void ResetVars()
        {
            CurrentEnergyStepCount = 0;
        }

    }
}