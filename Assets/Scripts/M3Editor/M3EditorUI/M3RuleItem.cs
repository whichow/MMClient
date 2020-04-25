#if UNITY_EDITOR
/** 
 *FileName:     #SCRIPTFULLNAME# 
 *Author:       #AUTHOR# 
 *Version:      #VERSION# 
 *UnityVersion：#UNITYVERSION#
 *Date:         #DATE# 
 *Description:    
 *History: 
*/
using Game.Match3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M3RuleItem : MonoBehaviour
{

    Text ruleName;
    GameObject obj;
    List<M3RuleElementItem> itemList = new List<M3RuleElementItem>();

    Dictionary<int, int> weightDic = new Dictionary<int, int>();
    public string name;
    public void Init()
    {
        ruleName = transform.Find("ruleName").GetComponent<Text>();
        obj = transform.Find("grid/Image").gameObject;
        obj.SetActive(false);
        name = "Rule";
    }
    public void UpdateData(string ind, SpawnRuleDefinition s)
    {
        name = ind;
        ruleName.text = ind;
        int count = RuleID.ruleIDArray.Length;
        //int count = ElementConfig.Get().Count;
        for (int i = 0; i < count; i++)
        {
            int id = RuleID.ruleIDArray[i];
            var go = Instantiate(obj);
            var item = go.GetComponent<M3RuleElementItem>();
            itemList.Add(item);
            go.transform.SetParent(obj.transform.parent, false);
            go.SetActive(true);
            item.Init();
            bool flag = false;
            if (s.ruleList != null)
                for (int j = 0; j < s.ruleList.Count; j++)
                {
                    if (s.ruleList[j].elementID == id)
                    {
                        item.UpdateData(RuleID.ruleIDArray[i], s.ruleList[j].weight);
                        flag = true;
                        break;
                    }

                }
            if (!flag)
                item.UpdateData(RuleID.ruleIDArray[i], 0);
        }

        //for (int i = 0; i < s.ruleList.Count; i++)
        //{
        //    var go = Instantiate(obj);
        //    var item = go.GetComponent<M3RuleElementItem>();
        //    itemList.Add(item);
        //    go.transform.SetParent(obj.transform.parent, false);
        //    go.SetActive(true);
        //    item.Init();
        //    item.UpdateData(s.ruleList[i].elementID, s.ruleList[i].weight);

        //}
    }

    public SpawnRuleDefinition GetRule()
    {
        SpawnRuleDefinition tmp;
        List<RuleCode> codeList = new List<RuleCode>();

        Int2 intTmp;
        for (int i = 0; i < itemList.Count; i++)
        {
            intTmp = itemList[i].Get();
            if (intTmp.y <= 0)
                continue;
            RuleCode tmpRule = new RuleCode(intTmp.x, intTmp.y);
            codeList.Add(tmpRule);
        }
        tmp = new SpawnRuleDefinition(ruleName.text, codeList);
        return tmp;
    }


}
#endif
