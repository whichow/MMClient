/** 
*FileName:     M3FishManager.cs 
*Author:       HASEE 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-10-23 
*Description:    
*History: 
*/
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class M3FishManager
    {
        public List<M3FishModelItem> collectList = new List<M3FishModelItem>();
        public List<Int2> collectPointList = new List<Int2>();
        public List<Int2> portPointList = new List<Int2>();
        public bool hasZombie;
        public Int2 zombiePos;
        public M3Zombie zombie;
        public bool lastCollect;

        private int currentCollectCount;

        public M3FishManager()
        {
            Init();
        }

        public void Init()
        {
            hasZombie = M3GameManager.Instance.level.hasZombie;
            collectPointList = M3GameManager.Instance.level.GetFishExit();
            for (int i = 0; i < collectPointList.Count; i++)
            {
                if (M3GameManager.Instance.CheckValid(collectPointList[i].x, collectPointList[i].y))
                {
                    M3Grid grid = M3GridManager.Instance.gridCells[collectPointList[i].x, collectPointList[i].y];
                    if (grid != null)
                    {
                        grid.isFishExit = true;
                        grid.gridView.GenFishBoard();
                    }
                }
            }

            portPointList = M3GameManager.Instance.level.GetFishPort();
            for (int i = 0; i < portPointList.Count; i++)
            {
                if (M3GameManager.Instance.CheckValid(portPointList[i].x, portPointList[i].y))
                {
                    M3Grid grid = M3GridManager.Instance.gridCells[portPointList[i].x, portPointList[i].y];
                    if (grid != null)
                    {
                        grid.isFishPort = true;
                    }
                }
            }

            if (hasZombie)
            {
                zombie = new M3Zombie();
                zombie.Init();
            }
            collectList = M3GameManager.Instance.level.GetFishModelList();
            currentCollectCount = 0;
        }

        public int GetCollectType(int x, int y)
        {
            if (this.collectList.Count == 0)
            {
                return 0;
            }
            int result = 0;
            for (int i = 0; i < this.collectList.Count; i++)
            {
                //Debug.Log()
                if (this.IsAllConditionPass(i) && CheckVaild(i, x, y))
                {
                    Debug.Log("移除金豆荚池 剩余 ： " + (collectList.Count - 1));

                    this.collectList.RemoveAt(i);
                    this.lastCollect = false;
                    result = 1;
                    break;
                }
            }
            return result;
        }

        private bool CheckVaild(int index, int x, int y)
        {
            var list = collectList[index].spawnList;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].x == x && list[i].y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public void OnOtherFishCollect()
        {
            currentCollectCount++;
            Debug.Log(currentCollectCount);
            for (int i = 0; i < this.collectList.Count; i++)
            {
                if (collectList[i].combineMode == 2)
                {
                    if (collectList[i].needPrevious >= currentCollectCount)
                    {
                        collectList[i].previousFlag = true;
                    }
                }
            }
            lastCollect = true;
        }

        private bool IsAllConditionPass(int index)
        {
            bool result = false;
            bool[] array = new bool[]
            {
                this.IsPreviousPass(index),
                this.IsStepPass(index),
                this.IsScorePass(index),
                this.IsRandomPass(index),
                this.IsTimePass(index)
            };

            //Debug.Log(this.collectList[index].combineMode + "___" + array[0] + "|" + array[1] + "|" + array[2] );
            if (this.collectList[index].combineMode == 1)
            {
                result = (array[0] || array[1] || array[2]);
            }
            if (this.collectList[index].combineMode == 2)
            {
                result = (array[0] && array[1]);
            }
            return result;
        }

        public void CheckDelayFish()
        {
            if (collectList == null)
                return;
            for (int i = 0; i < this.collectList.Count; i++)
            {
                if (collectList[i].combineMode == 2)
                {
                    if (collectList[i].needPrevious != -1)
                    {
                        if (collectList[i].needPrevious <= currentCollectCount)
                        {
                            Debug.Log("Fish Index Pre  :  " + i);
                            if (collectList[i].previousFlag)
                            {
                                collectList[i].needStep -= 1;
                            }
                            else
                                collectList[i].previousFlag = true;
                        }
                    }
                }
            }
        }

        private bool IsPreviousPass(int index)
        {
            if (this.collectList[index].combineMode == 1 && this.collectList[index].needPrevious == -1)
            {
                return this.lastCollect;
            }
            if (this.collectList[index].combineMode == 2)
            {
                return (this.collectList[index].previousFlag == true);
            }
            return false;
        }

        private bool IsStepPass(int index)
        {
            if (this.collectList[index].combineMode == 2)
            {
                if (collectList[index].needStep < 0)
                    return true;
                return false;
            }
            //Debug.Log(M3GameManager.Instance.modeManager.GameModeCtrl.currentStep >= this.collectList[index].needStep);
            return M3GameManager.Instance.modeManager.GameModeCtrl.currentStep >= this.collectList[index].needStep;
        }

        private bool IsTimePass(int index)
        {
            return false;
        }

        private bool IsRandomPass(int index)
        {
            //return this.collectList[index].isRandom == -1 && this.collectList[index].combineMode == 2;
            return false;
        }

        private bool IsScorePass(int index)
        {
            if (collectList[index].needScore != -1)
                return M3GameManager.Instance.modeManager.score >= this.collectList[index].needScore;
            return false;
        }

        /// <summary>
        /// 检测选择的鱼掉出收集
        /// </summary>
        public void CheckPieceSwitchedDrop()
        {
            if (M3GameManager.Instance.SelectedItem != null)
            {
                M3Item selectedPiece = M3GameManager.Instance.SelectedItem;
                if (selectedPiece != null && selectedPiece.itemInfo.GetElement() != null && selectedPiece.itemInfo.GetElement().eName == M3ElementType.FishElement)
                {
                    ((FishElement)selectedPiece.itemInfo.GetElement()).CheckDropOut();
                }
            }
            if (M3GameManager.Instance.SelectedItem2 != null)
            {
                M3Item selectedPiece = M3GameManager.Instance.SelectedItem2;
                if (selectedPiece != null && selectedPiece.itemInfo.GetElement() != null && selectedPiece.itemInfo.GetElement().eName == M3ElementType.FishElement)
                {
                    ((FishElement)selectedPiece.itemInfo.GetElement()).CheckDropOut();
                }
            }
        }

        public bool CheckFish()
        {
            bool result = false;
            if (hasZombie && !zombie.CheckStun())
            {
                M3Item item = null;
                for (int i = 0; i < M3Config.GridHeight; i++)
                {
                    for (int j = 0; j < M3Config.GridWidth; j++)
                    {
                        item = M3ItemManager.Instance.gridItems[i, j];
                        if (item != null && item.itemInfo.GetElement() != null)
                        {
                            if (item.itemInfo.GetElement().eName == M3ElementType.FishElement)
                            {
                                ((FishElement)item.itemInfo.GetElement()).Absorb();
                                result = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool CheckIsCollect(Int2 point)
        {
            var list = M3GameManager.Instance.fishManager.collectPointList;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].x == point.x && list[i].y == point.y)
                {
                    return true;
                }
            }
            return false;
        }

        public void Refresh()
        {
            if (hasZombie)
            {
                zombie.Refresh();
            }
        }

    }
}