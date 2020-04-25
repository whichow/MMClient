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
using System.Text;
using UnityEditor;
using UnityEngine;
[InitializeOnLoad]
public static class MapTools
{

    [MenuItem("GameObject/MapTools/GetMapNodePos", false, 2)]
    static void FindMapNodePosition()
    {
        List<Vector2> list = new List<Vector2>();
        var target = Selection.activeGameObject.transform;
        for (int i = 0; i < target.childCount; i++)
        {
            list.Add(target.GetChild(i).GetComponent<RectTransform>().anchoredPosition);
        }
        Debug.Log(ProcessNode(list));
    }

    static string ProcessNode(List<Vector2> list)
    {
        List<Vector2> list2 = new List<Vector2>();
        foreach (var item in list)
        {
            list2.Add(new Vector2((int)(item.x * 1000), (int)(item.y * 1000)));
        }

        StringBuilder sb = new StringBuilder();

        foreach (var item in list2)
        {
            sb.Append("[" + (int)item.x + "," + (int)item.y + "]" + "\r\n");
        }
        return sb.ToString();
    }




    [MenuItem("GameObject/MapTools/GetMask", false, 2)]
    static void FindMaskNodeAllPosition()
    {
        List<RectTransform> list = new List<RectTransform>();
        var target = Selection.activeGameObject.transform;
        for (int i = 0; i < target.childCount; i++)
        {
            list.Add(target.GetChild(i).GetComponent<RectTransform>());
        }
        Debug.Log(ProcessPositon(list));
    }
    static string ProcessPositon(List<RectTransform> list)
    {
        List<Vector2> list2 = new List<Vector2>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].name=="mask1"|| list[i].name == "mask2")
            {
                list2.Add(new Vector2(list[i].localPosition.x * 1000, list[i].localPosition.y * 1000));
                list2.Add(new Vector2(list[i].sizeDelta.x * 1000, list[i].sizeDelta.y * 1000));
            }
            else if(list[i].name=="arrow")
            {
                list2.Add(new Vector2(list[i].localPosition.x * 1000, list[i].localPosition.y * 1000));
            }
        
        }
   
     

        StringBuilder sb = new StringBuilder();

        for (int i = 1; i < list2.Count; i++)
        {
            if (i % 2 == 0)
            {
                sb.Append("长宽+[" + (int)list2[i - 1].x + "," + (int)list2[i - 1].y + "]" + "\r\n");
            }
            else
            {
                sb.Append("位置+[" + (int)list2[i - 1].x + "," + (int)list2[i - 1].y + "]" + "\r\n");
            }
        }
        sb.Append("手指位置+[" + (int)list2[list2.Count - 1].x + "," + (int)list2[list2.Count - 1].y + "]" + "\r\n");
        return sb.ToString();
    }

}
