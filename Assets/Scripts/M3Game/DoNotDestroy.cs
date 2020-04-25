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

namespace Game.Match3
{
    public class DoNotDestroy : MonoBehaviour
    {

        // Use this for initialization
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}