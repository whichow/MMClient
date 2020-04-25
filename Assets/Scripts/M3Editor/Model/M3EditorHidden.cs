/** 
*FileName:     M3EditorHidden.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2018-01-02 
*Description:    
*History: 
*/
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using M3Editor;
using Game.Match3;

public class M3EditorHidden : EditorModuleBase
{
    EditorMain editorMain;
    private List<M3HiddenModel> hiddens = new List<M3HiddenModel>();

    public Color color;

    private List<M3HiddenItem> list = new List<M3HiddenItem>();

    public int count;

    public int type;

    public int countmax;

    public int countrandom;

    private M3HiddenPoint first;

    private M3HiddenPoint second;

    public M3EditorHidden(EditorMain main) : base(main)
    {
        editorMain = main;
    }

    public void SetHidden(M3EditorCell grid)
    {
        if (grid.IsHidden)
        {
            return;
        }
        if (this.first == null)
        {
            this.first = new M3HiddenPoint(grid.gridX, grid.gridY);
            if (this.countrandom == 0)
            {
                this.color = new Color((float)Random.Range(1, 255) / 255f, (float)Random.Range(1, 128) / 255f, (float)Random.Range(1, 255) / 255f);
                this.SetGridHidden(grid, "hidden", this.color);
            }
        }
        else
        {
            this.second = new M3HiddenPoint(grid.gridX, grid.gridY);
            this.SetSquareGridHidden(false);
            this.AddHiddenToList();
        }
    }
    private void SetGridHidden(M3EditorCell grid, string piece, Color color)
    {
        grid.IsHidden = (piece == "hidden");
        grid.SetHiddenPiece(color);
    }
    private void SetSquareGridHidden(bool isRemove)
    {
        var gridCtrl = M3Editor.M3EditorController.instance.gridCtrl;
        for (int i = (this.first.x >= this.second.x) ? this.second.x : this.first.x; i <= ((this.first.x >= this.second.x) ? this.first.x : this.second.x); i++)
        {
            for (int j = (this.first.y >= this.second.y) ? this.second.y : this.first.y; j <= ((this.first.y >= this.second.y) ? this.first.y : this.second.y); j++)
            {
                this.SetGridHidden(gridCtrl.GetCell(i, j), (!isRemove) ? "hidden" : string.Empty, (!isRemove) ? this.color : new Color(1f, 1f, 1f));
            }
        }
        if (isRemove)
        {
            this.first = null;
            this.second = null;
        }
    }
    private void AddHiddenToList()
    {
        M3HiddenItem editorModelHiddenItem = new M3HiddenItem();
        editorModelHiddenItem.from = this.first;
        editorModelHiddenItem.to = this.second;
        this.list.Add(editorModelHiddenItem);
        this.count++;
        if (this.list.Count == this.countmax)
        {
            this.AddHidden();
            this.countmax = 0;
            this.count = 0;

            M3EditorController.instance.EditorMode = mEditorMode.mEditor;

        }
        this.first = null;
        this.second = null;
    }
    private void AddHidden()
    {
        M3HiddenModel editorModelHidden = new M3HiddenModel();
        editorModelHidden.type = this.type;
        editorModelHidden.count = this.count;
        editorModelHidden.list = new M3HiddenItem[this.list.Count];
        for (int i = 0; i < this.list.Count; i++)
        {
            editorModelHidden.list[i] = this.list[i];
        }
        this.hiddens.Add(editorModelHidden);
        this.list.Clear();
        M3EditorController.instance.EditorMode = mEditorMode.mEditor;
    }

    public void RemoveHidden(M3EditorCell grid)
    {
        int num = -1;
        if (grid.IsHidden)
        {
            for (int i = 0; i < this.hiddens.Count; i++)
            {
                for (int j = 0; j < this.hiddens[i].list.Length; j++)
                {
                    M3HiddenItem editorModelHiddenItem = this.hiddens[i].list[j];
                    if ((editorModelHiddenItem.from.x == grid.gridX && editorModelHiddenItem.from.y == grid.gridY) || (editorModelHiddenItem.to.x == grid.gridX && editorModelHiddenItem.to.y == grid.gridY))
                    {
                        this.first = editorModelHiddenItem.from;
                        this.second = editorModelHiddenItem.to;
                        this.SetSquareGridHidden(true);
                        num = i;
                        break;
                    }
                }
            }
        }
        if (num != -1)
        {
            this.hiddens.RemoveAt(num);
        }
            M3EditorController.instance.EditorMode = mEditorMode.mEditor;
    }

    public void RemoveRandom(M3EditorCell grid)
    {
        int num = -1;
        if (grid.IsHidden)
        {
            for (int i = 0; i < this.hiddens.Count; i++)
            {
                for (int j = 0; j < this.hiddens[i].list.Length; j++)
                {
                    M3HiddenItem editorModelHiddenItem = this.hiddens[i].list[j];
                    if ((editorModelHiddenItem.from.x == grid.gridX && editorModelHiddenItem.from.y == grid.gridY) || (editorModelHiddenItem.to.x == grid.gridX && editorModelHiddenItem.to.y == grid.gridY))
                    {
                        num = i;
                        break;
                    }
                }
            }
        }
        if (num != -1)
        {
            for (int k = 0; k < this.hiddens[num].list.Length; k++)
            {
                M3HiddenItem editorModelHiddenItem2 = this.hiddens[num].list[k];
                this.first = editorModelHiddenItem2.from;
                this.second = editorModelHiddenItem2.to;
                this.SetSquareGridHidden(true);
            }
            this.hiddens.RemoveAt(num);
        }
        M3EditorController.instance.EditorMode = mEditorMode.mEditor;
    }
    public void StartRandom(M3EditorCell grid)
    {
        if (this.countrandom == 0)
        {
            M3EditorController.instance.ShowRandomPopup(new string[] { "数量" });
        }
        else
        {
            this.SetHidden(grid);
        }
    }
    public void EndRandom()
    {
        this.count = this.countrandom;
        this.AddHidden();
        this.countmax = 0;
        this.count = 0;
        this.countrandom = 0;
    }
    public override void UpdateToJson()
    {
        base.UpdateToJson();
        var gridCtrl = M3Editor.M3EditorController.instance.gridCtrl; ;
        gridCtrl.GetCurrentLevelData().HiddenList = new List<M3HiddenModel>();
        for (int i = 0; i < hiddens.Count; i++)
        {
            gridCtrl.GetCurrentLevelData().HiddenList.Add(hiddens[i]);
        }
        ArrayList list = new ArrayList();
        for (int i = 0; i < hiddens.Count; i++)
        {
            ArrayList tmpList = new ArrayList();

            for (int j = 0; j < hiddens[i].list.Length; j++)
            {
                Hashtable hTmp = new Hashtable()
                {
                    { "fromX",hiddens[i].list[j].from.x},
                    { "fromY",hiddens[i].list[j].from.y},
                    { "toX",hiddens[i].list[j].to.x},
                    { "toY",hiddens[i].list[j].to.y},
                };
                tmpList.Add(hTmp);
            }

            Hashtable tmp = new Hashtable() {
                { "count",hiddens[i].count},
                { "type",hiddens[i].type},
                { "list",tmpList},

            };
            list.Add(tmp);
        }
        M3EditorGridCtrl.table.Add("Hidden", list);
    }
    public override void RefreshFromJson()
    {
        base.RefreshFromJson();
        var gridCtrl = M3Editor.M3EditorController.instance.gridCtrl; ;
        if (gridCtrl.GetCurrentLevelData().HiddenList != null)
        {
            for (int i = 0; i < gridCtrl.GetCurrentLevelData().HiddenList.Count; i++)
            {
                M3HiddenModel editorModelHidden = gridCtrl.GetCurrentLevelData().HiddenList[i];
                this.type = editorModelHidden.type;
                this.countmax = editorModelHidden.count;
                this.count = 0;
                if (editorModelHidden.list.Length > editorModelHidden.count)
                {
                    this.countrandom = editorModelHidden.count;
                    this.color = new Color((float)Random.Range(1, 255) / 255f, (float)Random.Range(1, 128) / 255f, (float)Random.Range(1, 255) / 255f);
                    this.countmax = 999;
                }
                for (int j = 0; j < editorModelHidden.list.Length; j++)
                {
                    if (this.countrandom == 0)
                    {
                        this.color = new Color((float)Random.Range(1, 255) / 255f, (float)Random.Range(1, 128) / 255f, (float)Random.Range(1, 255) / 255f);
                    }
                    this.first = editorModelHidden.list[j].from;
                    this.second = editorModelHidden.list[j].to;
                    this.SetSquareGridHidden(false);
                    this.AddHiddenToList();
                    if (this.countrandom > 0 && this.count >= editorModelHidden.list.Length)
                    {
                        this.EndRandom();
                    }
                }
            }
        }
    }
    public void RemoveAllHiddens()
    {
        this.hiddens.Clear();
        this.list.Clear();
        var gridCtrl = M3Editor.M3EditorController.instance.gridCtrl; ;
        for (int i = 0; i < gridCtrl.GetCurrentLevelData().lvMapHeight; i++)
        {
            for (int j = 0; j < gridCtrl.GetCurrentLevelData().lvMapWidth; j++)
            {
                if (gridCtrl.GetCell(i, j))
                {
                    this.SetGridHidden(gridCtrl.GetCell(i, j), string.Empty, new Color(1f, 1f, 1f));
                }
            }
        }
    }
    public override void Clear()
    {
        base.Clear();
        RemoveAllHiddens();
    }
}

#endif