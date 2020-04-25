using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public partial class SetLanguangeWindow
    {
        private Button _btnBack;
        private KUIGrid _uiPool;

       
        public void InitView()
        {
            _btnBack = Find<Button>("backBtn");
            _uiPool = Find<KUIGrid>("Scroll View");
            if (_uiPool)
            {
                _uiPool.uiPool.itemTemplate.AddComponent<SetLanguangeItem>();
            }
            _btnBack.onClick.AddListener(OnBackBtnClick);
        }
   
   

        private void RefreshView()
        {
            var allLanguange = KLanguageManager.Instance.allLanguages;
            _uiPool.uiPool.SetItemDatas(allLanguange);
            _uiPool.RefillItems();

        }

       
    }
}
