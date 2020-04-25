namespace Game.UI
{
    public partial class PhotoShopPickCardLowWindow
    {
        public enum EPickCardType
        {
            Cat,
            Building,
            Item
        }

        public class PickCardData
        {
            public int ID;
            public EPickCardType Type;
            public int Rarity;
            public string Icon;
        }


        private string GetBlueBlackNum()
        {
            if (BagDataModel.Instance.GetItemCountById(ItemIDConst.LowCard) >= 5)
            {
                return BagDataModel.Instance.GetItemCountById(ItemIDConst.LowCard) + "/5";
            }
          return BagDataModel.Instance.GetItemCountById(ItemIDConst.LowCard) + "/"+ BagDataModel.Instance.GetItemCountById(ItemIDConst.LowCard);
        }

        private string GetBlueBlack()
        {
            return BagDataModel.Instance.GetItemCountById(ItemIDConst.LowCard).ToString();
        }

        private string GetPurpoleBlack()
        {
            return BagDataModel.Instance.GetItemCountById(ItemIDConst.MidCard).ToString();
        }

        private string GetDiamBlack()
        {
            return PlayerDataModel.Instance.mPlayerData.mDiamond.ToString();
        }

    }

}
