// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KUIPool" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KUIPool : MonoBehaviour
{
    public GameObject itemTemplate;

    private List<GameObject> _activeItems = new List<GameObject>();
    private List<GameObject> _returnItems = new List<GameObject>();
    private ArrayList _itemDatas = new ArrayList();
    /// <summary>
    /// 
    /// </summary>
    public int itemCount
    {
        get { return _itemDatas.Count; }
    }

    internal RectTransform CreateItem(int index)
    {
        GameObject ret;
        if (_returnItems.Count > 0)
        {
            ret = _returnItems[_returnItems.Count - 1];
            _returnItems.RemoveAt(_returnItems.Count - 1);
        }
        else
        {
            ret = Instantiate(itemTemplate);
        }

        ret.hideFlags = HideFlags.None;
        ret.transform.SetParent(transform, false);
        ret.SetActive(true);
        var item = ret.GetComponent<KUIItem>();
        if (item)
        {
            item.SetData(_itemDatas[index]);
        }
        return ret.GetComponent<RectTransform>();
    }

    internal void ReturnItem(Transform item)
    {
        item.SetParent(null, false);
        item.gameObject.SetActive(false);
        item.gameObject.hideFlags = HideFlags.HideInHierarchy;
        _returnItems.Add(item.gameObject);
    }

    public void SetItemDatas(IList dataList)
    {
        _itemDatas.Clear();
        _itemDatas.AddRange(dataList);
    }

    public void AddItemData(object data)
    {
        _itemDatas.Add(data);
    }

    public void AddItemDatas(IList dataList)
    {
        _itemDatas.AddRange(dataList);
    }

    public void RefreshItems()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var item = transform.GetChild(i).GetComponent<KUIItem>();
            if (item)
            {
                item.SetData(item.data);
            }
        }
    }

    private void Awake()
    {
        if (!itemTemplate)
        {
            itemTemplate = transform.GetChild(0).gameObject;
        }

        if (itemTemplate)
        {
            itemTemplate.transform.SetParent(null, false);
            itemTemplate.SetActive(false);
            itemTemplate.hideFlags = HideFlags.HideInHierarchy;
        }
    }
}
