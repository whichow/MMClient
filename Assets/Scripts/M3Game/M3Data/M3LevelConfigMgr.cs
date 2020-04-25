/** 
 *FileName:     M3LevelConfigMgr.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-07-10 
 *Description:    
 *History: 
*/
using Game.DataModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 三消关卡配置管理器
    /// </summary>
    public class M3LevelConfigMgr : KGameModule
    {
        public Dictionary<string, M3LevelData> lvConfigDataDic;
        public int maxLv;
        public string mapPath;
        public static M3LevelConfigMgr Instance;
        public bool isUseBundle = false;

        private void Awake()
        {
            Instance = this;
        }

        public override void Load()
        {
            base.Load();
            Init();
        }

        public override void Init()
        {
            mapPath = "Map";
            lvConfigDataDic = new Dictionary<string, M3LevelData>();

            //TextAsset[] maps = null;
            //if (isUseBundle)
            //{
            //    // 所有关卡配置json都被挂在prfab上了，此处需要修改 从关卡表中读取棋盘ID，加载json文件
            //    GameObject go;
            //    if (KAssetManager.Instance.TryGetGlobalPrefab(mapPath, out go))
            //    {
            //        maps = go.GetComponent<M3Map>().GetAllMaps();
            //    }
            //}
            //else
            //{
            //    maps = Resources.LoadAll<TextAsset>("Map");
            //}
            //for (int i = 0; i < maps.Length; i++)
            //{
            //    StartLoadLevelCfg(maps[i], OnLevelCfgLoaded);
            //}


            //后面要改成用时才读取，不要一次全读取，
            List<string> files = new List<string>();
            var list = XTable.LevelXTable.GetAllList();
            foreach (var item in list)
            {
                files.Add(item.ChessboardID);
            }

#if UNITY_EDITOR
            if (KLaunch.MainScene == "M3EditorScene")
            {
                files.Clear();
                string[] fileEntries = Directory.GetFiles("Assets/Res/Chessboard", "*.json", SearchOption.AllDirectories);
                if (fileEntries.Length > 0)
                {
                    foreach (var item in fileEntries)
                    {
                        files.Add(Path.GetFileNameWithoutExtension(item));
                    }
                }
            }
#endif

            foreach (var item in files)
            {
                LoadAsset(item);
            }
        }

        private void LoadAsset(string name)
        {
            TextAsset asset;
            KAssetManager.Instance.TryGetChessboardAsset(name, out asset);
            StartLoadLevelCfg(asset, OnLevelCfgLoaded);
        }

        private void OnLevelCfgLoaded(M3LevelData levelData)
        {
            if (!lvConfigDataDic.ContainsKey(levelData.id))
                lvConfigDataDic.Add(levelData.id, levelData);
        }

        public M3LevelData MapReader(string id)
        {
            //int lv = -1;
            //try
            //{
            //    lv = int.Parse(mapName);
            //}
            //catch (System.Exception)
            //{
            //    Debug.LogError("地图名称错误");
            //    return null;
            //}
            return M3LevelConfigMgr.Instance.GetLevelConfigData(id);

        }
        public M3LevelData GetLevelConfigData(string id)
        {
            if (lvConfigDataDic.ContainsKey(id))
            {
                return lvConfigDataDic[id];
            }
            else
            {
                //Debug.LogError("没有 " + lvIndex + "关卡的配置");
                return null;
            }
        }
        public void AddNewLevel(string lv, Hashtable table)
        {

            M3LevelData data = new M3LevelData(lv);
            data.SetConfigData(table);
            if (lvConfigDataDic.ContainsKey(lv))
            {
                lvConfigDataDic[lv] = data;
            }
            else
            {
                lvConfigDataDic.Add(lv, data);
                M3GameEvent.DispatchEvent(M3EditorEnum.AddNewLevelBtn, data);
            }
        }

        public void DeleteLevel(string lv)
        {
            if (lvConfigDataDic.ContainsKey(lv))
            {
                lvConfigDataDic.Remove(lv);
                M3GameEvent.DispatchEvent(M3EditorEnum.RemoveLevel, lv);
            }
        }
        private void StartLoadLevelCfg(TextAsset ta, MEventDelegate<M3LevelData> onLevelCfgLoaded)
        {
            if (ta != null)
            {
                string content = ta.text;
                string mapId = ta.name;
                if (!string.IsNullOrEmpty(content))
                {
                    var table = content.ToJsonTable();
                    M3LevelData data = new M3LevelData();
                    data.SetConfigData(table);
                    data.id = mapId;
                    onLevelCfgLoaded(data);
                }
            }
        }

    }

    public class PortData
    {
        public Int2 pos;
        public string rule;
        public PortData(int x, int y, string r)
        {
            pos = new Int2(x, y);
            rule = r;
        }
    }
    public class SpawnData
    {
        public Int2 pos;
        public string rule;
        public SpawnData(Int2 p, string r)
        {
            pos = p;
            rule = r;
        }
    }
    public class RuleCode
    {
        public int elementID;
        public int weight;
        public RuleCode(int id, int w)
        {
            elementID = id;
            weight = w;
        }

        public RuleCode GetInfo()
        {
            return new RuleCode(this.elementID, this.weight);
        }
    }
    public class SpawnRuleDefinition
    {
        public string ruleName;
        public List<RuleCode> ruleList;
        public SpawnRuleDefinition(string n, List<RuleCode> list)
        {
            ruleName = n;
            ruleList = list;
        }
    }

}