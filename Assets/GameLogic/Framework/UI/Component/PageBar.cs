/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.18063
 * 
 * Author:          Coamy
 * Created:	        2014/11/26 13:17:02
 * Description:     列表翻页条
 * 
 * Update History:  
 * 
 *******************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PageBar : MonoBehaviour
{
    #region MonoBehaviour

    void                    Awake()
    {
        if (!m_isInit) Initialize();
    }

    #endregion

    #region Public Functions 公开方法

    /// <summary>
    /// 当前页 1~
    /// </summary>
    public int              Page
    {
        get { return m_page; }
        set
        {
            if (value < 1) value = 1;
            else if (value > m_totle) value = m_totle;
            if (value == m_page) return;
            if (m_items.Count > 0) {
                m_items[m_page - 1].toggleIsOn = false;
                m_items[value - 1].toggleIsOn = true;
            }
            m_page = value;
        }
    }

    /// <summary>
    /// 总页数
    /// </summary>
    public int              Totle
    {
        get { return m_totle; }
        set
        {
            if (value < 0) value = 1;
            m_totle = value;
            RenderItems();
        }
    }

    public void             SetScrollRect(ScrollRect scrollrect, RectTransform contentRt)
    {
        m_scrollRect = scrollrect;
        m_scrollRect.onValueChanged.AddListener(OnScrollHandler);
    }

    #endregion

    #region Private Functions 私有方法

    private void            Initialize()
    {
        m_isInit = true;
        m_item = this.transform.Find("ViewStack").gameObject;
    }

    private void            RenderItems()
    {
        if (!m_isInit) Initialize();
        ViewStack _vs;
        for (int i = 0; i < m_totle; i++) {
            if (m_items.Count > i) {
                _vs = m_items[i];
            }
            else {
                GameObject _go = (i == 0 ? m_item : Instantiate(m_item) as GameObject);
                _go.transform.SetParent(this.transform, false);
                _vs = _go.GetComponent<ViewStack>();
                m_items.Add(_vs);
            }
            _vs.gameObject.SetActive(true);
            _vs.toggleIsOn = i + 1 == m_page;

        }
        for (int j = m_totle; j < m_items.Count; j++) {
            m_items[j].gameObject.SetActive(false);
        }
    }

    private void            OnScrollHandler(Vector2 vec)
    {
        if (vec.x < 0) vec.x = 0;                                          //暂时只有横向翻页条
        if (vec.x > 1) vec.x = 1;
        int _page = Mathf.FloorToInt((m_totle - 1) * vec.x) + 1;           //content的当前比例是按减一屏算的
        if (_page < Page) _page = Mathf.CeilToInt((m_totle - 1) * vec.x) + 1;
        if (_page != Page) Page = _page;
    }

    #endregion

    #region Variables 变量

    private ScrollRect      m_scrollRect;
    private int             m_page = 1;                                    //从1开始
    private int             m_totle = 1;
    private bool			m_isInit = false;
    private GameObject      m_item;
    private List<ViewStack> m_items = new List<ViewStack>();

    #endregion
}