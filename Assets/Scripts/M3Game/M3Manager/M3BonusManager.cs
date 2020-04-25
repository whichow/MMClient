using Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class M3BonusManager
    {
        public Vector3 boardPos;
        public bool needPreBoom;
        public GameObject bonusStartObj;
        public Canvas canvas;
        public RectTransform bonusPos;

        List<Vector2> finishList = new List<Vector2>();

        public M3BonusManager()
        {
            Init();
        }

        public void Init()
        {
            needPreBoom = false;
            canvas = KUIRoot.Instance.GetComponent<Canvas>();
        }

        public void StartBonus()
        {
            boardPos = GetBoardPos();
            bonusStartObj = GetStartObj();
            if (M3GameManager.Instance.catManager != null && M3GameManager.Instance.catManager.hasCat)
            {
                M3GameManager.Instance.catManager.CatBehaviour.Win(true);
            }
            int num = 40;
            FrameScheduler.instance.Add(num, delegate ()
            {
                PlayFinishQuanAnimation();
            });
            FrameScheduler.instance.Add(num + 30, delegate ()
              {
                  PlayFinishAnimation();
              });

            FrameScheduler.Instance().Add(num + 60, delegate
              {
                  CrushBeforeBonus();
              });

            KSoundManager.StopMusic();
            M3GameManager.Instance.soundManager.PlayBonus();
            FrameScheduler.Instance().Add(750, delegate
            {
                KSoundManager.StopMusic();
                M3GameManager.Instance.soundManager.PlayMainMusic();
            });
        }

        public void PlayFinishQuanAnimation()
        {
            M3FxManager.Instance.PlayFinishQuan((M3Config.RealWidth) / 2.0f + M3Config.RealFirsCol - 0.5f, (M3Config.RealHeight) / 2.0f + M3Config.RealFirstRow - 0.5f);
        }

        public void PlayFinishAnimation()
        {
            finishList.Clear();
            int numMax = Mathf.Max(M3Config.GridHeight, M3Config.GridWidth);
            //int numMin = Mathf.Min(M3Config.GridHeight, M3Config.GridHeightIndex);
            //int num1 = numMax / 2;
            //int num2 = numMin / 2;
            int realWidth = M3Config.RealLastCol - M3Config.RealFirsCol + 1;
            int realHeight = M3Config.RealLastRow - M3Config.RealFirstRow + 1;
            int num1 = realHeight / 2;
            int num2 = realWidth / 2;
            List<Vector2> tmp = new List<Vector2>();
            for (int i = 0; i < numMax; i++)
            {
                int index = i;
                if (index == 0)
                {
                    if (realHeight % 2 == 0 && realWidth % 2 == 0)
                    {
                        var vec1 = new Vector2(num1 - 1 + M3Config.RealFirstRow, num2 - 1 + M3Config.RealFirsCol);
                        var vec2 = new Vector2(num1 - 1 + M3Config.RealFirstRow, num2 + M3Config.RealFirsCol);
                        var vec3 = new Vector2(num1 + M3Config.RealFirstRow, num2 - 1 + M3Config.RealFirsCol);
                        var vec4 = new Vector2(num1 + M3Config.RealFirstRow, num2 + M3Config.RealFirsCol);
                        tmp.Add(vec1);
                        tmp.Add(vec2);
                        tmp.Add(vec3);
                        tmp.Add(vec4);
                        finishList.Add(vec1);
                        finishList.Add(vec2);
                        finishList.Add(vec3);
                        finishList.Add(vec4);
                    }
                    if (realHeight % 2 == 0 && realWidth % 2 != 0)
                    {
                        var vec3 = new Vector2(num1 + M3Config.RealFirstRow, num2 + M3Config.RealFirsCol);
                        var vec4 = new Vector2(num1 - 1 + M3Config.RealFirstRow, num2 + M3Config.RealFirsCol);
                        tmp.Add(vec3);
                        tmp.Add(vec4);
                        finishList.Add(vec3);
                        finishList.Add(vec4);
                    }
                    if (realHeight % 2 != 0 && realWidth % 2 == 0)
                    {
                        var vec3 = new Vector2(num1 + M3Config.RealFirstRow, num2 - 1 + M3Config.RealFirsCol);
                        var vec4 = new Vector2(num1 + M3Config.RealFirstRow, num2 + M3Config.RealFirsCol);
                        tmp.Add(vec3);
                        tmp.Add(vec4);
                        finishList.Add(vec3);
                        finishList.Add(vec4);
                    }
                    if (realHeight % 2 != 0 && realWidth % 2 != 0)
                    {
                        var vec4 = new Vector2(num1 + M3Config.RealFirstRow, num2 + M3Config.RealFirsCol);
                        tmp.Add(vec4);
                        finishList.Add(vec4);
                    }
                }
                else
                {
                    tmp = PutIntoFinishList(tmp);
                }
                for (int j = 0; j < tmp.Count; j++)
                {
                    int indexJ = j;
                    int x = (int)tmp[indexJ].x;
                    int y = (int)tmp[indexJ].y;
                    if (M3GameManager.Instance.CheckValid(x, y) && M3ItemManager.Instance.gridItems[x, y] != null)
                    {
                        FrameScheduler.instance.Add(5 * index, delegate ()
                        {
                            M3FxManager.Instance.PlayFinishEffect(x, y);
                        });
                    }
                }
            }
        }
        private List<Vector2> PutIntoFinishList(List<Vector2> vec)
        {
            List<Vector2> vTmp = new List<Vector2>();
            for (int i = 0; i < vec.Count; i++)
            {
                for (int j = 0; j < M3Const.DirectionEightOffset.Length; j++)
                {
                    var tmp = new Vector2(vec[i].x + M3Const.DirectionEightOffset[j].x, vec[i].y + M3Const.DirectionEightOffset[j].y);
                    if (M3GameManager.Instance.CheckValid((int)tmp.x, (int)tmp.y) && !finishList.Contains(tmp))
                    {
                        vTmp.Add(tmp);
                        finishList.Add(tmp);
                    }
                }
            }
            return vTmp;
        }

        public Vector3 GetBoardPos()
        {
            M3GameUIWindow window = KUIWindow.GetWindow<M3GameUIWindow>();
            RectTransform rectTransform = window.bonusPos;
            bonusPos = rectTransform;
            //Vector3 vector = KUIRoot.Instance.uiCamera.WorldToScreenPoint(rectTransform.position);
            Vector3 vector = rectTransform.anchoredPosition;

            //Vector3 vector = KUIRoot.Instance.uiCamera.ScreenToWorldPoint(position);
            return vector;
        }

        public GameObject GetStartObj()
        {
            M3GameUIWindow window = KUIWindow.GetWindow<M3GameUIWindow>();
            if (window == null)
                return null;
            return window.bonuseStartObj;
        }

        public void CrushBeforeBonus()
        {
            List<Int2> list = GetSpecialItemList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[list[i].x, list[i].y];
                    Action action = delegate ()
                    {
                        if (item != null && !item.isCrushing)
                        {
                            item.OnSpecialCrush(ItemSpecial.fNormal);
                        }
                    };
                    FrameScheduler.instance.Add(i * 20, action);

                    needPreBoom = true;
                    M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
                }
            }
            else
            {
                needPreBoom = false;
                ShowBonusAnimation();
            }
        }

        public void ShowBonusAnimation()
        {

            if (M3GameManager.Instance.modeManager.GetStep() == 0)
            {
                FrameScheduler.instance.Add(60, delegate ()
                {
                    M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.Bonus);
                });
            }
            else
            {
                FrameScheduler.instance.Add(10, delegate ()
                {
                    KUIWindow.GetWindow<M3GameUIWindow>().PlayBonus();
                    FrameScheduler.Instance().Add(60, delegate
                    {
                        PrepareBoom();
                    });
                });

            }
        }

        public void PrepareBoom()
        {
            List<Int2> itemsList = GetNeedRandomItemList();
            List<M3Item> itemsNeedToCrush = new List<M3Item>();
            int num = M3GameManager.Instance.modeManager.GetStep();
            num = Mathf.Min(num, itemsList.Count);
            for (int i = 0; i < num; i++)
            {
                FrameScheduler.instance.Add(i * 10, delegate ()
                {
                    M3GameManager.Instance.modeManager.GameModeCtrl.ProcessSteps(1);
                    if (itemsList.Count > 0)
                    {
                        Int2 point = itemsList[M3Supporter.Instance.GetRandomInt(0, itemsList.Count)];
                        itemsList.Remove(point);

                        Vector3 gridPosition = M3Supporter.Instance.WordToScenePoint(M3Supporter.Instance.GetItemWorldPositon(point.x, point.y), bonusPos, canvas);
                        float time = Vector3.Distance(Vector3.zero, gridPosition) / 2000f;
                        M3FxManager.Instance.PlayBonusStepEffect(Vector3.zero, gridPosition, time, bonusPos, delegate ()
                        {
                            Int2 itemsTmp = point;
                            var item = M3ItemManager.Instance.gridItems[itemsTmp.x, itemsTmp.y];
                            item.MakeBaseElementSpecial();
                            itemsNeedToCrush.Add(item);

                            M3GameManager.Instance.modeManager.AddScore(M3GameManager.Instance.modeManager.ProcessComboScore(ConstElementScore.score_bonus, ItemSpecial.fNormal, 1, false), 1, M3Supporter.Instance.GetItemPositionByGrid(itemsTmp.x, itemsTmp.y));
                            if (itemsList.Count == 0 || M3GameManager.Instance.modeManager.GetStep() == 0)
                            {
                                FrameScheduler.instance.Add(30, delegate ()
                                {
                                    GoBoom(itemsNeedToCrush);
                                });
                            }
                        });
                    }
                });
            }
        }

        public void GoBoom(List<M3Item> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                M3Item item = list[i];
                if (item != null && !item.isCrushing)
                    item.OnSpecialCrush(ItemSpecial.fNormal);

            }
            M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.Bonus);
        }

        public List<Int2> GetNeedRandomItemList()
        {
            List<Int2> list = new List<Int2>();
            M3Item tmp;
            Element tmpEle;
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    tmp = M3ItemManager.Instance.gridItems[i, j];
                    if (tmp != null)
                    {
                        tmpEle = tmp.itemInfo.GetElement();
                        if (tmpEle != null && tmpEle.data.IsBaseElement())
                        {
                            list.Add(new Int2(i, j));
                        }
                    }
                }
            }
            return list;
        }

        public List<Int2> GetSpecialItemList()
        {
            List<Int2> list = new List<Int2>();
            M3Item tmp;
            for (int i = M3Config.GridHeight - 1; i >= 0; i--)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    tmp = M3ItemManager.Instance.gridItems[i, j];
                    if (tmp != null && tmp.itemInfo.GetElement() != null && tmp.itemInfo.GetElement().GetSpecial() > 0)
                    {
                        list.Add(new Int2(i, j));
                    }
                }
            }
            return list;
        }

    }
}