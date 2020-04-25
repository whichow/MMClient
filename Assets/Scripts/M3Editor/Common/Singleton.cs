#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： Singleton 
     * 创建人	： roy
     * 创建时间	： 2016/12/19 09:10:20 
     * 描述  	： Singleton
*/
#endregion

using UnityEngine;

public class Singleton<T> where T : new()
{
    private static T _instance;

    protected bool _blInited = false;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }

}

public class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    GameObject obj = new GameObject();

                    obj.name = typeof(T).ToString();
                    _instance = (T)obj.AddComponent(typeof(T));
                }
                _instance.Init();
            }
            return _instance;
        }
    }

    public virtual void Init()
    {

    }

}