using Game.DataModel;
using System.Collections.Generic;

namespace Game.Match3
{
    public enum DropPointType
    {
        /// <summary>
        /// 没有掉落口
        /// </summary>
        None,
        /// <summary>
        /// 正常掉落口，比如最上一行
        /// </summary>
        SpawnPoint,
    }

    public enum RopeTypeEnum
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Rigth = 8,
        TB = 3,
        LR = 12,
        TL = 5,
        TR = 9,
        BR = 10,
        TBL = 7,
        TBR = 11,
        TLR = 13,
        BLR = 14,
        TBLR = 15,
    }

    /// <summary>
    /// 三消格子内元素管理
    /// </summary>
    [System.Serializable]
    public class M3GridInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int posX = 0;
        /// <summary>
        /// 
        /// </summary>
        public int posY = 0;
        /// <summary>
        /// 没有绳子 1=上方 2=下方 4=左方 8=右方
        /// </summary>
        public int ropeType = 0;

        private bool haveIce = false;

        public RopeTypeEnum ropeTypeEnum = RopeTypeEnum.None;
        /// <summary>
        /// 0=没有传送门 1=入口 2=出口
        /// </summary>
        public int passType = 0;

        public Element floorElement = null;



        public ElementXDM ropeElement = null;

        public DropPointType spawnPointType = DropPointType.None;

        public List<RuleCode> spawnRule = new List<RuleCode>();

        public M3PortalModel flickerPortal;

        private M3Grid grid;

        public bool HaveIce
        {
            get
            {
                return floorElement != null;
            }
            set
            {
                haveIce = value;
            }
        }

        public LawnElement lawnElement = null;
        public bool HaveLawn
        {
            get { return lawnElement != null; }
        }

        public void Init(int x, int y, M3Grid g)
        {
            posX = x;
            posY = y;
            grid = g;
        }

        public void AddElement(List<ElementXDM> configs)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                if (M3Const.GRID_TYPE.Contains(configs[i].Type))
                {
                    if (configs[i].Type == M3ElementType.RopeElement)
                    {
                        ropeElement = configs[i];
                        ropeType = (int)configs[i].RopeType;
                        ropeTypeEnum = (RopeTypeEnum)ropeType;
                    }
                    else if (configs[i].Type == M3ElementType.IceElement)
                    {
                        HaveIce = true;
                        floorElement = new IceElement();
                        floorElement.Init(configs[i].ID, grid);
                    }
                    else if (configs[i].Type == M3ElementType.LawnElement)
                    {
                        if (lawnElement == null)
                        {
                            lawnElement = new LawnElement();
                            lawnElement.Init(configs[i].ID, grid);
                        }
                    }
                }
            }
        }

        public void AddElement(Element element)
        {
            HaveIce = false;
            var ele = element.Clone();
            if (ele.data.config.Type == M3ElementType.IceElement)
            {
                HaveIce = true;
                floorElement = new IceElement();
                floorElement.Init(ele.data.config.ID, grid);
            }
        }

        public void RefreshElement()
        {

        }

        public void RemoveFloorElement()
        {
            floorElement = null;
        }

        public Int2 GetFlickerPortalOut()
        {
            if (flickerPortal != null && passType == 1)
            {
                return new Int2(flickerPortal.out_x, flickerPortal.out_y);
            }
            return new Int2(-1, -1);
        }

        public Int2 GetFlickerPortalIn()
        {
            if (flickerPortal != null && passType == 2)
            {
                return new Int2(flickerPortal.in_x, flickerPortal.in_y);
            }
            return new Int2(-1, -1);
        }

    }
}