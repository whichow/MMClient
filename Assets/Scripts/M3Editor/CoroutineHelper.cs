/** 
 *FileName:     CoroutineHelper.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-07-10 
 *Description:    
 *History: 
*/
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    public static CoroutineHelper _instance;
    public static CoroutineHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<CoroutineHelper>();
            }
            return _instance;
        }
    }
}
#endif
