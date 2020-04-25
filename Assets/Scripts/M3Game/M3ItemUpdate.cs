using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class M3ItemUpdate : Singleton<M3ItemUpdate>
    {
        private List<M3Item> itemList = new List<M3Item>();

        public void RunUpdate()
        {
            if (M3GridManager.Instance.dropLock) return;

            itemList.Clear();
            M3Grid grid;
            for (int i = M3Config.GridHeight - 1; i >= 0; i--)
            {
                itemList.Clear();
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    grid = M3GridManager.Instance.gridCells[i, j];
                    if (grid == null)
                        continue;
                    M3Item item = grid.GetItem();
                    if (item != null && !item.isCrushing && item.itemInfo.GetElement() != null && item.CanDrop)
                    {
                        itemList.Add(item);
                        //ItemUpdate(item);
                    }
                }
                int count = itemList.Count;
                for (int j = 0; j < count; j++)
                {
                    M3Item item = itemList[M3Supporter.Instance.GetRandomInt(0, itemList.Count)]; //？？随机？？
                    ItemUpdate(item);
                    itemList.Remove(item);
                }
            }
        }

        private void ItemUpdate(M3Item item)
        {
            //检测下落，处理下落动画
            if (!item.isTweening)
            {
                if (item.dropSpeed > 0f)
                {
                    if (M3GameManager.Instance.isAutoAI)
                    {
                        //Debug.Log("Item Land " + FrameScheduler.instance.GetCurrentFrame());
                        item.IsDroping = false;
                        item.GetGrid().isDroping = false;
                        bool flag = !item.GetGrid().CheckDrop();
                        if (flag)
                        {
                            item.OnLanded();
                        }
                    }
                    else
                    {
                        SetPiecePosition(item);
                        CheckGridChanged(item);
                    }
                }
                else
                {
                    item.IsDroping = false;
                    item.GetGrid().CheckDrop();
                }
            }
        }

        private static void SetPiecePosition(M3Item item)
        {
            if (item.itemView.itemGameobject != null)
            {
                Vector3 localPosition = item.itemView.itemGameobject.transform.localPosition;
                //item.itemView.itemTransform.localPosition = new Vector3(localPosition.x, localPosition.y - item.dropSpeed * Time.fixedDeltaTime, localPosition.z);
                KTweenUtils.LocalMoveTo(item.itemView.itemTransform, new Vector3(localPosition.x, localPosition.y - item.dropSpeed * Time.fixedDeltaTime, localPosition.z), 0);
                //Debug.Log("speed : " + item.dropSpeed + "  time: " + Time.deltaTime);
                //Debug.Log("pos : " + (localPosition.y - item.dropSpeed * Time.deltaTime));
            }
        }

        private static void CheckGridChanged(M3Item item)
        {
            Vector3 position = item.itemView.itemGameobject.transform.localPosition;
            bool flag = false;
            if (item.disappearPosX == -999 && item.itemInfo.posX == -item.targetPosY && (item.itemView.itemTransform.localPosition.y - item.targetPosY <= M3Config.DropInitialSpeed * Time.fixedDeltaTime))
            {
                item.SetPosition(item.targetPosX, item.targetPosY);
                item.IsDroping = false;
                item.GetGrid().isDroping = false;
                flag = !item.GetGrid().CheckDrop();
                if (flag)
                {
                    item.OnLanded();
                }
            }
            if (item.disappearPosX != -999)
            {
                //item.itemView.itemGameobject.transform.localPosition = item.shownPos + new Vector3(0f, 1, 0f);
                KTweenUtils.LocalMoveTo(item.itemView.itemTransform, item.shownPos + new Vector3(0f, 1, 0f), 0);
                item.disappearPosX = -999;
            }
            if (!flag)
            {
                item.dropSpeed += M3Config.DropAcceleratedSpeed * Time.fixedDeltaTime;
                item.dropSpeed = Mathf.Min(item.dropSpeed, M3Config.DropSpeedMax);
            }
        }

    }
}