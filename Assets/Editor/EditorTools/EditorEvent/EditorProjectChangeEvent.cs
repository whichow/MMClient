using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorProjectChangeEvent : AbstractEditorEvent
{
    public override void ProjectWindowChanged()
    {
        //EditorApplication.isPlaying = false;
    }
    public override void UpDate()
    {
        //Debug.Log("ÏîÄ¿Ë¢ÐÂ");
    }

}
