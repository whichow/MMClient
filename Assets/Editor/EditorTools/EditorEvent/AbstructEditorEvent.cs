using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEditorEvent:IEditorEvent  {

    //public void UpDate()
    //{

    //}
    //public void ProjectWindowChanged()
    //{

    //}
    public abstract void UpDate();

    public  abstract void ProjectWindowChanged();

    public void UpdateSet()
    {
        EditorEventDate.Instantiate.UpDateSet(UpDate);
    }
    public void ProjectWindowChangedSet()
    {
        EditorEventDate.Instantiate.ProjectWindowChangedSet(ProjectWindowChanged);
    }
}
