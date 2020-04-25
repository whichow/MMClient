using Game.DataModel;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class MapItem : MonoBehaviour
    {
        public MapSelectItem item;

        private GameObject _enterBtnGO;
        private GameObject _stageRoot;

        private ChapterUnlockXDM chapter;
        private List<MapSelectItem> itemList;

        public void Init(ChapterUnlockXDM c)
        {
            chapter = c;
            itemList = new List<MapSelectItem>();
            InitView();
        }

        public void InitView()
        {
            _stageRoot = transform.Find("stageRoot").gameObject;
            _enterBtnGO = KUIWindow.GetWindow<MapSelectWindow>()._enterBtnGO;
            _enterBtnGO.SetActive(false);
        }

        public void AddBtnObj(LevelXDM level)
        {
            var obj = Instantiate(_enterBtnGO);
            obj.SetActive(true);
            obj.transform.SetParent(_stageRoot.transform, false);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(level.Position[0] / 1000.0f, level.Position[1] / 1000.0f);

            item = obj.AddComponent<MapSelectItem>();
            itemList.Add(item);
            item.Init(level);
        }

        public void RefreshView()
        {

        }

        public void RefreshAll()
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].Refresh();
            }
        }

    }
}
