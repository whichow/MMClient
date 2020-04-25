/** 
 *FileName:     DiscoveryWindow.View.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-19 
 *Description:    
 *History: 
*/
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 探险任务
    /// </summary>
    public partial class DiscoveryWindow
    {
        #region Field  


        private KUIItemPool _itemPool;

        private Button _closeButton;

        #endregion

        #region Method

        public void InitView()
        {
            _closeButton = Find<Button>("Panel/Close");
            _closeButton.onClick.AddListener(this.OnCloseBtnClick);

            _itemPool = Find<KUIItemPool>("Panel/Scroll View");
            if (_itemPool && _itemPool.elementTemplate)
            {
                _itemPool.elementTemplate.gameObject.AddComponent<DiscoveryItem>();
            }
        }

        public void RefreshItems()
        {
            _itemPool.RefreshItems();

        }

        public void RefreshView()
        {
            _itemPool.Clear();
            var tasks = KExplore.Instance.allTask;

            if (tasks!=null &&tasks.Length>0)
            {
                var items = _itemPool.SpawnElements(tasks.Length);
                for (int i = 0; i < tasks.Length; i++)
                {
                    items[i].GetComponent<DiscoveryItem>().ShowTask(tasks[i]);
                }

            }
        }



        #endregion
    }
}

