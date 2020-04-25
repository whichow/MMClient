using System;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 普通元素，如红色元素
    /// </summary>
    public class NormalElement : Element
    {
        public string specialType = string.Empty;
        //public bool isHide = false;
        //public bool needCrushOnCreate;

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.NormalElement;
            multipleRatio = 1;
            //needCrushOnCreate = false;
        }

        public override void InitClone(Element ele, object[] args)
        {
            base.InitClone(ele, args);
            isCrit = ele.isCrit;
            haveEnergy = ele.haveEnergy;
            multipleRatio = ele.multipleRatio;
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
            //Debuger.Log("NormalElement  ------  " + itemObtainer.itemInfo.posX + "-"+ itemObtainer.itemInfo.posY);
            if (itemObtainer.isCrushing)
                return;
            itemObtainer.isCrushing = true;

            base.ProcessSpecialEliminate(special, args, ignoreEffect);

            int x = itemObtainer.itemInfo.posX;
            int y = itemObtainer.itemInfo.posY;

            if (special == ItemSpecial.fColor || special == ItemSpecial.fNormal || special == ItemSpecial.Prop || special == ItemSpecial.Skill)
            {
                M3GameManager.Instance.AffectBoomGrid(x, y);
                M3GameManager.Instance.AffectBoomItem(x, y);
            }
            else if (needEffectNeighour)
            {
                M3GameManager.Instance.AffectBoomGrid(x, y);
                M3GameManager.Instance.AffectBoomItem(x, y);
                needEffectNeighour = false;
            }
            eSpecial = special;

            OnDisappear();
            if (special != ItemSpecial.fDoubleCol && special != ItemSpecial.fDoubleRow)
            {
                M3GridManager.Instance.gridCells[x, y].Boom();
            }
        }

        public override void OnDisappear()
        {
            if (itemObtainer.GetGrid() != null && itemObtainer.GetGrid().gridInfo.spawnPointType == DropPointType.SpawnPoint)
                itemObtainer.GetGrid().portDropSpeed = 0;

            Action action = delegate
            {
                itemObtainer.itemInfo.EliminateElement(this);
                itemObtainer.isCrushing = false;

                if (itemObtainer.itemInfo.CheckEmpty())
                {
                    int x = itemObtainer.itemInfo.posX;
                    int y = itemObtainer.itemInfo.posY;
                    itemObtainer.RemoveFrom(x, y);
                    itemObtainer.ItemDestroy();
                    if (haveEnergy)
                    {
                        M3GameManager.Instance.catManager.AddEnergy(energyValue);
                        if (M3GameManager.Instance.soundManager != null)
                            M3GameManager.Instance.soundManager.PlayCollectEnergy();
                    }
                }

                if (specialType != string.Empty)
                {
                    var item = M3GameManager.Instance.CreateSpecialPiece(new Vector2(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY), specialType, GetColor());
                    int tmpScore = item.itemInfo.GetElement().data.config.Score;
                    itemObtainer.crushScore += tmpScore;
                    //Debug.Log("SpawnSpecial" + specialType);
                }
                AddScore();
            };
            FrameScheduler.instance.Add((specialType != string.Empty) ? 1 : M3Config.NormalElementDisapperFrame, action);
            if (view != null)
            {
                view.PlayEliminateFx();
                if (haveEnergy)
                    KUIWindow.GetWindow<Game.UI.M3GameUIWindow>().PlayBottleLiziEffect(view.eleTransform.position, 1);
            }
            DestroyElement();
            if (M3GameManager.Instance.soundManager != null)
            {
                if (isCrit)
                    M3GameManager.Instance.soundManager.PlayElimainateCrit();
            }
        }

        public virtual void AddScore()
        {
            if (itemObtainer.crushScore <= 0)
            {
                int score = M3GameManager.Instance.modeManager.ProcessComboScore(data.config.Point * multipleRatio, eSpecial, M3GameManager.Instance.comboManager.GetCombo(), true);
                M3GameManager.Instance.modeManager.AddScore(score, 1, M3Supporter.Instance.GetItemPositionByGrid(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY));
            }
            else
            {
                int score = M3GameManager.Instance.modeManager.ProcessComboScore(itemObtainer.crushScore * multipleRatio, ItemSpecial.fNormal, M3GameManager.Instance.comboManager.GetCombo(), true);
                M3GameManager.Instance.modeManager.AddScore(score, 1, M3Supporter.Instance.GetItemPositionByGrid(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY));
            }
            itemObtainer.specialScore = 0;
            itemObtainer.crushScore = 0;
            multipleRatio = 1;
        }

        public override void ProcessNeighborEliminate(int sx, int sy)
        {
            base.ProcessNeighborEliminate(sx, sy);
        }

        public override void ProcessColorCrush()
        {
            base.ProcessColorCrush();
            if (!M3GameManager.Instance.isAutoAI)
            {
                view.PlayAnimation("linkage", true);
            }
        }

        public void ChangeToSpecial(ElementSpecial special)
        {
            switch (special)
            {
                case ElementSpecial.None:
                    break;
                case ElementSpecial.Row:
                    data.SetSpecial(ElementSpecial.Row);
                    break;
                case ElementSpecial.Column:
                    data.SetSpecial(ElementSpecial.Column);
                    break;
                case ElementSpecial.Area:
                    data.SetSpecial(ElementSpecial.Area);
                    break;
                case ElementSpecial.Color:
                    data.SetSpecial(ElementSpecial.Color);
                    data.SetColor(ItemColor.fNone);
                    break;
                default:
                    break;
            }
            int id = M3ItemManager.Instance.GetSpecialElementID(itemObtainer.itemInfo.GetPartakeEliminateElement().GetColor(), special);
            M3ItemManager.Instance.ChangeElement(itemObtainer, id);
        }

        public void ChangeToOtherColor(ItemColor color)
        {
            if (color == data.GetColor())
                return;
            int id = M3ItemManager.Instance.GetNormalElementID(color);
            M3ItemManager.Instance.ChangeElement(itemObtainer, id);
        }

        public override void AddCrit(int rate)
        {
            base.AddCrit(rate);
            isCrit = true;
            multipleRatio = rate;
            if (view != null)
                M3FxManager.Instance.PlayM3CommonEffect((int)MatchEffectType.Flag_Crit, view.gameObject, new Vector3(0, 0, -0.2f));
        }

        public override void AddEnergy(int value)
        {
            base.AddEnergy(value);
            haveEnergy = true;
            energyValue = value;
            if (view != null)
            {
                M3FxManager.Instance.PlayEnergyCornerEffect(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY, view.eleTransform);
            }
        }

        public void ChangeToGift()
        {
            int id = M3ItemManager.Instance.GetGiftElementID(data.GetColor());
            M3ItemManager.Instance.ChangeElement(itemObtainer, id);
        }

        public override void OnSpecialEffectAnimation(string eventName, M3Direction[] directions)
        {
            if (view == null)
                return;
            if (eventName == "Area")
            {
                for (int i = 0; i < directions.Length; i++)
                {
                    Int2 pos = M3Supporter.Instance.GetPosByDirection(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY, directions[i]);
                    if (M3GameManager.Instance.CheckValid(pos.x, pos.y) && M3ItemManager.Instance.gridItems[pos.x, posY] != null)
                    {

                    }
                }
            }
        }

        public override Element Clone()
        {
            var normalEle = new NormalElement();
            normalEle.isCrit = this.isCrit;
            normalEle.haveEnergy = this.haveEnergy;
            normalEle.multipleRatio = this.multipleRatio;
            return Clone(normalEle);
        }

    }
}