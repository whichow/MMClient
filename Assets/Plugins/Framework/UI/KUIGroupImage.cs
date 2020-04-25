// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KUIGroupImage" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[System.Obsolete("KUIImage")]
public class KUIGroupImage : MonoBehaviour
{
    public Sprite[] imageGroup;

    #region Method

    public void ShowImage(int index)
    {
        if (imageGroup != null && imageGroup.Length > 0)
        {
            if (index >= 0 && index < imageGroup.Length)
            {
                GetComponent<Image>().overrideSprite = imageGroup[index];
            }
        }
    }

    #endregion 
}

