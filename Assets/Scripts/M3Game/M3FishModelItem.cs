/** 
 *FileName:     M3FishModelItem.cs 
 *Author:       HASEE 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-10-31 
 *Description:    
 *History: 
*/
using System.Collections.Generic;

namespace Game.Match3
{
    public class M3FishModelItem
    {

        public int needPrevious;//是否需要前面的鱼掉落完

        public int needScore;//是否需要规定分数

        public int needStep;//是否需要规定步数

        public int needTime;//是否需要规定时间

        public int isRandom;//是否随机掉落

        public int combineMode;//金豆荚掉落模式（预留）

        public List<Int2> spawnList;//掉落口

        public int type;//金豆荚的类型（预留）

        public bool previousFlag;


        public M3FishModelItem CreateInfo()
        {
            var tmpList = new List<Int2>();
            for (int i = 0; i < this.spawnList.Count; i++)
            {
                tmpList.Add(new Int2(spawnList[i].x, spawnList[i].y));
            }
            return new M3FishModelItem()
            {
                needPrevious = this.needPrevious,
                needScore = this.needScore,
                needStep = this.needStep,
                needTime = this.needTime,
                isRandom = this.isRandom,
                combineMode = this.combineMode,
                spawnList = tmpList,
                previousFlag = this.previousFlag,
                type = this.type,
            };
        }
    }
}