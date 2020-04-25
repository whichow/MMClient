///** 
//*FileName:     #SCRIPTFULLNAME# 
//*Author:       #AUTHOR# 
//*Version:      #VERSION# 
//*UnityVersion：#UNITYVERSION#
//*Date:         #DATE# 
//*Description:    
//*History: 
//*/
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Game.Match3
//{
//    public class LevelData
//    {
//        //关卡序号
//        public int Id;
//        //关卡章号
//        public int Chapter;
//        //棋盘编号
//        public int MapId;
//        //关卡名字
//        public string Name;
//        //关卡界面坐标
//        public int[] Position;
//        //关卡图标
//        public string Icon;
//        //关卡目标描述
//        public string Notes;
//        //关卡目标ICON
//        public string NotesIcon;
//        //关卡消耗体力
//        public int NeedPower;
//        //关卡猫咪选择等级
//        public int CatChoiceLevel;
//        //关卡猫咪选择星级
//        public int CatChoiceStar;
//        //战前道具选择
//        public int ItemChoice1;
//        //关卡步数
//        public int Step;
//        //关卡时间
//        public int Time;
//        //红权重1
//        public int RedWeight1;
//        //黄权重1
//        public int YellowWeight1;
//        //蓝权重1
//        public int BlueWeight1;
//        //绿权重1
//        public int GreenWeight1;
//        //紫权重1
//        public int PurpleWeight1;
//        //棕权重1
//        public int BrownWeight1;
//        //红权重2
//        public int RedWeight2;
//        //黄权重2
//        public int YellowWeight2;
//        //蓝权重2
//        public int BlueWeight2;
//        //绿权重2
//        public int GreenWeight2;
//        //紫权重2
//        public int PurpleWeight2;
//        //棕权重2
//        public int BrownWeight2;
//        //能量块权重
//        public int PowerWeight;
//        //能量修正系数（步数相关）
//        public int PowerCorrectRatio;
//        //特殊元素掉落
//        public int[] SpecialElement;
//        //关卡内道具选择
//        public int ItemChoice2;
//        //关卡目标类型
//        public int MissionType;
//        //目标1,数量
//        public int[] Mission1;
//        //目标2,数量
//        public int[] Mission2;
//        //目标3,数量
//        public int[] Mission3;
//        //目标4,数量
//        public int[] Mission4;
//        //目标分数
//        public int MissionScore;
//        //一星得分
//        public int StarScore1;
//        //二星得分
//        public int StarScore2;
//        //三星得分
//        public int StarScore3;
//        //首次三星奖励ID,数量
//        public int[] FirstAllStarReward;
//        //金币奖励基数
//        public int CoinReward;
//        //额外奖励1，概率%
//        public string ExtraReward1;
//        //额外奖励2,概率%
//        public string ExtraReward2;
//        //初始计分次数
//        public int N;
//        //失败加分
//        public int P1;
//        //成功减分
//        public int P2;
//        //当前关卡影响系数
//        public int M;
//        //解锁关卡
//        public int NextLevel;
//        //新手引导编号
//        public int Guidance;


//    }

//    public class LevelDataManager : Singleton<LevelDataManager>
//    {

//        public static string Key = "LevelDataConfig";

//        public Dictionary<int, LevelData> levelDataDic;


//        public void LoadTable(Hashtable table)
//        {
//            if (table == null)
//            {
//                Debuger.LogError("");
//                return;
//            }
//            levelDataDic = new Dictionary<int, LevelData>();
//            var itemList = table.GetArrayList("LevelConfig");
//            var indexList = itemList.GetList(0);

//            int indexId = indexList.IndexOf("Id");
//            int indexChapter = indexList.IndexOf("Chapter");
//            int indexMapId = indexList.IndexOf("MapId");
//            int indexName = indexList.IndexOf("Name");
//            int indexPosition = indexList.IndexOf("Position");
//            int indexIcon = indexList.IndexOf("Icon");
//            int indexNotes = indexList.IndexOf("Notes");
//            int indexNotesIcon = indexList.IndexOf("NotesIcon");
//            int indexNeedPower = indexList.IndexOf("NeedPower");
//            int indexCatChoiceLevel = indexList.IndexOf("CatChoiceLevel");

//            int indexCatChoiceStar = indexList.IndexOf("CatChoiceStar");
//            int indexItemChoice1 = indexList.IndexOf("ItemChoice1");
//            int indexStep = indexList.IndexOf("Step");
//            int indexTime = indexList.IndexOf("Time");
//            int indexRedWeight1 = indexList.IndexOf("RedWeight1");
//            int indexYellowWeight1 = indexList.IndexOf("YellowWeight1");
//            int indexBlueWeight1 = indexList.IndexOf("BlueWeight1");
//            int indexGreenWeight1 = indexList.IndexOf("GreenWeight1");
//            int indexPurpleWeight1 = indexList.IndexOf("PurpleWeight1");
//            int indexBrownWeight1 = indexList.IndexOf("BrownWeight1");
//            int indexRedWeight2 = indexList.IndexOf("RedWeight2");
//            int indexYellowWeight2 = indexList.IndexOf("YellowWeight2");
//            int indexBlueWeight2 = indexList.IndexOf("BlueWeight2");
//            int indexGreenWeight2 = indexList.IndexOf("GreenWeight2");
//            int indexPurpleWeight2 = indexList.IndexOf("PurpleWeight2");
//            int indexBrownWeight2 = indexList.IndexOf("BrownWeight2");
//            int indexPowerWeight = indexList.IndexOf("PowerWeight");
//            int indexPowerCorrectRatio = indexList.IndexOf("PowerCorrectRatio");
//            int indexSpecialElement = indexList.IndexOf("SpecialElement");
//            int indexItemChoice2 = indexList.IndexOf("ItemChoice2");
//            int indexMissionType = indexList.IndexOf("MissionType");
//            int indexMission1 = indexList.IndexOf("Mission1");
//            int indexMission2 = indexList.IndexOf("Mission2");
//            int indexMission3 = indexList.IndexOf("Mission3");
//            int indexMission4 = indexList.IndexOf("Mission4");
//            int indexMissionScore = indexList.IndexOf("MissionScore");
//            int indexStarScore1 = indexList.IndexOf("StarScore1");
//            int indexStarScore2 = indexList.IndexOf("StarScore2");
//            int indexStarScore3 = indexList.IndexOf("StarScore3");
//            int indexFirstAllStarReward = indexList.IndexOf("FirstAllStarReward");
//            int indexCoinReward = indexList.IndexOf("CoinReward");
//            int indexExtraReward1 = indexList.IndexOf("ExtraReward1");
//            int indexExtraReward2 = indexList.IndexOf("ExtraReward2");
//            int indexN = indexList.IndexOf("N");
//            int indexP1 = indexList.IndexOf("P1");
//            int indexP2 = indexList.IndexOf("P2");
//            int indexM = indexList.IndexOf("M");
//            int indexNextLevel = indexList.IndexOf("NextLevel");
//            int indexGuidance = indexList.IndexOf("Guidance");

//            for (int i = 1; i < itemList.Count; i++)
//            {
//                var infoList = itemList[i] as ArrayList;
//                LevelData data = new LevelData();

//                data.Id = (int)infoList[indexId];
//                data.Chapter = (int)infoList[indexChapter];

//                data.MapId = (int)infoList[indexMapId];
//                data.Name = infoList[indexName].ToString();
//                data.Position = infoList.GetArray<int>(indexPosition);
//                data.Icon = infoList[indexIcon].ToString();
//                data.Notes = infoList[indexNotes].ToString();
//                data.NotesIcon = infoList[indexNotesIcon].ToString();
//                data.NeedPower = (int)infoList[indexNeedPower];
//                data.CatChoiceLevel = (int)infoList[indexCatChoiceLevel];
//                data.CatChoiceStar = (int)infoList[indexCatChoiceStar];

//                data.ItemChoice1 = (int)infoList[indexItemChoice1];

//                data.Step = (int)infoList[indexStep];
//                data.Time = (int)infoList[indexTime];
//                data.RedWeight1 = (int)infoList[indexRedWeight1];
//                data.YellowWeight1 = (int)infoList[indexYellowWeight1];
//                data.BlueWeight1 = (int)infoList[indexBlueWeight1];
//                data.GreenWeight1 = (int)infoList[indexGreenWeight1];
//                data.PurpleWeight1 = (int)infoList[indexPurpleWeight1];
//                data.BrownWeight1 = (int)infoList[indexBrownWeight1];
//                data.RedWeight2 = (int)infoList[indexRedWeight2];
//                data.YellowWeight2 = (int)infoList[indexYellowWeight2];
//                data.BlueWeight2 = (int)infoList[indexBlueWeight2];
//                data.GreenWeight2 = (int)infoList[indexGreenWeight2];
//                data.PurpleWeight2 = (int)infoList[indexPurpleWeight2];
//                data.BrownWeight2 = (int)infoList[indexBrownWeight2];
//                data.PowerWeight = (int)infoList[indexPowerWeight];
//                data.PowerCorrectRatio = (int)infoList[indexPowerCorrectRatio];
//                data.SpecialElement = infoList.GetArray<int>(indexSpecialElement);
//                data.ItemChoice2 = (int)infoList[indexItemChoice2];
//                data.MissionType = (int)infoList[indexMissionType];
//                data.Mission1 = infoList.GetArray<int>(indexMission1);
//                data.Mission2 = infoList.GetArray<int>(indexMission2);
//                data.Mission3 = infoList.GetArray<int>(indexMission3);
//                data.Mission4 = infoList.GetArray<int>(indexMission4);
//                data.MissionScore = (int)infoList[indexMissionScore];
//                data.StarScore1 = (int)infoList[indexStarScore1];
//                data.StarScore2 = (int)infoList[indexStarScore2];
//                data.StarScore3 = (int)infoList[indexStarScore3];

//                data.FirstAllStarReward = infoList.GetArray<int>(indexFirstAllStarReward);
//                data.CoinReward = (int)infoList[indexCoinReward];
//                data.ExtraReward1 = infoList[indexExtraReward1].ToString();
//                data.ExtraReward2 = infoList[indexExtraReward2].ToString();
//                data.N = (int)infoList[indexN];
//                data.P1 = (int)infoList[indexP1];
//                data.P2 = (int)infoList[indexP2];
//                data.M = (int)infoList[indexM];
//                data.NextLevel = (int)infoList[indexNextLevel];
//                data.Guidance = (int)infoList[indexGuidance];

//                levelDataDic.Add(data.Id, data);
//            }



//        }
//        public void LoadTable(TextAsset tex)
//        {
//            LoadTable(tex.text.ToJsonTable());
//        }
//        public LevelData GetLevelInfo(int levelID)
//        {
//            if (levelDataDic.ContainsKey(levelID))
//                return levelDataDic[levelID];
//            else
//            {
//                Debuger.Log("关卡读取错误 错误ID: " + levelID);
//                return null;
//            }
//        }

//    }
//}