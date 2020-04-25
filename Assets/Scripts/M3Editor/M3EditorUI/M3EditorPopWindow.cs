/** 
*FileName:     M3EditorPopWindow.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2018-01-02 
*Description:    
*History: 
*/
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using M3Editor;

public class M3EditorPopWindow : MonoBehaviour
{
    public Button saveBtn;
    public Button closeBtn;
    public Action action;
    public GameObject templete;
    public List<M3PopWindowTemplete> list = new List<M3PopWindowTemplete>();
    private void Awake()
    {
        templete = transform.Find("grid/Templete").gameObject;
        saveBtn = transform.Find("saveBtn").GetComponent<Button>();
        closeBtn = transform.Find("closeBtn").GetComponent<Button>();
        saveBtn.onClick.AddListener(OnSave);
        closeBtn.onClick.AddListener(OnClose);
        templete.SetActive(false);
        this.gameObject.SetActive(false);
    }

    private void OnClose()
    {
        Clear();
        this.gameObject.SetActive(false);
    }

    private void OnSave()
    {
        if (action != null)
        {
            action();
        }
        this.gameObject.SetActive(false);
    }

    public void OpenRandomHidden(string[] titles)
    {
        Clear();
        this.gameObject.SetActive(true);
        action = delegate ()
        {
            M3EditorHidden module = M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorHidden>();
            module.countmax = 999;
            module.countrandom = int.Parse(list[0].input.text);
            module.count = 0;
            module.color= new Color((float)UnityEngine.Random.Range(1, 255) / 255f, (float)UnityEngine.Random.Range(1, 128) / 255f, (float)UnityEngine.Random.Range(1, 255) / 255f);
        };
        if (titles != null)
        {
            for (int i = 0; i < titles.Length; i++)
            {
                var obj = Instantiate(templete);
                obj.transform.SetParent(templete.transform.parent,false);
                obj.SetActive(true);
                var tmp = obj.GetComponent<M3PopWindowTemplete>();
                tmp.Show(titles[i]);
                list.Add(tmp);
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject.Destroy(list[i].gameObject);
        }
        list.Clear();
        action = null;
    }
}
#endif
