#if UNITY_EDITOR

using Game.Match3;
using M3Editor;
using System;
/** 
*FileName:     M3EditorFish.cs 
*Author:       HASEE 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-10-24 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace M3Editor
{
    public class M3EditorFish : EditorModuleBase
    {
        EditorMain editorMain;

        List<Int2> portList = new List<Int2>();

        List<Int2> exitList = new List<Int2>();


        List<M3FishModelItem> fishItemList = new List<M3FishModelItem>();

        M3EditorFishCtrl fishCtrl;

        public M3EditorFish(EditorMain main) : base(main)
        {
            editorMain = main;
            fishCtrl = M3EditorController.instance.fishCtrl;
        }

        public override void RefreshFromJson()
        {
            base.RefreshFromJson();

            fishItemList = new List<M3FishModelItem>();
            fishCtrl.Clear();
            exitList.Clear();
            portList.Clear();
            var level = M3Editor.M3EditorController.instance.gridCtrl.GetCurrentLevelData();
            var list = level.GetFishExit();
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    M3Editor.M3EditorController.instance.gridCtrl.GetCell(list[i].x, list[i].y).IsFishExit = true;
                    exitList.Add(new Int2(list[i].x, list[i].y));
                }
            }
            var port = level.GetFishPort();
            if (port != null)
            {
                for (int i = 0; i < port.Count; i++)
                {
                    M3Editor.M3EditorController.instance.gridCtrl.GetCell(port[i].x, port[i].y).IsFishPort = true;
                    portList.Add(new Int2(port[i].x, port[i].y));
                }
            }

            fishItemList = level.GetFishModelList();
            if (fishItemList != null)
                foreach (var item in fishItemList)
                {
                    fishCtrl.AddRule(item);
                }

        }

        internal void SetCollectPort(M3EditorCell cell)
        {
            Int2 tmp = new Int2(-1, -1);
            bool flag = false;
            foreach (var item in portList)
            {
                if (item.x == cell.gridX && item.y == cell.gridY)
                {
                    flag = true;
                    tmp = item;
                }
            }
            if (flag)
                portList.Remove(tmp);
            else
            {
                portList.Add(new Int2(cell.gridX, cell.gridY));
            }
            cell.IsFishPort = !cell.IsFishPort;
        }

        public override void UpdateToJson()
        {
            base.UpdateToJson();
            var fishList = GetFishExitList();
            var tmpfish = new ArrayList();
            if (fishList != null && fishList.Count > 0)
            {
                foreach (var v in fishList)
                {
                    var tmpHash = new Hashtable {
                            { "x",v.x},
                            { "y",v.y},
                };
                    tmpfish.Add(tmpHash);
                }
                M3EditorGridCtrl.table.Add("FishExit", tmpfish);
            }

            var tmpfishPort = new ArrayList();
            if (portList != null && portList.Count > 0)
            {
                foreach (var v in portList)
                {
                    var tmpHash = new Hashtable {
                            { "x",v.x},
                            { "y",v.y},
                };
                    tmpfishPort.Add(tmpHash);
                }
                M3EditorGridCtrl.table.Add("FishPort", tmpfishPort);
            }


            M3EditorGridCtrl.table.Add("Zombie", M3EditorController.instance.gridCtrl.GetCurrentLevelData().hasZombie ? 1 : 0);


            ArrayList fishArr = new ArrayList();
            if (fishItemList != null)
            {
                for (int i = 0; i < fishItemList.Count; i++)
                {
                    ArrayList arr = new ArrayList();

                    for (int j   = 0; j < fishItemList[i].spawnList.Count; j++)
                    {
                        arr.Add(new Hashtable() { { "x", fishItemList[i].spawnList[j].x }, { "y", fishItemList[i].spawnList[j].y } });
                    }

                    Hashtable fishTable = new Hashtable(){
                { "NeedPrevious" ,fishItemList[i].needPrevious},
                { "NeedStep" ,fishItemList[i].needStep},
                { "NeedTime" ,fishItemList[i].needTime},
                { "NeedScore" ,fishItemList[i].needScore},
               { "IsRandom" ,fishItemList[i].isRandom},
                { "CombineMode" ,fishItemList[i].combineMode},
                { "SpawnPos",arr}
            };
                    fishArr.Add(fishTable);
                }

                M3EditorGridCtrl.table.Add("FishRule", fishArr);
            }
        }



        public void SetCollectExit(M3EditorCell cell)
        {
            Int2 tmp = new Int2(-1, -1);
            bool flag = false;
            foreach (var item in exitList)
            {
                if (item.x == cell.gridX && item.y == cell.gridY)
                {
                    flag = true;
                    tmp = item;
                }
            }
            if (flag)
                exitList.Remove(tmp);
            else
            {
                exitList.Add(new Int2(cell.gridX, cell.gridY));
            }
            cell.IsFishExit = !cell.IsFishExit;
        }


        public void OnShow()
        {
            fishCtrl.gameObject.SetActive(true);


        }

        public void SetList(List<M3FishModelItem> l)
        {
            fishItemList = l;
            Debug.Log(fishItemList.Count);
        }



        public List<Int2> GetFishExitList()
        {
            return exitList;
        }
    }
}
#endif