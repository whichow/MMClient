// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-11-14
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KActivity" company=""></copyright>
// <summary></summary>
// ***********************************************************************
namespace Game
{
    using System.Collections;

    public class KActivity
    {
        public class SubInfo
        {
            /// <summary>
            /// 子活动ID
            /// </summary>
            public int subID;
            /// <summary>
            /// 子活动描述
            /// </summary>
            public string description;
            /// <summary>
            /// 子活动目标图标
            /// </summary>
            public string iconName;
            /// <summary>
            /// 子活动当前完成数量
            /// </summary>
            public int curValue;
            /// <summary>
            /// 子活动最大完成数量
            /// </summary>
            public int maxValue;
            /// <summary>
            /// 奖励
            /// </summary>
            public KItem.ItemInfo[] rewardInfos;
            /// <summary>
            /// 标签类型:0.正在进行;1.已经结束;2.还未开启
            /// </summary>
            public int tagState;
            /// <summary>
            /// 子活动状态:0.未完成;1.已完成;2.已领取
            /// </summary>
            public int status;
        }

        /// <summary>
        /// 活动ID
        /// </summary>
        public int id;
        /// <summary>
        /// 
        /// </summary>
        public int type;
        /// <summary>
        /// 
        /// </summary>
        public int nameId;
        /// <summary>
        /// 
        /// </summary>
        public int descriptionId;
        /// <summary>
        /// 图片
        /// </summary>
        public string iconName;
        /// <summary>
        /// 标签类型
        /// </summary>
        public int flagState;
        /// <summary>
        /// 活动名称
        /// </summary>
        public string displayName
        {
            get { return KLocalization.GetLocalString(nameId); }
        }
        /// <summary>
        /// 活动描述
        /// </summary>
        public string description
        {
            get { return KLocalization.GetLocalString(descriptionId); }
        }
        /// <summary>
        /// 活动当前完成数量
        /// </summary>
        public int curValue;
        /// <summary>
        /// 活动最大完成数量
        /// </summary>
        public int maxValue;
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public int startTimeStamp;
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public int endTimeStamp;
        /// <summary>
        /// 活动剩余时间
        /// </summary>
        public int leftTimeStamp;
        /// <summary>
        /// tips状态:0.关闭;1.开启
        /// </summary>
        public int tipsStatus;
        /// <summary>
        /// 活动子列表
        /// </summary>
        public SubInfo[] subInfos;
        /// <summary>
        /// 奖励
        /// </summary>
        public KItem.ItemInfo[] rewardInfos;
        /// <summary>
        /// 活动状态:0.未完成;1.已完成;2.已领取
        /// </summary>
        public int status;
    }
}
