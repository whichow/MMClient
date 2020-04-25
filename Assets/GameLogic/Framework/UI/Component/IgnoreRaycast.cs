using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 挂载后忽略鼠标事件
/// </summary>
public class IgnoreRaycast : MonoBehaviour, ICanvasRaycastFilter 
{
    public static IgnoreRaycast Ignore(GameObject go)
    {
        IgnoreRaycast raycast = go.GetComponent<IgnoreRaycast>();
        if (raycast == null)
            raycast = go.AddComponent<IgnoreRaycast>();
        return raycast;
    }

    public static void      UnIgnore(GameObject go)
    {
        Destroy(go.GetComponent<IgnoreRaycast>());
    }

    public static void      Raycast(GameObject go, bool isIgnore)
    {
        if (isIgnore) Ignore(go);
        else UnIgnore(go);
    }

    public bool             IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return !IsIgnore;
    }

    public bool             IsIgnore = true;

}