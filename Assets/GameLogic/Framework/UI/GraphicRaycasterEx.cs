// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "GraphicRaycasterEx" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphicRaycasterEx : GraphicRaycaster
{
    public static event Action<PointerEventData, List<RaycastResult>> OnRaycast;

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        base.Raycast(eventData, resultAppendList);

        bool ignore = false;
        if (resultAppendList.Count > 0)
        {
            if (OnRaycast != null)
            {
                OnRaycast(eventData, resultAppendList);
            }
            if (resultAppendList.Count == 0)
            {
                ignore = true;
            }
        }

        if (ignore)
        {
            resultAppendList.Clear();
            var result = new RaycastResult
            {
                gameObject = KUIRoot.Instance.gameObject,
                module = this
            };
            resultAppendList.Add(result);
        }
    }
}

