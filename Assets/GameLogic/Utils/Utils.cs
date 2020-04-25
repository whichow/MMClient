/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/4 18:56:22
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;

namespace Game
{
    public class Utils
    {
        /// <summary>
        /// DateTime时间格式转换为13位带毫秒的Unix时间戳
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static long ConvertDateTimeLong(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }

        /// <summary>
        /// DateTime时间格式转换为10位不带毫秒的Unix时间戳
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 时间戳转换成时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="format"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public static string GetTime(int time, string format = "yyyy-MM-dd HH:mm", params int[] startTime)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 1, 1).AddSeconds(time)).ToString(format);
        }

        /// <summary>
        /// 转换成 天、小时
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetCountTime(int time)
        {
            string timeStr = "";
            if (time >= 86400)
            {
                timeStr += (time / 86400).ToString("#00") + KLocalization.GetLocalString(54189);
                timeStr += (time % 86400 / 3600).ToString("#00") + KLocalization.GetLocalString(54190);
            }
            else
            {
                timeStr += (time / 3600).ToString("#00") + KLocalization.GetLocalString(54190);
            }
            return timeStr;
        }

        public static string GetCountTimes(int time)
        {
            string timeStr = "";
            if (time>=3600)
            {
                timeStr += (time / 3600).ToString("#00") + ":";
                timeStr += (time % 3600 / 60).ToString("#00") + ":";
                timeStr += (time % 60).ToString("#00");
            }
            else
            {
                timeStr += (time / 60).ToString("#00") + ":";
                timeStr += (time % 60).ToString("#00");
            }
            return timeStr;
        }
    }
}
