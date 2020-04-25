// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 2016-10-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;

namespace K.Extension
{
    /// <summary>
    /// 时间扩展
    /// </summary>
    public static class TimeExtension
    {
        public static readonly DateTime UnixUtcDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime UnixLocalDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToDataTime(this int timestamp)
        {
            return UnixUtcDateTime.AddSeconds(timestamp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int ToUnixTime(this DateTime dateTime)
        {
            return (int)((dateTime - UnixUtcDateTime).Ticks / TimeSpan.TicksPerSecond);
        }

        public static string ToTimeString(this int second)
        {
            var ts = TimeSpan.FromSeconds(second);
            string ret = "";

            if (ts.Days > 0)
            {
                ret += ts.Days + "日";
            }

            if (ts.Hours > 0)
            {
                ret += ts.Hours + "时";
            }

            if (ts.Minutes > 0)
            {
                ret += ts.Minutes + "分";
            }

            if (ts.Seconds > 0)
            {
                ret += ts.Seconds + "秒";
            }

            return ret;
        }
    }
}
