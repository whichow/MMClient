// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KGameRes" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using K.AB;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// 资源托管
/// </summary>
public class KGameRes : MonoBehaviour
{
    #region Method 

    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    public static Object LoadAssetAtPath(string assetPath, bool retain = false)
    {
        var abInfo = ABManager.Instance.LoadAssetAtPath(assetPath);
        if (abInfo != null)
        {
            if (retain)
            {
                abInfo.Retain();
            }
            else
            {
                abInfo.Retain(Instance);
            }
            return abInfo.mainObject;
        }
        return null;
    }

    public static Object[] LoadAll(string path)
    {
        return null;
    }

    public static void LoadAsync(string path, Action<Object> callback)
    {
        ABManager.Instance.LoadAsync(path, (abInfo) =>
        {
            Object ret = null;
            if (abInfo != null)
            {
                ret = abInfo.mainObject;
            }
            if (callback != null)
            {
                callback(ret);
            }
        });
    }

    public static void LoadAsync(string path, bool retain, Action<Object> callback)
    {
        ABManager.Instance.LoadAsync(path, (abInfo) =>
        {
            Object ret = null;
            if (abInfo != null)
            {
                if (retain)
                {
                    abInfo.Retain(_Instance);
                }
                ret = abInfo.mainObject;
            }
            if (callback != null)
            {
                callback(ret);
            }
        });
    }

    public static string[] GetAllAssetPath(string path)
    {
        return ABDataManager.Instance.GetAllPath(path);
    }

    public static void UnloadAsset(Object assetToUnload)
    {
    }

    public static AsyncOperation UnloadUnusedAssets()
    {
        ABManager.Instance.UnloadUnused(true);
        _Instance.StartCoroutine(UnloadUnused());
        return null;
    }

    private static IEnumerator UnloadUnused()
    {
        yield return null;
        yield return null;
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    public static void Initialize(Action<string> callback)
    {
        ABManager.Instance.Initialize(callback);
    }


    #endregion

    #region Unity

    private static KGameRes _Instance;

    public static KGameRes Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject("GameRes").AddComponent<KGameRes>();
            }
            return _Instance;
        }
    }

    private void LateUpdate()
    {
        if (Time.frameCount % 6000 == 0)
        {
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
    }

    #endregion
}