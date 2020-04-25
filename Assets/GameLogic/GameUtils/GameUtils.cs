/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/4 18:56:56
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
using System.Collections;

namespace Game
{
    public class GameUtils
    {
        #region SetTimeout
        public static void SetTimeout(Action callback, float s)
        {
            GameApp.Instance.StartCoroutine(IESetTimeout(callback, s));
        }
        private static IEnumerator IESetTimeout(Action callback, float s)
        {
            yield return new WaitForSeconds(s);
            callback?.Invoke();
        }
        #endregion

        #region WaitNextFrame
        public static void WaitNextFrame(Action callback)
        {
            GameApp.Instance.StartCoroutine(IEWaitNextFrame(callback));
        }
        private static IEnumerator IEWaitNextFrame(Action callback)
        {
            yield return null;
            callback?.Invoke();
        }
        #endregion

    }
}
