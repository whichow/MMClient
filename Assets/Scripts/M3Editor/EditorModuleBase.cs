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


public class EditorModuleBase
{
    public EditorMain main;

    public EditorModuleBase(EditorMain main)
    {
        this.main = main;
    }

    public virtual void UpdateToJson()
    {
    }

    public virtual void RefreshFromJson()
    {
    }
    public virtual void Clear()
    {

    }
}
#endif