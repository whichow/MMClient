// ***********************************************************************
// Company          : 
// Author           : KimCh
// Copyright(c)     : 
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
using Game;
using System;
using UnityEngine;

/// <summary>
/// Bind Some Delegate Func For Yours.
/// </summary>
public class KUIBind : MonoBehaviour
{
    static bool _IsBind = false;

    public static void Bind(Func<string, GameObject> loader, Action<string, Action<GameObject>> asyncLoader)
    {
        if (!_IsBind)
        {
            _IsBind = true;
            //Debug.LogWarning("Bind For UI Framework.");

            //bind for your loader api to load UI.
            KUIManager.UILoader = loader;
            KUIManager.UIAsyncLoader = asyncLoader;
        }
    }

    internal static GameObject CreateUI(string path)
    {
        var prefab = KUIManager.UILoader(path);
        if (prefab)
        {
            var gameObj = Instantiate(prefab);
            return gameObj;
        }
        return null;
    }

    internal static void CreateUIAsync(string path, Action<GameObject> callback)
    {
        KUIManager.UIAsyncLoader(path, callback);
    }
}
