#if UNITY_EDITOR
using Game.Match3;
using System;
/** 
*FileName:     M3EditorFishCtrl.cs 
*Author:       Hejunjie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-11-01 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace M3Editor
{
    public class M3EditorFishCtrl : MonoBehaviour
    {



        public Button back;

        public Button saveBtn;

        public Button addBtn;

        public GameObject item;

        public GameObject itemParent;


        public M3FishRuleItem current;

        List<M3FishRuleItem> list = new List<M3FishRuleItem>();
        List<M3FishModelItem> modelList = new List<M3FishModelItem>();
        private int[] default_values = new int[]
{
            1,
            -1,
            -1,
            -1,
            1,
            1,
};
        public void Awake()
        {
            back.onClick.AddListener(OnBackClick);
            saveBtn.onClick.AddListener(OnSaveClick);
            addBtn.onClick.AddListener(OnAddClick);
            item.SetActive(false);
        }

        private void OnAddClick()
        {
            AddRule(null);
        }

        private void OnSaveClick()
        {
            modelList.Clear();
            foreach (var item in list)
            {
                modelList.Add(item.GetModel());
            }

            M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorFish>().SetList(modelList);
            OnBackClick();
        }

        private void OnBackClick()
        {
            this.gameObject.SetActive(false);
        }

        public void AddRule(M3FishModelItem fish)
        {
            var obj = Instantiate(item);
            obj.transform.SetParent(itemParent.transform, false);
            obj.SetActive(true);
            M3FishRuleItem rule = obj.GetComponent<M3FishRuleItem>();
            rule.DeleteAction = delegate ()
            {
                list.Remove(rule);
                Destroy(rule.gameObject);
            };
            if (fish == null)
                rule.SetDefalut(default_values);
            else
            {
                rule.OnShow(fish);
            }
            list.Add(rule);
        }
        public void Clear()
        {
            foreach (var item in list)
            {
                Destroy(item.gameObject);
            }
            list.Clear();
        }
    }
}
#endif