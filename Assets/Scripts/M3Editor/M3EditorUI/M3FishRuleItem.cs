#if UNITY_EDITOR

using Game.Match3;
using System;
/** 
*FileName:     M3FishRuleItem.cs 
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
    public class M3FishRuleItem : MonoBehaviour
    {

        public List<InputField> inputList = new List<InputField>();

        public Button deleteBtn;

        public Action DeleteAction;
        M3FishModelItem item;

        public List<Int2> posList = new List<Int2>();
        public Button selectBtn;
        public Button clearAllBtn;

        public void SetDefalut(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                inputList[i].text = arr[i].ToString();
            }
        }
        private void Awake()
        {
            deleteBtn.onClick.AddListener(delegate ()
            {
                DeleteAction();
            });

            selectBtn.onClick.AddListener(OnSelectBtnClick);
            clearAllBtn.onClick.AddListener(OnClearBtnClick);

        }



        private void OnClearBtnClick()
        {
            posList.Clear();
        }

        private void OnSelectBtnClick()
        {

            M3EditorController.instance.fishCtrl.current = this;


            M3EditorController.instance.EditorMode = mEditorMode.SelectFish;
            M3EditorController.instance.fishCtrl.gameObject.SetActive(false);

            M3EditorController.instance.gridCtrl.ShowFishPort(posList);
        }

        public void OnShow(M3FishModelItem model)
        {
            item = model;
            if (model != null)
            {
                inputList[0].text = model.needPrevious.ToString();
                inputList[1].text = model.needStep.ToString();
                inputList[2].text = model.needTime.ToString();
                inputList[3].text = model.needScore.ToString();
                inputList[4].text = model.isRandom.ToString();
                inputList[5].text = model.combineMode.ToString();
                posList = model.spawnList;
            }

        }
        public bool ReciveCell(M3EditorCell cell)
        {
            bool flag = false;
            int index = -1;
            if (posList == null)
                posList = new List<Int2>();
            for (int i = 0; i < posList.Count; i++)
            {
                if (posList[i].x == cell.gridX && posList[i].y == cell.gridY)
                {
                    index = i;
                    flag = true;
                }
            }
            if (flag)
                posList.RemoveAt(index);
            else
            {
                posList.Add(new Int2(cell.gridX, cell.gridY));
            }
            Debug.Log(posList.Count);
            return flag;

        }
        public M3FishModelItem GetModel()
        {
            M3FishModelItem model = new M3FishModelItem();
            model.needPrevious = int.Parse(inputList[0].text);
            model.needStep = int.Parse(inputList[1].text);
            model.needTime = int.Parse(inputList[2].text);
            model.needScore = int.Parse(inputList[3].text);
            model.isRandom = int.Parse(inputList[4].text);
            model.combineMode = int.Parse(inputList[5].text);
            model.spawnList = posList;
            return model;
        }
    }

}
#endif