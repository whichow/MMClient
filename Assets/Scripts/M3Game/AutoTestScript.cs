
using Game;
using Game.DataModel;
/** 
*FileName:     AutoTestScript.cs 
*Author:       Hejunjie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-11-20 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game.Match3
{
    public class AutoTestScript : SingletonUnity<AutoTestScript>
    {

        private M3Grid[,] gridArr;

        private M3Item[,] itemArr;
        KCat cat;

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log(FrameScheduler.instance.GetCurrentFrame());
                var list = M3Supporter.Instance.GetAllCanMoves();
                List<Int2> random = list[Random.Range(0, list.Count)];
                SwapItem(random[0].x, random[0].y, random[1].x, random[1].y);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log(M3GameManager.Instance.modeManager.roundScore);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log(M3ItemManager.Instance.gridItems);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("回溯上一个棋盘");
                M3GameManager.Instance.BackDate();
            }


            if (Input.GetKeyDown(KeyCode.A))
            {
                for (int i = 0; i < M3Config.GridHeight; i++)
                {
                    for (int j = 0; j < M3Config.GridWidth; j++)
                    {
                        if (M3ItemManager.Instance.gridItems[i, j] != null)
                        {
                            if (M3ItemManager.Instance.gridItems[i, j].isTweening)
                            {
                                Debug.Log(i + "_" + j);
                            }
                        }
                    }
                }
            }
            StringBuilder sb;
            if (Input.GetKeyDown(KeyCode.W))
            {
                sb = DebugMap();
            }
        }
        /// <summary>
        /// 返回初始棋盘
        /// </summary>
        public void ReturnToFirst()
        {
            M3GameManager.Instance.BackDate(M3GameManager.Instance.firstBackData);
            M3GameManager.Instance.modeManager.isGameEnd = false;
            M3GameManager.Instance.modeManager.GameModeCtrl.isEnd = false;
            M3GameManager.Instance.modeManager.GameModeCtrl.isTargetFinish= false;
        }
        public static StringBuilder DebugMap()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    if (M3ItemManager.Instance.gridItems[i, j] == null)
                        sb.Append("       ");
                    else
                    {
                        try
                        {
                            for (int k = 0; k < M3ItemManager.Instance.gridItems[i, j].itemInfo.allElementList.Count; k++)
                            {

                                sb.Append(M3ItemManager.Instance.gridItems[i, j].itemInfo.allElementList[k].data.config.ID);
                                if (k != 0)
                                    sb.Append("_");
                            }

                        }
                        catch (System.Exception)
                        {
                            Debug.LogError("  Error  : " + i + "_" + j);
                        }
                    }
                    sb.Append("(" + i + "," + j + ")");
                    sb.Append(" ");

                }
                sb.Append("\r\n");
            }
            Debug.LogWarning(sb);
            return sb;
        }

        public void Init()
        {
            //ElementConfig.Init();
            XTable.ElementXTable.Load();

            M3LevelConfigMgr.Instance.Init();

            //Debug.Log(M3LevelConfigMgr.Instance.lvConfigDataDic.Count);
        }
        /// <summary>
        /// 检测猫技能是否准备
        /// </summary>
        /// <returns></returns>
        public bool CheckCatSkillReady()
        {
            if (M3GameManager.Instance.catManager != null)
            {
                if (M3GameManager.Instance.catManager.hasCat)
                {
                    return M3GameManager.Instance.catManager.EnergyCompleted();
                }
            }
            return false;
        }

        /// <summary>
        /// 使用猫的技能
        /// </summary>
        /// <returns></returns>
        public void DoCatSkill()
        {
            if (M3GameManager.Instance.catManager != null)
            {
                if (M3GameManager.Instance.catManager.hasCat)
                {
                    var skill = M3GameManager.Instance.catManager.GetSkillEntity();
                    //if (skill.skillOperationType == SkillOperationType.PointTouch)
                    //{
                    //    var list = skill.GetOperationElementPos();
                    //    if (list.Count > 0)
                    //    {
                    //        var pos = list[M3Supporter.Instance.GetRandomInt(0, list.Count)];
                    //        skill.OnUseSkill(new M3UseSkillArgs(M3GameManager.Instance.catManager.GameCat, pos.x, pos.y));
                    //    }
                    //    else
                    //    {
                    //        //return false;
                    //    }
                    //}
                    //else if (skill.skillOperationType == SkillOperationType.None)
                    {
                        skill.OnUseSkill(new M3UseSkillArgs(M3GameManager.Instance.catManager.GameCat));
                    }
                }
            }
            //return false; 
        }

        /// <summary>
        /// 获取当前棋盘所有可以执行交换的Item
        /// </summary>
        /// <returns></returns>
        public List<List<Int2>> GetCurrentAllCanMoveItem()
        {
            return M3Supporter.Instance.GetAllCanMoves();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="catID">猫的ID dafult</param>
        /// <param name="star">猫的星级</param>
        /// <param name="lv">猫的等级</param>
        /// <param name="matchAbility">消除能力</param>
        /// <param name="skillLV">技能等级</param>
        public void SetCat(int catID, int star, int lv, int matchAbility, int skillLV)
        {
            cat = new KCat
            {
                //shopId = catID,
                shopId = 10101,
                star = star,
                grade = lv,
                initMatchAbility = matchAbility,
                skillGrade = skillLV
            };
        }

        /// <summary>
        /// 获取回合分数
        /// </summary>
        /// <returns></returns>
        public int GetRoundScore()
        {
            return M3GameManager.Instance.modeManager.roundScore;
        }
        /// <summary>
        /// 获取回合能量
        /// </summary>
        /// <returns></returns>
        public float GetRoundEnergy()
        {
            return M3GameManager.Instance.catManager.RoundEnergy;
        }
        /// <summary>
        /// 获取回合消除元素ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetRoundElement()
        {
            return M3GameManager.Instance.modeManager.roundElement;
        }
        /// <summary>
        /// 执行交换
        /// </summary>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        public void SwapItem(int fx, int fy, int sx, int sy)
        {
            DebugMap();
            Debug.LogWarning("执行交换操作 ： From: " + fx + "|" + fy + " To: " + sx + "|" + sy);
            //if (M3GameManager.Instance.modeManager.GameModeCtrl.targetDic != null)
            //    foreach (var item in M3GameManager.Instance.modeManager.GameModeCtrl.targetDic)
            //    {
            //        Debug.LogWarning(item.Key + "__" + item.Value);
            //    }
            M3Item item1 = M3ItemManager.Instance.gridItems[fx, fy];
            M3Item item2 = M3ItemManager.Instance.gridItems[sx, sy];
            if (item1 != null && item2 != null)
            {
                Debug.Log("类型： " + item1.itemInfo.GetElement() + " | " + item2.itemInfo.GetElement());
                if (M3GameManager.Instance.CheckSwap(item1, item2))
                {
                    if (M3GameManager.Instance.CheckLogic(item1, item2))
                    {
                        Debug.Log(M3ItemManager.Instance.gridItems);
                    }
                    else
                    {
                        Debug.LogError("匹配失败: 无法消除");
                        Debug.LogError(item1.itemInfo.GetElement().GetColor() + " | " + item2.itemInfo.GetElement().GetColor());
                        Debug.LogError(item1.itemInfo.GetElement().GetSpecial() + " | " + item2.itemInfo.GetElement().GetSpecial());

                    }
                }
                else
                {
                    Debug.Log("无法交换");
                }
            }
            else
            {
                Debug.Log("item is null");
            }
        }

        //private void OnGUI()
        //{

        //    if (GUILayout.Button("AAAAAAAAAAAAAAAAAA"))
        //    {
        //        LoadMap("999");
        //    }
        //}

        public void LoadMap(string mapID)
        {
            FrameScheduler.Instance().Clear();
            var levelData = M3LevelConfigMgr.Instance.MapReader(mapID);
            M3Config.GridHeight = levelData.lvMapHeight;
            M3Config.GridWidth = levelData.lvMapWidth;
            M3Config.GridHeightIndex = levelData.lvMapHeight - 1;
            M3Config.GridWidthIndex = levelData.lvMapWidth - 1;
            M3GameManager.Instance.level = levelData;
            var map = new M3CellData[levelData.lvMapHeight, levelData.lvMapWidth];
            for (int i = 0; i < map.Length; i++)
            {
                map[levelData.celldata[i].gridX, levelData.celldata[i].gridY] = levelData.celldata[i];
            }
            M3GameManager.Instance.specialHandler = new M3SpecialHandler();
            M3GameManager.Instance.propManager = new M3PropManager();
            M3GameManager.Instance.comboManager = new M3ComboManager();
            M3GameManager.Instance.matcher = new M3Matcher();
            M3GameManager.Instance.modeManager = new M3GameModeManager();
            M3GameManager.Instance.fishManager = new M3FishManager();
            M3GameManager.Instance.conveyorManager = new M3ConveyorManager();
            M3GameManager.Instance.conveyorManager.Init();
            M3GameManager.Instance.catManager = new M3CatManager();


            M3GameManager.Instance.catManager.Init(cat);


            M3GridManager.Instance.CreateGrid(map);
            M3GridManager.Instance.InitPort(levelData);
            M3GridManager.Instance.InitPortal(M3GameManager.Instance.level);//初始化传送门
            M3GridManager.Instance.InitBlankInfo();//初始化下落格
            EliminateManager.Instance.Init();

            ///Init ItemManager
            M3ItemManager.Instance.CreateMap(map);


            M3GameManager.Instance.venomManager = new M3VenomManager();
            M3GameManager.Instance.RunGame();
            M3GameManager.Instance.SetCurrentBackDate();
            M3GameManager.Instance.SetFirstBackDate();
        }




    }
}