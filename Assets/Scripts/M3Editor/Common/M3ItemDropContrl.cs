#if UNITY_EDITOR

using Game.Match3;
using System;
/** 
*FileName:     M3ItemDropContrl.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-06 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace M3Editor
{
    public class M3ItemDropContrl : MonoBehaviour, IDropHandler
    {
        public MEventDelegate<int> dropHandler;

        #region unity

        public void OnDrop(PointerEventData eventData)
        {
            if (M3EditorController.instance.EditorMode != mEditorMode.mEditor && M3EditorController.instance.EditorMode != mEditorMode.mDelete&& M3EditorController.instance.EditorMode != mEditorMode.mBrush)
            {
                //Debug.Log("在非编辑状态下不能编辑");
                return;
            }

            var data = eventData.pointerDrag.GetComponent<M3ItemDragContrl>();
            if (data != null)
                dropHandler.Invoke(data._eleCofing.ID);

        }

        #endregion
    }
}
#endif
