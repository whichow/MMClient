/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/29 15:36:23
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    #region 计时器

    public class SNTimer
    {
        #region static

        private static SNTimer m_instance = null;
        public static SNTimer Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new SNTimer();
                }
                return m_instance;
            }
        }

        public static void ClearTimer(SgTimer timer)
        {
            SNTimer.Instance.RemoveTimer(timer);
        }

        /// <summary>
        /// 间隔执行
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        /// <param name="interval">间隔时间 毫秒</param>
        /// <param name="count">执行次数 无限填0</param>
        /// <returns></returns>
        public static SgTimer SetTimeInterval(TimeEvent callback, object args, uint interval, uint count)
        {
            if (interval < m_timerInterval)
            {
                throw new Exception("SetTimeInterval： interval 不能小于计时器最小时间 " + m_timerInterval);
            }
            SgTimer t = new SgTimer(callback, args, interval, count);
            SNTimer.Instance.AddTimer(t);
            return t;
        }
        public static SgTimer SetTimeInterval(TimeEvent callback, uint interval, uint count)
        {
            return SetTimeInterval(callback, null, interval, count);
        }
        public static SgTimer SetTimeInterval(TimeEvent callback, uint interval)
        {
            return SetTimeInterval(callback, null, interval, 0);
        }

        /// <summary>
        /// 超时执行
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        /// <param name="timeout">超时时间 毫秒</param>
        /// <returns></returns>
        public static SgTimer SetTimeOut(TimeEvent callback, object args, uint timeout)
        {
            SgTimer t = new SgTimer(callback, args, timeout);
            SNTimer.Instance.AddTimer(t);
            return t;
        }
        public static SgTimer SetTimeOut(TimeEvent callback, uint timeout)
        {
            return SetTimeOut(callback, null, timeout);
        }


        /// <summary>
        /// 定时执行
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        /// <param name="fixedTime">定时钟</param>
        /// <returns></returns>
        public static SgTimer SetTimeFixed(TimeEvent callback, object args, Clock fixedTime)
        {
            SgTimer t = new SgTimer(callback, args, fixedTime);
            SNTimer.Instance.AddTimer(t);
            return t;
        }

        #endregion

        public SNTimer()
        {
            if (m_instance != null)
            {
                throw new Exception("Cannot be instantiated!");
            }
            Start();
        }

        private void Game_OnUpdate()
        {
            timeNow = (uint)(Time.realtimeSinceStartup * 1000);
            if (timeNow > lastInterval + m_timerInterval)
            {
                Timer_Update(timeNow, DateTime.Now);
                lastInterval = timeNow;
            }
        }

        public void Start()
        {
            GameApp.Instance.OnUpdateEvent += Game_OnUpdate;
        }

        public void Stop()
        {
            GameApp.Instance.OnUpdateEvent -= Game_OnUpdate;
        }

        public void Clear()
        {
            m_list.Clear();
            m_delList.Clear();
            m_temp = null;
        }

        public void AddTimer(SgTimer t)
        {
            if (!m_list.Contains(t)) m_list.Add(t);
        }

        public void RemoveTimer(SgTimer timer)
        {
            m_delList.Add(timer);
        }

        protected void Timer_Update(uint realtimeSinceStartup, DateTime nowDateTime)
        {
            m_listLen = m_delList.Count;
            for (int i = 0; i < m_listLen; i++)
            {
                m_list.Remove(m_delList[i]);
            }
            m_delList.Clear();

            m_listLen = m_list.Count;
            for (int i = 0; i < m_listLen; i++)
            {
                m_temp = m_list[i];
                if (m_temp.Type == ETimerType.TimeInterval)
                {
                    if ((int)((realtimeSinceStartup - m_temp.StartTimeU3D) / m_temp.Interval) > m_temp.CurCount)
                    {
                        m_temp.Do();
                    }
                    if (m_temp.Count > 0 && m_temp.CurCount >= m_temp.Count)
                    {
                        m_delList.Add(m_temp);
                        continue;
                    }
                }
                else if (m_temp.Type == ETimerType.TimerOut)
                {
                    if ((realtimeSinceStartup - m_temp.StartTimeU3D) >= m_temp.Timeout)
                    {
                        m_delList.Add(m_temp);
                        m_temp.Do();
                        continue;
                    }
                }
                else if (m_temp.Type == ETimerType.TimerFixed)
                {
                    //定时执行  如果 日、时、分、秒 等于某个值就开始执行
                    if ((m_temp.Clock.Day == -1 || m_temp.Clock.Day == nowDateTime.Day) &&
                        (m_temp.Clock.Hour == -1 || m_temp.Clock.Hour == nowDateTime.Hour) &&
                        (m_temp.Clock.Minute == -1 || m_temp.Clock.Minute == nowDateTime.Minute) &&
                        (m_temp.Clock.Second == -1 || m_temp.Clock.Second == nowDateTime.Second) && nowDateTime.Millisecond <= m_timerInterval)
                        m_temp.Do();
                }
            }
        }


        protected int m_listLen;
        protected SgTimer m_temp;

        protected List<SgTimer> m_list = new List<SgTimer>();
        protected List<SgTimer> m_delList = new List<SgTimer>();

        protected const int m_timerInterval = 10; // Timer执行间隔时间,单位为毫秒

        private uint lastInterval;
        private uint timeNow;
    }

    //public delegate void TimeEvent(SgTimer timer);
    public delegate void TimeEvent(object args, uint count);


    public class SgTimer
    {
        private ETimerType m_type;
        private TimeEvent m_callback;                                     //回调
        private object m_args;                                         //参数
        private uint m_interval;                                     //间隔执行时间 毫秒
        private uint m_count;                                        //间隔执行次数
        private uint m_timeout;                                      //超时执行 毫秒
        private Clock m_clock;                                        //定时执行 日时分秒
        private uint m_startTimeU3D;                                 //开始时间 毫秒
        private uint m_curCount = 0;                                 //当前执行次数

        public ETimerType Type { get { return m_type; } }
        public TimeEvent Callback { get { return m_callback; } }
        public object Args { get { return m_args; } }
        public uint Interval { get { return m_interval; } }
        public uint Count { get { return m_count; } }
        public uint Timeout { get { return m_timeout; } }
        public Clock Clock { get { return m_clock; } }
        public uint StartTimeU3D { get { return m_startTimeU3D; } }
        public uint CurCount { get { return m_curCount; } }
        public int RemainCount { get { return (int)(m_count - m_curCount); } } // 计数剩余次数

        public SgTimer(TimeEvent callback, object args, uint interval, uint count)
        {
            m_type = ETimerType.TimeInterval;
            m_callback = callback;
            m_args = args;
            m_interval = interval;
            m_count = count;
            m_startTimeU3D = (uint)(Time.realtimeSinceStartup * 1000);
        }

        public SgTimer(TimeEvent callback, object args, uint timeout)
        {
            m_type = ETimerType.TimerOut;
            m_callback = callback;
            m_args = args;
            m_timeout = timeout;
            m_startTimeU3D = (uint)(Time.realtimeSinceStartup * 1000);
        }

        public SgTimer(TimeEvent callback, object args, Clock fixedTime)
        {
            m_type = ETimerType.TimerFixed;
            m_callback = callback;
            m_args = args;
            m_clock = fixedTime;
            m_startTimeU3D = (uint)(Time.realtimeSinceStartup * 1000);
        }

        public void Start()
        {
            if (m_callback != null)
            {
                SNTimer.Instance.AddTimer(this);
            }
        }

        public void Stop()
        {
            SNTimer.Instance.RemoveTimer(this);
        }

        public void Do()
        {
            m_curCount++;
            try
            {
                if (m_callback != null)
                {
                    m_callback(m_args, m_curCount);
                    //m_callback(this);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("[SgTimer.Do] " + m_callback.Method.DeclaringType + "." + m_callback.Method.Name + " - " + m_callback.Target.ToString() + "\n" + e.ToString());
            }
        }

        public void Dispose()
        {
            Stop();
            m_callback = null;
            m_args = null;
        }

    }


    public struct Clock
    {
        public int Day;
        public int Hour;
        public int Minute;
        public int Second;

        /// <summary>
        /// 定时
        /// </summary>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        public Clock(int day, int hour, int minute, int second)
        {
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        /// <summary>
        /// 定时
        /// </summary>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        public Clock(int hour, int minute, int second)
        {
            Day = -1;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        /// <summary>
        /// 定时
        /// </summary>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        public Clock(int minute, int second)
        {
            Day = -1;
            Hour = -1;
            Minute = minute;
            Second = second;
        }

        /// <summary>
        /// 定时
        /// </summary>
        /// <param name="second">秒</param>
        public Clock(int second)
        {
            Day = -1;
            Hour = -1;
            Minute = -1;
            Second = second;
        }
    }


    public enum ETimerType
    {
        TimeInterval = 1,
        TimerOut = 2,
        TimerFixed = 3,
    }

    #endregion
}
