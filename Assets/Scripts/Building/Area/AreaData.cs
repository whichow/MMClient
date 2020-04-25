// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Area" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

namespace Game.Build
{
    [System.Flags]
    public enum AreaGeography
    {
        fLand = 1,
        fWater = 2,
        fSand =3,
        fWaterOfLand =4,
        fAll = -1,
    }

    [RequireComponent(typeof(PolygonCollider2D))]
    public class AreaData : MonoBehaviour
    {
        #region Field

        /// <summary>
        /// 
        /// </summary>
        [Alias("序号")]
        public int index;

        /// <summary>
        /// 
        /// </summary>
        [Header("区域")]
        public Int4 gridRect;
        /// <summary>
        /// 
        /// </summary>
        public Int2[] addGrids;
        /// <summary>
        /// 
        /// </summary>
        public Int2[] delGrids;

        /// <summary>
        /// 
        /// </summary>
        [Alias("地理特征")]
        public AreaGeography geography = AreaGeography.fLand;

        /// <summary>
        /// 
        /// </summary>
        public int groupMask = 1;
        /// <summary>
        /// 解锁的人物等级
        /// </summary>
        [Alias("解锁需要人物等级")]
        public int unlockGrade;
        /// <summary>
        /// 解锁的星星数
        /// </summary>
        [Alias("解锁需要星星总数")]
        public int unlockStar;
        /// <summary>
        /// 
        /// </summary>
        [Alias("解锁消耗")]
        public int unlockMoneyCost;
        /// <summary>
        /// 
        /// </summary>
        [Alias("解锁消耗货币")]
        public int unlockMoneyType;
        /// <summary>
        /// 
        /// </summary>
        [Alias("快速解锁消耗")]
        public int quickUnlockMoneyCost;
        /// <summary>
        /// 
        /// </summary>
        [Alias("快速解锁消耗货币")]
        public int quickUnlockMoneyType;
        /// <summary>
        /// 障碍物上限
        /// </summary>
        [Alias("障碍物上限")]
        public int maxObstacleCount;
        /// <summary>
        /// 相邻区域
        /// </summary>
        public int[] neighbors;
        /// <summary>
        /// 前置区域
        /// </summary>
        public int[] requirements;

        #endregion

        #region Method
        /// <summary>
        /// 获取区域可用网格点
        /// </summary>
        /// <returns></returns>
        public Int2[] GetGrids()
        {
            int count = gridRect.z * gridRect.w + (addGrids != null ? addGrids.Length : 0) - (delGrids != null ? delGrids.Length : 0);

            var retGids = new Int2[count];
            //获取非删除网格坐标
            for (int i = 0; i < gridRect.z; i++)
            {
                for (int j = 0; j < gridRect.w; j++)
                {
                    if (delGrids != null && delGrids.Length > 0)
                    {
                        if (System.Array.Exists(delGrids, delGrid => delGrid.x == i && delGrid.y == j))
                        {
                            continue;
                        }
                    }
                    retGids[--count] = new Int2(gridRect.x + i, gridRect.y + j);
                }
            }
            //添加额外点
            for (int i = 0; i < addGrids.Length; i++)
            {
                retGids[--count] = new Int2(gridRect.x + addGrids[i].x, gridRect.y + addGrids[i].y);
            }

            return retGids;
        }

        #endregion

        #region Unity

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (addGrids != null && addGrids.Length > 0)
            {
                for (int i = 0; i < addGrids.Length; i++)
                {
                    var addGrid = addGrids[i];
                    if (addGrid.x > 0 && addGrid.x < gridRect.z && addGrid.y > 0 && addGrid.y < gridRect.w)
                    {
                        Debug.Log("附加格子数据错误:" + i);
                    }
                }
            }

            if (delGrids != null && delGrids.Length > 0)
            {
                for (int i = 0; i < delGrids.Length; i++)
                {
                    var delGrid = delGrids[i];
                    if (delGrid.x < 0 || delGrid.x >= gridRect.z || delGrid.y < 0 || delGrid.y >= gridRect.w)
                    {
                        Debug.Log("剔除格子数据错误:" + i);
                    }
                }
            }
        }

        public Color gizmosColor = Color.green;

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;

            int x = this.gridRect.x;
            int y = this.gridRect.y;
            int w = this.gridRect.z;
            int h = this.gridRect.w;

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (delGrids != null && delGrids.Length > 0)
                    {
                        if (System.Array.Exists(delGrids, delGrid => delGrid.x == i && delGrid.y == j))
                        {
                            continue;
                        }
                    }
                    MapHelper.DrawRhombus(x + i, y + j);
                }
            }

            if (addGrids != null && addGrids.Length > 0)
            {
                for (int i = 0; i < addGrids.Length; i++)
                {
                    MapHelper.DrawRhombus(x + addGrids[i].x, y + addGrids[i].y);
                }
            }
        }
#endif 
        #endregion
    }
}
