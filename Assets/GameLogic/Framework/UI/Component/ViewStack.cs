/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.18063
 * 
 * Author:			Coamy
 * Created:			2014/10/31	16:20
 * Description:		ViewStack组件
 *                  子对象命名格式： Item0 , Item1 ... 
 *                  在编辑模式可手动拖入子对象至ItemList中，无需按格式命名
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//[ExecuteInEditMode]
[AddComponentMenu("UI/ViewStack")]
public class ViewStack : MonoBehaviour
{
    void                    Awake()
    {
        if (!m_isInit) Initialize();
    }

//#if UNITY_EDITOR
//    void                    Update()
//    {
//        //Initialize();
//    }
//#endif

    /// <summary>
    /// 选中的索引
    /// </summary>
    public int              selectedIndex
    {
        get { return m_selectedIndex; }
        set
        {
            if (!m_isInit) Initialize();

            m_selectedIndex = (value >= m_itemGOs.Count || value < 0 ? 0 : value);

            for (int i = 0; i < m_itemGOs.Count; i++) {
                if (m_itemGOs[i] != null)
                    GameObjectSetActive(m_itemGOs[i], i == m_selectedIndex);
            }
        }
    }

    public List<Transform> ItemList
    {
        get { return m_itemList; }
    }

    public List<GameObject> ItemGOs
    {
        get { return m_itemGOs; }
    }

    /// <summary>
    /// 当前显示的对象
    /// </summary>
    public Transform        selectedItem
    {
        get {
            if (!m_isInit) Initialize();

            return m_itemList[m_selectedIndex]; 
        }
    }

    /// <summary>
    /// 配合Toggle - selectedIndex 0/1开关
    /// </summary>
    public bool             toggleIsOn
    {
        get { return m_toggleIsOn; }
        set {
            selectedIndex = value ? 1 : 0;
            m_toggleIsOn = value;
            if (OnToggleSwitchEvent != null)
                OnToggleSwitchEvent(m_toggleIsOn);
        }
    }

    private void            Initialize()
    {
        m_isInit = true;
        if (m_itemList == null || m_itemList.Count == 0) {
            m_itemList = new List<Transform>();
            Transform _tf;
            for (int i = 0; i < transform.childCount; i++) {
                m_sb.Length = 0;
                m_sb.Append("Item").Append(i);
                _tf = transform.Find(m_sb.ToString());
                //_tf = transform.FindChild("Item" + i);
                if (_tf != null) 
                    m_itemList.Add(_tf);
            }
        }

        for (int i = 0; i < m_itemList.Count; i++) {
            if (m_itemList[i] != null) {
                m_itemGOs.Add(m_itemList[i].gameObject);
                GameObjectSetActive(m_itemGOs[i], i == m_selectedIndex);
            }
        }

        if (m_isToggle) {
            toggleIsOn = m_toggleIsOn;
            if (OnToggleSwitchEvent != null)
                OnToggleSwitchEvent(m_toggleIsOn);
        }
    }

    private void GameObjectSetActive(GameObject go, bool active)
    {
        if (go.activeSelf == active) return;
        go.SetActive(active);
    }

    #region Variables 变量

    public Action<bool>     OnToggleSwitchEvent;                            //开关选择事件
    public int              m_selectedIndex = 0;                            //选择的索引
    public bool             m_isToggle = false;                             //配合Toggle - selectedIndex 0/1开关
    public bool             m_toggleIsOn = false;                           //配合Toggle - selectedIndex 0/1开关
    public List<Transform>  m_itemList;                                     //所有条目集合
    
    private bool			m_isInit = false;
    private List<GameObject> m_itemGOs = new List<GameObject>();             //所有条目集合

    private static StringBuilder m_sb = new StringBuilder();

    #endregion

}