#if UNITY_EDITOR

using System;
/** 
*FileName:     #SCRIPTFULLNAME# 
*Author:       #AUTHOR# 
*Version:      #VERSION# 
*UnityVersion：#UNITYVERSION#
*Date:         #DATE# 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using M3Editor;
using Game.Match3;

public class M3EditorPortData
{
    int defaultRule;
}

public class M3ToolBarCtrl : MonoBehaviour
{
    M3EditorCell cell;
    public Button back;

    public Button portBtn;

    public Button emptyBtn;

    public Button fishExitBtn;
    public Button fishPortBtn;

    public ToggleGroup group;
    public GameObject toggleObj;
    public GameObject ruleSelectObj;
    public Button saveBtn;


    List<SpawnRuleDefinition> ruleList;
    List<GameObject> objList;

    private string tmpRule;
    private void Awake()
    {
        back.onClick.AddListener(OnBack);
        portBtn.onClick.AddListener(OnPortBtnClick);
        emptyBtn.onClick.AddListener(OnEmptyClick);
        saveBtn.onClick.AddListener(OnSaveClick);
        fishExitBtn.onClick.AddListener(OnFishExitClick);
        fishPortBtn.onClick.AddListener(OnFishPortClick);
        objList = new List<GameObject>();
        toggleObj.SetActive(false);
    }

    private void OnFishPortClick()
    {
        M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorFish>().SetCollectPort(cell);
    }

    private void OnFishExitClick()
    {
        M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorFish>().SetCollectExit(cell);
    }

    private void OnEmptyClick()
    {
        cell.SetEmpty();
        gameObject.SetActive(false);
    }

    private void OnSaveClick()
    {
        cell.SetPort(tmpRule);
        ruleSelectObj.SetActive(false);
        gameObject.SetActive(false);
        for (int i = 0; i < objList.Count; i++)
        {
            Destroy(objList[i]);
        }
        tmpRule = string.Empty;
    }

    private void OnPortBtnClick()
    {
        if (!cell.IsPort)
        {
            ruleSelectObj.SetActive(true);
            OnOpen();
        }
        else
        {
            cell.SetPort(string.Empty);
            ruleSelectObj.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void OnOpen()
    {
        ruleList = M3EditorController.instance.gridCtrl.GetCurrentLevelData().ruleList;

        for (int i = 0; i < ruleList.Count; i++)
        {
            var t = AddToggle(ruleList[i].ruleName);
            if (tmpRule == string.Empty && i == 0)
            {
                tmpRule = ruleList[0].ruleName;
                t.isOn = true;
            }
            if (tmpRule == ruleList[i].ruleName)
            {
                t.isOn = true;
            }
        }

    }

    private Toggle AddToggle(string name)
    {
        var obj = Instantiate(toggleObj);
        obj.transform.SetParent(group.transform, false);
        obj.SetActive(true);
        Toggle t = obj.GetComponent<Toggle>();
        t.onValueChanged.AddListener((check) => { onToggleValueChanged(check, name); });
        t.group = group;
        t.isOn = false;
        t.transform.Find("Label").GetComponent<Text>().text = name;
        var toggleData = obj.AddComponent<M3EditorRuleToggle>();
        toggleData.UpdateData(name);
        objList.Add(obj);
        return t;
    }

    private void onToggleValueChanged(bool arg0, string n)
    {
        if (arg0)
            tmpRule = n;
    }

    private void OnBack()
    {
        gameObject.SetActive(false);
    }

    public void SetData(M3EditorCell c)
    {
        cell = c;
        tmpRule = c.portRule;
    }
}
#endif