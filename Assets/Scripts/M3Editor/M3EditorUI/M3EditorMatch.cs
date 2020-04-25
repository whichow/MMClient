#if UNITY_EDITOR

using System;
/** 
*FileName:     M3EditorMatch.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-27 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Game.Match3;

public class M3EditorMatch : MonoBehaviour
{

    Button back;

    void Awake()
    {
    }
    private void Start()
    {
        back = transform.GetComponent<Button>();
        back.onClick.AddListener(OnBack);
    }

    private void OnBack()
    {
        if (M3Config.isEditor)
        {
            M3Config.isEditor = false;
            SceneManager.LoadScene("M3EditorScene");
        }
        else
        {
            M3Config.isEditor = false;
            //Game.KLoading.UnloadAssets("buildingScene");
            Game.KLaunch.LoadLevel("buildingScene");
            //SceneManager.LoadScene("buildingScene");
        }
    }
}
#endif