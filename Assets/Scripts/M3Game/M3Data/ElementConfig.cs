//using System.Collections;
//using System.Collections.Generic;
//using Game;
//using Game.DataModel;
//using UnityEngine;

//namespace Game.Match3
//{
//    public class ElementConfig
//    {
//        //元素序号
//        public int ID;
//        //元素名称
//        public string name;
//        //元素图标
//        public string icon;
//        //模型名字
//        public string modelName;
//        //皮肤
//        public string skin;

//        public Hashtable animations;
//        //元素待机动画
//        public string idleAnim;
//        //元素待机特效
//        public string idleEffect;
//        //元素选中动画
//        public string selectAnim;
//        //元素选中特效
//        public string selectEffect;
//        //元素消除动画
//        public string clearAnim;
//        //元素消除特效
//        public string clearEffect;
//        //元素回合动画
//        public string roundAnim;
//        //元素回合特效
//        public string roundEffect;
//        //元素选中音效
//        public string sound1;
//        //元素消除音效
//        public string sound2;
//        //元素类型层级
//        public int typeLevel;
//        //元素层级
//        public int level;
//        //元素类型
//        public int type;
//        //是否移动
//        public int move;
//        //是否下落
//        public int drop;
//        //技能阻挡
//        public int resist;
//        //消除类型
//        public int[] clear;
//        //关联元素ID
//        public int linkID;
//        //消除事件
//        public int[] clearEvent;
//        //消除后转化ID
//        public int clearTransfor;
//        //消除后转化个数
//        public int clearTransforNum;

//        public int jumpType;

//        public int[] jumpGroup;

//        public int jumpSpace;

//        public int overlyingRound;

//        public int overlyingMaxNum;

//        public int overlyingAddType;

//        public int overlyingType;

//        public int overlyingClearType;

//        public int overlyingRatio;

//        //生成积分
//        public int score;
//        //基础消除积分
//        public int point;
//        //被直线消除积分
//        public int lineScore;
//        //被爆炸消除积分
//        public int boomScore;
//        //被单个活力猫消除积分
//        public int singleCateScore;
//        //被双重直线消除积分
//        public int doubleLineScore;
//        //被爆炸与直线消除积分
//        public int boomAndLineScore;
//        //被双爆消除积分
//        public int doubleBoomScore;
//        //被双全屏消除积分
//        public int doubuleCatScore;
//        //颜色类型
//        public int colorType;
//        //绳索阻挡类型
//        public int ropeType;
//        //回合移动类型
//        public int moveType;
//        //回合移动方向
//        public int RoundDirection;
//        //出入类型
//        public int exit;

//        public int[] collect;

//        public int health;



//        static public readonly string urlKey = "ElementXDM";
//        static Dictionary<int, ElementConfig> Dictionary;



//        public static void Init()
//        {
//            TextAsset tex;
//            KAssetManager.Instance.TryGetExcelAsset(ElementConfig.urlKey, out tex);
//            ElementConfig.Parse(tex.text.ToJsonTable());
//        }
//        static public void Parse(Hashtable table)
//        {
//            Dictionary = new Dictionary<int, ElementConfig>();
//            var list = table.GetArrayList(urlKey);
//            ArrayList proList = (ArrayList)list[0];
//            ArrayList eList = new ArrayList();
//            for (int i = 1; i < list.Count; i++)
//            {
//                ElementConfig config = new ElementConfig();
//                eList = (ArrayList)list[i];

//                config.animations = eList.GetTable(proList.IndexOf("Animations"));
//                config.ID = (int)eList[proList.IndexOf("ID")];
//                config.name = eList[proList.IndexOf("name")].ToString();
//                config.icon = eList[proList.IndexOf("icon")].ToString();
//                config.modelName = eList[proList.IndexOf("modelName")].ToString();
//                config.skin = eList[proList.IndexOf("skin")].ToString();
//                config.idleAnim = eList[proList.IndexOf("idleAnim")].ToString();
//                config.idleEffect = eList[proList.IndexOf("idleEffect")].ToString();
//                config.selectAnim = eList[proList.IndexOf("selectAnim")].ToString();
//                config.selectEffect = eList[proList.IndexOf("selectEffect")].ToString();
//                config.clearAnim = eList[proList.IndexOf("clearAnim")].ToString();
//                config.clearEffect = eList[proList.IndexOf("clearEffect")].ToString();
//                config.roundAnim = eList[proList.IndexOf("roundAnim")].ToString();
//                config.roundEffect = eList[proList.IndexOf("roundEffect")].ToString();
//                config.sound1 = eList[proList.IndexOf("sound1")].ToString();
//                config.sound2 = eList[proList.IndexOf("sound2")].ToString();
//                config.typeLevel = (int)eList[proList.IndexOf("typeLevel")];
//                config.level = (int)eList[proList.IndexOf("level")];
//                config.type = (int)eList[proList.IndexOf("type")];
//                config.move = (int)eList[proList.IndexOf("move")];
//                config.drop = (int)eList[proList.IndexOf("drop")];
//                config.resist = (int)eList[proList.IndexOf("resist")];
//                config.clear = eList.GetArray<int>(proList.IndexOf("clear"));
//                config.linkID = (int)eList[proList.IndexOf("linkID")];
//                config.clearEvent = eList.GetArray<int>(proList.IndexOf("clearEvent"));
//                config.clearTransfor = (int)eList[proList.IndexOf("clearTransfor")];
//                config.clearTransforNum = (int)eList[proList.IndexOf("clearTransforNum")];
//                config.jumpType = (int)eList[proList.IndexOf("jumpType")];
//                config.jumpGroup = eList.GetArray<int>(proList.IndexOf("jumpGroup"));
//                config.jumpSpace = (int)eList[proList.IndexOf("jumpSpace")];
//                config.overlyingRound = (int)eList[proList.IndexOf("overlyingRound")];
//                config.overlyingMaxNum = (int)eList[proList.IndexOf("overlyingMaxNum")];
//                config.overlyingAddType = (int)eList[proList.IndexOf("overlyingAddType")];
//                config.overlyingType = (int)eList[proList.IndexOf("overlyingType")];
//                config.overlyingClearType = (int)eList[proList.IndexOf("overlyingClearType")];
//                config.overlyingRatio = (int)eList[proList.IndexOf("overlyingRatio")];
//                config.score = (int)eList[proList.IndexOf("score")];
//                config.point = (int)eList[proList.IndexOf("point")];
//                config.lineScore = (int)eList[proList.IndexOf("lineScore")];
//                config.boomScore = (int)eList[proList.IndexOf("boomScore")];
//                config.singleCateScore = (int)eList[proList.IndexOf("singleCateScore")];
//                config.doubleLineScore = (int)eList[proList.IndexOf("doubleLineScore")];
//                config.boomAndLineScore = (int)eList[proList.IndexOf("boomAndLineScore")];
//                config.doubleBoomScore = (int)eList[proList.IndexOf("doubleBoomScore")];
//                config.doubuleCatScore = (int)eList[proList.IndexOf("doubuleCatScore")];
//                config.colorType = (int)eList[proList.IndexOf("colorType")];
//                config.ropeType = (int)eList[proList.IndexOf("ropeType")];
//                config.moveType = (int)eList[proList.IndexOf("moveType")];
//                config.RoundDirection = (int)eList[proList.IndexOf("RoundDirection")];
//                config.exit = (int)eList[proList.IndexOf("exit")];
//                config.collect = eList.GetArray<int>(proList.IndexOf("collect"));
//                config.health = (int)eList[proList.IndexOf("health")];


//                Dictionary.Add(config.ID, config);
//            }

//        }
//        static public ElementXDM Get(int id)
//        {
//            if (Dictionary != null && Dictionary.ContainsKey(id))
//                return Dictionary[id];
//            return null;
//            return XTable.ElementXTable.GetByID(id);
//        }
//        static public Dictionary<int, ElementConfig> Get()
//        {
//            return Dictionary;
//        }
//    }
//}