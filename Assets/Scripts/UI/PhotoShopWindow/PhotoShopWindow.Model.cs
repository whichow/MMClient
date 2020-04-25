namespace Game.UI
{
    public partial class PhotoShopWindow
    {
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
