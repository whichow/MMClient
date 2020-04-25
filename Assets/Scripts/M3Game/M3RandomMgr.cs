#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： M3RandomMgr
     * 创建人	： roy
     * 创建时间	： 12/7/2017 10:21:05 AM
     * 描述  	：   	
*/
#endregion

using System;

namespace Game.Match3
{
    public class M3RandomMgr : Singleton<M3RandomMgr>
    {
        public bool mBlAIRunning { get; private set; }
        private int[] mRdmIntPool;
        public const int MaxSize = 2000;
        private int _step = 0;
        public int mRandomSeed { get; private set; }

        public void InitRandomSeed(int seed)
        {
            _step = 0;
            mBlAIRunning = true;
            Random rdm = new Random(seed);
            rdm.Next();
            mRdmIntPool = new int[MaxSize];
            mRandomSeed = seed;
            for (int i = 0; i < mRdmIntPool.Length; i++)
                mRdmIntPool[i] = rdm.Next();
        }

        public void ResetStep(int step)
        {
            _step = step;
        }

        public int RandStep
        {
            get { return _step; }
        }

        public int GetRandomInt(int min, int max)
        {
            if (!mBlAIRunning)
            {
                //InitRandomSeed(303);
                return UnityEngine.Random.Range(min, max);
            }
            if (_step >= MaxSize)
                _step = 0; //测试防止报错
            int result = mRdmIntPool[_step] % (max - min) + min;
            _step++;
            UnityEngine.Debug.LogError("_step:" + _step);
            return result;
        }
    }
}