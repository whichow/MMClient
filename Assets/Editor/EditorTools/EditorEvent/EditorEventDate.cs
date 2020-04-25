using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorEventDate {
    private event System.Action UpDate;
    private event System.Action ProjectWindowChanged;
    //public System.Action EditorEventBuildFun;
    private static EditorEventDate instantiate;
    public static EditorEventDate Instantiate
    {
        get
        {
            if (instantiate == null)
                instantiate = new EditorEventDate();
            return instantiate;
        }
    }

    public void UpDateSet(System.Action fun)
    {
         
            UpDate += fun;
    }

    public void ProjectWindowChangedSet(System.Action fun)
    {
            ProjectWindowChanged += fun;
    }

    public void LuanchUpDate()
    {
        if (UpDate != null)
            UpDate();
    }

    public void LuanchProjectWindowChanged()
    {
        if (ProjectWindowChanged != null)
            ProjectWindowChanged();
    }
     
}
