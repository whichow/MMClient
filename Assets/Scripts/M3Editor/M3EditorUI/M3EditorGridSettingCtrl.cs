#if UNITY_EDITOR

using Game.Match3;
using System;
/** 
*FileName:     M3EditorGridSettingCtrl.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-07 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace M3Editor
{
    public class M3EditorGridSettingCtrl : MonoBehaviour
    {

        public Button yesBtn;
        public Button cancelBtn;
        public InputField levelID;
        public InputField gridHeight;
        private void Awake()
        {
            M3GameEvent.AddEvent(M3EditorEnum.OnNewBtnClick, OnNewBtnClick);
            yesBtn.onClick.AddListener(OnYesBtnClick);
            cancelBtn.onClick.AddListener(OnCancelBtnClick);
            this.gameObject.SetActive(false);
        }

        private void OnCancelBtnClick()
        {
            this.gameObject.SetActive(false);
        }

        private void OnYesBtnClick()
        {
            int y = 0;
            var lvID = levelID.text;
            var gridY = gridHeight.text;
            if (int.TryParse(gridY, out y))
            {
                if (y > 0)
                {
                    M3EditorConst.GridRealHeight = y;

                    M3LevelData data = new M3LevelData(lvID);
                    data.SetDefaultData(lvID, M3EditorConst.GridWidth, y);
                    M3GameEvent.DispatchEvent(M3EditorEnum.StartGenMap, lvID, data);
                    M3GameEvent.DispatchEvent(M3EditorEnum.ChangLevel, lvID);
                    M3EditorController.instance.EditorMode = mEditorMode.mEditor;
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("Input is Out Of Order");
            }
        }

        private void OnNewBtnClick(object[] args)
        {
            this.gameObject.SetActive(true);
        }
    }
}
#endif