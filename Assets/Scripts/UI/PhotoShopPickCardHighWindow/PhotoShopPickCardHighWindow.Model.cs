using System.Collections;
using System.Collections.Generic;


namespace Game.UI
{
    public class PhtotShopPickCardWindowData
    {
        public  int _type;
        public PhtotShopPickCardWindowData(int type)
        {
            _type = type;
        }
    }
    public partial class PhotoShopPickCardHighWindow
    {
        public  PhtotShopPickCardWindowData windowdata;
        private KItem kShopitemMid;

        private enum DrawCardType
        {
            Mid=1,
            Hight=2,
        }

    }
}
