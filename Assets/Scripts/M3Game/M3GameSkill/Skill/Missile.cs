/** 
*FileName:     Missile.cs 
*Author:       HASEE 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-10-26 
*Description:    
*History: 
*/
using System;
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 向棋盘发射N个导弹，随机砸中目标（飞碟>章鱼>牢笼>雪块>冰块>云块>宝藏>毒液>棕毛球>灰毛球>猫窝>银币>能量块>礼物>附有能量的基础元素>暴击元素>水晶球>基础元素>特效）
    /// </summary>
    public class Missile : M3SkillBase
    {
        private int count = 0;
        bool hasZombie;

        public Missile()
        {
            skillType = (int)ESkillType.ShockWave;
            skillOperationType = SkillOperationType.None;
        }

        public override bool CheckCanUseSkill()
        {
            bool b = true;
            int skillParam = m_cat.GetSkillParam();
            if (itemList.Count < skillParam)
            {
                b = false;
            }
            return b;
        }

        public override void OnUseSkill(M3UseSkillArgs args)
        {
            count = m_cat.GetSkillParam();
            var list = FindTarget();
            if (list.Count == 0)
            {
                OnCancelUse();
            }
            else
            {
                base.OnUseSkill(args);

                M3GridManager.Instance.dropLock = true;

                if (hasZombie)
                {
                    M3GameManager.Instance.fishManager.zombie.StunZombie(1);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    int index = i;
                    var vec1 = M3GameManager.Instance.catManager.GetSkillRootPosition();
                    M3Item item = M3ItemManager.Instance.gridItems[list[index].x, list[index].y];
                    var vec2 = item.itemView.itemTransform.position;

                    Action callBack = delegate ()
                    {
                        item.OnSpecialCrush(ItemSpecial.Skill);
                        if (index == list.Count - 1)
                        {
                            FrameScheduler.instance.Add(20, AfterUseSkill);
                        }
                        PlaySkillBoom(KFxBehaviour.skillBoom1, item);
                    };
                    PlaySkillEffect(KFxBehaviour.skillEffect1, vec1, vec2, callBack);
                }
            }
        }

        public override void AfterUseSkill()
        {
            base.AfterUseSkill();
            M3GridManager.Instance.dropLock = false;
        }

        public List<Int2> FindTarget()
        {
            hasZombie = M3GameManager.Instance.fishManager.hasZombie;
            List<Int2> venomParentList = new List<Int2>();
            List<Int2> lockList = new List<Int2>();
            List<Int2> bookList = new List<Int2>();
            List<Int2> iceList = new List<Int2>();
            List<Int2> cloudtList = new List<Int2>();
            List<Int2> treasureList = new List<Int2>();
            List<Int2> venomList = new List<Int2>();
            List<Int2> brownCoomList = new List<Int2>();
            List<Int2> greyCoomList = new List<Int2>();

            List<Int2> catteryList = new List<Int2>();
            List<Int2> coinList = new List<Int2>();
            List<Int2> energyBottleList = new List<Int2>();
            List<Int2> giftList = new List<Int2>();
            List<Int2> energyElementList = new List<Int2>();
            List<Int2> critElementList = new List<Int2>();
            List<Int2> crystalList = new List<Int2>();
            List<Int2> normalList = new List<Int2>();
            List<Int2> specialList = new List<Int2>();
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item;
                    if (M3GameManager.Instance.CheckValid(i, j))
                    {
                        item = M3ItemManager.Instance.gridItems[i, j];
                        if (item != null && item.itemInfo.GetElement() != null)
                        {
                            var ele = item.itemInfo.GetElement();
                            if (ele.eName == M3ElementType.VenomParentElement)
                            {
                                venomParentList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.LockElement)
                            {
                                lockList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.MagicBookElement)
                            {
                                bookList.Add(new Int2(i, j));
                            }
                            else if (item.GetGrid().gridInfo.HaveIce)
                            {
                                iceList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.CurtainElement)
                            {
                                cloudtList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.TreasureElement)
                            {
                                treasureList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.VenomElement)
                            {
                                venomList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.BrownCoomElement)
                            {
                                brownCoomList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.GreyCoomElement)
                            {
                                greyCoomList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.CatteryElement)
                            {
                                catteryList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.CoinElement)
                            {
                                coinList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.EnergyElement)
                            {
                                energyBottleList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.GiftElement)
                            {
                                giftList.Add(new Int2(i, j));
                            }
                            else if (ele.haveEnergy)
                            {
                                energyElementList.Add(new Int2(i, j));
                            }
                            else if (ele.isCrit)
                            {
                                critElementList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.CrystalElement)
                            {
                                crystalList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.NormalElement)
                            {
                                normalList.Add(new Int2(i, j));
                            }
                            else if (ele.eName == M3ElementType.SpecialElement || ele.eName == M3ElementType.MagicCatElement)
                            {
                                specialList.Add(new Int2(i, j));
                            }
                        }
                    }
                }
            }
            #region MyRegion
            //foreach (var item in bookList)
            //{
            //    Debug.Log("bookList" + item.x + "|" + item.y);
            //}
            //foreach (var item in iceList)
            //{
            //    Debug.Log("iceList" + item.x + "|" + item.y);
            //}
            //foreach (var item in energyElementList)
            //{
            //    Debug.Log("energyElementList" + item.x + "|" + item.y);
            //}
            //foreach (var item in critElementList)
            //{
            //    Debug.Log("critElementList" + item.x + "|" + item.y);
            //}
            //foreach (var item in crystalList)
            //{
            //    Debug.Log("crystalList" + item.x + "|" + item.y);
            //}
            //foreach (var item in normalList)
            //{
            //    Debug.Log("normalList" + item.x + "|" + item.y);
            //}
            //foreach (var item in specialList)
            //{
            //    Debug.Log("specialList" + item.x + "|" + item.y);
            //}
            #endregion
            List<Int2> targetList = new List<Int2>();
            if (hasZombie)
            {
                count--;
            }
            if (count > 0 && venomParentList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(venomParentList));
            }
            if (count > 0 && lockList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(lockList));
            }
            if (count > 0 && bookList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(bookList));
            }
            if (count > 0 && iceList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(iceList));
            }
            if (count > 0 && cloudtList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(cloudtList));
            }
            if (count > 0 && treasureList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(treasureList));
            }
            if (count > 0 && venomList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(venomList));
            }
            if (count > 0 && brownCoomList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(brownCoomList));
            }
            if (count > 0 && greyCoomList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(greyCoomList));
            }
            if (count > 0 && catteryList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(catteryList));
            }
            if (count > 0 && coinList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(coinList));
            }

            if (count > 0 && energyBottleList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(energyBottleList));
            }
            if (count > 0 && giftList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(giftList));
            }
            if (count > 0 && energyElementList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(energyElementList));
            }
            if (count > 0 && critElementList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(critElementList));
            }
            if (count > 0 && crystalList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(crystalList));
            }
            if (count > 0 && critElementList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(critElementList));
            }
            if (count > 0 && crystalList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(crystalList));
            }
            if (count > 0 && normalList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(normalList));
            }
            if (count > 0 && specialList.Count > 0)
            {
                targetList.InsertRange(targetList.Count, GetRandomList(specialList));
            }
            return targetList;
        }

        public List<Int2> GetRandomList(List<Int2> list)
        {
            List<Int2> result = new List<Int2>();
            int listCount = list.Count;
            if ((count - listCount) >= 0)
            {
                count -= listCount;
                return list;
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    var tmp = list[M3Supporter.Instance.GetRandomInt(0, list.Count)];
                    result.Add(tmp);
                    list.Remove(tmp);
                }
                count = 0;
            }
            return result;
        }

    }
}