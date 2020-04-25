#if UNITY_EDITOR

using Game.Match3;
using M3Editor;
using System;
/** 
*FileName:     M3EditorLevelItem.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-06 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M3EditorLevelItem : MonoBehaviour
{
    #region field
    private Toggle LevelBtn;
    private Color selectColor = Color.grey;
    private Color unSelectColor = Color.white;
    private Image btnImage;
    private Text idText;
    public string levelId = "";
    #endregion
    #region method
    public void Init(M3LevelData data)
    {
        levelId = data.id;
        LevelBtn = GetComponent<Toggle>();
        idText = transform.Find("Text").GetComponent<Text>();
        idText.text = data.id.ToString();
        btnImage = transform.GetComponent<Image>();
        LevelBtn.onValueChanged.AddListener(OnLevelBtnClick);
    }

    private void OnLevelBtnClick(bool arg0)
    {
        if (arg0)
        {

            if (M3EditorController.instance.EditorMode == mEditorMode.mEditor || M3EditorController.instance.EditorMode == mEditorMode.mDelete)
            {
                M3GameEvent.DispatchEvent(M3EditorEnum.OnSaveBtnClick,1);
            }
            M3GameEvent.DispatchEvent(M3EditorEnum.StartGenMap, levelId, M3LevelConfigMgr.Instance.GetLevelConfigData(levelId));
            M3GameEvent.DispatchEvent(M3EditorEnum.ChangLevel, levelId);
            M3EditorController.instance.EditorMode = mEditorMode.mEditor;
        }
    }

    private void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
    }
    #endregion
    #region unity
    private void Start()
    {
    }
    #endregion
}
#endif