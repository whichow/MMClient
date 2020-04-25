// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
namespace Game
{
    using UnityEngine;

    /// <summary>
    /// 游戏数据状态
    /// </summary>
    public class KDatabase : MonoBehaviour
    {
        #region Const

        public const string ITEM_TABLE_NAME = "item";
        public const string AREA_TABLE_NAME = "area";
        public const string LEVEL_TABLE_NAME = "level";

        #endregion

        #region Property

        /// <summary>
        /// 内存数据库
        /// </summary>
        public static Database Database
        {
            get;
            private set;
        }

        #endregion

        #region UNITY  

        private void Awake()
        {
            LoadDatabase();
        }

        #endregion

        #region ARCHIVE 

        /// <summary>
        /// 
        /// </summary>
        private static void LoadDatabase()
        {
            Database = Database.Create("main");
            var areaSchema = new Schema(AREA_TABLE_NAME, new string[] { "unlock" });
            Database.CreateTable(areaSchema);
            var itemSchema = new Schema(ITEM_TABLE_NAME, new string[] { "count" });
            Database.CreateTable(itemSchema);
            var levelSchema = new Schema(LEVEL_TABLE_NAME, new string[] { "star", "score" });
            Database.CreateTable(levelSchema);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SaveDatabase()
        {
        }

        #endregion

        #region GET SET

        /// <summary>Gets the int.</summary>
        public static int GetInt(int index, string key, int defaultValue = 0)
        {
            if (Database != null)
            {
                return Database.archive.GetInt(index, key, defaultValue);
            }
            return defaultValue;
        }

        /// <summary>Sets the int.</summary>
        public static void SetInt(int index, string key, int value)
        {
            if (Database != null)
            {
                Database.archive.SetInt(index, key, value);
            }
        }

        /// <summary>Gets the string.</summary>
        public static string GetString(int index, string key, string defaultValue = null)
        {
            if (Database != null)
            {
                return Database.archive.GetString(index, key, defaultValue);
            }
            return defaultValue;
        }

        /// <summary>Sets the string.summary>
        public static void SetString(int index, string key, string value)
        {
            if (Database != null)
            {
                Database.archive.SetString(index, key, value);
            }
        }

        #endregion
    }
}
