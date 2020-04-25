// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KFramework" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using K.Events;
using K.Fsm;
using UnityEngine;

public sealed class KFramework : MonoBehaviour
{
    #region Static

    /// <summary>
    /// 获取版本号。
    /// </summary>
    public static string Version
    {
        get
        {
            return "1.0.0";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static IFsmManager FsmManager
    {
        get;
        private set;
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEventManager EventManager
    {
        get;
        private set;
    }

    #endregion

    #region Unity  

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Awake()
    {
        FsmManager = gameObject.AddComponent<FsmManager>();
        EventManager = gameObject.AddComponent<EventManager>();
    }

    #endregion
}

