namespace Game.Build
{
    public class MapNode
    {
        /// <summary>
        /// 正在使用
        /// </summary>
        public bool isUsed;
        /// <summary>
        /// 地图位置
        /// </summary>
        public Int2 mapPoint;
        /// <summary>
        /// 
        /// </summary>
        public Area ownerArea;
        /// <summary>
        /// 
        /// </summary>
        public MapObject ownerItem;

        public UnityEngine.Vector3 position
        {
            get
            {
                return MapHelper.GridToPosition(mapPoint);
            }
        }
    }
}
