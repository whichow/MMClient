/** 
*FileName:     M3EditorLevelMenuCtrl.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-06 
*Description:    
*History: 
*/
#if UNITY_EDITOR
using Game;
using Game.Match3;
using M3Editor;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M3EditorLevelMenuCtrl : MonoBehaviour
{
    #region field

    private Button newBtn;
    public Toggle searchTypeToggle;
    private InputField goLvInput;

    private Button searchBtn;

    private GameObject levelItemGrid;
    private GameObject levelItemPrefab;
    private Dictionary<string, GameObject> lvObjDic = new Dictionary<string, GameObject>();
    private ScrollRect sr;

    #endregion

    private void Awake()
    {
        M3GameEvent.AddEvent(M3EditorEnum.ConfigCompelte, Init);
        M3GameEvent.AddEvent(M3EditorEnum.AddNewLevelBtn, AddNewBtn);
        M3GameEvent.AddEvent(M3EditorEnum.RemoveLevel, RemoveLevel);
    }

    private void RemoveLevel(object[] args)
    {
        M3EditorController.instance.EditorMode = mEditorMode.mNone;
        RemoveLevel(args[0].ToString());
    }

    private void AddNewBtn(object[] args)
    {
        foreach (var item in lvObjDic)
        {
            Destroy(item.Value);
        }
        lvObjDic.Clear();
        //AddNewLevel(data);

        Refresh();
    }

    void Init(object[] args)
    {
        newBtn = transform.Find("NewBtn").GetComponent<Button>();
        goLvInput = transform.Find("Search/InputField").GetComponent<InputField>();
        searchBtn = transform.Find("Search/goBtn").GetComponent<Button>();

        levelItemGrid = TransformUtils.GetChildByName(this.transform, "Grid").gameObject;
        levelItemPrefab = levelItemGrid.transform.Find("LevelItem").gameObject;
        sr = transform.Find("LevelList/scrollRect").GetComponent<ScrollRect>();

        newBtn.onClick.AddListener(OnNewBtnClick);
        searchBtn.onClick.AddListener(OnSearchClick);
        levelItemPrefab.SetActive(false);
        Refresh();
    }
    List<KeyValuePair<string, M3LevelData>> lst;
    private void Refresh()
    {
        lst = new List<KeyValuePair<string, M3LevelData>>();
        var dic = M3LevelConfigMgr.Instance.lvConfigDataDic;
        if (dic.Count > 0)
        {
            Dictionary<string, M3LevelData> tmpDic = new Dictionary<string, M3LevelData>();
            lst = new List<KeyValuePair<string, M3LevelData>>(dic);
            lst.Sort(delegate (KeyValuePair<string, M3LevelData> s1, KeyValuePair<string, M3LevelData> s2)
            {
                //return SortString(s1.Key,s2.Key);
                //return string.Compare(s1.Value.mapName, s2.Value.mapName, StringComparison.Ordinal);
                //return s1.Key.CompareTo(s2.Key);
                return s1.Key.CompareTo(s2.Key);

            });
            for (int i = 0; i < lst.Count; i++)
            {
                AddNewLevel(lst[i].Value);
            }
        }
    }

    public int SortString(string str1, string str2)
    {
        var charArr1 = str1.ToCharArray();
        var charArr2 = str2.ToCharArray();
        if (charArr1.Length < charArr2.Length)
        {
            for (int i = 0; i < charArr1.Length; i++)
            {
                if (charArr1[i] < charArr2[i])
                    return -1;
                if (charArr1[i] > charArr2[i])
                    return 1;
                else
                    continue;
            }
        }
        else if (charArr1.Length > charArr2.Length)
        {
            for (int i = 0; i < charArr2.Length; i++)
            {
                if (charArr1[i] < charArr2[i])
                    return -1;
                if (charArr1[i] > charArr2[i])
                    return 1;
                else
                    continue;
            }
        }
        else
        {
            for (int i = 0; i < charArr1.Length; i++)
            {
                if (charArr1[i] < charArr2[i])
                    return -1;
                if (charArr1[i] > charArr2[i])
                    return 1;
                else
                    continue;
            }
        }
        return 0;
    }
    private void OnSearchClick()
    {
        string tmp = goLvInput.text;
        int index = 0;
        bool flag = false;
        if (lvObjDic.ContainsKey(tmp))
        {
            flag = true;
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].Key == tmp)
                {
                    break;
                }
                index++;
            }
        }
        if (flag)
        {
            sr.verticalNormalizedPosition = 1 - (float)(index) / (lvObjDic.Count - 10);

            if (M3EditorController.instance.EditorMode == mEditorMode.mEditor || M3EditorController.instance.EditorMode == mEditorMode.mDelete)
            {
                M3GameEvent.DispatchEvent(M3EditorEnum.OnSaveBtnClick);
            }
            M3GameEvent.DispatchEvent(M3EditorEnum.StartGenMap, tmp, M3LevelConfigMgr.Instance.GetLevelConfigData(tmp));
            M3GameEvent.DispatchEvent(M3EditorEnum.ChangLevel, tmp);
            M3EditorController.instance.EditorMode = mEditorMode.mEditor;
        }
    }

    public void AddNewLevel(M3LevelData data)
    {
        GameObject go = Game.TransformUtils.Instantiate(levelItemPrefab, levelItemGrid.transform);
        go.SetActive(true);
        //go.transform.SetSiblingIndex(data.id[0]);
        lvObjDic.Add(data.id, go);
        M3EditorLevelItem item = go.GetComponent<M3EditorLevelItem>();
        item.Init(data);
    }
    public void RemoveLevel(string lv)
    {

        if (lvObjDic.ContainsKey(lv))
        {
            GameObject tmpGo = lvObjDic[lv];
            var item = tmpGo.GetComponent<M3EditorLevelItem>();
            item.Dispose();
            Destroy(tmpGo);
            lvObjDic.Remove(lv);
        }
    }
    private void OnNewBtnClick()
    {
        //M3GameEvent.DispatchEvent(M3EditorEnum.OnNewBtnClick);
        M3EditorController.instance.gridsettingCtrl.gameObject.SetActive(true);
    }
}
#endif