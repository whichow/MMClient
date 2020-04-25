using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class FrameScheduler
    {
        private int curFrameID;


        private static int MAX_DELAY = 1024;

        private Dictionary<int, List<Action>> delayList;

        public static FrameScheduler instance;

        public FrameScheduler()
        {
            this.curFrameID = 1;
            this.delayList = new Dictionary<int, List<Action>>();
        }

        public static FrameScheduler Instance()
        {
            if (FrameScheduler.instance == null)
            {
                FrameScheduler.instance = new FrameScheduler();
            }
            return FrameScheduler.instance;
        }

        public void Update()
        {
            if (this.delayList.ContainsKey(this.curFrameID))
            {
                for (int i = 0; i < this.delayList[this.curFrameID].Count; i++)
                {
                    this.delayList[this.curFrameID][i]();
                }
                this.delayList.Remove(this.curFrameID);
            }
            this.curFrameID++;
        }

        public void Add(int frameDelayed, Action func)
        {
            if (frameDelayed <= 0)
            {
                func();
                return;
            }
            if (frameDelayed >= FrameScheduler.MAX_DELAY)
            {
                Debug.LogError("Error!Frame delayed is over " + FrameScheduler.MAX_DELAY);
            }
            int key = this.curFrameID + frameDelayed;
            if (!this.delayList.ContainsKey(key))
            {
                List<Action> list = new List<Action>();
                list.Add(func);
                this.delayList.Add(key, list);
            }
            else
            {
                this.delayList[key].Add(func);
            }
        }

        public void Clear()
        {
            this.curFrameID = 1;
            this.delayList = new Dictionary<int, List<Action>>();
        }

        public int GetCurrentFrame()
        {
            return this.curFrameID;
        }



    }
}