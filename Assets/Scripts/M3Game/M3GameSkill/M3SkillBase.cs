
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class M3SkillBase
    {
        /// <summary>
        /// 技能类型
        /// </summary>
        public int skillType;
        /// <summary>
        /// 技能操作类型
        /// </summary>
        public SkillOperationType skillOperationType;

        protected List<M3Item> itemList;
        protected KCat m_cat;

        public M3SkillBase()
        {
        }

        public virtual void InitSkill(KCat cat)
        {
            m_cat = cat;
        }

        /// <summary>
        /// 使用技能前
        /// </summary>
        public virtual void BeforeUseSkill()
        {
            itemList = GetSkillAffectItemList();
        }

        /// <summary>
        /// 技能是否可以使用
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckCanUseSkill()
        {
            return true;
        }

        /// <summary>
        /// 获取技能可作用的元素列表
        /// </summary>
        /// <returns></returns>
        protected virtual List<M3Item> GetSkillAffectItemList()
        {
            List<M3Item> list = new List<M3Item>();
            M3Item tmp;
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    tmp = M3ItemManager.Instance.gridItems[i, j];
                    if (tmp != null && tmp.itemInfo.GetElement() != null
                        && tmp.itemInfo.GetElement().eName != M3ElementType.FishElement
                        && tmp.itemInfo.GetElement().isObstacle == false)
                    {
                        list.Add(tmp);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取技能目标元素列表
        /// </summary>
        /// <returns></returns>
        protected virtual List<M3Item> GetSkillTargetItemList()
        {
            List<M3Item> list = new List<M3Item>();
            list.Add(itemList[M3Supporter.Instance.GetRandomInt(0, itemList.Count)]);
            return list;
        }

        /// <summary>
        /// 获取随机元素
        /// </summary>
        /// <returns></returns>
        protected virtual M3Item GetRandomItem()
        {
            M3Item item = itemList[M3Supporter.Instance.GetRandomInt(0, itemList.Count)];
            return item;
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnUseSkill(M3UseSkillArgs args)
        {
            M3GameManager.Instance.catManager.Operation.OnSkillUsed();

            M3GameManager.Instance.catManager.CatBehaviour.Skill(false, delegate (KCatBehaviour.State currentState)
            {
                M3GameManager.Instance.catManager.CatBehaviour.SkillPost(false, delegate (KCatBehaviour.State currentState2)
                {
                    M3GameManager.Instance.catManager.CatBehaviour.Idle();
                });
            });
        }

        /// <summary>
        /// 技能使用后
        /// </summary>
        public virtual void AfterUseSkill()
        {
            itemList.Clear();
            M3GameManager.Instance.catManager.isReleasingSkill = false;
            M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
        }

        /// <summary>
        /// 取消技能使用
        /// </summary>
        public virtual void OnCancelUse()
        {
            itemList.Clear();
            M3GameManager.Instance.skillOperation = null;
            M3GameManager.Instance.skillLock = false;
            M3GameManager.Instance.catManager.isReleasingSkill = false;
            M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckEnergy);
        }

        /// <summary>
        /// 播放技能爆炸特效
        /// </summary>
        /// <param name="effectName"></param>
        /// <param name="item"></param>
        protected void PlaySkillBoom(string effectName, M3Item item)
        {
            var bFxBehaviour = M3FxManager.Instance.PlayCatSkillBoom(m_cat.GetSkillEffectName(effectName), M3Supporter.Instance.GetItemPositionByGrid(item.itemInfo.posX, item.itemInfo.posY));
            if (bFxBehaviour != null)
                bFxBehaviour.PlaySelfDestroyEffect();
        }

        /// <summary>
        /// 播放技能释放特效
        /// </summary>
        /// <param name="effectName"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="callback"></param>
        protected void PlaySkillEffect(string effectName, Vector3 from, Vector3 to, Action callback)
        {
            M3GameManager.Instance.catManager.EffectLauncher.DoEffect(new object[] { m_cat.GetSkillEffectName(effectName), to, to, from }, callback);
        }

    }

    public class M3UseSkillArgs
    {
        public KCat Cat { get; private set; }
        public int PosX { get; private set; }
        public int PosY { get; private set; }

        public M3UseSkillArgs(KCat cat, int x, int y)
        {
            this.Cat = cat;
            this.PosX = x;
            this.PosY = y;
        }

        public M3UseSkillArgs(KCat cat)
        {
            this.Cat = cat;
        }

    }
}