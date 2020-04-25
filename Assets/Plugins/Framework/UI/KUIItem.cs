// ***********************************************************************
// Company          : 
// Author           : KimCh
// Copyright(c)     : KimCh
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
using UnityEngine;

public class KUIItem : MonoBehaviour
{
    #region Field

    private bool _visible = true;
    /// <summary>
    /// 
    /// </summary>
    private RectTransform _rectTransform;

    #endregion

    #region Property 

    public int index
    {
        get;
        set;
    }

    public object data
    {
        get;
        private set;
    }

    protected bool visible
    {
        get;
        set;
    }

    #endregion 

    #region Method

    public void SetData(object data)
    {
        this.data = data;
        Refresh();
    }

    private void Cull(bool visible)
    {
        OnCull(visible);
    }

    protected virtual void OnCull(bool visible)
    {
        transform.GetChild(0).gameObject.SetActive(visible);
    }

    protected virtual void Refresh()
    {
    }

    public T Find<T>(string path) where T : Component
    {
        var child = transform.Find(path);
        if (child)
        {
            return child.GetComponent<T>();
        }
        return null;
    }
    public T Find<T>() where T : Component
    {
        return transform.GetComponent<T>();
    }
    public GameObject Find(string path)
    {
        var child = transform.Find(path);
        if (child)
        {
            return child.gameObject;
        }
        return null;
    }

    #endregion
}

