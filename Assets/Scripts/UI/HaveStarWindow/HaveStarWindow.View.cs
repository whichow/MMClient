/** 
 *FileName:     DiscoveryWindow.View.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-19 
 *Description:    
 *History: 
*/
using Game.DataModel;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class HaveStarWindow
    {
        #region Field  


        private KUIItemPool _itemPoolNoStar;
        private KUIItemPool _itemPoolOneStar;
        private KUIItemPool _itemPoolTowStar;
        private Button _closeButton;

        #endregion

        #region Method

        public void InitView()
        {
            _closeButton = Find<Button>("Panel/Close");
            _closeButton.onClick.AddListener(this.OnCloseBtnClick);

            _itemPoolNoStar = Find<KUIItemPool>("Panel/Item1/Scroll View");
            if (_itemPoolNoStar && _itemPoolNoStar.elementTemplate)
            {
                _itemPoolNoStar.elementTemplate.gameObject.AddComponent<HaveStarItem>();
            }
            _itemPoolOneStar = Find<KUIItemPool>("Panel/Item2/Scroll View");
            if (_itemPoolOneStar && _itemPoolOneStar.elementTemplate)
            {
                _itemPoolOneStar.elementTemplate.gameObject.AddComponent<HaveStarItem>();
            }
            _itemPoolTowStar = Find<KUIItemPool>("Panel/Item2/Scroll View");
            if (_itemPoolTowStar && _itemPoolTowStar.elementTemplate)
            {
                _itemPoolTowStar.elementTemplate.gameObject.AddComponent<HaveStarItem>();
            }

        }

        public void RefreshItems()
        {
            _itemPoolNoStar.RefreshItems();
            _itemPoolOneStar.RefreshItems();
            _itemPoolTowStar.RefreshItems();
        }

        public void RefreshView()
        {
            _itemPoolNoStar.Clear();
            _itemPoolOneStar.Clear();
            _itemPoolTowStar.Clear();
            var noStarLevels = XTable.LevelXTable.AllNoStarUnlockLevels;
            var oneStarLevels = XTable.LevelXTable.AllOneStarUnlockLevels;
            var twoStarLevels = XTable.LevelXTable.AllTwoStarUnlockLevels;
            if (noStarLevels != null && noStarLevels.Count > 0)
            {
                var items = _itemPoolNoStar.SpawnElements(noStarLevels.Count);
                for (int i = 0; i < noStarLevels.Count; i++)
                {
                    items[i].GetComponent<HaveStarItem>().ShowLevel(noStarLevels[i]);
                }
            }
            if (oneStarLevels != null && oneStarLevels.Count > 0)
            {
                var items = _itemPoolOneStar.SpawnElements(oneStarLevels.Count);
                for (int i = 0; i < oneStarLevels.Count; i++)
                {
                    items[i].GetComponent<HaveStarItem>().ShowLevel(oneStarLevels[i]);
                }

            }
            if (twoStarLevels != null && twoStarLevels.Count > 0)
            {
                var items = _itemPoolOneStar.SpawnElements(twoStarLevels.Count);
                for (int i = 0; i < twoStarLevels.Count; i++)
                {
                    items[i].GetComponent<HaveStarItem>().ShowLevel(twoStarLevels[i]);
                }
            }
        }

        #endregion
    }
}

