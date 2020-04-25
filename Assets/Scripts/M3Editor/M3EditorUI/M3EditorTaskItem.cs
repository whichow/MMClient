#if UNITY_EDITOR

using Game.DataModel;
using Game.Match3;
using System;
/** 
*FileName:     M3EditorTaskItem.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-12 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M3Editor
{
    public class M3EditorTaskItem : MonoBehaviour
    {
        public M3ItemDropContrl drop;
        public bool hasElement;
        public int elementID;
        private int elementCount;
        public Image icon;
        public InputField countField;
        public Button delBtn;
        internal MEventDelegate<M3EditorTaskItem> clearHandler;

        public int ElementCount
        {
            get
            {
                return int.Parse(countField.text);
            }

            set
            {
                elementCount = value;
                countField.text = value.ToString();
            }
        }

        public void Init(int id=-1,int count=-1)
        {
            drop = GetComponent<M3ItemDropContrl>();
            icon = transform.Find("icon").GetComponent<Image>();
            countField = transform.Find("count").GetComponent<InputField>();
            delBtn = transform.Find("delBtn").GetComponent<Button>();
            delBtn.onClick.AddListener(OnDeleteBtn);
            hasElement = false;
            drop.dropHandler = OnItemDrop;
            if (count == -1)
                ElementCount = 0;
            if (id > 0 && count >= 0)
            {
                ElementCount = count;
                OnItemDrop(id);
            }
        }

        private void OnDeleteBtn()
        {
            if (clearHandler != null)
                clearHandler(this);
        }
        public void Clear()
        {
            elementID = -1;
            elementCount = -1;

        }

        private void OnItemDrop(int t)
        {
            elementID = t;
            hasElement = true;
            AddElement(t);

        }
        private void AddElement(int id)
        {
            ElementXDM config = XTable.ElementXTable.GetByID(id);
            icon.sprite = Game.KIconManager.Instance.GetMatch3ElementIcon(config.Icon);
            countField.text = ElementCount.ToString();
        }
    }
}
#endif