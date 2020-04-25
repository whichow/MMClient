using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorEvent : AbstractEditorEvent
{
    public override void ProjectWindowChanged()
    {
       // Debug.Log("项目改变");
    }
    public override void UpDate()
    {
        //Debug.Log("项目刷新");
    }
}
