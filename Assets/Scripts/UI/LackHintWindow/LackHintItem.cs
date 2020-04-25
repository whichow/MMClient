
using System;
/** 
*FileName:     LackHintItem.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2018-01-22 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using Game.UI;

public class LackHintItem : MonoBehaviour
{

    private KUIImage iconImg;
    private Text countText;
    private Button btn;
    private GameObject numObj;

    private KItem item;
    public void Init()
    {
        iconImg = transform.Find("Icon").GetComponent<KUIImage>();
        countText = transform.Find("CorBack/LeftText").GetComponent<Text>();
        btn = transform.GetComponent<Button>();
        numObj = transform.Find("CorBack").gameObject;

        btn.onClick.AddListener(OnBtnClick);
    }

    private void OnBtnClick()
    {
        if (item.curCount <= 0)
            return;
        KUIWindow.OpenWindow<BagPowerBoxWindow>(new BagPowerBoxWindow.Data() { itemdata = item, onConfirm=OnConfirm, onCancel=OnCancel });
    }

    private void OnCancel()
    {
    }

    private void OnConfirm()
    {
        
    }

    public void Refresh(int itemID)
    {
        item = KItemManager.Instance.GetItem(itemID);
        iconImg.overrideSprite = KIconManager.Instance.GetItemIcon(item.iconName);
        if (item.curCount <= 0)
        {
            iconImg.ShowGray(true);
            numObj.SetActive(false);
        }
        else
        {
            numObj.SetActive(true);
            countText.text = item.curCount.ToString();
        }
    }
}
