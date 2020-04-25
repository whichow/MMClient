/** 
 *FileName:     ChooseCatWindow.View.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-23 
 *Description:    
 *History: 
*/
using UnityEngine.UI;

namespace Game.UI
{
    public partial class ChooseCatWindow
    {
        #region Field  


        private KUIItemPool _itemPool;

        private Button _closeButton;

        public CatDataVO _cat;

        private int _indx;
        #endregion

        #region Method

        public void InitView()
        {
            _closeButton = Find<Button>("Background/Close");
            _closeButton.onClick.AddListener(this.OnCloseBtnClick);

            _itemPool = Find<KUIItemPool>("Background/Scroll View");
            if (_itemPool && _itemPool.elementTemplate)
            {
                _itemPool.elementTemplate.gameObject.AddComponent<ChooseCatItem>();
            }
        }

        public void RefreshItems()
        {
            _itemPool.RefreshItems();

        }

        public void RefreshView()
        {
            _itemPool.Clear();

            var retCat = _chooseCatData.catsList;

            if (retCat != null && retCat.Count > 0)
            {
                var items = _itemPool.SpawnElements(retCat.Count);
                for (int i = 0; i < retCat.Count; i++)
                {
                    items[i].GetComponent<ChooseCatItem>().ShowCat(retCat[i], _chooseCatData.type);
                }

            }
        }

        #endregion
    }
}

