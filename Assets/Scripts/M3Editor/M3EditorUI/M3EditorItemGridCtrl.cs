/** 
*FileName:     M3EditorItemGridCtrl.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-07 
*Description:    
*History: 
*/
#if UNITY_EDITOR
using Game;
using Game.DataModel;
using Game.Match3;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M3Editor
{
    public enum EditorElementLayer
    {
        Bottom1,
        Bottom2,
        Middle1,
        Middle2,
        Top1,
        Top2,
    }

    public class M3EditorItemGridCtrl : MonoBehaviour
    {
        #region field

        private Transform grid;
        private GameObject itemPrefab;
        public Button defaultBtn;
        public Button emptyBtn;

        public Image currentBrushIcon;
        #endregion

        #region unity
        private void Awake()
        {
            grid = TransformUtils.GetChildByName(this.transform, "grid");
            itemPrefab = grid.Find("itemPrefab").gameObject;
            M3GameEvent.AddEvent(M3EditorEnum.ConfigCompelte, OnCompeleted);
            defaultBtn.onClick.AddListener(OnDefaultClick);
            emptyBtn.onClick.AddListener(OnEmptyClick);
        }
        private void OnDefaultClick()
        {
            M3EditorController.instance.gridCtrl.SetBrush(-1);
        }
        private void OnEmptyClick()
        {
            M3EditorController.instance.gridCtrl.SetBrush(-2);
        }



        private void Start()
        {

        }
        public void SetCurrentBrushIcon(string name)
        {
            var sprite = KIconManager.Instance.GetMatch3ElementIcon(name);
            if (sprite != null)
                currentBrushIcon.overrideSprite = sprite;
        }
        private void OnCompeleted(object[] args)
        {
            var list = GetEditorElement("baseElement");
            foreach (var id in list)
            {
                GameObject go = TransformUtils.Instantiate(itemPrefab, grid);
                go.GetComponent<M3ItemDragContrl>().Init(id);
            }
            itemPrefab.SetActive(false);
        }
        #endregion

        #region method
        public List<int> GetEditorElement(string fileName)
        {
            List<int> result = new List<int>();
            //string eleStr = "1001-1006|2001-2025|3001-3013|3014-3017|3020|3029-3038|3051|3052|4035-4041|3054-3055";
            string eleStr = "1001|2001|2007|2013|1002|2002|2008|2014|1003|2003|2009|2015|1004|2004|2010|2016|1005|2005|2011|2017|1006|2006|2012|2018|3001|3002|3003|3020|3009|3010|3011|3012|2019|4035|3013|3051|3029|3030|3032|3031|2020|3033|4036|3004|2021|3034|4037|3005|2022|3035|4038|3006|2023|3036|4039|3007|2024|3037|4040|3008|2025|3038|4041|3055|3014|3015|3016|3017|3052|3054|3057|3039";
            string[] baseEleIds = eleStr.Split('|');
            string tempIds;
            string[] continueEleIds;
            int startId, endId, eleId;
            for (int i = 0; i < baseEleIds.Length; i++)
            {
                tempIds = baseEleIds[i];
                if (tempIds.Contains("-"))
                {
                    continueEleIds = tempIds.Split('-');
                    startId = int.Parse(continueEleIds[0]);
                    endId = int.Parse(continueEleIds[1]);
                    for (int j = startId; j <= endId; j++)
                    {
                        if (result.IndexOf(j) == -1 && j != 0)
                            result.Add(j);
                    }
                }
                else
                {
                    eleId = int.Parse(tempIds);
                    if (result.IndexOf(eleId) == -1 && eleId != 0)
                        result.Add(eleId);
                }
            }
            foreach (var item in result)
            {
                if (XTable.ElementXTable.GetByID(item)==null)
                {
                    Debug.LogError("GetEditorElement: " + item);
                }
            }
            return result;
        }


        #endregion
    }
}
#endif