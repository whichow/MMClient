using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 
    /// </summary>
    public static class MapHelper
    {
        /// <summary>
        /// 网格到点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 GridToPosition(int x, int y)
        {
            float z = -x * 0.001f + y * 0.001f;     //根据xy计算z轴深度
            return new Vector3((x + y) * MapConfig.UnitWidth_2, (y - x) * MapConfig.UnitHeight_2, z);
        }
        /// <summary>
        /// 网格到地图中心点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 GridCenterToPosition(Int2 mapSize)
        {
            float z = -mapSize.x * 0.001f + mapSize.y * 0.001f;
            float posx = mapSize.x * 0.5f;
            float posy = mapSize.y * 0.5f;
            return new Vector3((posx + posy) * MapConfig.UnitWidth_2, (posy - posx) * MapConfig.UnitHeight_2, z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static Vector3 GridToPosition(Int2 grid)
        {
            float z = -grid.x * 0.001f + grid.y * 0.001f;
            return new Vector3((grid.x + grid.y) * MapConfig.UnitWidth_2, (grid.y - grid.x) * MapConfig.UnitHeight_2, z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Int2 PositionToGrid(Vector3 pos)
        {
            float x = pos.x / MapConfig.UnitWidth;
            float y = pos.y / MapConfig.UnitHeight;

            return new Int2(Mathf.FloorToInt(x - y), Mathf.FloorToInt(x + y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Vector3 AlignToGrid(Vector3 pos)
        {
            float x = pos.x / MapConfig.UnitWidth;
            float y = pos.y / MapConfig.UnitHeight;
            float xx = Mathf.FloorToInt(x - y);
            float yy = Mathf.FloorToInt(x + y);
            float z = -xx * 0.001f + yy * 0.001f;
            return new Vector3((xx + yy) * MapConfig.UnitWidth_2, (yy - xx) * MapConfig.UnitHeight_2, z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void DrawRhombus(int x, int y, float z = 0f)
        {
#if UNITY_EDITOR
            float uw_2 = MapConfig.UnitWidth_2;
            float uh_2 = MapConfig.UnitHeight_2;

            var point = GridToPosition(x, y);
            point.z = z;

            var points = new Vector3[]
            {
                new Vector3(point.x, point.y, point.z),
                new Vector3(point.x + uw_2, point.y + uh_2, point.z),
                new Vector3(point.x + uw_2 + uw_2, point.y, point.z),
                new Vector3(point.x + uw_2, point.y - uh_2, point.z),
            };

            Gizmos.DrawLine(points[0], points[1]);
            Gizmos.DrawLine(points[1], points[2]);
            Gizmos.DrawLine(points[2], points[3]);
            Gizmos.DrawLine(points[3], points[0]);
#endif
        }
    }
}
