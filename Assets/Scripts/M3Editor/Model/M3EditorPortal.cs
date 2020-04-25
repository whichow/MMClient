/** 
*FileName:     M3EditorPortal.cs 
*Author:       Hejunjie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-11-13 
*Description:    
*History: 
*/
#if UNITY_EDITOR
using Game.Match3;
using M3Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3EditorPortal : EditorModuleBase
{
    EditorMain editorMain;
    private List<M3PortalModel> portals;
    public M3EditorCell first;

    public M3EditorCell second;

    public Int2 firstPos;
    public Int2 secondPos;

    public Color color;
    public M3EditorPortal(EditorMain main) : base(main)
    {
        portals = new List<M3PortalModel>();
        editorMain = main;
    }



    public void SetPortal(M3EditorCell grid, bool isHidden)
    {
        if (grid.GetPortal() != string.Empty)
        {
            return;
        }
        if (grid.validElementCount == -1)
            return;
        if (first == null)
        {
            first = grid;
            this.firstPos = new Int2(grid.gridX, grid.gridY);
            this.color = new Color((float)UnityEngine.Random.Range(1, 255) / 255f, (float)UnityEngine.Random.Range(1, 128) / 255f, (float)UnityEngine.Random.Range(1, 255) / 255f);
            grid.SetPortal((!isHidden) ? "portal_in" : "portal_in_hide", this.color);
        }
        else
        {
            second = grid;
            this.secondPos = new Int2(grid.gridX, grid.gridY);
            grid.SetPortal((!isHidden) ? "portal_out" : "portal_out_hide", this.color);
            this.AddPortal(isHidden);
            M3EditorController.instance.EditorMode = mEditorMode.mEditor;
        }
    }

    private void AddPortal(bool isHidden)
    {
        M3PortalModel model = new M3PortalModel();
        model.in_x = firstPos.x;
        model.in_y = firstPos.y;
        model.out_x = secondPos.x;
        model.out_y = secondPos.y;
        model.isHidden = isHidden;
        portals.Add(model);
        first = null;
        second = null;

    }
    public void removePortal(M3EditorCell grid)
    {

       var gridCtrl= M3Editor.M3EditorController.instance.gridCtrl;
        for (int i = 0; i < this.portals.Count; i++)
        {
            M3PortalModel editorModelPortal = this.portals[i];
            if (this.IsGridAPortal(grid, editorModelPortal))
            {
                gridCtrl.GetCell(editorModelPortal.in_x, editorModelPortal.in_y).RemovePortal();
                gridCtrl.GetCell(editorModelPortal.out_x, editorModelPortal.out_y).RemovePortal();
                this.portals.RemoveAt(i);
                break;
            }
        }
    }
    private bool IsGridAPortal(M3EditorCell grid, M3PortalModel portal)
    {
        return (portal.in_x == grid.gridX && portal.in_y == grid.gridY) || (portal.out_x == grid.gridX && portal.out_y == grid.gridY);
    }
    public void RemoveAllPortal()
    {
        //var gridCtrl = M3Editor.M3EditorController.instance.gridCtrl;
        //Debug.Log(portals.Count);
        //for (int i = 0; i < this.portals.Count; i++)
        //{
        //    var editorModelPortal = this.portals[i];

        //    gridCtrl.GetCell(editorModelPortal.in_x, editorModelPortal.in_y).RemovePortal();
        //    gridCtrl.GetCell(editorModelPortal.out_x, editorModelPortal.out_y).RemovePortal();
        //}
        this.portals.Clear();
    }

    public override void RefreshFromJson()
    {
        base.RefreshFromJson();
        var gridCtrl = M3Editor.M3EditorController.instance.gridCtrl;
        if (gridCtrl.GetCurrentLevelData().FlickerPortalList == null)
            return;
        for (int i = 0; i < gridCtrl.GetCurrentLevelData().FlickerPortalList.Count; i++)
        {
            M3PortalModel model = gridCtrl.GetCurrentLevelData().FlickerPortalList[i];
            this.SetPortal(gridCtrl.GetCell(model.in_x, model.in_y), model.isHidden);
            this.SetPortal(gridCtrl.GetCell(model.out_x, model.out_y), model.isHidden);
        }
    }
    public override void UpdateToJson()
    {
        base.UpdateToJson();
        var gridCtrl = M3Editor.M3EditorController.instance.gridCtrl; ;
        gridCtrl.GetCurrentLevelData().FlickerPortalList = new List<M3PortalModel>();
        for (int i = 0; i < this.portals.Count; i++)
        {
            gridCtrl.GetCurrentLevelData().FlickerPortalList.Add(this.portals[i]);
        }

        ArrayList list = new ArrayList();
        for (int i = 0; i < portals.Count; i++)
        {
            Hashtable tmp = new Hashtable() {
                { "in_x",portals[i].in_x },
                   { "in_y",portals[i].in_y },
                      { "out_x",portals[i].out_x },
                         { "out_y",portals[i].out_y },
                          { "isHidden",portals[i].isHidden?1:0 },
            };
            list.Add(tmp);
        }
        M3EditorGridCtrl.table.Add("Portal", list);
    }
    public override void Clear()
    {
        base.Clear();
        RemoveAllPortal();
    }
}

#endif