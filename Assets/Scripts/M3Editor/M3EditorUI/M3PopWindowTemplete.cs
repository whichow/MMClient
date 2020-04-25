/** 
 *FileName:     M3PopWindowTemplete.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2018-01-02 
 *Description:    
 *History: 
*/
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M3PopWindowTemplete : MonoBehaviour {
    public Text title;
    public InputField input;

    private void Awake()
    {
        title = transform.Find("title").GetComponent<Text>();
        input= transform.Find("InputField").GetComponent<InputField>();
    }
    public void Show(string t)
    {
        title.text = t;
        input.text = "0";
    }
}
#endif
