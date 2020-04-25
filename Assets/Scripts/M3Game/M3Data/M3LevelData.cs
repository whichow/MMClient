/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/15 10:21:39
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.Collections;
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 三消关卡数据 （编辑器导出）
    /// </summary>
    public class M3LevelData
    {
        #region Field

        public string id;
        /// <summary>
        /// 棋盘宽度
        /// </summary>
        public int lvMapWidth;
        /// <summary>
        /// 棋盘高度
        /// </summary>
        public int lvMapHeight;

        /// <summary>
        /// 格子数据列表
        /// </summary>
        public List<M3CellData> celldata;

        #region 能量
        /// <summary>
        /// 能量修正 单个能量符增加的能量
        /// </summary>
        public int energyRate;
        /// <summary>
        /// 能量修正 多少步后生成能量符
        /// </summary>
        public int energyStepCount;
        /// <summary>
        /// 能量修正 生成能量符的数量
        /// </summary>
        public int energyCreateCount;
        #endregion

        #region 游戏模式
        /// <summary>
        /// 玩法类型 GameModeEnum  1,计时 2,步数
        /// </summary>
        public int gameMode;
        /// <summary>
        /// 限制步数 对应表 ModeValue
        /// </summary>
        public int steps;
        /// <summary>
        /// 限制时间 对应表 ModeValue
        /// </summary>
        public int time;
        #endregion

        #region 通关目标
        /// <summary>
        /// 目标类型 1,收集 2,积分
        /// </summary>
        public int gameTarget;
        /// <summary>
        /// 目标积分
        /// </summary>
        public int score;
        /// <summary>
        /// 目标消除元素  对应表 TaskElementID[] TaskElementCount[]
        /// </summary>
        public Dictionary<int, int> LevelTaskElementDic { get; private set; }
        /// <summary>
        /// 通关星数的积分
        /// </summary>
        public int[] star;
        #endregion

        #region 元素掉落
        /// <summary>
        /// 元素掉落口列表
        /// </summary>
        public List<PortData> portTilesList;
        /// <summary>
        /// 掉落口生成元素规则
        /// </summary>
        public List<SpawnRuleDefinition> ruleList;
        #endregion

        #region 传送带
        /// <summary>
        /// 传送带
        /// </summary>
        private List<List<Int2>> conveyor;
        #endregion

        #region 鱼
        /// <summary>
        /// 鱼掉落口列表
        /// </summary>
        private List<Int2> fishExitList;
        /// <summary>
        /// 鱼掉出口列表
        /// </summary>
        private List<Int2> fishPortList;
        private List<M3FishModelItem> fishModelList;
        /// <summary>
        /// 是否启用大嘴僵尸
        /// </summary>
        public bool hasZombie;
        #endregion

        #region 随机生成元素规则列表
        /// <summary>
        /// 开局空格随机生成元素规则列表
        /// </summary>
        private List<RuleCode> initSpawnList;
        #endregion

        #region 瞬移传送门
        /// <summary>
        /// 瞬移传送门列表
        /// </summary>
        public List<M3PortalModel> FlickerPortalList;
        #endregion

        /// <summary>
        /// 隐藏在冰块底下，占两格，消除冰块可收集 （埋藏）
        /// </summary>
        private List<M3HiddenModel> hiddenList;

        #endregion

        #region C&D
        public M3LevelData(string lv)
        {
            id = lv;
        }

        public M3LevelData()
        {
        }
        #endregion

        #region Init

        public void SetDefaultData(string _id, int x, int y)
        {
            id = _id;
            lvMapWidth = x;
            lvMapHeight = y;

            celldata = new List<M3CellData>();
            portTilesList = new List<PortData>();
            ruleList = new List<SpawnRuleDefinition>();
            star = new int[] { 1000, 2000, 3000 };

            int index = 0;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    celldata.Add(new M3CellData(x, y, index));
                    index++;
                }
            }
        }

        public void SetConfigData(Hashtable table)
        {
            InitMode(table);
            InitTarget(table);
            InitMap(table);
            InitPort(table);
            InitConveryor(table);
            InitFish(table);
            InitRandomSpawn(table);
            InitPortal(table);
            InitHidden(table);
        }

        public void InitMode(Hashtable table)
        {
            gameMode = table.GetInt("GameMode");
            if (gameMode == 1)
            {
                time = table.GetInt("ModeValue");
            }
            else if (gameMode == 2)
            {
                steps = table.GetInt("ModeValue");
            }
        }

        public void InitTarget(Hashtable table)
        {
            LevelTaskElementDic = new Dictionary<int, int>();
            gameTarget = table.GetInt("GameTarget");
            if ((GameTargetEnum)gameTarget == GameTargetEnum.Score)
            {
                score = table.GetInt("TargetScore");
            }
            else
            {
                var idStr = table.GetArrayList("TaskElementID");
                var countStr = table.GetArrayList("TaskElementCount");
                int[] idArr = new int[idStr.Count];
                int[] countArr = new int[countStr.Count];
                for (int i = 0; i < idStr.Count; i++)
                {
                    idArr[i] = idStr[i].ToString().ToInt();
                    countArr[i] = countStr[i].ToString().ToInt();
                }

                for (int i = 0; i < idStr.Count; i++)
                {
                    LevelTaskElementDic[idArr[i]] = countArr[i];
                }
            }
            star = table.GetArray<int>("StarScore");
        }

        public void InitMap(Hashtable table)
        {
            //string str = table.GetString("index", "null");
            //if (str == "null")
            //{
            //    int tmpID = table.GetInt("index");
            //    id = tmpID.ToString();
            //}
            //else
            //{
            //    id = str;
            //}
            var sizeStr = table.GetArrayList("MapSize");
            lvMapWidth = sizeStr[0].ToString().ToInt();
            lvMapHeight = sizeStr[1].ToString().ToInt();

            celldata = new List<M3CellData>();
            M3CellData data;
            var list = table.GetArray<Hashtable>("Item");
            foreach (var node in list)
            {
                data = new M3CellData();
                data.Parse(node);
                celldata.Add(data);
            }

            energyRate = table.GetInt("EnergyRate");
            energyStepCount = table.GetInt("EnergyValue1");
            energyCreateCount = table.GetInt("EnergyValue2");
        }

        public void InitPort(Hashtable table)
        {
            portTilesList = new List<PortData>();
            ruleList = new List<SpawnRuleDefinition>();

            var portList = table.GetArray<Hashtable>("PortTiles");
            if (portList != null)
            {
                int portX;
                int portY;
                string portRule;
                for (int i = 0; i < portList.Length; i++)
                {
                    portX = portList[i].GetInt("x");
                    portY = portList[i].GetInt("y");
                    portRule = portList[i].GetString("rule");
                    portTilesList.Add(new PortData(portX, portY, portRule));
                }
            }

            var rList = table.GetArray<Hashtable>("RuleDefine");
            if (rList != null)
            {
                SpawnRuleDefinition ruleDefine;
                List<RuleCode> code;
                for (int i = 0; i < rList.Length; i++)
                {
                    code = new List<RuleCode>();
                    string ruleName = "Rule_" + (i + 1).ToString();
                    var ruleEleList = rList[i].GetArray<Hashtable>(ruleName);
                    if (ruleEleList != null)
                    {
                        for (int j = 0; j < ruleEleList.Length; j++)
                        {
                            code.Add(new RuleCode(ruleEleList[j].GetInt("code"), ruleEleList[j].GetInt("weight")));
                        }
                        ruleDefine = new SpawnRuleDefinition(ruleName, code);
                        ruleList.Add(ruleDefine);
                    }
                }
            }
        }

        private void InitConveryor(Hashtable table)
        {
            conveyor = new List<List<Int2>>();
            var con = table.GetArrayList("Conveyor");//[[{},{}],[{},{}]]
            if (con != null)
            {
                for (int i = 0; i < con.Count; i++)
                {
                    ArrayList arrlist = con.GetList(i);
                    List<Int2> tmpList = new List<Int2>();
                    for (int j = 0; j < arrlist.Count; j++)
                    {
                        var hash = arrlist.GetTable(j);
                        Int2 pos = new Int2(hash.GetInt("x"), hash.GetInt("y"));
                        tmpList.Add(pos);
                    }
                    conveyor.Add(tmpList);
                }
            }
        }

        private void InitFish(Hashtable table)
        {
            var list = table.GetArrayList("FishExit");
            fishExitList = new List<Int2>();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    fishExitList.Add(new Int2(list.GetTable(i).GetInt("x"), list.GetTable(i).GetInt("y")));
                }
            }

            var portList = table.GetArrayList("FishPort");
            fishPortList = new List<Int2>();
            if (portList != null && portList.Count > 0)
            {
                for (int i = 0; i < portList.Count; i++)
                {
                    fishPortList.Add(new Int2(portList.GetTable(i).GetInt("x"), portList.GetTable(i).GetInt("y")));
                }
            }

            hasZombie = table.GetInt("Zombie") == 0 ? false : true;

            fishModelList = new List<M3FishModelItem>();

            var fishRuleList = table.GetArrayList("FishRule");
            if (fishRuleList != null)
            {
                for (int i = 0; i < fishRuleList.Count; i++)
                {
                    List<Int2> spawnFishPortList = new List<Int2>();
                    var arrSpawn = fishRuleList.GetTable(i).GetArrayList("SpawnPos");
                    for (int j = 0; j < arrSpawn.Count; j++)
                    {
                        spawnFishPortList.Add(new Int2(arrSpawn.GetTable(j).GetInt("x"), arrSpawn.GetTable(j).GetInt("y")));
                    }

                    fishModelList.Add(new M3FishModelItem()
                    {
                        needPrevious = fishRuleList.GetTable(i).GetInt("NeedPrevious"),
                        needStep = fishRuleList.GetTable(i).GetInt("NeedStep"),
                        needTime = fishRuleList.GetTable(i).GetInt("NeedTime"),
                        needScore = fishRuleList.GetTable(i).GetInt("NeedScore"),
                        isRandom = fishRuleList.GetTable(i).GetInt("IsRandom"),
                        combineMode = fishRuleList.GetTable(i).GetInt("CombineMode"),
                        spawnList = spawnFishPortList,
                    });
                }
            }
        }

        private void InitRandomSpawn(Hashtable table)
        {
            var list = table.GetArrayList("InitSpawn");
            initSpawnList = new List<RuleCode>();
            if (list == null)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                initSpawnList.Add(new RuleCode(list.GetTable(i).GetInt("id"), list.GetTable(i).GetInt("weight")));
            }
        }

        private void InitPortal(Hashtable table)
        {
            FlickerPortalList = new List<M3PortalModel>();
            var list = table.GetArrayList("Portal");
            if (list == null)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                FlickerPortalList.Add(new M3PortalModel()
                {
                    in_x = list.GetTable(i).GetInt("in_x"),
                    in_y = list.GetTable(i).GetInt("in_y"),
                    out_x = list.GetTable(i).GetInt("out_x"),
                    out_y = list.GetTable(i).GetInt("out_y"),
                    isHidden = list.GetTable(i).GetInt("isHidden") == 1 ? true : false,
                });
            }
        }

        private void InitHidden(Hashtable table)
        {
            hiddenList = new List<M3HiddenModel>();
            var list = table.GetArrayList("Hidden");
            if (list == null)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                var tList = list.GetTable(i).GetArrayList("list");
                M3HiddenItem[] itemArry = new M3HiddenItem[tList.Count];
                for (int j = 0; j < tList.Count; j++)
                {
                    itemArry[j] = new M3HiddenItem()
                    {
                        from = new M3HiddenPoint(tList.GetTable(j).GetInt("fromX"), tList.GetTable(j).GetInt("fromY")),
                        to = new M3HiddenPoint(tList.GetTable(j).GetInt("toX"), tList.GetTable(j).GetInt("toY"))

                    };
                }
                hiddenList.Add(new M3HiddenModel()
                {
                    count = list.GetTable(i).GetInt("count"),
                    type = list.GetTable(i).GetInt("type"),
                    list = itemArry,
                });
            }
        }

        #endregion

        /// <summary>
        /// 获取开场随机生成元素规则
        /// </summary>
        /// <returns></returns>
        public List<RuleCode> GetSpawnList()
        {
            List<RuleCode> ruleList = new List<RuleCode>();
            if (initSpawnList != null)
            {
                for (int i = 0; i < initSpawnList.Count; i++)
                {
                    ruleList.Add(initSpawnList[i].GetInfo());
                }

                return new List<RuleCode>(this.initSpawnList);
            }
            return ruleList;
        }

        /// <summary>
        /// 获取掉落口生成规则
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public List<RuleCode> GetRule(string rule)
        {
            for (int i = 0; i < ruleList.Count; i++)
            {
                if (ruleList[i].ruleName == rule)
                {
                    return new List<RuleCode>(ruleList[i].ruleList);
                }
            }
            return null;
        }

        public List<List<Int2>> GetConveyorList()
        {
            if (conveyor != null)
                return new List<List<Int2>>(this.conveyor);
            return null;
        }

        public List<Int2> GetFishExit()
        {
            if (fishExitList != null)
                return new List<Int2>(fishExitList);
            return null;
        }

        public List<Int2> GetFishPort()
        {
            if (fishPortList != null)
                return new List<Int2>(fishPortList);
            return null;
        }

        public List<M3FishModelItem> GetFishModelList()
        {
            List<M3FishModelItem> itemList = new List<M3FishModelItem>();
            if (fishModelList != null)
            {
                for (int i = 0; i < this.fishModelList.Count; i++)
                {
                    itemList.Add(fishModelList[i].CreateInfo());
                }
            }
            return itemList;
        }

        public List<M3HiddenModel> HiddenList
        {
            get
            {
                List<M3HiddenModel> list = new List<M3HiddenModel>();
                if (hiddenList != null)
                {
                    for (int i = 0; i < hiddenList.Count; i++)
                    {
                        list.Add(hiddenList[i].Clone());
                    }
                }
                return list;
            }
            set
            {
                hiddenList = value;
            }
        }

    }
}
