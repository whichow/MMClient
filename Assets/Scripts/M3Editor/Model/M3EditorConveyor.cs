#if UNITY_EDITOR

using System;
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
using UnityEngine;
public class EditorModelConveyorItem
{
    public Int2[] list;
}



public class M3EditorConveyor : EditorModuleBase
{
    private List<EditorModelConveyorItem> conveyors = new List<EditorModelConveyorItem>();

    private List<Int2> list = new List<Int2>();
    EditorMain editorMain;
    public M3EditorConveyor(EditorMain main) : base(main)
    {
        editorMain = main;

    }

    public List<EditorModelConveyorItem> GetConveyor()
    {
        return conveyors;
    }


    public override void UpdateToJson()
    {
        base.UpdateToJson();
    }
    public void SetConveyor(M3EditorCell cell)
    {
        if (!cell.IsConveyor)
        {
            Int2 point = new Int2(cell.gridX, cell.gridY);
            list.Add(point);
            this.SetConveyorForGrid(cell, false);
        }
    }
    public void EndConveyor()
    {
        EditorModelConveyorItem editorModelConveyorItem = new EditorModelConveyorItem();
        editorModelConveyorItem.list = new Int2[this.list.Count];
        for (int i = 0; i < this.list.Count; i++)
        {
            editorModelConveyorItem.list[i] = this.list[i];
        }
        if (editorModelConveyorItem.list.Length > 0)
        {
            this.conveyors.Add(editorModelConveyorItem);
        }
        this.list.Clear();
    }
    public void RemoveConveyor(M3EditorCell cell)
    {
        int num = -1;
        if (cell.IsConveyor)
        {
            for (int i = 0; i < this.conveyors.Count; i++)
            {
                for (int j = 0; j < this.conveyors[i].list.Length; j++)
                {
                    Int2 editorModelPoint = this.conveyors[i].list[j];
                    if (editorModelPoint.x == cell.gridX && editorModelPoint.y == cell.gridY)
                    {
                        num = i;
                    }
                }
            }
        }
        if (num != -1)
        {
            //EditorGridManager module = this.main.moduleManager.GetModule<EditorGridManager>();
            var ctrl = M3Editor.M3EditorController.instance.gridCtrl;
            for (int k = 0; k < this.conveyors[num].list.Length; k++)
            {
                Int2 editorModelPoint2 = this.conveyors[num].list[k];
                this.SetConveyorForGrid(M3Editor.M3EditorController.instance.gridCtrl.GetCell(editorModelPoint2.x, editorModelPoint2.y), true);
            }
            Debug.Log(num);
            Debug.Log(conveyors.Count);
            this.conveyors.RemoveAt(num);
            Debug.Log(conveyors.Count);
        }
    }
    private void SetConveyorForGrid(M3EditorCell cell, bool v)
    {
        cell.IsConveyor = !v;
    }
    public void RemoveAllConveyor()
    {
        this.conveyors.Clear();
        this.list.Clear();
        //var list = M3Editor.M3EditorController.instance.gridCtrl.cellList;
        //for (int i = 0; i < length; i++)
        //{

        //}

    }
    public void LoadConveyor(List<List<Int2>> clist)
    {

    }
    public override void RefreshFromJson()
    {
        base.RefreshFromJson();
         var ctrl = M3Editor.M3EditorController.instance.gridCtrl;
        var level = ctrl.GetCurrentLevelData().GetConveyorList();
        if (level == null || level.Count == 0)
            return;
        List<EditorModelConveyorItem> conlist = new List<EditorModelConveyorItem>();
        for (int i = 0; i < level.Count; i++)
        {
            conlist.Add(new EditorModelConveyorItem() { list = level[i].ToArray() });
        }
        for (int i = 0; i < conlist.Count; i++)
        {
            for (int j = 0; j < conlist[i].list.Length; j++)
            {
                Int2 editorModelPoint = conlist[i].list[j];
                SetConveyor(ctrl.GetCell(editorModelPoint.x, editorModelPoint.y));

            }
            this.EndConveyor();
        }
    }
    public override void Clear()
    {
        base.Clear();
        RemoveAllConveyor();
    }

}
#endif