// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KPlayer" company=""></copyright>
// <summary></summary>
// ***********************************************************************
namespace Game
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 角色相关
    /// </summary>
    public class KPlayer
    {
        public int id
        {
            get;
            private set;
        }

        public string name
        {
            get;
            private set;
        }

        public string account
        {
            get;
            private set;
        }

        public string token
        {
            get;
            private set;
        }

        ///// <summary>
        ///// 金币
        ///// </summary>
        //public int coin
        //{
        //    get { return KDatabase.GetInt(2, "count"); }
        //    set { KDatabase.SetInt(2, "count", value); }
        //}

        ///// <summary>
        ///// 钻石
        ///// </summary>
        //public int stone
        //{
        //    get { return KDatabase.GetInt(3, "count"); }
        //    set { KDatabase.SetInt(3, "count", value); }
        //}

        ///// <summary>
        ///// 食物
        ///// </summary>
        //public int food
        //{
        //    get { return KDatabase.GetInt(4, "count"); }
        //    set { KDatabase.SetInt(4, "count", value); }
        //}

        ///// <summary>
        ///// 行动力 
        ///// </summary>
        ///// <returns></returns>
        //public int Vigour
        //{
        //    get
        //    {
        //        var item =  KItemManager.Instance.GetThing(14);
        //        return item.curCount;
        //    }
        //    //get { return KDatabase.GetInt(14, "count"); }
        //    //set { KDatabase.SetInt(14, "count", value); }
        //}

        ///// <summary>
        ///// 魂石
        ///// </summary>
        //public int soulStone
        //{
        //    get { return KDatabase.GetInt(9, "count"); }
        //    set { KDatabase.SetInt(9, "count", value); }
        //}

        ///// <summary>
        ///// 魅力值
        ///// </summary>
        //public int charm
        //{
        //    get { return KDatabase.GetInt(7, "count"); }
        //    set { KDatabase.SetInt(7, "count", value); }
        //}

        //public int charmMadal
        //{
        //    get { return KDatabase.GetInt(10, "count"); }
        //    set { KDatabase.SetInt(10, "count", value); }
        //}
        ///// <summary>
        ///// 友情点
        ///// </summary>
        //public int friendPoint
        //{
        //    get { return KDatabase.GetInt(6, "count"); }
        //    set { KDatabase.SetInt(6, "count", value); }
        //}

        ///// <summary>
        ///// 体力值
        ///// </summary>
        //public int power
        //{
        //    get { return KDatabase.GetInt(5, "count"); }
        //    set { KDatabase.SetInt(5, "count", value); }
        //}

        public int maxPower
        {
            get
            {
                var gi = GetGradeInfo(PlayerDataModel.Instance.mPlayerData.mLevel);
                return gi != null ? gi.maxPower : 0;
            }
        }

        ///// <summary>
        ///// 点赞数
        ///// </summary>
        //public int praise
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 等级
        ///// </summary>
        //public int grade
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 经验
        ///// </summary>
        //public int exp
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 当前等级所需经验
        /// </summary>
        public int maxExp
        {
            get
            {
                var gi = GetGradeInfo(PlayerDataModel.Instance.mPlayerData.mLevel);
                return gi != null ? gi.maxExp : 0;
            }
        }

        //public int buyPowerCount
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 昵称
        ///// </summary>
        //public string nickName
        //{
        //    get;
        //    set;
        //}
        ///// <summary>
        ///// 改名次数
        ///// </summary>
        //public int changeNameCount
        //{
        //    get;
        //    set;
        //}
        ///// <summary>
        ///// 改名价格
        ///// </summary>
        //public KItem.ItemInfo changeNameCost
        //{
        //    get
        //    {
        //        return new KItem.ItemInfo
        //        { itemID = 3, itemCount = 0 };
        //    }
        //}

        //private string _headURL;
        ///// <summary>
        ///// 头像
        ///// </summary>
        //public string headURL
        //{
        //    get { return _headURL; }
        //    set
        //    {
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            _headURL = value;
        //        }
        //        else
        //        {
        //            _headURL = "Icon_Touxiang1_01";
        //        }
        //    }
        //}

        //private int _headId;
        ///// <summary>
        ///// 头像ID
        ///// </summary>
        //public int headID
        //{
        //    get { return _headId; }
        //    set
        //    {
        //        if (_headId == value)
        //            return;
        //        _headId = value;
        //    }
        //}

        ///// <summary>
        ///// 当前星数
        ///// </summary>
        //public int curStar
        //{
        //    get { return KDatabase.GetInt(12, "count"); }
        //    set { KDatabase.SetInt(12, "count", value); }
        //}

        ///// <summary>
        ///// 最大星星数
        ///// </summary>
        //public int maxStar
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 当前通关关卡数目
        ///// </summary>
        //public int maxLevel
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 最大可通关关卡
        ///// </summary>
        //public int maxUnlockLevel
        //{
        //    get;
        //    set;
        //}

        public int maxFarmland
        {
            get
            {
                var gi = GetGradeInfo(PlayerDataModel.Instance.mPlayerData.mLevel);
                return gi != null ? gi.maxFarmland : 0;
            }
        }

        public int maxCattery
        {
            get
            {
                var gi = GetGradeInfo(PlayerDataModel.Instance.mPlayerData.mLevel);
                return gi != null ? gi.maxCattery : 0;
            }
        }

        public KPlayer(int id, string name, string account, string token)
        {
            this.id = id;
            this.name = name;
            this.account = account;
            this.token = token;
        }

        #region 角色信息表
        //Grade MaxExp  MaxFarm MaxCattery  MaxPower

        /// <summary>
        /// 等级信息表
        /// </summary>
        public class GradeInfo
        {
            public int grade
            {
                get;
                private set;
            }
            public int maxExp
            {
                get;
                private set;
            }
            public int maxFarmland
            {
                get;
                private set;
            }
            public int maxCattery
            {
                get;
                private set;
            }
            /// <summary>
            /// 最大体力
            /// </summary>
            public int maxPower
            {
                get;
                private set;
            }

            public void Load(Hashtable table)
            {
                grade = table.GetInt("Grade");
                maxExp = table.GetInt("MaxExp");
                maxFarmland = table.GetInt("MaxFarm");
                maxCattery = table.GetInt("MaxCattery");
                maxPower = table.GetInt("MaxPower");
            }
        }

        /// <summary>
        /// 头像信息表
        /// </summary>
        public class HeadInfo
        {
            public enum Type
            {
                PlayerHead = 1,
                CatHead = 2,
            }
            public int id
            {
                get;
                private set;
            }

            public int type
            {
                get;
                private set;
            }

            public string icon
            {
                get;
                private set;
            }

            public bool unlocked
            {
                get;
                private set;
            }

            public void Load(Hashtable table)
            {
                id = table.GetInt("Id");
                type = table.GetInt("Type");
                icon = table.GetString("Icon");
            }
        }

        public class NameInfo
        {
            public int id;
            public int type;

            public void Load(Hashtable table)
            {
                id = table.GetInt("Id");
                type = table.GetInt("Type");
            }
        }

        private static GradeInfo[] _gradeInfos;
        private static Dictionary<int, HeadInfo> _headInfos;
        private static List<NameInfo> _familyNames;
        private static List<NameInfo> _firstNames;

        public GradeInfo GetGradeInfo(int grade)
        {
            if (grade > 0 && _gradeInfos != null && grade <= _gradeInfos.Length)
            {
                return _gradeInfos[grade - 1];
            }
            return null;
        }

        public List<HeadInfo> GetAllHeads()
        {
            return new List<HeadInfo>(_headInfos.Values);
        }

        public HeadInfo GetHeadInfo(int id)
        {
            HeadInfo ret = null;
            if (_headInfos != null)
            {
                _headInfos.TryGetValue(id, out ret);
            }
            return ret;
        }

        public string GetHeadIcon(int id)
        {
            string icon = "";
            HeadInfo head = GetHeadInfo(id);
            if (head != null)
            {
                icon = head.icon;
            }
            return icon;
        }

        public static string GetRandomName()
        {
            var firstString = "";
            if (_firstNames != null && _firstNames.Count > 0)
            {
                int fIndex = UnityEngine.Random.Range(0, _firstNames.Count);
                firstString = KLocalization.GetLocalString(_firstNames[fIndex].id);
            }
            var familyString = "";
            if (_familyNames != null && _familyNames.Count > 0)
            {
                int fIndex = UnityEngine.Random.Range(0, _familyNames.Count);
                familyString = KLocalization.GetLocalString(_familyNames[fIndex].id);
            }
            return firstString + familyString;
        }

        public static void Load(Hashtable table)
        {
            var gradeList = table.GetArrayList("grade");
            if (gradeList != null && gradeList.Count > 0)
            {
                var tmpLT = new Hashtable();

                _gradeInfos = new GradeInfo[gradeList.Count - 1];
                for (int i = 0; i < _gradeInfos.Length; i++)
                {
                    var tmpL0 = (ArrayList)gradeList[0];
                    var tmpLi = (ArrayList)gradeList[i + 1];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpLT[tmpL0[j]] = tmpLi[j];
                    }
                    _gradeInfos[i] = new GradeInfo();
                    _gradeInfos[i].Load(tmpLT);
                }
            }

            var headList = table.GetArrayList("avatar");
            if (headList != null && headList.Count > 0)
            {
                var tmpLT = new Hashtable();

                _headInfos = new Dictionary<int, HeadInfo>(headList.Count - 1);
                for (int i = 1; i < headList.Count; i++)
                {
                    var tmpL0 = (ArrayList)headList[0];
                    var tmpLi = (ArrayList)headList[i];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpLT[tmpL0[j]] = tmpLi[j];
                    }
                    var headInfo = new HeadInfo();
                    headInfo.Load(tmpLT);
                    _headInfos.Add(headInfo.id, headInfo);
                }
            }

            var nameList = table.GetArrayList("name");
            if (nameList != null && nameList.Count > 0)
            {
                var tmpLT = new Hashtable();
                _firstNames = new List<NameInfo>();
                _familyNames = new List<NameInfo>();

                for (int i = 1; i < nameList.Count; i++)
                {
                    var tmpL0 = (ArrayList)nameList[0];
                    var tmpLi = (ArrayList)nameList[i];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpLT[tmpL0[j]] = tmpLi[j];
                    }
                    var nameInfo = new NameInfo();
                    nameInfo.Load(tmpLT);

                    if (nameInfo.type == 1)
                    {
                        _firstNames.Add(nameInfo);
                    }
                    else
                    {
                        _familyNames.Add(nameInfo);
                    }
                }
            }

            var functionList = table.GetArrayList("function");
            if (functionList != null && functionList.Count > 0)
            {
                var tmpLT = new Hashtable();
                for (int i = 1; i < functionList.Count; i++)
                {
                    var tmpL0 = (ArrayList)functionList[0];
                    var tmpLi = (ArrayList)functionList[i];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpLT[tmpL0[j]] = tmpLi[j];
                    }
                    int type = tmpLT.GetInt("Id");
                    int[] unlockTypes = tmpLT.GetArray<int>("UnlockType");
                    int[] unlockParams = tmpLT.GetArray<int>("Parameter");

                    PlayerDataVO._functions[tmpLT.GetInt("Id")] = new Function
                    {
                        type = (FunctionType)type,
                        state = FunctionState.fNone,
                        conditions = unlockTypes ?? new int[0],
                        parameters = unlockParams ?? new int[0],
                    };
                }
            }
        }

        #endregion

        //#region 解锁系统

        //public enum FunctionType : byte
        //{
        //    kNone = 0,
        //    kBag = 1,
        //    kCat = 2,
        //    kShop = 3,
        //    kBuilding = 4,
        //    kHandbook = 5,
        //    kMission = 6,
        //    kFriend = 7,
        //    kDrawCard = 8,
        //    kMatch3 = 9,
        //    kMail = 10,
        //    kActivity = 11,
        //    kRanking = 12,
        //    kMax,
        //}

        //public enum FunctionState : byte
        //{
        //    fNone = 0,
        //    fLock = 1,
        //    fUnlock = 2,
        //    fForce = 4,
        //    fForceLock = 5,
        //    fForceUnlock = 6,
        //}

        //public enum FunctionUnlockType
        //{
        //    kNone = 0,
        //    kGrade = 1,
        //    kLevel = 2,
        //}

        //public class Function
        //{
        //    public FunctionType type;
        //    public FunctionState state;
        //    public int[] conditions;
        //    public int[] parameters;

        //    public void Lock(bool force = false)
        //    {
        //        if (force)
        //        {
        //            state = FunctionState.fForceLock;
        //        }
        //        else
        //        {
        //            if ((state & FunctionState.fForce) == 0)
        //            {
        //                state = FunctionState.fUnlock;
        //            }
        //        }
        //    }

        //    public void Unlock(bool force = false)
        //    {
        //        if (force)
        //        {
        //            state = FunctionState.fForceUnlock;
        //        }
        //        else
        //        {
        //            if ((state & FunctionState.fForce) == 0)
        //            {
        //                state = FunctionState.fUnlock;
        //            }
        //        }
        //    }

        //    public bool IsUnlock()
        //    {
        //        return (state & FunctionState.fUnlock) != 0;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //private static Function[] _functions = new Function[(int)FunctionType.kMax];

        ///// <summary>
        ///// 解锁状态(true 解锁)
        ///// </summary>
        ///// <param name="system"></param>
        ///// <returns></returns>
        //public bool GetFunction(FunctionType type)
        //{
        //    var func = _functions[(int)type];
        //    if (func != null)
        //    {
        //        return func.IsUnlock();
        //    }
        //    return true;
        //}

        //public void RecoverFunction()
        //{
        //    for (int i = _functions.Length - 1; i >= 0; i--)
        //    {
        //        var func = _functions[i];
        //        if (func != null)
        //        {
        //            func.state = FunctionState.fNone;
        //        }
        //    }
        //    UpdateFunction();
        //}

        //public void UnlockFunction(FunctionType type, bool force)
        //{
        //    var func = _functions[(int)type];
        //    if (func != null)
        //    {
        //        func.Unlock(force);
        //    }
        //    UpdateFunction();
        //}

        //public void LockFunction(FunctionType type, bool force)
        //{
        //    var func = _functions[(int)type];
        //    if (func != null)
        //    {
        //        func.Lock(force);
        //    }
        //    UpdateFunction();
        //}

        //public void UpdateFunction()
        //{
        //    for (int i = _functions.Length - 1; i >= 0; i--)
        //    {
        //        var func = _functions[i];
        //        if (func != null && !func.IsUnlock())
        //        {
        //            bool unlock = true;
        //            var conditions = func.conditions;
        //            for (int j = 0; j < conditions.Length; j++)
        //            {
        //                if (conditions[j] == (int)FunctionUnlockType.kGrade)
        //                {
        //                    if (PlayerDataModel.Instance.mPlayerData.mLevel < func.parameters[j])
        //                    {
        //                        unlock = false;
        //                        break;
        //                    }
        //                }
        //                else if (conditions[j] == (int)FunctionUnlockType.kLevel)
        //                {
        //                    if (PlayerDataModel.Instance.mPlayerData.mCurMaxStage < func.parameters[j])
        //                    {
        //                        unlock = false;
        //                        break;
        //                    }
        //                }
        //            }

        //            if (unlock)
        //            {
        //                func.Unlock();
        //            }
        //        }
        //    }
        //}

        //#endregion
    }
}


