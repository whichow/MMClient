// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KItemCrop" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;

namespace Game
{
    /// <summary>
    /// 植物
    /// </summary>
    public class KItemCrop : KItem
    {
        public string[] animations
        {
            get;
            private set;
        }

        public int unlockGrade
        {
            get;
            private set;
        }

        public int output
        {
            get;
            private set;
        }

        public int Exe
        {
            get;
            private set;
        }

        public int[] produceTimes
        {
            get;
            private set;
        }

        public string GetAnimation(int step)
        {
            if (animations != null && step < animations.Length)
            {
                return animations[step];
            }
            return null;
        }

        public int GetTotalTime()
        {
            if (produceTimes != null && produceTimes.Length > 0)
            {
                return produceTimes[produceTimes.Length - 1];
            }
            return 1;
        }

        public int GetStep(int remainTime)
        {
            if (produceTimes == null || produceTimes.Length == 0)
            {
                return -1;
            }

            int step = produceTimes.Length;
            int time = produceTimes[step - 1] - remainTime;
            for (int i = 0; i < step; i++)
            {
                if (time < produceTimes[i])
                {
                    return i;
                }
            }
            return step;
        }

        public string GetStepAnimation(int remainTime)
        {
            if (produceTimes == null || produceTimes.Length == 0 || animations == null || animations.Length == 0)
            {
                return null;
            }

            int step = produceTimes.Length;
            int time = produceTimes[step - 1] - remainTime;
            for (int i = 0; i < step; i++)
            {
                if (time < produceTimes[i])
                {
                    step = i;
                    break;
                }
            }

            if (step < animations.Length)
            {
                return animations[step];
            }

            return null;
        }

        public bool GetStep(int remainTime, out int step, out string animation)
        {
            if (produceTimes == null || produceTimes.Length == 0 || animations == null || animations.Length == 0)
            {
                step = -1;
                animation = null;
                return false;
            }

            step = produceTimes.Length;
            int time = produceTimes[step - 1] - remainTime;
            for (int i = 0; i < step; i++)
            {
                if (time < produceTimes[i])
                {
                    step = i;
                    break;
                }
            }

            animation = null;
            if (step < animations.Length)
            {
                animation = animations[step];
            }
            return true;
        }

        public override void Load(Hashtable table)
        {
            base.Load(table);

            animations = table.GetArray<string>("Anim");
            unlockGrade = table.GetInt("Level");
            output = table.GetInt("OutPut");
            produceTimes = table.GetArray<int>("Time");
            Exe = table.GetInt("Exp");
        }
    }
}

