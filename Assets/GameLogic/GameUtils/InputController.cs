/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/30 18:22:40
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using Game.UI;
using Framework.Core;

namespace Game
{
    public class InputController : Singleton<InputController>
    {

        #region Static

        public const string InputDown = "InputDown";
        public const string InputUp = "InputUp";
        public const string InputLateDown = "InputLateDown";
        public const string InputLateUp = "InputLateUp";

        #endregion

        #region Member

        #endregion

        #region Public
        public void Init()
        {
            
        }

        public void Update()
        {
            if (EventManager.Instance.InputDispatcher != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    EventManager.Instance.InputDispatcher.DispatchEvent(InputController.InputDown);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    EventManager.Instance.InputDispatcher.DispatchEvent(InputController.InputUp);
                }
            }
        }

        public void LateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                EventManager.Instance.InputDispatcher.DispatchEvent(InputController.InputLateDown);
            }
            if (Input.GetMouseButtonUp(0))
            {
                EventManager.Instance.InputDispatcher.DispatchEvent(InputController.InputLateUp);
            }
        }

        #endregion

        #region Private

        #endregion

    }
}
