// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************

using UnityEngine;

/// <summary>
/// 单例模式
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonUnity<T> : MonoBehaviour where T : SingletonUnity<T>
{
    #region UNITY

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        if (!_Instance)
        {
            _Instance = this as T;
        }
    }

    #endregion

    #region STATIC

    /// <summary>
    /// 
    /// </summary>
    private static T _Instance;

    /// <summary>
    /// 
    /// </summary>
    public static T Instance
    {
        get
        {
            if (!_Instance)
            {
                new GameObject(typeof(T).Name, typeof(T));
            }
            return _Instance;
        }
    }

    #endregion
}
