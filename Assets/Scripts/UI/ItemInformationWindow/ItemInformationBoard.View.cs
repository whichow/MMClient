// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MessageBox.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    partial class ItemInformationBoard
    {
        #region Field
        private int _id;
        private Image _img_icon;
        private Text _title;
        private Text _txt_first;
        private Text _txt_fourth;
        private Text _txt_fifth;
        private Button _blackback;
        #endregion

        #region Method
        public void InitView()
        {
            _img_icon = Find<Image>("panelback/img_icon");
            _title = Find<Text>("panelback/txt_title");
            _txt_first = Find<Text>("panelback/txt_first");
            _txt_fourth = Find<Text>("panelback/txt_fourth");
            _txt_fifth = Find<Text>("panelback/txt_fifth");
            _blackback = Find<Button>("blackback");
            _blackback.onClick.AddListener(this.CloseUI);
        }
        public void RefreshView()
        {
            _title.text = _messageData._title;
            _img_icon.overrideSprite = KIconManager.Instance.GetItemIcon(_messageData._int_id);
            _txt_first.text = _messageData._txt_first;//KLocalization.GetLocalString(Convert.ToInt32(_messageData._txt_first));
            _txt_fourth.text = "获取途径：";
            //string allPath = string.Empty;
            //_txt_fifth.text = KItemGetPathManager.Instance.ShowGetPaths(KItemManager.Instance.GetItem(_messageData._int_id).getInformation);
            KItem onedataforshow = KItemManager.Instance.GetItem(_messageData._int_id);
            List<int> allPathId = new List<int>();
            if (onedataforshow.getInformation != null)
            {
                for (int i = 0; i < onedataforshow.getInformation.Length; i++)
                {
                    allPathId.Add(onedataforshow.getInformation[i]);
                }
                int[] intArryID = allPathId.ToArray();
                _txt_fifth.text = KItemGetPathManager.Instance.ShowGetPaths(intArryID);
            }
            //int[] allpathid = KItemGetPathManager.Instance.ShowGetPaths(KItemManager.Instance.GetItem(_messageData._int_id).getInformation);//KItemGetPathManager.Instance.ShowGetPaths(_messageData._int_id);//DictGetPath[_messageData._int_id].getPathSequence;
            //for (int i = 0; i < allpathid.Length; i++)
            //{
            //    allPath += KLocalization.GetLocalString(allpathid[i]) + ".";
            //}
            //_txt_fifth.text = allPath;
        }
        #endregion
    }
}

