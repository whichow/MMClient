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
using System.Collections.Generic;

public class EditorModuleManager
{
    public List<EditorModuleBase> modules = new List<EditorModuleBase>();

    public void AddModule<T>(T t) where T : EditorModuleBase
    {
        this.modules.Add(t);
    }

    public T GetModule<T>() where T : EditorModuleBase
    {
        return (T)((object)this.modules.Find((EditorModuleBase module) => module is T));
    }

    public void RemoveModule<T>() where T : EditorModuleBase
    {
        T t = (T)((object)this.modules.Find((EditorModuleBase module) => module is T));
        if (t != null)
        {
            this.modules.Remove(t);
        }
    }
}
#endif
