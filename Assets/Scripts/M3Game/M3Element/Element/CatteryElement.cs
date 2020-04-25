using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 猫窝
    /// </summary>
    public class CatteryElement : ObstacleElement
    {
        private int dyeCount = 3;//分裂数量

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.CatteryElement;
            hp = totalHp;
        }

        public override void InitClone(Element ele, object[] args)
        {
            base.InitClone(ele, args);

            hp = ele.hp;
        }

        public override void Crush()
        {
            if (itemObtainer.isCrushing)
                return;
            DoEliminate();
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
            Crush();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            if (view != null)
                view.PlayAnimation(data.GetAnimationsByKey(M3Const.CatterAnimationKeys[totalHp - hp]));
        }

        private void DoEliminate()
        {
            hp--;
            if (hp < 0)
            {
                hp = 0;
            }
            if (hp == 0)
            {
                if (view != null)
                    view.PlayAnimation(data.GetAnimationsByKey(M3Const.CatterAnimationKeys[4]), null, true);
            }
            if (hp == 1)
            {
                if (view != null)
                    view.PlayAnimation(data.GetAnimationsByKey(M3Const.CatterAnimationKeys[totalHp - hp]), delegate ()
                {
                    view.PlayAnimation(data.GetAnimationsByKey(M3Const.CatterAnimationKeys[4]), null, true);
                });

            }
            if (this.hp > 1 && !itemObtainer.isCrushing)
            {
                if (view != null)
                    view.PlayAnimation(data.GetAnimationsByKey(M3Const.CatterAnimationKeys[totalHp - hp]));
            }
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEleminateCattery();
        }

        public override void RefreshElement()
        {
            if (view != null)
                view.PlayAnimation(data.GetAnimationsByKey(data.config.IdleAnim));
        }

        public override void DoLogic()
        {
            if (this.hp >= 0 && !this.itemObtainer.isCrushing)
            {
                this.RefreshElement();
            }
            if (this.hp == 0)
            {
            }
        }

        public bool Dye()
        {
            if (this.hp <= 0 && !itemObtainer.isCrushing)
            {
                if (view != null)
                {
                    view.PlayAnimation(data.GetAnimationsByKey(M3Const.CatterAnimationKeys[3]),
                      delegate ()
                      {
                          view.PlayAnimation(data.GetAnimationsByKey(M3Const.CatterAnimationKeys[0]));
                      });
                    view.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                this.hp = totalHp;
                this.GeneratePiece();
                return true;
            }
            else
                return false;
        }

        private void GeneratePiece()
        {
            var list = GetSendList();
            if (list.Count > 0)
            {
                RandomItems(list);
                int num = Mathf.Min(list.Count, dyeCount);
                for (int i = 0; i < num; i++)
                {
                    ChangeElement(list[i]);
                }
            }
        }

        private void ChangeElement(Int2 point)
        {
            //Debuger.Log(point.x + "_" + point.y);
            M3Item item = M3ItemManager.Instance.gridItems[point.x, point.y];
            if (item != null && item.itemInfo.GetElement().eName == M3ElementType.NormalElement)
            {
                item.isTargetByCattery = true;
                Element lastEle = item.itemInfo.GetElement();
                item.itemInfo.RemoveHighestElement();

                Element currentEle;
                // 特殊猫窝，没有颜色，随机给特殊元素
                if (data.GetColor() == ItemColor.fNone)
                {
                    ElementSpecial special;
                    int randomInt = M3Supporter.Instance.GetRandomInt(0, 3);
                    switch (randomInt)
                    {
                        case 0:
                            special = ElementSpecial.Row;
                            break;
                        case 1:
                            special = ElementSpecial.Column;
                            break;
                        case 2:
                            special = ElementSpecial.Area;
                            break;
                        default:
                            special = ElementSpecial.Row;
                            break;
                    }
                    currentEle = item.AddElement(M3ItemManager.Instance.GetSpecialElementID(lastEle.data.GetColor(), special));
                }
                else
                {
                    currentEle = item.AddElement(M3ItemManager.Instance.GetNormalElementID(data.GetColor()));
                }

                if (view != null)
                {
                    Vector3 vec = currentEle.view.transform.localPosition;
                    float layerZ = vec.z;
                    Vector3 moveVec = new Vector3(itemObtainer.coordinate.x - item.coordinate.x, itemObtainer.coordinate.y + 0.5f * M3Config.DistancePerUnit - item.coordinate.y, vec.z - 1.2f);
                    currentEle.view.transform.localPosition = moveVec;

                    Vector3 from = M3Supporter.Instance.GetItemPositionByGrid(currentEle.itemObtainer.itemInfo.posX, currentEle.itemObtainer.itemInfo.posY);
                    Vector3 to = M3Supporter.Instance.GetItemPositionByGrid(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY);
                    currentEle.view.eleTransform.localPosition = currentEle.view.eleTransform.localPosition + new Vector3(0, 0, -1);
                    M3FxManager.Instance.PlayCatterySendAnimation(currentEle.view.eleTransform, (to - from), new Vector3(0, 0, -1.2f), delegate ()
                        {
                            item.isTargetByCattery = false;
                            currentEle.view.transform.localPosition = new Vector3(0, 0, layerZ);
                            currentEle.view.RefreshView();
                            lastEle.DestroyElement();
                        });
                    //KTweenUtils.LocalMoveTo(currentEle.view.transform, new Vector3(0, 0, moveVec.z), 0.4f, delegate ()
                    //{
                    //    currentEle.view.transform.localPosition = new Vector3(0, 0, layerZ);
                    //    lastEle.DestroyElement();
                    //});
                }
            }
        }

        private List<Int2> GetSendList()
        {
            List<Int2> list = new List<Int2>();
            M3Item item;
            Element ele;
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && !item.isCrushing)
                    {
                        ele = item.itemInfo.GetElement();
                        if (!item.isTargetByCattery
                            && ele.eName == M3ElementType.NormalElement
                            && data.GetColor() != ele.GetColor())
                        {
                            list.Add(new Int2(i, j));
                        }
                    }
                }
            }
            return list;
        }

        private void RandomItems(List<Int2> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int random = M3Supporter.Instance.GetRandomInt(i, list.Count);
                Int2 value = list[i];
                list[i] = list[random];
                list[random] = value;
            }
        }

        public override Element Clone()
        {
            CatteryElement ele = new CatteryElement();
            ele.hp = this.hp;
            return Clone(ele);
        }

    }
}