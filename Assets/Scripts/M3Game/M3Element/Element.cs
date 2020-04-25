/** 
*FileName:     Element.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-08-14 
*Description:    
*History: 
*/
using Game.DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public enum ElementStatus
    {
        None,
        Idle,
        Select,
        Transform,
        Die,
    }

    /// <summary>
    /// 三消元素
    /// </summary>
    public class Element
    {
        #region Field

        public int posX;
        public int posY;

        public ElementData data;
        public ElementView view;
        public M3Item itemObtainer;
        public M3Grid gridObtainer;

        //private ElementStatus status;
        public int sortLayer = 0;
        public bool crushWithOutSpecial = false;
        public bool isObstacle;

        public int multipleRatio;

        /// <summary>
        /// M3ElementType
        /// </summary>
        public int eName;

        public bool isCrit;

        public int hp;
        public int totalHp = 0;//消除层数

        public bool haveEnergy;
        public int energyValue;

        public bool needEffectNeighour;

        public ItemColor currentColor;

        public ItemSpecial eSpecial;

        public int jumpRoundNum;
        public bool isJumping = false;
        public int overlyingRoundNum;//叠层回合
        public int overlyingLvNumber;//叠层数

        /// <summary>
        /// 是否需要在底部生成草地
        /// </summary>
        public bool needCreateLawn;

        public bool crushFlag = false;
  
        #endregion

        public virtual void Init(int id, M3Item item)
        {
            itemObtainer = item;
            posX = item.itemInfo.posX;
            posY = item.itemInfo.posY;

            data = new ElementData();
            data.Init(id);

            isCrit = false;
            totalHp = data.config.Health;
            sortLayer = data.config.Level;
            overlyingLvNumber = 1;
            overlyingRoundNum = 0;
            multipleRatio = 1;
            ResetJumpRound();

            if (item.itemView != null && item.itemView.itemTransform != null)
            {
                GameObject tmp = null;
                KAssetManager.Instance.TryGetMatchPrefab(data.config.ModelName, out tmp);
                GameObject go = KPool.Spawn(tmp);
                go.transform.SetParent(item.itemView.elementRoot, false);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.transform.localEulerAngles = Vector3.zero;
                TransformUtils.SetLayer(go, LayerMask.NameToLayer("Default"));
                view = go.AddComponent<ElementView>();
                view.Init(this, go.transform);
                RefreshOverlyingView();
            }

        }

        public virtual void Init(int id, M3Grid grid)
        {
            gridObtainer = grid;
            posX = grid.gridInfo.posX;
            posY = grid.gridInfo.posY;

            data = new ElementData();
            data.Init(id);

            multipleRatio = 1;
            isCrit = false;
            sortLayer = data.config.Level;
            overlyingLvNumber = 1;
            overlyingRoundNum = 0;
            multipleRatio = 1;
            ResetJumpRound();

            if (grid.gridView != null && grid.gridView.gridTransform != null)
            {
                GameObject tmp = null;
                KAssetManager.Instance.TryGetMatchPrefab(data.config.ModelName, out tmp);
                if (tmp != null)
                {
                    GameObject go = KPool.Spawn(tmp);
                    go.transform.SetParent(grid.gridView.gridTransform, false);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    go.transform.localEulerAngles = Vector3.zero;
                    TransformUtils.SetLayer(go, LayerMask.NameToLayer("Default"));
                    view = go.AddComponent<ElementView>();
                    view.Init(this, go.transform);
                    RefreshOverlyingView();
                }
                else
                {
                    Debug.LogError("[Element.Init] prefab is null! "+ data.config.ModelName);
                }
            }
        }

        public virtual void InitClone(Element ele, object[] args)
        {

        }

        public virtual void OnCreate()
        {

        }

        /// <summary>
        /// 消除后转化
        /// </summary>
        /// <param name="id"></param>
        public virtual void TransformElement(int id)
        {
            M3GameManager.Instance.modeManager.GameModeCtrl.AddTarget(data.GetTargetId(), (view != null) ? view.eleTransform.position : Vector3.zero, this);
            data.Init(id);
        }

        /// <summary>
        /// 消除
        /// </summary>
        public virtual void Crush()
        {
            if (data.config.OverlyingAddType == 1)
                ProcessOverlyingLevel();
        }

        /// <summary>
        /// 处理特殊元素消除
        /// </summary>
        /// <param name="special"></param>
        /// <param name="args"></param>
        /// <param name="ignoreEffect"></param>
        public virtual void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
            eSpecial = special;
            crushFlag = false;
            //if(itemObtainer!=null)
            //    Debuger.Log("ProcessSpecialEliminate  ------  " + itemObtainer.itemInfo.posX + "-" + itemObtainer.itemInfo.posY + " -- " + special + " -- " + this.data.GetColor());
            if (needCreateLawn)
            {
                int x = itemObtainer.itemInfo.posX;
                int y = itemObtainer.itemInfo.posY;
                //Debuger.Log("铺草坪  ------  " + x + "-" + y + " -- " + special + " -- " + this.data.config.Type);
                M3Grid grid = M3GridManager.Instance.gridCells[x, y];
                if (!grid.gridInfo.HaveLawn)
                {
                    grid.gridInfo.AddElement(new List<ElementXDM>() { XTable.ElementXTable.GetByID(3020) });
                    M3GameManager.Instance.modeManager.GameModeCtrl.AddTarget(new List<int>() { 3020 }, (view != null) ? view.eleTransform.position : Vector3.zero, this);
                }
            }
        }

        /// <summary>
        /// 处理相邻元素消除
        /// </summary>
        public virtual void ProcessNeighborEliminate(int sx, int sy)
        {
            if (data.CanScrapingEliminate())
            {
                Crush();
            }
        }

        public virtual void ProcessLineBombEliminate()
        {

        }

        /// <summary>
        /// 处理回合逻辑
        /// </summary>
        public virtual void ProcessRoundLogic()
        {
            overlyingRoundNum--;
            jumpRoundNum--;
            overlyingRoundNum = Mathf.Max(1, overlyingRoundNum);
            jumpRoundNum = Mathf.Max(0, jumpRoundNum);
            //if (eName == M3ElementType.BellElement)
            //    Debug.Log(jumpRoundNum);
        }

        /// <summary>
        /// 落地处理 已经下落完成
        /// </summary>
        public virtual void OnLanded()
        {

        }

        public virtual void OnArriveGrid()
        {
        }

        public virtual void OnLandedAnimation()
        {
            if (view == null || view.eleGameObject == null)
                return;
            //itemObtainer.isTweening = true;
            KTweenUtils.LocalMoveAdd(view.eleTransform, new Vector3(0, -0.1f, 0), 0.1f, DG.Tweening.Ease.OutQuad, null);
            KTweenUtils.ScaleTo(view.eleTransform, new Vector3(1.1f, 0.8f, 1), 0.1f, delegate ()
            {
                KTweenUtils.LocalMoveAdd(view.eleTransform, new Vector3(0, 0.1f, 0), 0.12f, DG.Tweening.Ease.OutSine, delegate ()
            {
                view.RefreshView(null);
                //itemObtainer.isTweening = false;
            });
                KTweenUtils.ScaleTo(view.eleTransform, new Vector3(1, 1, 1), 0.12f, delegate ()
                {
                });
            });
        }

        /// <summary>
        /// 消毁元素
        /// </summary>
        public virtual void DestroyElement()
        {
            if (view != null && view.eleGameObject)
            {
                view.DestroyAllEffect();
                Game.KPool.Despawn(view.eleGameObject);
            }
        }

        public virtual void RefreshElement()
        {

        }

        public virtual void DisposeElement()
        {
            data = null;
            view = null;
            itemObtainer = null;
        }

        public void ResetOverlyingNum()
        {
            overlyingRoundNum = data.config.OverlyingRound;
            if (overlyingRoundNum <= 0)
            {

            }
        }

        public void ResetJumpRound()
        {
            jumpRoundNum = data.config.JumpSpace;
        }

        /// <summary>
        /// 被消除时
        /// </summary>
        public virtual void OnDisappear()
        {

        }

        public ElementSpecial GetSpecial()
        {
            return data.GetSpecial();
        }

        public ItemColor GetColor()
        {
            return data.GetColor();
        }

        public void SetColor(ItemColor color)
        {
            data.SetColor(color);
        }

        public virtual void ProcessColorCrush()
        {

        }

        public virtual void Jump()
        {
            //if (eName == M3ElementType.BellElement)
            //    Debug.Log(jumpRoundNum);
            if (jumpRoundNum > 0)
            {
                return;
            }
            var list = GetJumpTargetElementByID((data.config.JumpGroup != null) ? data.config.JumpGroup : null);
            if (list.Count > 0)
            {
                M3Item item = list[M3Supporter.Instance.GetRandomInt(0, list.Count)];
                JumpAndInjectToGrid(item.itemInfo.posX, item.itemInfo.posY);
            }
            ResetJumpRound();
        }

        public virtual void JumpAndInjectToGrid(int toX, int toY)
        {
            Int2 point2 = new Int2(toX, toY);
            Int2 lastPos = new Int2(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY);
            M3Item lastItem = itemObtainer;
            M3Item targetItem = M3ItemManager.Instance.gridItems[point2.x, point2.y];
            targetItem.isTargetByJump = true;
            if (view != null)
            {
                view.eleTransform.SetParent(targetItem.itemView.itemTransform);
                view.eleTransform.localScale = Vector3.one;
                view.eleTransform.localPosition = view.eleTransform.localPosition + new Vector3(0, 0, -1);
                var vec1 = M3Supporter.Instance.GetItemPositionByGrid(point2.x, point2.y);
                var vec2 = M3Supporter.Instance.GetItemPositionByGrid(lastPos.x, lastPos.y);

                Vector3 vec = new Vector3((vec2.x - vec1.x), (vec2.y - vec1.y), -1.2f);
                M3GridManager.Instance.dropLock = true;
                M3FxManager.Instance.PlayJumpAnimation(view.eleTransform, vec, Vector3.zero, 0.8f, delegate ()
                 {
                     lastItem.itemInfo.RemoveHighestElement();
                     if (lastItem.itemInfo.CheckEmpty())
                     {
                         int x = lastItem.itemInfo.posX;
                         int y = lastItem.itemInfo.posY;
                         lastItem.RemoveFrom(x, y);
                         lastItem.ItemDestroy();
                     }

                     itemObtainer = targetItem;
                     itemObtainer.itemInfo.DestroyHighestElement();
                     itemObtainer.itemInfo.AddElement(this);
                     itemObtainer.isTargetByJump = true;
                     M3GridManager.Instance.dropLock = false; ;
                     view.RefreshView();
                 });
            }

        }

        public List<M3Item> GetJumpTargetElementByID(List<int> idList)
        {
            List<M3Item> itemList = new List<M3Item>();
            if (idList != null)
                for (int i = 0; i < M3Config.GridHeight; i++)
                {
                    for (int j = 0; j < M3Config.GridWidth; j++)
                    {
                        M3Item item = M3ItemManager.Instance.gridItems[i, j];
                        if (item != null
                            && !item.isCrushing
                            && item.itemInfo.GetElement() != null
                            && (((IList)idList).Contains(item.itemInfo.GetElement().data.config.ID))
                            && !item.isTargetByJump)
                        {
                            itemList.Add(item);
                        }
                    }
                }
            return itemList;
        }

        /// <summary>
        /// 处理叠加
        /// </summary>
        public virtual void ProcessOverlyingLevel()
        {
            if (data.config.OverlyingType == 0)
                return;
            else if (data.config.OverlyingType == 1)
            {
                overlyingLvNumber = overlyingLvNumber * data.config.OverlyingRatio;
            }
            else if (data.config.OverlyingType == 2)
            {
                overlyingLvNumber += data.config.OverlyingRatio;
            }
            overlyingLvNumber = Mathf.Min(overlyingLvNumber, data.config.OverlyingMaxNum);
            if (view != null)
                view.corner.ShowNumber(overlyingLvNumber);
        }

        public virtual void RefreshOverlyingView()
        {
            if (view != null)
            {
                view.corner.ShowNumber(overlyingLvNumber);
            }
        }

        public virtual void PlayElementJumpView()
        {

        }

        public virtual void OnSpecialEffectAnimation(string eventName, M3Direction[] direction)
        {

        }

        public virtual void Refresh()
        {
            if (view != null)
            {
                view.RefreshView();
            }
        }

        public virtual void AddCrit(int rate)
        {

        }

        public virtual void AddEnergy(int value)
        {

        }

        public virtual Element Clone()
        {
            Element ele = new Element();
            return Clone(ele);
        }

        public virtual Element Clone(Element ele)
        {
            ele.data = new ElementData();
            ele.data.Init(data.config.ID);
            return ele;
        }

    }
}