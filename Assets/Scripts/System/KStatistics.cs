// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using UnityEngine;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 数据统计管理类
    /// </summary>
    public class KStatistics : MonoBehaviour
    {
        #region ENUM
        public enum EventID
        {

        }
        #endregion

        #region METHOD
        /// <summary>
        /// 数据统计
        /// </summary>
        /// <param name="eventID">id</param>
        /// <param name="eventLabel">label</param>
        /// <param name="eventCount">count</param>
        /// <param name="eventInfo">info</param>
        public void Statistics(EventID eventID, string eventLabel, int eventCount, Hashtable eventInfo = null)
        {
            if (KPlatform.Instance != null)
            {
                KPlatform.Instance.Statistic((int)eventID, eventLabel, eventCount, eventInfo);
            }
        }
        #endregion

        #region UNITY
        private void Awake()
        {
            _Instance = this;
        }
        #endregion

        #region STATIC
        private static KStatistics _Instance;
        public static KStatistics Instance { get { return _Instance; } }
        #endregion
    }
}