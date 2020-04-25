namespace Game
{
    public class BagEvent
    {
        /// <summary>
        /// 背包所有数据刷新
        /// </summary>
        public static readonly string BagAllDataRefresh = "bagAllDataRefresh";

        /// <summary>
        /// 背包数据变化
        /// </summary>
        public static readonly string BagDataRefresh = "bagDataRefresh";

        /// <summary>
        /// 出售物品事件
        /// </summary>
        public static readonly string BagSellItem = "bagSellItem";

        /// <summary>
        /// 使用物品
        /// </summary>
        public static readonly string BagUseItem = "bagUseItem";
    }
}
