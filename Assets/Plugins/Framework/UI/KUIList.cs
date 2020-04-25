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

public class KUIList : MonoBehaviour
{
    #region Field

    /// <summary>
    /// 物品模版
    /// </summary>
    public GameObject itemTemplate;

    private List<KUIItem> _allItems = new List<KUIItem>();

    #endregion

    #region Property

    public int count
    {
        get
        {
            return _allItems.Count;
        }
    }

    public KUIItem this[int index]
    {
        get
        {
            return _allItems[index];
        }
    }

    #endregion

    #region Method

    private void ActivateAvailableObjects(int amount)
    {
        if (_allItems.Count < amount)
        {
            amount = _allItems.Count;
        }

        for (int i = 0; i < amount; i++)
        {
            _allItems[i].gameObject.SetActive(true);
        }

        for (int j = amount; j < _allItems.Count; j++)
        {
            _allItems[j].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="active">返回</param>
    /// <returns></returns>
    private KUIItem AddItem(bool active = true)
    {
        var gameObj = Instantiate(itemTemplate);
        gameObj.transform.SetParent(transform, false);
        gameObj.transform.localPosition = Vector3.zero;
        gameObj.transform.localScale = Vector3.one;

        var item = gameObj.GetComponent<KUIItem>();
        item.gameObject.SetActive(active);
        item.index = _allItems.Count;
        _allItems.Add(item);
        return item;
    }

    public void Clear(int amount = 0)
    {
        this.ActivateAvailableObjects((amount <= _allItems.Count) ? amount : _allItems.Count);
    }

    public KUIItem GetItem(int index, bool active = true)
    {
        if (_allItems.Count > index)
        {
            var item = _allItems[index];
            item.gameObject.SetActive(active);
            return item;
        }
        return this.AddItem(active);
    }

    public KUIItem GetItem()
    {
        var item = _allItems.Find(e => !e.gameObject.activeSelf);
        if (item == null)
        {
            return AddItem(true);
        }
        item.gameObject.SetActive(true);
        return item;
    }

    public void RefreshList(int amount)
    {
        ActivateAvailableObjects(amount);

        if (_allItems.Count < amount)
        {
            int num = amount - _allItems.Count;
            for (int i = 0; i < num; i++)
            {
                AddItem(true);
            }
        }
    }

    public void RestoreOrder()
    {
        for (int i = 0; i < _allItems.Count; i++)
        {
            _allItems[i].transform.SetSiblingIndex(i);
        }
    }

    #endregion

    #region Unity

    private void Awake()
    {
        if (itemTemplate != null)
        {
            itemTemplate.gameObject.SetActive(false);
        }
        else
        {
            itemTemplate = transform.GetChild(0).gameObject;
            itemTemplate.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Layout

    private void InitLayout()
    {

    }

    #endregion
}

