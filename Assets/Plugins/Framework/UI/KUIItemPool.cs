// ***********************************************************************
// Company          : 
// Author           : KimCh
// Copyright(c)     : KimCh
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KUIItemPool : MonoBehaviour
{
    #region Grid View

    /// <summary>
    /// 
    /// </summary>
    private ScrollRect _scrollRect;
    private RectTransform _contentRect;
    private RectTransform _viewportRect;

    private int _preferredElementCount;
    /// <summary>
    /// 
    /// </summary>
    private Vector3[] _corners = new Vector3[4];
    /// <summary>
    /// 子节点是否可见
    /// </summary>
    Dictionary<LayoutElement, bool> _elementCullStatus = new Dictionary<LayoutElement, bool>();

    private void Cull(LayoutElement element, bool visible)
    {
        element.SendMessage("Cull", visible, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitLayout()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _scrollRect.onValueChanged.AddListener(ScrollCallback);

        _contentRect = _scrollRect.content;
        _viewportRect = _scrollRect.viewport;

        var gridLayoutGroup = _contentRect.GetComponent<GridLayoutGroup>();
        var rect = _viewportRect.rect;
        var padding = gridLayoutGroup.padding;
        var spacing = gridLayoutGroup.spacing;
        var cellSize = gridLayoutGroup.cellSize;

        float cellPartX = (rect.width - padding.horizontal + spacing.x) / (cellSize.x + spacing.x);
        float cellPartY = (rect.height - padding.vertical + spacing.y) / (cellSize.y + spacing.y);

        if (_scrollRect.vertical)
        {
            _preferredElementCount = Mathf.FloorToInt(cellPartX) * Mathf.CeilToInt(cellPartY + 1f);
        }
        else
        {
            _preferredElementCount = Mathf.FloorToInt(cellPartY) * Mathf.CeilToInt(cellPartX + 1f);
        }
    }

    /// <summary>
    /// 重置下
    /// </summary>
    public void ResetLayout()
    {
        _scrollRect.normalizedPosition = Vector2.up;
    }

    /// <summary>
    /// 刷新
    /// </summary>
    public void Refresh()
    {
        UpdateChildren();
        _elementCullStatus.Clear();
    }

    void ScrollCallback(Vector2 data)
    {
        UpdateChildren();
    }

    void UpdateChildren()
    {
        if (_activeElements.Count <= _preferredElementCount)
        {
            for (int i = 0; i < _activeElements.Count; i++)
            {
                var element = _activeElements[i];

                Cull(element, true);
            }
            return;
        }

        if (_scrollRect.vertical)
        {
            _viewportRect.GetWorldCorners(_corners);

            float viewportTop = _corners[1].y;
            float viewportBottom = _corners[0].y;

            for (int i = 0; i < _activeElements.Count; i++)
            {
                var element = _activeElements[i];
                var elementRect = element.transform as RectTransform;

                elementRect.GetWorldCorners(_corners);

                float elementTop = _corners[1].y;
                float elementBottom = _corners[0].y;

                if (elementBottom <= viewportTop && elementTop >= viewportBottom)
                {
                    bool visible;
                    if (_elementCullStatus.TryGetValue(element, out visible) && visible)
                    {
                        continue;
                    }
                    _elementCullStatus[element] = true;
                    Cull(element, true);
                }
                else
                {
                    bool visible;
                    if (_elementCullStatus.TryGetValue(element, out visible) && !visible)
                    {
                        continue;
                    }
                    _elementCullStatus[element] = false;
                    Cull(element, false);
                }
            }
        }
        else
        {
            _viewportRect.GetWorldCorners(_corners);

            float viewportLeft = _corners[0].x;
            float viewportRight = _corners[2].x;

            for (int i = 0; i < _activeElements.Count; i++)
            {
                var element = _activeElements[i];
                var elementRect = element.transform as RectTransform;

                elementRect.GetWorldCorners(_corners);

                float elementLeft = _corners[0].x;
                float elementRight = _corners[2].x;

                if (elementRight >= viewportLeft && elementLeft <= viewportRight)
                {
                    bool visible;
                    if (_elementCullStatus.TryGetValue(element, out visible) && visible)
                    {
                        continue;
                    }
                    _elementCullStatus[element] = true;
                    Cull(element, true);
                }
                else
                {
                    bool visible;
                    if (_elementCullStatus.TryGetValue(element, out visible) && !visible)
                    {
                        continue;
                    }
                    _elementCullStatus[element] = false;
                    Cull(element, false);
                }
            }
        }
    }

    #endregion

    #region Field

    /// <summary>
    /// 
    /// </summary>
    private LayoutElement _elementTemplate;
    /// <summary>
    /// 
    /// </summary>
    private List<LayoutElement> _activeElements;
    /// <summary>
    /// 
    /// </summary>
    private Stack<LayoutElement> _inactiveElements;

    private KUIItem _itemTemplate;

    #endregion

    #region Property

    public LayoutElement elementTemplate
    {
        get { return _elementTemplate; }
    }

    public KUIItem itemTemplate
    {
        get { return _itemTemplate; }
    }

    #endregion

    #region Method

    public void SetItemType(System.Type type)
    {
        if (!_itemTemplate)
        {
            _elementTemplate = GetComponentInChildren<LayoutElement>();
            if (_elementTemplate)
            {
                _elementTemplate.ignoreLayout = true;
                _elementTemplate.gameObject.SetActive(false);
                _itemTemplate = _elementTemplate.gameObject.AddComponent(type) as KUIItem;
            }
        }
    }

    public void SpawnItem()
    {

    }

    public void SpawnItems()
    {

    }

    public void RefreshItems()
    {
        for (int i = 0; i < _activeElements.Count; i++)
        {
            var element = _activeElements[i];
            element.SendMessage("Refresh", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void InitPool()
    {
        if (_activeElements == null)
        {
            _activeElements = new List<LayoutElement>();
        }

        if (_inactiveElements == null)
        {
            _inactiveElements = new Stack<LayoutElement>();
        }

        if (!_elementTemplate)
        {
            _elementTemplate = GetComponentInChildren<LayoutElement>();
        }

        if (_elementTemplate)
        {
            _elementTemplate.ignoreLayout = true;
            _elementTemplate.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Clear()
    {
        ResetLayout();

        while (_activeElements.Count > 0)
        {
            this.Recycle(_activeElements[_activeElements.Count - 1]);
        }
        Refresh();
    }
    /// <summary>
    /// 
    /// </summary>
    public void ClearAndKeepPosition()
    {

        while (_activeElements.Count > 0)
        {
            this.Recycle(_activeElements[_activeElements.Count - 1]);
        }
        Refresh();
    }


    /// <summary>
    /// 生成一个元素引发刷新
    /// </summary>
    /// <param name="active"></param>
    /// <returns></returns>
    public LayoutElement SpawnElement()
    {
        var element = Create();
        Refresh();
        return element;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public LayoutElement[] SpawnElements(int count)
    {
        var elements = new LayoutElement[count];
        for (int i = 0; i < count; i++)
        {
            elements[i] = Create();
        }
        Refresh();
        return elements;
    }

    /// <summary>
    /// 删除一个元素引发刷新 
    /// </summary>
    /// <param name="element"></param>
    public void DespawnElement(LayoutElement element)
    {
        Recycle(element);
        Refresh();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="elements"></param>
    public void DespawnElements(LayoutElement[] elements)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            Recycle(elements[i]);
        }
        Refresh();
    }

    public LayoutElement GetElement(int index, bool recreate = false)
    {
        if (_activeElements.Count > index)
        {
            return _activeElements[index];
        }

        if (recreate)
        {
            return this.SpawnElement();
        }
        return null;
    }

    private LayoutElement Create()
    {
        LayoutElement element;
        if (_inactiveElements.Count > 0)
        {
            element = _inactiveElements.Pop();
        }
        else
        {
            element = Instantiate(_elementTemplate);
            element.transform.SetParent(_contentRect, false);
        }
        element.ignoreLayout = false;
        element.gameObject.SetActive(true);
        var elementRT = element.transform as RectTransform;
        if (elementRT)
        {
            elementRT.SetAsLastSibling();
        }

        _activeElements.Add(element);
        return element;
    }

    private void Recycle(LayoutElement element)
    {
        _inactiveElements.Push(element);
        _activeElements.Remove(element);
        element.ignoreLayout = true;
        element.gameObject.SetActive(false);
    }

    #endregion

    #region Unity 

    private void Awake()
    {
        InitLayout();
        InitPool();
    }

    #endregion 
}

