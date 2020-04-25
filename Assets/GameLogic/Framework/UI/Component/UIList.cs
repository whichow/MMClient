/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.18063
 * 
 * Author:			Coamy
 * Created:			2014/10/31	16:20
 * Description:		UIList组件
 * 
 * Update History:  14.11/04 增加自定义列表，手动添加子项，位置自定义，命名格式（Item0,Item1...） item == null  -- 【自定义列表】
 *                  14.11/04 无滚动效果，只生成子项，需自行挂载布局脚本 isAutoLayout = false     -- 【手动布局列表】
 *                  14.11/26 增加页数显示标记（PageBar），与滚动条同时只有一个
 *                  14.12/09 自动列表渲染机制修改为动态显示渲染，取消一次性生成所有格子
 *                  14.12/23 优化动态渲染为滚动时渲一排，减少渲染量
 *                  15.04/16 增加选中背景效果，item里添加GameObject名为SelectBox即可选中后显示该对象
 *                           Tip:遇到滚动错乱时,item的GameObject不要为空可添加Image组件alpha0...
 *                  16.03.02 增加页数功能 只适用于自动布局
 *                  16.09.28 SelectBox支持多个， UnSelectBox
 *                  16.10.10 在数量不足无需滚动时禁用Mask和ScrollRect
 *                  16.10.17 缓存UIListItem
 *                  18.05.25 手动布局不能填横列数量
 *                  19.04.29 去除有scollbal才能滚动的限制
 *                  
 *                  定位可设置scrollbal的value
 * 
 *******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Msg.ClientMessage;

[AddComponentMenu("UI/UIList")]
public class UIList : MonoBehaviour
{
    #region MonoBehaviour

    void					Awake()
    {
        if (!m_isInit) {
            Initialize();
            if (Application.isEditor) {                                    //编辑时测试用
                ArrayList arr = new ArrayList();
                int num = (int)(repeat.x * repeat.y);
                for (int i = 0; i < num; i++)
                    arr.Add(null);
                DataArray = arr;
            }
        }
    }

    #endregion

    #region Public Functions 公开方法

    /// <summary>
    /// 列表数据集合
    /// </summary>
    public IList            DataArray
    {
        get { return m_array; }
        set
        {
            if (!m_array.Equals(value))
            {
                if (value == null)
                {
                    //m_array.Clear();
                    m_array = new ArrayList();
                }
                else
                {
                    //ArrayList arr = new ArrayList(value.Count);
                    //foreach (var i in value) {
                    //    arr.Add(i);
                    //}
                    //m_array = arr;
                    m_array = value;
                }
            }

            if (!m_isInit) Initialize();

            if (item == null) m_totle = m_items.Count;
            else if (m_array.Count > 0) m_totle = m_array.Count;
            else m_totle = 1;

            ScrollReset();
            ChangeCells();
            RenderLayout();
            RenderItems();
        }
    }

    /// <summary>
    /// 选中项索引
    /// </summary>
    public int              SelectedIndex
    {
        get { return m_selectedIndex; }
        set
        {
            if (m_selectedIndex != value)
            {
                m_selectedIndex = value;
                RenderSelectBox();
                if (m_selectHandler != null)
                {
                    //selectHandler(m_selectedIndex < 0 ? null : GetItem(m_selectedIndex).GetComponent<UIListItem>(), m_selectedIndex);
                    GameObject go = GetItem(m_selectedIndex);
                    if (go != null)
                    {
                        m_selectHandler(m_uiListItemDic[go], m_selectedIndex);
                    }
                    else
                    {
                        if (m_selectedIndex > -1)
                            //Logger.Main.LogWarning("Index is not in items."); //todo

                            m_selectHandler(null, m_selectedIndex);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 非自动布局列表项显示对象集合
    /// </summary>
    public List<GameObject> Items
    {
        get
        {
            if (item == null || isAutoLayout == false) return m_items.GetRange(0, m_totle);
            else throw new Exception("Is not available, the list automatically.");
        }
    }

    /// <summary>
    /// 当前页码 适用于自动布局
    /// </summary>
    public int              Page
    {
        get { return m_page; }
        set
        {
            int __page = value;
            if (__page < 1) __page = 1;
            if (__page > TotlePage) __page = TotlePage;
            SetStartIndex((int)(repeat.y * repeat.x) * (__page - 1));
        }
    }

    /// <summary>
    /// 总页数 适用于自动布局
    /// </summary>
    public int              TotlePage
    {
        get { return m_totlePage; }
    }

    /// <summary>
    /// 列数
    /// </summary>
    public int              RepeatX
    {
        get { return (int)repeat.x; }
    }

    /// <summary>
    /// 行数
    /// </summary>
    public int              RepeatY
    {
        get { return (int)repeat.y; }
    }

    /// <summary>
    /// 显示的开始行数
    /// </summary>
    public int              StartLine
    {
        get { return m_startLine; }
    }

    /// <summary>
    /// 是否可以滚动
    /// </summary>
    public bool CanScroll
    {
        get { return m_canScroll; }
        set { m_canScroll = value; }
    }

    /// <summary>
    /// 设置数据渲染处方法
    /// </summary>
    /// <param name="handler"></param>
    public void             SetRenderHandler(UIItemDelegate handler)
    {
        m_renderHandler = handler;
    }

    /// <summary>
    /// 设置选中项目处理方法
    /// </summary>
    /// <param name="handler"></param>
    public void             SetSelectHandler(UIItemDelegate handler)
    {
        m_selectHandler = handler;
    }


    /// <summary>
    /// 设置选中项目处理方法
    /// </summary>
    /// <param name="handler"></param>
    public void SetPointerHandler(UIItemDelegate handler)
    {
        m_pointerHandler = handler;
    }
    
    /// <summary>
    /// 刷新列表
    /// </summary>
    public void             Refresh()
    {
        RenderItems();
    }

    public void             Refresh(IList _array)
    {
        DataArray.Clear();
        for (int i = 0; i < _array.Count; i++) {
            DataArray.Add(_array[i]);
        }
        RenderItems();
    }

    /// <summary>
    /// 清除并刷新列表
    /// </summary>
    public void             Clear()
    {
        DataArray = new ArrayList();
        //Refresh();
    }

    /// <summary>
    /// 根据数据索引获取项目数据
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public object           GetItemData(int index)
    {
        return index < m_array.Count && index > -1 ? m_array[index] : null;
    }

    /// <summary>
    /// 根据格子索引获取当前列表项显示对象
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject       GetItem(int index)
    {
        int itemIndex;
        if (item == null || isAutoLayout == false) itemIndex = index;
        else itemIndex = index - m_startIndex;

        if (itemIndex < m_items.Count && itemIndex > -1) return m_items[itemIndex];
        else return null;
    }

    public UIListItem       GetUIListItem(int index)
    {
        UIListItem item = null;
        GameObject key = GetItem(index);
        if (key != null) {
            m_uiListItemDic.TryGetValue(key, out item);
        }   
        return item;
    }
    
    /// <summary>
    /// 更新指定项数据
    /// </summary>
    public void             UpdateItem(int index, object data)
    {
        if (index < m_array.Count && index > -1) {
            m_array[index] = data;
            if (m_renderHandler != null && m_items.Count > index - m_startIndex && index - m_startIndex > -1) {
                UIListItem item = m_uiListItemDic[m_items[index - m_startIndex]];
                item.dataSource = data;
                m_renderHandler(item, m_startIndex + index);
            }
        }
        else {
            Debug.Log("index error:" + index);
        }
    }

    /// <summary>
    /// 更新指定项
    /// </summary>
    /// <param name="index"></param>
    public void             UpdateItem(int index)
    {
        if (index < m_array.Count && index > -1) {
            if (m_renderHandler != null && m_items.Count > index - m_startIndex && index - m_startIndex > -1) {
                m_renderHandler(m_uiListItemDic[m_items[index - m_startIndex]], m_startIndex + index);
            }
        }
        else {
            Debug.Log("index error:" + index);
        }
    }

    /// <summary>
    /// 实际的格子数量
    /// </summary>
    /// <returns></returns>
    public int              GetRealityCellNum()
    {
        int num;
        if (item == null || !isAutoLayout) {
            num = m_items.Count;
        }
        else {
            //if (scrollbar == null && pagebar == null)
            //    num = (int)(repeat.x * repeat.y);
            //else
                num = (int)(direction == Direction.Horizontal ? (repeat.x + 1) * repeat.y : repeat.x * (repeat.y + 1));
        }
        return num;                                                        //实际的格子数量
    }

    /// <summary>
    /// 滚动条定位 按开始行设置滚动条值
    /// </summary>
    //public void             SetScrollballValByLine(int line)
    //{
    //    float val;
    //    if (direction == Direction.Horizontal) {
    //        val = 1f - line / (array.Count / repeat.y - repeat.x - 1);
    //    }
    //    else {
    //        val = 1f - line / (array.Count / repeat.x - repeat.y - 1);
    //    }
    //    Debug.Log(val);
    //    scrollbar.value = val;
    //    //Utils.WaitForNextFrameUpdate(delegate() {
    //    //    scrollbar.value = val;
    //    //});

    //    //StartCoroutine(LateUpdate(delegate() {
    //    //    scrollbar.value = val;
    //    //}));
    //}

    #endregion

    #region Private Functions 私有方法

    private void            Initialize()
    {
        m_isInit = true;
        if (item == null) {
            CustomList();
        }
        else if (!isAutoLayout) {
            NoScrollList();
        }
        else {
            AutoList();
        }
    }

    /// <summary>
    /// 自定义列表：手动加入子项，命名为Item0，Item1 ...
    /// </summary>
    private void            CustomList()
    {
        Transform _tf;
        for (int i = 0; i < this.transform.childCount; i++) {
            _tf = this.transform.Find("Item" + i);
            if (_tf == null) {
                throw new Exception("List item name is break : Item" + i);
            }
            else {
                m_items.Add(_tf.gameObject);
            }
        }
    }

    /// <summary>
    /// 半自动列表：没有滚动，没有布局的列表，需自己挂载布局效果，否则会叠在一起
    /// </summary>
    private void            NoScrollList()
    {
    }
    
    /// <summary>
    /// 全自动列表：滚动，布局等自动生成
    /// </summary>
    private void            AutoList()
    {
        m_listRt = this.GetComponent<RectTransform>();
        m_itemSize = item.GetComponent<RectTransform>().sizeDelta;
        CreateComp();
    }

    private void            CreateComp()
    {
        Transform _contentTf = this.transform.Find("Content_sn");
        Transform _itemboxTf = this.transform.Find("Content_sn/Items_sn");
        if (_contentTf != null && _itemboxTf != null) {
            m_scrollRect = this.gameObject.GetComponent<ScrollRect>();
            m_contentRt = _contentTf.GetComponent<RectTransform>();
            m_itemboxRt = _itemboxTf.GetComponent<RectTransform>();

            int num = m_itemboxRt.childCount;
            for (int i = 0; i < num; i++) {
                m_items.Add(m_itemboxRt.GetChild(i).gameObject);
            }
            return;
        }

        if (this.gameObject.GetComponent<Image>() == null)
            m_scrollRectMaskImage = this.gameObject.AddComponent<Image>();

        m_scrollRectMask = this.gameObject.AddComponent<Mask>();
        m_scrollRectMask.showMaskGraphic = false;

        m_scrollRect = this.gameObject.AddComponent<ScrollRect>();
        m_scrollRect.onValueChanged.AddListener(OnScrollHandler);
        m_scrollRect.scrollSensitivity = 30;

        //if (scrollbar == null && pagebar == null) {
        //    m_scrollRect.enabled = false;
        //    gameObject.GetComponent<Image>().enabled = false;
        //    gameObject.GetComponent<Mask>().enabled = false;
        //}

        GameObject m_content = new GameObject("Content_sn");
        m_content.transform.SetParent(this.transform, false);
        m_contentRt = m_content.AddComponent<RectTransform>();
        m_contentRt.pivot = Vector2.up;
        m_contentRt.anchorMax = Vector2.up;
        m_contentRt.anchorMin = Vector2.up;

        GameObject m_itembox = new GameObject("Items_sn");
        m_itembox.transform.SetParent(m_content.transform, false);
        m_itemboxRt = m_itembox.AddComponent<RectTransform>();
        m_itemboxRt.pivot = Vector2.up;
        m_itemboxRt.anchorMax = Vector2.up;
        m_itemboxRt.anchorMin = Vector2.up;
        m_gridLG = m_itembox.AddComponent<GridLayoutGroup>();
        m_gridLG.cellSize = m_itemSize;

        m_scrollRect.content = m_contentRt;
        if (pagebar != null) {
            pagebar.SetScrollRect(m_scrollRect, m_contentRt);
        }

        if(pagebar != null){
            pagebar.transform.SetAsLastSibling();
        }
        if (scrollbar != null) {
            scrollbar.transform.SetAsLastSibling();
        }

        SetStyle();
        CreateCells();
    }

    private void            SetStyle()
    {
        Vector2 _cellSize = m_itemSize + spacing;
        float w = _cellSize.x * repeat.x - spacing.x;
        float h = _cellSize.y * repeat.y - spacing.y;
        Vector2 _listSize = new Vector2(w, h);
        m_gridLG.spacing = spacing;
        if (direction == Direction.Horizontal) {
            m_scrollRect.horizontal = true;
            m_scrollRect.vertical = false;
            m_scrollRect.horizontalScrollbar = scrollbar;
            m_gridLG.startAxis = GridLayoutGroup.Axis.Vertical;
            m_itemboxRt.sizeDelta = new Vector2(w + _cellSize.x, h);
            if (m_listRt.sizeDelta.y > h) _listSize.y = m_listRt.sizeDelta.y;
        }
        else {
            m_scrollRect.horizontal = false;
            m_scrollRect.vertical = true;
            m_scrollRect.verticalScrollbar = scrollbar;
            m_gridLG.startAxis = GridLayoutGroup.Axis.Horizontal;
            m_itemboxRt.sizeDelta = new Vector2(w, h + _cellSize.y);
            if (m_listRt.sizeDelta.x > w) _listSize.x = m_listRt.sizeDelta.x;
        }
        if (isAutoScrollRectSize) m_listRt.sizeDelta = _listSize;
    }

    private void            CreateCells()
    {
        int num = GetRealityCellNum();
        for (int i = 0; i < num; i++) {
            GameObject _go = (i == 0 ? item : Instantiate(item) as GameObject);
            _go.transform.SetParent(m_itemboxRt.transform, false);
            LayoutElement le = _go.GetComponent<LayoutElement>();
            if (le == null) {
               le = _go.AddComponent<LayoutElement>();
            }
            le.preferredWidth = m_itemSize.x;
            le.preferredHeight = m_itemSize.y;
            m_items.Add(_go);
        }
    }

    private void            OnScrollHandler(Vector2 vec)                   //设置ItemsBox位置和渲染索引
    {
        if (m_totle <= repeat.x * repeat.y) return;

        Vector2 _size = Vector2.zero;
        _size.x = vec.x * (m_contentRt.sizeDelta.x - m_listRt.rect.width);
        _size.y = (1-vec.y) * (m_contentRt.sizeDelta.y - m_listRt.rect.height); //Rect top1 bottom0

        int _startLine;                                                    //开始的行
        int _lineTotle;                                                    //横竖总数
        int _startLineMax;                                                 //开始行的最大值
        if (direction == Direction.Horizontal) {
            _startLine = (int)(_size.x / (m_itemSize.x + spacing.x));
            _lineTotle = Mathf.CeilToInt(m_totle / repeat.y);
            _startLineMax = (int)(_lineTotle - (repeat.x + 1));
        }
        else {
            _startLine = (int)(_size.y / (m_itemSize.y + spacing.y));
            _lineTotle = Mathf.CeilToInt(m_totle / repeat.x);
            _startLineMax = (int)(_lineTotle - (repeat.y + 1));
        }

        if ((direction == Direction.Horizontal && vec.x > 1) || (direction == Direction.Vertical && vec.y < 0)) _startLine = _startLineMax;   //drag out  update 16.06.08

        if (m_startLine < _startLine) {
            for (int i = m_startLine + 1; i < _startLine + 1; i++) {
                SetStartLine(i, _startLineMax);
            }
        }
        else if (m_startLine > _startLine) {
            for (int i = m_startLine - 1; i > _startLine - 1; i--) {
                SetStartLine(i, _startLineMax);
            }
        }
        //else {
        //    SetStartLine(_startLine, _startLineMax);
        //}
    }

    private void            SetStartLine(int startLine, int starLineMax)
    {
        if (startLine < 0 || startLine > starLineMax || startLine == m_startLine) return;

        if (direction == Direction.Horizontal) {
            SetStartIndex(startLine * (int)repeat.y, true);
            m_itemboxRt.anchoredPosition = new Vector2(startLine * (m_itemSize.x + spacing.x), 0);
        }
        else {
            SetStartIndex(startLine * (int)repeat.x, true);
            m_itemboxRt.anchoredPosition = new Vector2(0, -startLine * (m_itemSize.y + spacing.y));
        }
        m_startLine = startLine;
    }

    private void            SetStartIndex(int index, bool onScroll = false)
    {
        if (m_startIndex == index) return;
        bool dir = m_startIndex < index;
        m_startIndex = index;
        m_page = (int)(m_startIndex / (repeat.y * repeat.x)) + 1;
        if (onScroll) OnScrollRender(dir);                                 //只渲一排
        else RenderItems();                                                //渲固定格子
    }

    private void            ScrollReset()
    {
        //if (m_scrollRect && scrollbar != null)
        if (m_scrollRect)
        {
            m_scrollRect.StopMovement();
            m_scrollRect.horizontalNormalizedPosition = 0;
            m_scrollRect.verticalNormalizedPosition = 1;                       //top1 bottom0
            m_itemboxRt.anchoredPosition = Vector2.zero;
            m_startLine = 0;
            m_startIndex = 0;

            bool needScroll = false;
            if (m_canScroll)
            {
                if (direction == Direction.Horizontal)
                {
                    int dataCol = Mathf.CeilToInt((float)m_array.Count / repeat.y);
                    needScroll = dataCol >= repeat.x;
                }
                else
                {
                    int dataRow = Mathf.CeilToInt((float)m_array.Count / repeat.x);
                    needScroll = dataRow >= repeat.y;
                }
                //needScroll = (scrollbar != null || pagebar != null) && needScroll;
            }

            m_scrollRect.enabled = needScroll;
            m_scrollRectMask.enabled = needScroll;
            if (m_scrollRectMaskImage != null) m_scrollRectMaskImage.enabled = needScroll;
        }
    }

    private void            RenderLayout()
    {
        if (item == null || !isAutoLayout) return;
        //if (m_scrollRect.enabled) ScrollReset(); //15.03.11   //15.09.03 ??

        Vector2 _contentSize;
        Vector2 _cellSize = m_itemSize + spacing;
        float w = _cellSize.x * repeat.x - spacing.x;
        float h = _cellSize.y * repeat.y - spacing.y;

        m_totlePage = Mathf.CeilToInt((float)m_totle / (repeat.y * repeat.x));
        if (m_totlePage < 1) m_totlePage = 1;

        if (pagebar != null) {
            pagebar.Totle = m_totlePage;
            m_totle = pagebar.Totle * (int)(repeat.y * repeat.x);
        } 
        if (direction == Direction.Horizontal) {
            int _col = Mathf.CeilToInt((float)m_totle / repeat.y);
            _contentSize = new Vector2(_col * m_itemSize.x + (_col - 1) * spacing.x, h);
        }
        else {
            int _row = Mathf.CeilToInt((float)m_totle / repeat.x);
            _contentSize = new Vector2(w, _row * m_itemSize.y + (_row - 1) * spacing.y);
        }
        m_contentRt.sizeDelta = _contentSize;
        m_contentRt.anchoredPosition = Vector2.zero;
    }

    private void            RenderItems()
    {
        int num = GetRealityCellNum();
        GameObject _go;
        int _index;
        int _cellnum = (int)(repeat.x * repeat.y);                         //可见的格子数量
        for (int i = 0; i < num; i++) {
            _go = m_items[i];
            _index = m_startIndex + i;
            _go.SetActive(m_array.Count > _index);
            SetEventTriggerListener(_go, _index);
            if (m_renderHandler != null) {
                if (m_array.Count > _cellnum || i < _cellnum) {            //数据量小于格子数时，滚动用一排隐藏格子将不回调渲染
                    //renderHandler(_go.GetComponent<UIListItem>(), _index);
                    m_renderHandler(m_uiListItemDic[_go], _index);
                }
            }
        }
    }

    private void            RenderSelectBox()
    {
        int num = GetRealityCellNum();
        //Transform _tf;
        int _index;
        for (int i = 0; i < num; i++) {
            _index = m_startIndex + i;
            //m_items[i].GetComponent<UIListItem>().Selected = m_selectedIndex == _index;
            m_uiListItemDic[m_items[i]].Selected = m_selectedIndex == _index;
            //_tf = m_items[i].transform.FindChild("SelectBox");
            //if (_tf) {
            //    _tf.gameObject.SetActive(m_selectedIndex == _index);
            //}
        }
    }

    private void            OnScrollRender(bool dir)
    {
        GameObject _go;
        int _index;
        int cellNum = (int)(repeat.x * repeat.y);
        int num = (int)(direction == Direction.Horizontal ? repeat.y : repeat.x);
        for (int i = 0; i < num; i++) {
            if (dir) {                                                     //正向滚
                _index = m_startIndex + cellNum + i;
                _go = m_items[0];
                _go.transform.SetAsLastSibling();
                m_items.RemoveAt(0);
                m_items.Add(_go);
            }
            else {
                _index = m_startIndex + i;
                _go = m_items[cellNum + i];
                _go.transform.SetSiblingIndex(i);
                m_items.RemoveAt(cellNum + i);
                m_items.Insert(i, _go);
            }
            _go.SetActive(m_array.Count > _index);
            SetEventTriggerListener(_go, _index);
            if (m_renderHandler != null)
                //renderHandler(_go.GetComponent<UIListItem>(), _index);
                m_renderHandler(m_uiListItemDic[_go], _index);
        }
    }

    private void            SetEventTriggerListener(GameObject go, int index)
    {
        //IgnoreRaycast.Raycast(go, selectHandler == null);
        //UIListItem e = UIListItem.Get(go);
        UIListItem e;
        if (!m_uiListItemDic.TryGetValue(go, out e)) {
            e = UIListItem.Get(go);
            m_uiListItemDic.Add(go, e);
        }
        e.IgnoreRay = m_selectHandler == null && m_pointerHandler == null;
        e.OnClick = ClickHandler;
        e.dataSource = m_array.Count > index ? m_array[index] : null;
        e.index = index;
        e.Selected = m_selectedIndex == index;
        //Transform _tf = go.transform.FindChild("SelectBox");
        //if (_tf) {
        //    _tf.gameObject.SetActive(m_selectedIndex == index);             //set selectbox
        //}
    }

    private void            ChangeCells()                                  //无滚动条，手动布局时创建格子
    {
        if (isAutoLayout || item == null) return;
        int num = DataArray.Count;
        GameObject _go;
        for (int i = 0; i < num; i++) {
            if (m_items.Count > i) _go = m_items[i];
            else {
                _go = (i == 0 ? item : Instantiate(item) as GameObject);
                _go.transform.SetParent(item.transform.parent, false);
                m_items.Add(_go);
            }
            _go.SetActive(true);
        }
        if (m_items.Count == 0) {
            m_items.Add(item);
        }

        for (int j = num; j < m_items.Count; j++) {
            m_items[j].SetActive(false);
        }
    }

    private void            ClickHandler(IEventTriggerListener e, PointerEventData eventData)
    {
        SelectedIndex = (e as UIListItem).index;
        m_pointerHandler?.Invoke((e as UIListItem), m_selectedIndex);
    }

    #endregion

    #region Variables 变量

    public delegate void UIItemDelegate(UIListItem item, int index);
    private UIItemDelegate m_renderHandler;                                 //渲染处理方法
    private UIItemDelegate m_selectHandler;							        //选中处理方法
    private UIItemDelegate m_pointerHandler;							    //点击处理方法

    public bool             isAutoLayout = true;                            //自动布局
    public bool             isAutoScrollRectSize = true;                    //自动设置滚动区域
    public GameObject		item;										    //列表项
    public Vector2          repeat = Vector2.one;                           //横竖数量
    public Vector2			spacing;									    //项之间的间距
    public Direction		direction = Direction.Vertical;				    //滚动方向
    public Scrollbar		scrollbar;									    //滚动条
    public PageBar          pagebar;                                        //翻页条

    private int				m_totle = 0;								    //列表项总数
    private Vector2			m_itemSize;                                     //格子大小
    private RectTransform	m_listRt;
    private RectTransform	m_contentRt;
    private RectTransform	m_itemboxRt;
    private GridLayoutGroup m_gridLG;
    private ScrollRect      m_scrollRect;
    private Mask            m_scrollRectMask;
    private Image           m_scrollRectMaskImage;
    private int             m_startLine = 0;
    private int             m_startIndex = 0;                               //分页时用
    private int             m_page = 1;
    private int             m_totlePage = 1;
    private bool            m_canScroll = true;

    private bool			m_isInit = false;
    private int				m_selectedIndex = -1;
    private IList		    m_array = new ArrayList();
    private List<GameObject> m_items = new List<GameObject>();
    private Dictionary<GameObject, UIListItem> m_uiListItemDic = new Dictionary<GameObject, UIListItem>();
    
    #endregion

    public enum Direction
    {
        Horizontal,
        Vertical
    }

}