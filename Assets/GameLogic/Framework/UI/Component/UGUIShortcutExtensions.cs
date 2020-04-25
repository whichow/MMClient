/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.18444
 * 
 * Author:          Coamy
 * Created:	        2015/12/11 19:21:36
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UGUIShortcutExtensions
{
    #region GameObject
    public static Component GetComponent(this GameObject target, string component, string name)
    {
        return target.transform.GetComponent(component, name);
    }

    public static T         GetComponent<T>(this GameObject target, string name) where T : Component
    {
        return target.transform.GetComponent<T>(name);
    }

    public static Component GetComp(this GameObject target, string component, string name) //Lua专用
    {
        return target.transform.GetComponent(component, name);
    }

    public static Component GetCompInChildren(this GameObject target, string component)
    {
        return target.transform.GetCompInChildren(component);
    }

    public static T         GetCompInChildren<T>(this GameObject target) where T : Component
    {
        return target.transform.GetCompInChildren<T>();
    }

    public static List<Transform> GetChildren(this GameObject target)
    {
        return target.transform.GetChildren();
    }
    #endregion

    #region Transform
    public static Component GetComponent(this Transform target, string component, string name)
    {
        Transform child = target.Find(name);
        if (child == null) {
            Debuger.Log("[GetComponent] Child no found!  Name:" + name, target);
            throw new Exception("[GetComponent] Child no found!  Name:" + name);
        }
        Component comp = child.GetComponent(component);
        if (comp == null) {
            Debuger.Log("[GetComponent] Component no found! Component:" + component + " - From:" + name, target);
            throw new Exception("[GetComponent] Component no found! Component:" + component + " - From:" + name);
        }
        return comp;
    }

    public static T         GetComponent<T>(this Transform target, string name) where T : Component
    {
        Transform child = target.Find(name);
        if (child == null) {
            //Debug.Log("[GetComponent] Child no found!  Name:" + name, target);
            throw new Exception("[GetComponent] Child no found!  Name:" + name);
        }
        T comp = child.GetComponent<T>();
        if (comp == null) {
            //Debug.Log("[GetComponent] Component no found! Component:" + typeof(T) + " - From:" + name, target);
            throw new Exception("[GetComponent] Component no found! Component:" + typeof(T) + " - From:" + name);
        }
        return comp;
    }

    public static Component GetComp(this Transform target, string component, string name) //Lua专用
    {
        return target.GetComponent(component, name);
    }

    public static Component GetComp(this RectTransform target, string component, string name) //Lua专用
    {
        return target.GetComponent(component, name);
    }
    
    public static Component GetCompInChildren(this Transform target, string component)
    {
        Component comp = null;
        Transform[] tfs = target.GetComponentsInChildren<Transform>();
        for (int i = 0; i < tfs.Length; i++)
        {
            Component c = tfs[i].GetComponent(component);
            if (c != null)
            {
                comp = c;
                break;
            }
        }
        return comp;
    }

    public static T         GetCompInChildren<T>(this Transform target) where T : Component
    {
        T comp = null;
        Transform[] tfs = target.GetComponentsInChildren<Transform>();
        for (int i = 0; i < tfs.Length; i++)
        {
            T c = tfs[i].GetComponent<T>();
            if (c != null)
            {
                comp = c;
                break;
            }
        }
        return comp;
    }

    public static List<Transform> GetChildren(this Transform target)
    {
        List<Transform> list = new List<Transform>();
        for (int i = 0; i < target.childCount; i++)
        {
            list.Add(target.GetChild(i));
        }
        return list;
    }
    #endregion


    #region ScrollRect
    public static void ScrollToTop(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    public static void ScrollToBottom(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
    #endregion

    #region CanvasGroup

    public static void SetInteractable(this CanvasGroup target, bool b)
    {
        target.interactable = b;
    }

    public static void SetBlocksRaycasts(this CanvasGroup target, bool b)
    {
        target.blocksRaycasts = b;
    }

    #endregion

}


public class DoTweenExtend
{
    #region RectTransform

    public static Tweener   DOMove(RectTransform target, Vector3 endValue, float duration, bool snapping = false)
    {
        return target.DOMove(endValue, duration, snapping);
    }
    public static Tweener   DOAnchorPos(RectTransform target, Vector2 endValue, float duration, bool snapping = false)
    {
        return target.DOAnchorPos(endValue, duration, snapping);
    }
    public static Tweener   DoScaleX(RectTransform target, float endValue, float duration)
    {
        return target.DOScaleX(endValue, duration);
    }
    public static Tweener   DoScale(RectTransform target, Vector3 endValue, float duration)
    {
        return target.DOScale(endValue, duration);
    }
    public static Tweener   DoScale(RectTransform target, Vector3 endValue, float duration, Ease ease)
    {
        return target.DOScale(endValue, duration).SetEase(ease);
    }
    public static Tweener   DoScale(RectTransform target, float endValue, float duration)
    {
        return target.DOScale(endValue, duration);
    }
    public static Tweener   DoScale(RectTransform target, float endValue, float duration , float delayTime)
    {
        return target.DOScale(endValue, duration).SetDelay(delayTime);
    }
    public static Tweener   DORotationY(RectTransform target, Vector3 endValue, float duration)
    {
        return target.DORotate(endValue, duration, RotateMode.Fast);
    }
    public static Tweener DOShakeAnchorPos(RectTransform target, float duration, Vector2 strength, int vibrato, float randomness)
    {
        return target.DOShakeAnchorPos(duration, strength, vibrato, randomness);
    }
    //public static void      DoAnchorPos(RectTransform target, Vector2 endValue, float duration, bool snapping = false)
    //{
    //    target.DOAnchorPos(endValue, duration, snapping);
    //}
    #endregion

    #region CanvasGroup
    public static Tweener   DOFade(CanvasGroup target, float endValue, float duration)
    {
        return target.DOFade(endValue, duration);
    }

    public static Tweener DOFade(CanvasGroup target, float startValue, float endValue, float duration)
    {
        target.alpha = startValue;
        return target.DOFade(endValue, duration);
    }
    #endregion

    #region LayoutElement
    public static void      DOPreferredSize(LayoutElement target, Vector2 endValue, float duration)
    {
        target.DOPreferredSize(endValue, duration).SetEase(Ease.InCubic);
    }
    #endregion

    #region Slider

    public static int DOKill(Slider target)
    {
        return target.DOKill();
    }
    public static Tweener   DOValue(Slider target, float endValue, float duration)
    {
        return target.DOValue(endValue, duration).SetEase(Ease.Linear);
    }

    #endregion

}