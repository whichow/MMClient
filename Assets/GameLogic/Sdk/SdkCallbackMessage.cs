/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/23 13:36:26
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using UnityEngine;

namespace Game
{
    public partial class SdkCallbackMessage : MonoBehaviour
    {
        public void LogDebug(string message)
        {
            Debuger.Log("[Android]" + message);
        }

        public void LogError(string message)
        {
            Debuger.LogError("[Android]" + message);
        }

    }
}
