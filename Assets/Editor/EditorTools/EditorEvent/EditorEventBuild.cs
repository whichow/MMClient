using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EditorEventBuild
{
     static EditorEventBuild()
    {
        EditorApplication.update = EditorEventDate.Instantiate.LuanchUpDate;
        EditorApplication.projectWindowChanged= EditorEventDate.Instantiate.LuanchProjectWindowChanged;
        CallIEditorEventAll();
       //EditorApplication.projectWindowChanged = ()=>EditorApplication.isPlaying = false;
        //EditorApplication.projectWindowItemOnGUI;
        //EditorApplication.hierarchyWindowItemOnGUI;
        //EditorApplication.update;
        //EditorApplication.delayCall;
        //EditorApplication.hierarchyWindowChanged;
        //EditorApplication.projectWindowChanged;
        //EditorApplication.searchChanged;
        //EditorApplication.modifierKeysChanged;
        //EditorApplication.playmodeStateChanged;
        //EditorApplication.contextualPropertyMenu;
    }
    static void CallIEditorEventAll()
    {
        Type[] IEditorEventLst = Assembly.GetExecutingAssembly().GetTypes();
        for (int i = 0; i < IEditorEventLst.Length; i++)
        {
            if (IEditorEventLst[i].GetInterface("IEditorEvent") != null)
            {
                if (IEditorEventLst[i] == typeof(AbstractEditorEvent))
                {
                    //Debug.Log("抽象继承接口");
                    continue;
                }

                AbstractEditorEvent ABEvent = Activator.CreateInstance(IEditorEventLst[i]) as AbstractEditorEvent;

                if (ABEvent != null)
                {
                    ABEvent.UpdateSet();
                    ABEvent.ProjectWindowChangedSet();
                }
                //Debug.Log("继承接口" + IEditorEventLst[i]);
            }
            else
            {
                //Debug.Log("no继承接口");
            }
        }
    }

}