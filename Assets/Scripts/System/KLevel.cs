// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
namespace Game
{
    using Game.DataModel;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Chapter
    /// </summary>
    //public class ChapterUnlockXDM
    //{
    //    #region FIELD 

    //    private int _curStar = -1;
    //    private LevelXDM[] _allLevels;

    //    #endregion

    //    #region PROPERTY

    //    /// <summary>Gets the chapter identifier.</summary>
    //    /// <value>The chapter identifier.</value>
    //    public int ID
    //    {
    //        get;
    //        private set;
    //    }

    //    /// <summary>Gets the name of the chapter.</summary>
    //    /// <value>The name of the chapter.</value>
    //    public string chapterName
    //    {
    //        get;
    //        private set;
    //    }

    //    /// <summary>Gets the display name.</summary>
    //    /// <value>The display name.</value>
    //    public string displayName
    //    {
    //        get;
    //        private set;
    //    }

    //    /// <summary>Gets the description.</summary>
    //    /// <value>The description.</value>
    //    public string description
    //    {
    //        get;
    //        private set;
    //    }

    //    /// <summary>Gets the name of the icon.</summary>
    //    /// <value>The name of the icon.</value>
    //    public string Icon
    //    {
    //        get;
    //        private set;
    //    }
    //    public string UIPrefabName
    //    {
    //        get;
    //        private set;
    //    }

    //    public int UnlockChapter
    //    {
    //        get;
    //        private set;
    //    }
    //    /// <summary>Gets all levels.</summary>
    //    /// <value>All levels.</value>
    //    public LevelXDM[] allLevels
    //    {
    //        get
    //        {
    //            return _allLevels;
    //        }
    //    }
    //    public LevelXDM lastLevel
    //    {
    //        get {
    //            return allLevels[allLevels.Length - 1];
    //        }
    //    }
    //    /// <summary>Gets a value indicating whether this <see cref="LevelXDM"/> is unlocked.</summary>
    //    public bool unlocked
    //    {
    //        get
    //        {
    //            return allLevels[0].ID < KLevelManager.Instance.currUnlockMaxLevelId;
    //        }
    //        //set
    //        //{
    //        //    unlocked = value;
    //        //}
    //    }

    //    public int curStar
    //    {
    //        get { return _curStar; }
    //    }

    //    public int maxStar
    //    {
    //        get { return _allLevels.Length * 3; }
    //    }

    //    public int UnlockStarNum { get; private set; }
    //    public int UnlockTime { get; private set; }

    //    #endregion

    //    public void Load(Hashtable table)
    //    {
    //        this.ID = table.GetInt("ChapterId");
    //        //this.chapterName = table.GetString("Name");
    //        //this.displayName = table.GetString("Name");
    //        this.description = table.GetString("Description");
    //        this.Icon = table.GetString("Icon");
    //        this.UIPrefabName = table.GetString("ModelName");
    //        this.UnlockStarNum = table.GetInt("UnlockStarNum");
    //        this.UnlockTime = table.GetInt("UnlockTime");
    //        this.UnlockChapter = table.GetInt("UnlockChapter");
    //        _allLevels = System.Array.FindAll(KLevelManager.Instance.allLevels, l => l.ChapterID == ID);
    //    }

    //    public void From(Hashtable table)
    //    {
    //    }

    //    public void Clean()
    //    {
    //    }
    //}

    /// <summary>
    /// Level
    /// </summary>
    //public class LevelXDM
    //{
    //    #region PROPERTY

    //    /// <summary>The level identifier.唯一关卡ID.</summary>
    //    public int ID
    //    {
    //        get;
    //        private set;
    //    }

    //    /// <summary>Gets the chapter identifier.所属地图ID.</summary>
    //    public int ChapterID
    //    {
    //        get;
    //        private set;
    //    }
    //    public int LevelIndex
    //    {
    //        get;
    //        private set;
    //    }
    //    /// <summary>The level name ex. "level 1" ,"level 20" .</summary>
    //    public string Name
    //    {
    //        get;
    //        private set;
    //    }

    //    /// <summary>The description.显示的描述.</summary>
    //    public string description
    //    {
    //        get;
    //        private set;
    //    }

    //    public int[] IgnoreCatID
    //    {
    //        get;
    //        private set;
    //    }

    //    /// <summary>
    //    /// 消耗体力
    //    /// </summary>
    //    public int NeedPower
    //    {
    //        get;
    //        private set;
    //    }
    //    public string ChessboardID
    //    {
    //        get;
    //        private set;
    //    }
    //    /// <summary>Gets a value indicating whether this <see cref="LevelXDM"/> is unlocked.</summary>
    //    public bool unlocked
    //    {
    //        get;
    //        set;
    //    }
    //    public bool isFinish
    //    {
    //        get {
    //            return currStar > 0;
    //        }
    //    }
    //    public int CoinReward
    //    {
    //        get;
    //        set;
    //    }
    //    public int CatChoiceLevel
    //    {
    //        get;
    //        private set;
    //    }
    //    public int CatChoiceStar
    //    {
    //        get;
    //        private set;
    //    }
    //    public int[] ChooseProp;
    //    public Int2 Position
    //    {
    //        get;
    //        private set;
    //    }
    //    /// <summary>Gets a value indicating whether this <see cref="LevelXDM"/> is completed.</summary>
    //    public bool completed
    //    {
    //        get { return currScore > 0; }
    //    }

    //    /// <summary>Gets the current star.</summary>
    //    public int currStar
    //    {
    //        get { return KDatabase.GetInt(ID, "star"); }
    //        set
    //        {
    //            var star = KDatabase.GetInt(ID, "star");
    //            if (value > star)
    //            {
    //                KDatabase.SetInt(ID, "star", value);
    //            }
    //        }
    //    }

    //    /// <summary>Gets the curr score.</summary>
    //    public int currScore
    //    {
    //        get { return KDatabase.GetInt(ID, "score", -1); }
    //        set
    //        {
    //            var score = KDatabase.GetInt(ID, "score");
    //            if (value > score)
    //            {
    //                KDatabase.SetInt(ID, "score", value);
    //            }
    //        }
    //    }

    //    public int NextLevelID;
    //    public int[] ChooseShopId;

    //    public int[] BattleProp;
    //    public int[] BattleShopId;
    //    #endregion

    //    #region METHOD  



    //    /// <summary>Froms the specified table.</summary>
    //    public void From(Hashtable table)
    //    {
    //    }

    //    /// <summary>Loads the specified table.</summary>
    //    public void Load(Hashtable table)
    //    {
    //        this.ID = table.GetInt("ID");
    //        this.Name = table.GetString("Name");
    //        this.ChapterID = table.GetInt("ChapterID");
    //        this.LevelIndex = table.GetInt("LevelIndex");
    //        this.ChessboardID = table.GetString("ChessboardID", "null");
    //        if (this.ChessboardID == "null")
    //        {
    //            ChessboardID = table.GetInt("ChessboardID").ToString();
    //        }

    //        this.Position = new Int2((int)(table.GetArrayList("Position")[0]), (int)(table.GetArrayList("Position")[1]));
    //        this.NeedPower = table.GetInt("NeedPower");
    //        this.IgnoreCatID = table.GetArray<int>("IgnoreCatID");
    //        this.CatChoiceLevel = table.GetInt("CatChoiceLevel");
    //        this.CatChoiceStar = table.GetInt("CatChoiceStar");
    //        this.ChooseProp = table.GetArray<int>("ChooseProp");
    //        this.ChooseShopId = table.GetArray<int>("ChooseShopId");

    //        this.BattleProp = table.GetArray<int>("BattleProp");
    //        this.BattleShopId = table.GetArray<int>("BattleShopId");
    //        this.NextLevelID = table.GetInt("NextLevelID");
    //        this.CoinReward = table.GetInt("CoinReward");
    //        this.description = table.GetString("Description");//
    //    }

    //    /// <summary>Resets this instance.</summary>
    //    public void Clean()
    //    {

    //    }

    //    #endregion

    //    #region UNITY

    //    #endregion

    //    #region STATIC


    //    #endregion
    //}
}

