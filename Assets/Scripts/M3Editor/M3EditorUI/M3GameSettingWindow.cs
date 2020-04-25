#if UNITY_EDITOR

using Game.Match3;
using M3Editor;
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

public class M3GameSettingWindow : MonoBehaviour
{


    public Button backBtn;
    public M3LevelData data;

    public Button addBtn;
    public Button delBtn;

    public Button saveBtn;

    public Toggle zombieToggle;

    public GameObject ruleObj;
    public GameObject ruleObjParent;

    public InputField energyRateInput;

    public InputField energyValue1;

    public InputField energyValue2;

    public List<InputField> starsInput;

    public List<GameObject> ruleDic;

    public List<InputField> initSpawnInput;
    public List<M3RuleItem> itemList;


    private int index;
    private bool hasZombie;
    public List<SpawnRuleDefinition> RuleList;

    public List<RuleCode> initSpawnList;

    private int[] stars = new int[] { 1000, 2000, 3000 };

    private int defaultEnergyRate = 100;
    public void Awake()
    {
        Init();
    }
    public void Init()
    {
        ruleDic = new List<GameObject>();
        itemList = new List<M3RuleItem>();



        backBtn.onClick.AddListener(OnBackBtnClick);
        addBtn.onClick.AddListener(OnAddBtnClick);
        delBtn.onClick.AddListener(OnDelBtnClick);
        saveBtn.onClick.AddListener(OnSaveBtnClick);
        zombieToggle.onValueChanged.AddListener(OnZombieChange);
        ruleObj.SetActive(false);
        M3GameEvent.AddEvent(M3EditorEnum.ChangLevel, Reset);
    }

    private void OnZombieChange(bool arg0)
    {
        hasZombie = arg0;
    }

    private void Reset(object[] args)
    {
        //M3LevelData data = M3LevelConfigMgr.Instance.GetLevelConfigData((int)args[0]);
        //RuleList = data.ruleList;

    }

    private void OnSaveBtnClick()
    {
        RuleList = new List<SpawnRuleDefinition>();
        for (int i = 0; i < itemList.Count; i++)
        {
            RuleList.Add(itemList[i].GetRule());
        }
        M3EditorController.instance.gridCtrl.SaveRuleListIntoData(RuleList);
        int[] sTmp = new int[3];

        for (int i = 0; i < starsInput.Count; i++)
        {
            sTmp[i] = int.Parse(starsInput[i].text);
        }
        M3EditorController.instance.gridCtrl.SetStarConfig(sTmp);
        M3EditorController.instance.gridCtrl.SetEnergy(int.Parse(energyRateInput.text), int.Parse(energyValue1.text), int.Parse(energyValue2.text));
        M3EditorController.instance.gridCtrl.GetCurrentLevelData().hasZombie = this.hasZombie;

        for (int i = 0; i < initSpawnList.Count; i++)
        {
           initSpawnList[i]=new RuleCode(1001+i, int.Parse(initSpawnInput[i].text));
        }
        M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorInitSpawn>().spawnList = initSpawnList;
        OnBackBtnClick();
    }

    private void OnDelBtnClick()
    {
        index--;
        RemoveRule();
    }

    private void OnAddBtnClick()
    {
        index++;
        AddRule("Rule_" + index, new SpawnRuleDefinition("Rule", null));
    }

    private void AddRule(string name, SpawnRuleDefinition define)
    {
        var obj = Instantiate(ruleObj);
        obj.transform.SetParent(ruleObjParent.transform, false);
        obj.SetActive(true);
        ruleDic.Add(obj);
        var item = obj.GetComponent<M3RuleItem>();
        itemList.Add(item);
        item.Init();
        item.UpdateData(name, define);

    }
    private void RemoveRule()
    {
        if (itemList.Count > 0)
        {
            itemList.RemoveAt(itemList.Count - 1);
            Destroy(ruleDic[ruleDic.Count - 1]);
            ruleDic.RemoveAt(ruleDic.Count - 1);
        }
    }
    private void OnBackBtnClick()
    {
        this.gameObject.SetActive(false);
        OnClose();
    }

    public void SetStars(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            starsInput[i].text = arr[i].ToString();
        }
    }

    public void OnShow()
    {
        data = M3EditorController.instance.gridCtrl.GetCurrentLevelData();
        hasZombie = data.hasZombie;
        zombieToggle.isOn = (hasZombie);

        int ruleCount = data.ruleList.Count;
        for (int i = 0; i < ruleCount; i++)
        {
            AddRule(data.ruleList[i].ruleName, data.ruleList[i]);
        }
        index = ruleCount;

        int[] starArr = data.star;
        //Debug.Log(starArr == null);
        SetStars(starArr == null ? stars : starArr);
        energyRateInput.text = data.energyRate.ToString();
        energyValue1.text = data.energyStepCount.ToString();
        energyValue2.text = data.energyCreateCount.ToString();

        initSpawnList = M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorInitSpawn>().spawnList;

        if (initSpawnList == null || initSpawnList.Count < 6)
        {
            for (int i = 0; i < 6; i++)
            {
                initSpawnList.Add(new RuleCode(1001+i,10));
            }
        }
        for (int i = 0; i < initSpawnInput.Count; i++)
        {
            initSpawnInput[i].text = initSpawnList[i].weight.ToString(); ;
        }
    }
    public void OnClose()
    {
        itemList.Clear();
        for (int i = 0; i < ruleDic.Count; i++)
        {
            Destroy(ruleDic[i]);
        }
        SetStars(stars);
    }
}
#endif