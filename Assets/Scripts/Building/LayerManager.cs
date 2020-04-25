using UnityEngine;

namespace Game.Build
{
    public static class LayerManager
    {
        static LayerManager()
        {
            BuildingLayer = LayerMask.NameToLayer("Clickable");
            ClickerLayer = LayerMask.NameToLayer("Clicker");
            AreaLayer = LayerMask.NameToLayer("Area");
        }

        public static bool IsInLayerMask(GameObject obj, LayerMask mask)
        {
            if (obj)
            {
                return ((mask.value & (1 << obj.layer)) > 0);
            }
            return false;
        }

        /// <summary>
        /// 区域的层
        /// </summary>
        public static LayerMask AreaLayerMask
        {
            get
            {
                return 1 << AreaLayer;
            }
        }

        public static int AreaLayer
        {
            get;
            private set;
        }

        /// <summary>
        /// 建筑的层
        /// </summary>
        public static LayerMask BuildingLayerMask
        {
            get
            {
                return 1 << BuildingLayer;
            }
        }

        public static int BuildingLayer
        {
            get;
            private set;
        }

        /// <summary>
        /// 场景可点击的层
        /// </summary>
        public static LayerMask ClickerLayerMask
        {
            get
            {
                return 1 << ClickerLayer;
            }
        }

        public static int ClickerLayer
        {
            get;
            private set;
        }
    }
}
