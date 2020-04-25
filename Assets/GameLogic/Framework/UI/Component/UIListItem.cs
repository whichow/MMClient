/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.18444
 * 
 * Author:          Coamy
 * Created:	        2015/7/24 12:08:46
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UIListItem : EventTriggerListener, IEventTriggerListener
{
    /// <summary>
    /// 获取 EventTriggerListener ，无则自动附加
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static new UIListItem Get(GameObject go)
    {
        UIListItem listener = go.GetComponent<UIListItem>();
        if (listener == null) {
            listener = go.AddComponent<UIListItem>();
        }
        return listener;
    }


    /// <summary>
    /// 数据源
    /// </summary>
    public object           dataSource
    {
        get { return m_dataSource; }
        set { m_dataSource = value; }
    }

    public int              index
    {
        get { return m_index; }
        set { m_index = value;
        }
    }

    public int              ID
    {
        get { return m_ID; }
        set { m_ID = value; }
    }

    public bool             IgnoreRay
    {
        get
        {
            return m_IgnoreRaycast != null && m_IgnoreRaycast.IsIgnore;
        }
        set
        {
            if (value) {
                if (m_IgnoreRaycast == null)
                    m_IgnoreRaycast = IgnoreRaycast.Ignore(gameObject);
                else
                    m_IgnoreRaycast.IsIgnore = true;
            }
            else {
                if (m_IgnoreRaycast == null)
                    m_IgnoreRaycast = gameObject.GetComponent<IgnoreRaycast>();

                if (m_IgnoreRaycast != null)
                    m_IgnoreRaycast.IsIgnore = false;
            }
        }
    }

    public bool             Selected
    {
        get { return m_selected; }
        set {
            m_selected = value;
            for (int i = 0; i < SelectBoxGOs.Count; i++) {
                SelectBoxGOs[i].SetActive(m_selected);
            }
            for (int j = 0; j < UnSelectBoxGOs.Count; j++) {
                UnSelectBoxGOs[j].SetActive(!m_selected);
            }
        }
    }

    public List<GameObject> SelectBoxGOs
    {
        get {
            if (m_selectedGOs == null) {
                _InitSelectBox();
            }
            return m_selectedGOs; 
        }
    }

    public List<GameObject> UnSelectBoxGOs
    {
        get {
            if (m_unSelectedGOs == null) {
                _InitSelectBox();
            }
            return m_unSelectedGOs;
        }
    }

    private void            _InitSelectBox()
    {
        m_selectedGOs = new List<GameObject>();
        m_unSelectedGOs = new List<GameObject>();
        Transform[] tfs = transform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < tfs.Length; i++) {
            if (tfs[i].name == "SelectBox") {
                m_selectedGOs.Add(tfs[i].gameObject);
            }
            else if (tfs[i].name == "UnSelectBox") {
                m_unSelectedGOs.Add(tfs[i].gameObject);
            }
        }
    }

    #region GetComp

    public new GameObject   gameObject
    {
        get {
            if (m_go == null)
                m_go = base.gameObject;
            return m_go;
        }
    }

    public new Transform    transform
    {
        get {
            if (m_tf == null)
                m_tf = base.transform;
            return m_tf;
        }
    }

    public T                GetComp<T>(string name = "") where T : Component
    {
        m_key = GetKey(typeof(T).Name, name);
        //string m_key = typeof(T).Name + "_" + name;
        if (!_CompDic.ContainsKey(m_key)) {
            if (string.IsNullOrEmpty(name)) {
                _CompDic.Add(m_key, transform.GetComponent<T>());
            }
            else {
                _CompDic.Add(m_key, transform.GetComponent<T>(name));
            }
        }
        return (T)_CompDic[m_key];
    }

    public Component        GetComp(string component, string name = "")
    {
        m_key = GetKey(component, name);
        //string m_key = component + "_" + name;
        if (!_CompDic.ContainsKey(m_key)) {
            if (string.IsNullOrEmpty(name)) {
                _CompDic.Add(m_key, transform.GetComponent(component));
            }
            else {
                _CompDic.Add(m_key, transform.GetComponent(component, name));
            }
        }
        return _CompDic[m_key];
    }

    public GameObject       GetGameObject(string name = "")
    {
        if (!_GoDic.ContainsKey(name)) {
            if (string.IsNullOrEmpty(name)) {
                _GoDic.Add(name, gameObject);
            }
            else {
                _GoDic.Add(name, transform.Find(name).gameObject);
            }
        }
        return _GoDic[name];
    }

    private string          GetKey(string component, string name)
    {
        m_strb.Length = 0;
        m_strb.Append(component);
        m_strb.Append("_");
        m_strb.Append(name);
        return m_strb.ToString();
    }

    private static StringBuilder   m_strb = new StringBuilder(80);
    private string          m_key;
    
    private Dictionary<string, Component> _CompDic
    {
        get {
            if (m_compDic == null)
                m_compDic = new Dictionary<string, Component>();
            return m_compDic;
        }
    }

    private Dictionary<string, GameObject> _GoDic
    {
        get {
            if (m_goDic == null)
                m_goDic = new Dictionary<string, GameObject>();
            return m_goDic;
        }
    }

    private Dictionary<string, Component> m_compDic;
    private Dictionary<string, GameObject> m_goDic;

    #endregion

    #region Member

    private object          m_dataSource;
    private int             m_index;
    private int             m_ID;

    private List<GameObject> m_selectedGOs;
    private List<GameObject> m_unSelectedGOs;
    private bool             m_selected;

    private Transform       m_tf;
    private GameObject      m_go;
    private IgnoreRaycast   m_IgnoreRaycast;

    #endregion
}