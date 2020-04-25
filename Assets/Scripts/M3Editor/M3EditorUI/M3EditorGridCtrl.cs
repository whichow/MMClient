#if UNITY_EDITOR
using System;
/** 
*FileName:     M3EditorGridCtrl.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-07 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Game;
using UnityEngine.UI;
using Game.Match3;

namespace M3Editor
{
    public enum GridShowType
    {
        Disk,
        Hidden,
    }
    public class M3EditorGridCtrl : MonoBehaviour
    {
        public bool isDisk = true;
        public bool isHidden = true;


        public Toggle conveyorStart;
        public Toggle conveyorEnd;
        public Toggle conveyorRemove;
        public Button portalStart;
        public Button portalRemove;

        public Button hiddenStart;
        public Button hiddenRemove;
        public Button randomHiddenStart;
        public Button randomHiddenEnd;
        public Button randomHiddenRemove;


        public GameObject cellPrefab;
        public GameObject grid;
        public List<M3EditorCell> cellList;
        private string lvID;
        public string oriLvID;

        string path;
        private M3LevelData data;
        public Button openSettingBtn;
        public Button fishSettingBtn;
        public static Hashtable table;

        public int currentBrush = -999;

        public string LvID
        {
            get
            {
                return M3EditorController.instance.stageSettingCtrl.lvText.text;
            }

            set
            {
                lvID = value;
                M3EditorController.instance.stageSettingCtrl.lvText.text = lvID.ToString();
            }
        }

        private void Awake()
        {
            path = Application.dataPath + "/Res/Chessboard/";
            openSettingBtn.onClick.AddListener(OnOpenSettingClick);
            fishSettingBtn.onClick.AddListener(OnFishSettingClick);

            conveyorStart.onValueChanged.AddListener(OnConveyorStart);
            conveyorEnd.onValueChanged.AddListener(OnConveyorEnd);
            conveyorRemove.onValueChanged.AddListener(OnConveyorRemove);
            portalStart.onClick.AddListener(OnPortalStart);
            portalRemove.onClick.AddListener(OnPortalRemove);

            hiddenStart.onClick.AddListener(OnHiddenStart);
            hiddenRemove.onClick.AddListener(OnHiddenRemove);
            randomHiddenStart.onClick.AddListener(OnRandomHiddenStart);
            randomHiddenEnd.onClick.AddListener(OnRandomHiddenEnd);
            randomHiddenRemove.onClick.AddListener(OnRandomHiddenRemove);



            M3GameEvent.AddEvent(M3EditorEnum.StartGenMap, GenMap);
            M3GameEvent.AddEvent(M3EditorEnum.OnSaveBtnClick, OnSaveMap);
            M3GameEvent.AddEvent(M3EditorEnum.DeleteMap, OnDeleteMap);
            M3GameEvent.AddEvent(M3EditorEnum.LoadScene, OnLoadScene);
            openSettingBtn.gameObject.SetActive(false);
            fishSettingBtn.gameObject.SetActive(false);
        }

        private void OnRandomHiddenEnd()
        {
            M3EditorHidden module = M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorHidden>();
            module.EndRandom();
        }

        private void OnHiddenStart()
        {
            M3EditorController.instance.EditorMode = mEditorMode.mHiddenStart;

        }

        private void OnRandomHiddenStart()
        {
            M3EditorController.instance.EditorMode = mEditorMode.mRandomHiddenStart;
            M3EditorController.instance.ShowRandomPopup(new string[] { "随机数量" });
        }

        private void OnHiddenRemove()
        {
            M3EditorController.instance.EditorMode = mEditorMode.mHiddenRemove;

        }

        private void OnRandomHiddenRemove()
        {
            M3EditorController.instance.EditorMode = mEditorMode.mRandomHiddenRemove;
        }

        public void SetBrush(int id)
        {
            currentBrush = id;
        }



        private void OnPortalStart()
        {

            M3EditorController.instance.EditorMode = mEditorMode.mPortal;
        }
        private void OnPortalRemove()
        {

            Debug.Log(1);
            M3EditorController.instance.EditorMode = mEditorMode.mPortalRemove;
        }

        private void OnFishSettingClick()
        {
            if (M3EditorController.instance.EditorMode == mEditorMode.ShowFish || M3EditorController.instance.EditorMode == mEditorMode.SelectFish)
            {
                for (int i = 0; i < cellList.Count; i++)
                {
                    cellList[i].OnShowFishFlag(false);
                    cellList[i].SetRoot(true);
                }
                M3EditorController.instance.EditorMode = mEditorMode.mEditor;
            }
            M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorFish>().OnShow();
        }

        private void OnConveyorStart(bool arg0)
        {
            if (arg0)
            {
                M3EditorController.instance.EditorMode = mEditorMode.mConveyor;
            }
        }
        private void OnConveyorEnd(bool arg0)
        {
            if (arg0)
            {
                M3EditorController.instance.EditorMode = mEditorMode.mEditor;
                M3Editor.M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorConveyor>().EndConveyor();
            }
        }
        private void OnConveyorRemove(bool arg0)
        {
            if (arg0)
            {
                M3EditorController.instance.EditorMode = mEditorMode.mConveyorRemove;
            }
        }

        private void OnOpenSettingClick()
        {
            M3EditorController.instance.OpenSettingWindow();
        }

        private void OnLoadScene(object[] args)
        {
            M3Config.levelId = LvID.ToString();
        }

        private void OnDeleteMap(object[] args)
        {
            //string id;
            //if (args != null && args.Length == 1)
            //{
            //    id = (string)args[0];
            //}
            //else
            //{
            //    id = LvID;
            //}
            string p = path + LvID + ".json";

            M3LevelConfigMgr.Instance.DeleteLevel(LvID);


            File.Delete(p);
            UnityEditor.AssetDatabase.Refresh();
        }
        public void ClearMap()
        {
            for (int i = 0; i < cellList.Count; i++)
            {
                Destroy(cellList[i].gameObject);
            }
            cellList.Clear();
        }


        public M3EditorCell GetCell(int x, int y)
        {
            if (cellList != null)
                return cellList[x * data.lvMapWidth + (y)];
            return null;
        }
        private void GenMap(object[] args)
        {

            openSettingBtn.gameObject.SetActive(true);
            fishSettingBtn.gameObject.SetActive(true);
            LvID = args[0].ToString();
            oriLvID = LvID;
            data = (M3LevelData)args[1];
            if (data == null)
                return;
            M3EditorConst.GridRealHeight = data.lvMapHeight;

            var port = data.portTilesList;
            var conveyor = data.GetConveyorList();
            ClearMap();
            cellList.Clear();
            int index = 0;
            for (int i = 0; i < data.lvMapHeight; i++)
            {
                for (int j = 0; j < data.lvMapWidth; j++)
                {
                    GameObject go = Instantiate(cellPrefab, grid.transform);
                    go.SetActive(true);
                    var cell = go.GetComponent<M3EditorCell>();
                    M3CellData cellData;
                    cellData = data.celldata[i * data.lvMapWidth + (j)];
                    cell.Init(i, j, index, cellData);
                    cellList.Add(cell);
                    cell.IsEmpty = cellData.isEmpty;
                    for (int k = 0; k < port.Count; k++)
                    {
                        if ((i == port[k].pos.x) && (j == port[k].pos.y))
                        {
                            cell.IsPort = true;
                            cell.portRule = port[k].rule;
                        }
                    }
                    index++;
                }
            }
            foreach (var item in M3EditorController.instance.editorMain.moduleManager.modules)
            {
                item.Clear();
            }
            foreach (var item in M3EditorController.instance.editorMain.moduleManager.modules)
            {
                item.RefreshFromJson();
            }
            //M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorConveyor>().RefreshFromJson();
            //M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorFish>().RefreshFromJson();
            cellPrefab.SetActive(false);
        }
        public void OnSaveMap(object[] args)
        {
            if (string.IsNullOrEmpty(LvID))
                return;
            //M3GameEvent.DispatchEvent(M3EditorEnum.DeleteMap, new object[] { oriLvID });
            bool needRename = true;
            if (args.Length == 1)
            {
                needRename = !((int)args[0] == 1);
            }
            string name;
            var table = GenerateMapData(out name, needRename);
            M3LevelConfigMgr.Instance.AddNewLevel(name, table);

        }

        public M3LevelData GetCurrentLevelData()
        {
            return data;
        }
        public void SaveRuleListIntoData(List<SpawnRuleDefinition> list)
        {
            data.ruleList = list;
        }
        public void SetStarConfig(int[] arr)
        {
            data.star = arr;
        }

        public void SetEnergy(int rate, int value1, int value2)
        {
            data.energyRate = rate;
            data.energyStepCount = value1;
            data.energyCreateCount = value2;
        }

        public void ShowFishPort(List<Int2> list)
        {
            if (list == null)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                cellList[list[i].x * data.lvMapWidth + list[i].y].OnShowFishFlag(true);

            }
            for (int i = 0; i < cellList.Count; i++)
            {
                cellList[i].SetRoot(false);
            }
        }

        /// <summary>
        /// 生成三消关卡数据文件
        /// </summary>
        /// <param name="lvName"></param>
        /// <param name="needReame"></param>
        /// <returns></returns>
        public Hashtable GenerateMapData( out string lvName,bool needReame = true)
        {
            string m = LvID + ".json";
            string p = path + m;
            table = new Hashtable();
            var list = new ArrayList() { M3EditorConst.GridWidth.ToString(), M3EditorConst.GridRealHeight.ToString() };
            table.Add("MapSize", list);

            int gameTarget = (int)M3EditorTaskCtrl.Instance.GetGameTarget();
            table.Add("GameTarget", gameTarget);
            if (gameTarget == 2)
            {
                int score = (int)M3EditorTaskCtrl.Instance.GetTargetScore();
                table.Add("TargetScore", score);
            }
            int gameMode = (int)M3EditorTaskCtrl.Instance.GetGameMode();
            table.Add("GameMode", gameMode);
            int modeValue = (int)M3EditorTaskCtrl.Instance.GetModeValue();
            table.Add("ModeValue", modeValue);

            var tList = new ArrayList();
            var taskList = M3EditorTaskCtrl.Instance.GetTakeItem();
            for (int i = 0; i < taskList.Count; i++)
            {
                tList.Add(taskList[i].elementID);
            }
            table.Add("TaskElementID", tList);

            var tcList = new ArrayList();
            for (int i = 0; i < taskList.Count; i++)
            {
                tcList.Add(taskList[i].ElementCount);
            }
            table.Add("TaskElementCount", tcList);

            var itemList = new ArrayList();
            for (int i = 0; i < cellList.Count; i++)
            {
                var itemTable = new Hashtable
                    {
                        { "index",i },
                        { "col", cellList[i].gridY},
                        { "row", cellList[i].gridX },
                        { "elements", cellList[i].ToString() },
                        { "random", cellList[i].needRandomElement()},
                        { "empty",cellList[i].IsEmpty?1:0 },
                    };
                itemList.Add(itemTable);
            }

            var portList = new ArrayList();
            for (int i = 0; i < cellList.Count; i++)
            {
                if (cellList[i].IsPort)
                {
                    var tmpTable = new Hashtable {
                        { "x",cellList[i].gridX},
                        { "y",cellList[i].gridY},
                        { "rule",cellList[i].portRule}
                    };
                    portList.Add(tmpTable);
                }
            }
            table.Add("PortTiles", portList);
            table.Add("Item", itemList);


            var ruleList = new ArrayList();
            var editorRuleList = data.ruleList;
            if (editorRuleList != null)
            {
                for (int i = 0; i < editorRuleList.Count; i++)
                {
                    var hash = new Hashtable();
                    ArrayList arr = new ArrayList();
                    for (int j = 0; j < editorRuleList[i].ruleList.Count; j++)
                    {
                        var tmpHash = new Hashtable
                    {
                        { "code",editorRuleList[i].ruleList[j].elementID},
                            { "weight",editorRuleList[i].ruleList[j].weight},
                    };
                        arr.Add(tmpHash);
                    }
                    hash.Add(editorRuleList[i].ruleName, arr);
                    ruleList.Add(hash);
                }
            }
            table.Add("RuleDefine", ruleList);

            if (data.star != null)
                table.Add("StarScore", new ArrayList(data.star));

            table.Add("EnergyRate", data.energyRate);
            table.Add("EnergyValue1", data.energyStepCount);
            table.Add("EnergyValue2", data.energyCreateCount);


            List<EditorModelConveyorItem> conveyorList = M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorConveyor>().GetConveyor();
            var conList = new ArrayList();
            if (conveyorList != null && conveyorList.Count > 0)
            {
                foreach (var item in conveyorList)
                {
                    ArrayList arr = new ArrayList();
                    foreach (var v in item.list)
                    {
                        var tmphash = new Hashtable {
                            { "x",v.x},
                            { "y",v.y},
                        };
                        arr.Add(tmphash);
                    }
                    conList.Add(arr);
                }
                table.Add("Conveyor", conList);
            }

            foreach (var item in M3EditorController.instance.editorMain.moduleManager.modules)
            {
                item.UpdateToJson();
            }


            var saveText = table.ToJsonBytes();
            if (needReame)
            {
              string lv=  M3EditorFileOperation.OpenFile(LvID + ".json", saveText, "json");
                lv = lv.Split('/')[lv.Split('/').Length-1];
                lv = lv.Split('.')[0];
                lvName = lv;
            }
            else
            {
                lvName = LvID;
                File.WriteAllBytes(p, saveText);
                UnityEditor.AssetDatabase.Refresh();
            }
            return table;
        }


        public void ShowDisk(bool value)
        {
            if (value)
            {
                for (int i = 0; i < cellList.Count; i++)
                {
                    cellList[i].ShowDisk(value);
                }
            }
        }
    }
}
#endif
