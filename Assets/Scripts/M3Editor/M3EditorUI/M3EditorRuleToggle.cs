#if UNITY_EDITOR
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

public class M3EditorRuleToggle : MonoBehaviour {

    public string ruleName;
    public void UpdateData(string n)
    {
        ruleName = n;
    }
}
#endif