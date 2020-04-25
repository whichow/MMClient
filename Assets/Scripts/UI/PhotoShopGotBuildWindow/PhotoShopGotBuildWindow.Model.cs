namespace Game.UI
{

    public partial class PhotoShopGotBuildWindow
    {
        private KItem kShopitem;
        public class Data
        {
            public KCat cat;
            public KItem item;
            public int type;
        }

        private Data _photoShopCardData;
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

        private void InitModel()
        {
            _photoShopCardData = new Data();
        }

        private void RefreshModel()
        {

            var passData = data as Data;
            if (passData!=null)
            {
                kShopitem = passData.item;
                cat = passData.cat;
                _photoShopCardData.type = passData.type;
            }
            else
            {
                kShopitem = null;
                cat =null;
                _photoShopCardData.type = 0;
            }
           
         
        }

    }
   
}
