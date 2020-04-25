#if UNITY_EDITOR
/** 
 *FileName:     M3InitSpawn.cs 
 *Author:       HASEE 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-10-31 
 *Description:    
 *History: 
*/
using Game.Match3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace M3Editor
{
    public class M3EditorInitSpawn : EditorModuleBase
    {
        EditorMain editorMain;

        public List<RuleCode> spawnList = new List<RuleCode>();
        public M3EditorInitSpawn(EditorMain main) : base(main)
        {
            editorMain = main;
        }

        public override void RefreshFromJson()
        {
            base.RefreshFromJson();
            var level = M3Editor.M3EditorController.instance.gridCtrl.GetCurrentLevelData();
            if (level.GetSpawnList() == null|| level.GetSpawnList().Count==0)
            {
                for (int i = 0; i < 6; i++)
                {
                    spawnList.Add(new RuleCode(1001 + i,10));
                }
            }
            else
                spawnList = level.GetSpawnList();
        }


        public override void UpdateToJson()
        {
            base.UpdateToJson();
            ArrayList arr = new ArrayList();
            foreach (var v in spawnList)
            {
                var tmphash = new Hashtable {
                            { "id",v.elementID},
                            { "weight",v.weight},
                        };
                arr.Add(tmphash);
            }
            M3EditorGridCtrl.table.Add("InitSpawn", arr);
        }
        public override void Clear()
        {
            base.Clear();
            spawnList.Clear();
        }
    }
}
#endif