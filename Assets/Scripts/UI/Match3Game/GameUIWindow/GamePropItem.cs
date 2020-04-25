
using Game;
using Game.Match3;
using Game.UI;
using System;
/** 
*FileName:     #SCRIPTFULLNAME# 
*Author:       #AUTHOR# 
*Version:      #VERSION# 
*UnityVersion：#UNITYVERSION#
*Date:         #DATE# 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class GamePropItem : KUIItem
    {
        private Text count;
        private Image icon;
        private KItemProp prop;
        private int propID;
        private int propShopID;
        private GameObject frame;
        //public Canvas canvas;
        private Button btn;
        private int propCount;
        private int propUserCount;

        private void Awake()
        {
            count = Find<Text>("propItem/Num/Text");
            icon = Find<Image>("propItem/icon");
            frame = transform.Find("propItem/frame").gameObject;
            btn = transform.Find("propItem/icon").GetComponent<Button>();
            //canvas = transform.GetComponent<Canvas>();
            //canvas.sortingOrder = 101;
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (M3GameManager.Instance.gameFsm.GetFSM().GetCurrentStateEnum() != Game.Match3.StateEnum.Idle || M3GameManager.Instance.modeManager.IsStepModeLevelEnd())
                return;
            if (propCount > 0)
            {
                M3GameManager.Instance.propManager.UseDrop(prop.itemID, this);
            }
            else
            {
                M3GameManager.Instance.propItemLock = true;
                KUIWindow.OpenWindow<BuyPopNumWindow>(new BuyPopNumWindow.Data()
                {
                    itemId = propShopID,
                    onCancel = delegate () { OnBuyCancel(); },
                    onConfirm = delegate () { OnBuyProp(); },
                });
            }

        }
        public void OnBuyProp()
        {
            M3GameManager.Instance.propItemLock = false;
            KUIWindow.CloseWindow<BuyPopNumWindow>();
            Debug.Log(KItemManager.Instance.GetProp(propID).curCount);
            propCount = KItemManager.Instance.GetProp(propID).curCount - propUserCount;
            Refresh();
        }
        public void OnBuyCancel()
        {
            M3GameManager.Instance.propItemLock = false;
        }
        public void OnUsed()
        {
            propCount--;
            propUserCount++;
            M3GameManager.Instance.propManager.SaveUsedPropID(propID);
            Refresh();
        }
        public void ShowProp(int pID, int sID)
        {
            propShopID = sID;
            propID = pID;
            prop = KItemManager.Instance.GetProp(propID);
            propCount = prop.curCount;
            if (count != null)
            {
                count.text = propCount.ToString();
            }
            if (icon != null)
            {
                icon.overrideSprite = KIconManager.Instance.GetItemIcon(prop.iconName);
            }
        }

        protected override void Refresh()
        {
            if (count != null)
                count.text = propCount.ToString();
        }


        public void SetFrame(bool v)
        {
            frame.SetActive(v);
        }


    }
}