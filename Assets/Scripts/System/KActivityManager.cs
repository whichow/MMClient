//namespace Game
//{
//    using Game.DataModel;
//    using System.Collections.Generic;
//    using Callback = System.Action<int, string, object>;

//    public class KActivityManager : KGameModule
//    {

//        private Dictionary<int, KActivity> _activitys = new Dictionary<int, KActivity>();
        

//        public KActivity[] activitys
//        {
//            get
//            {
//                var ret = new KActivity[_activitys.Count];
//                _activitys.Values.CopyTo(ret, 0);
//                return ret;
//            }
//        }

//        public KActivity GetActivity(int id)
//        {
//            KActivity activity;
//            _activitys.TryGetValue(id, out activity);
//            return activity;
//        }

//        public void ActivityGets(Callback callback)
//        {
//            KUser.ActivityGets((code, message, data) =>
//            {
//                if (code == 0)
//                {

//                }
//                if (callback != null)
//                {
//                    callback(code, message, data);
//                    ProcessData(data);
//                }
//            });
//        }

//        public void ActivityGetRewards(int id, int[] values, Callback callback)
//        {
//            KUser.ActivityGetRewards(id, values, (code, message, data) =>
//             {
//                 if (code == 0)
//                 {

//                 }
//                 if (callback != null)
//                 {
//                     callback(code, message, data);
//                 }
//             });
//        }

//        public void ProcessData(object data)
//        {
//            if (data is Msg.ClientMessage.S2CActivityInfosUpdate)
//            {
//                LocalProcess();

//                var originData = (Msg.ClientMessage.S2CActivityInfosUpdate)data;
//                foreach (var item in originData.Activityinfos)
//                {
//                    var activity = GetActivity(item.CfgId);
//                    if (activity != null)
//                    {
//                        switch (item.CfgId)
//                        {
//                            case 1001:
//                                if (item.States.Count > 0)
//                                {
//                                    activity.status = item.States[0];
//                                }
//                                break;
//                            case 2001:
//                                for (int i = 0; i < item.Vals.Count; i++)
//                                {
//                                    activity.subInfos[item.Vals[i] - 1].status = 2;
//                                }
//                                activity.curValue = item.States[2];
//                                activity.subInfos[activity.curValue - 1].status = item.States[0];
//                                break;
//                            case 3001:
//                                for (int i = 0; i < item.States.Count; i++)
//                                {
//                                    var subId = item.States[i];
//                                    var subInfo = System.Array.Find(activity.subInfos, si => si.subID == subId);
//                                    if (subInfo != null)
//                                    {
//                                        subInfo.status = 2;
//                                    }
//                                }
//                                break;
//                            case 4001:
//                                break;
//                            case 5001:
//                                int value1 = 0;
//                                int value2 = 0;
//                                if (item.States.Count > 0)
//                                {
//                                    value1 = item.States[0];
//                                    activity.curValue = item.States[0];
//                                }
//                                if (item.Vals.Count > 0)
//                                {
//                                    value2 = item.Vals[item.Vals.Count - 1];
//                                    for (int i = 0; i < activity.subInfos.Length; i++)
//                                    {
//                                        if (activity.subInfos[i].maxValue <= value1)
//                                        {
//                                            activity.subInfos[i].status = 1;
//                                        }
//                                        if (activity.subInfos[i].maxValue <= value2)
//                                        {
//                                            activity.subInfos[i].status = 2;
//                                        }
//                                    }
//                                }
//                                break;
//                        }
//                    }
//                }
//            }
//        }

//        private void LocalProcess()
//        {
//            var lActivity = GetActivity(3001);
//            if (lActivity != null)
//            {
//                var playerGrade = KUser.SelfPlayer.grade;
//                foreach (var item in lActivity.subInfos)
//                {
//                    item.curValue = playerGrade;
//                    if (playerGrade >= item.maxValue)
//                    {
//                        item.status = 1;
//                    }
//                    else
//                    {
//                        item.status = 0;
//                    }
//                }
//            }
//        }

//        public void Loads()
//        {
//            var activityList = XTable.ActivityXTable.GetAllList();
//            if (activityList != null && activityList.Count > 0)
//            {
//                _activitys.Clear();
//                for (int i = 0; i < activityList.Count; i++)
//                {
//                    var activity = new KActivity
//                    {
//                        id = activityList[i].ID,
//                        type = activityList[i].Type,
//                        nameId = activityList[i].Title,
//                        descriptionId = activityList[i].Description,
//                        rewardInfos = KItem.ItemInfo.FromList(activityList[i].Rewards),
//                    };
//                    _activitys.Add(activity.id, activity);
//                }
//            }

//            var activityDayRewardList = XTable.ActivityDailyRewardXTable.GetAllList();
//            if (activityDayRewardList != null && activityDayRewardList.Count > 0)
//            {
//                var subInfo = new KActivity.SubInfo[activityDayRewardList.Count];
//                for (int i = 0; i < activityDayRewardList.Count; i++)
//                {
//                    subInfo[i] = new KActivity.SubInfo
//                    {
//                        subID = activityDayRewardList[i].ID,
//                        rewardInfos = KItem.ItemInfo.FromList(activityDayRewardList[i].Rewards),
//                    };
//                }
//                GetActivity(2001).subInfos = subInfo;
//            }

//            var activityGradeRewardList = XTable.ActivityGradeRewardXTable.GetAllList();
//            if (activityGradeRewardList != null && activityGradeRewardList.Count > 0)
//            {
//                var subInfos = new KActivity.SubInfo[activityGradeRewardList.Count];
//                for (int i = 0; i < activityGradeRewardList.Count; i++)
//                {
//                    subInfos[i] = new KActivity.SubInfo
//                    {
//                        subID = activityGradeRewardList[i].ID,
//                        maxValue = activityGradeRewardList[i].ID,
//                        rewardInfos = KItem.ItemInfo.FromList(activityGradeRewardList[i].Rewards),
//                    };
//                }
//                GetActivity(3001).subInfos = subInfos;
//            }

//            var activityyAddDayRewardList = XTable.ActivityAddDayRewardXTable.GetAllList();
//            if (activityyAddDayRewardList != null && activityyAddDayRewardList.Count > 0)
//            {
//                var subInfos = new KActivity.SubInfo[activityyAddDayRewardList.Count];
//                for (int i = 0; i < activityyAddDayRewardList.Count; i++)
//                {
//                    subInfos[i] = new KActivity.SubInfo
//                    {
//                        subID = activityyAddDayRewardList[i].ID,
//                        rewardInfos = KItem.ItemInfo.FromList(activityyAddDayRewardList[i].Rewards),
//                        maxValue = activityyAddDayRewardList[i].ID,
//                    };
//                }
//                var local = GetActivity(5001);
//                local.subInfos = subInfos;
//                local.maxValue = subInfos[subInfos.Length - 1].maxValue;
//            }

//        }

//        private void Awake()
//        {
//            Instance = this;
//        }

//        public static KActivityManager Instance;
//    }
//}
