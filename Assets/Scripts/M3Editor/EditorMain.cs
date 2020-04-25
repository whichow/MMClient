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
using M3Editor;

public class EditorMain  {

    public string levelname;

    public EditorModuleManager moduleManager;

    public EditorMain()
    {
        this.moduleManager = new EditorModuleManager();
        this.initModules();
        this.init();
    }

    private void initModules()
    {
        moduleManager.AddModule<M3EditorConveyor>(new M3EditorConveyor(this));
        moduleManager.AddModule<M3EditorFish>(new M3EditorFish(this));
        moduleManager.AddModule<M3EditorInitSpawn>(new M3EditorInitSpawn(this));
        moduleManager.AddModule<M3EditorPortal>(new M3EditorPortal(this));
        moduleManager.AddModule<M3EditorHidden>(new M3EditorHidden(this));
        this.loadLastLevel();
    }

    private void init()
    {

    }

    private void loadLastLevel()
    {

    }
    public void ClearAll()
    {
        foreach (var item in moduleManager.modules)
        {

        }
    }
}
#endif