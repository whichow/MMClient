// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 2016-03-30
// ***********************************************************************
namespace Game
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// 转换方法集
    /// </summary>
    public static class KUtils
    {
        #region CONST

        #endregion

        #region ENUM

        #endregion

        #region MODEL

        #endregion

        #region FIELD

        #endregion

        #region PROPERTY

        #endregion

        #region METHOD

        #endregion

        #region STATIC

        private static string _PersistentDataPath;

        public static string PersistentDataPath
        {
            get
            {
                if (_PersistentDataPath.IsNullOrEmpty())
                {
                    _PersistentDataPath = Application.persistentDataPath;
                    if (_PersistentDataPath.IsNullOrEmpty())
                    {
                        //_PersistentDataPath = KPlatform.InternalGetPersistentDataPath();
                    }
                }
                return _PersistentDataPath;
            }
        }

        /// <summary>
        /// add component if not have one
        /// </summary>
        /// <typeparam name="T">Component</typeparam>
        /// <param name="go">target</param>
        /// <param name="checkChild">是否从子物体中获取T</param>
        public static T AddComponentIfNotHave<T>(GameObject go, bool checkChild = false) where T : UnityEngine.Component
        {
            T t = default(T);
            if (checkChild)
            {
                t = go.GetComponentInChildren<T>();
            }
            else
            {
                t = go.GetComponent<T>();
            }
            if (t == null)
            {
                t = (T)go.AddComponent(typeof(T));
            }
            return t;
        }
        /// <summary>
        /// add an element to array by index
        /// </summary>
        /// <param name="index">默认加到数组尾部</param>
        public static void AddElementAt<T>(ref T[] arr, T t, int index = -1) where T : new()
        {
            if (arr == null)
            {
                arr = new T[] { };
            }
            T[] temp = new T[arr.Length + 1];
            if (index == -1)
            {
                index = arr.Length;
            }
            if (index > temp.Length)
            {
                //KLog.LogWarning("insert index out of range");
                return;
            }
            if (t == null)
            {
                t = new T();
            }
            if (temp.Length > 1)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    if (i < index)
                    {
                        temp[i] = arr[i];
                    }
                    else if (i == index)
                    {
                        temp[index] = t;
                    }
                    else
                    {
                        temp[i] = arr[i - 1];
                    }
                }
            }
            arr = temp;
        }

        /// <summary>
        /// 检查特定DateTime和当前是不是差距特定天数
        /// </summary>
        /// <param name="checkDate"></param>
        /// <param name="day"></param>
        /// <param name="afterNow">如果是true，则认为checkDate在当前之后</param>
        /// <returns></returns>
        public static bool CheckDate(System.DateTime checkDate, int day, bool afterNow = false)
        {
            System.DateTime now = System.DateTime.Now;
            double days;
            if (afterNow)
            {
                days = (checkDate - now).TotalDays;
            }
            else
            {
                days = (now - checkDate).TotalDays;
            }
            if (days >= day)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查特定DateTime和当前是不是差距特定秒数
        /// </summary>
        /// <param name="checkMinute"></param>
        /// <param name="second"></param>
        /// <param name="afterNow">如果是true，则认为checkDate在当前之后</param>
        /// <returns></returns>
        public static bool CheckSecond(System.DateTime checkMinute, int second, bool afterNow = false)
        {
            System.DateTime now = System.DateTime.Now;
            double seconds;
            if (afterNow)
            {
                seconds = (checkMinute - now).TotalSeconds;
            }
            else
            {
                seconds = (now - checkMinute).TotalSeconds;
            }
            if (seconds >= second)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 为空或者是""
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 为空或者全是占位符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str) || str.Trim().Length == 0;
        }
        /// <summary>
        /// string转boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string value)
        {
            bool result;
            bool.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// string转float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this string value)
        {
            float result;
            float.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this string value)
        {
            int result;
            int.TryParse(value, out result);
            return result;
        }

        /// <summary>Generates the unique identifier.</summary>
        /// <returns></returns>
        public static string GenerateGUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        /// <summary>Generates the hash.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static uint GenerateHash(string value)
        {
            uint b = 378551;
            uint a = 63689;
            uint hash = 0;
            foreach (var str in value)
            {
                hash = hash * a + str;
                a *= b;
            }
            return hash;
        }
        public static int GetRandomInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }
 
        #endregion
    }
}
