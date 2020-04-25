/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/8 15:32:14
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using Game.UI;
using System.Text;

namespace Game
{
    public static class ExtensionMethod
    {

        #region IList

        public static T[] ToArray<T>(this IList<T> list)
        {
            T[] array = new T[list.Count];
            for (var i = 0; i < list.Count; i++)
            {
                array[i] = list[i];
            }
            return array;
        }

        public static List<T> ToList<T>(this IList<T> list)
        {
            List<T> array = new List<T>(list.Count);
            for (var i = 0; i < list.Count; i++)
            {
                array[i] = list[i];
            }
            return array;
        }

        public static List<T> ToList<T>(this T[] array)
        {
            List<T> list = new List<T>(array.Length);
            for (var i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }
            return list;
        }

        //public static IList<T> RandSort<T>(this RepeatedField<T> list)
        //{
        //    var rand = new Random();
        //    var count = list.Count * 2;
        //    for (var i = 0; i < count; i++)
        //    {
        //        var randomNum = rand.Next(0, list.Count);
        //        var item = list[randomNum];
        //        list.Remove(item);
        //        list.Add(item);
        //    }
        //    return list;
        //}

        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action(list[i]);
            }
        }

        public static T Find<T>(this IList<T> list, Predicate<T> match)
        {
            T val = default(T);
            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    val = list[i];
                    break;
                }
            }
            Debuger.Log("Find待处理");
            return val;
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    list.Add(item);
                }
            }
        }

        #endregion


        public static string ToString<T>(this List<T> list, string format)
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(format);
                }
                sb.Append(list[i].ToString());
            }
            return sb.ToString();
        }

        //public static string ToString(this Vector2 vec)
        //{

        //    vec.ToString();
        //    return array;
        //}

    }
}
