// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KExpolorManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    using Callback = System.Action<int, string, object>;

    /// <summary>
    /// 
    /// </summary>
    public class KExplore : KGameModule
    {
        #region Struct

        public class Condition
        {
            public enum Type
            {
                CatGrade = 1, // 猫咪等级条件
                CatRarity = 2, // 猫咪品阶条件
                CatStar = 3, // 猫咪星阶条件
                CatColor = 4, // 猫咪颜色条件
                CatCount = 5, // 猫咪数量条件
            }

            private List<object> _values = new List<object>();

            public int type;
            public string description;

            public List<object> values
            {
                get { return _values; }
            }

            public object[] GetFormatValues()
            {
                var ret = new object[_values.Count + 3];
                int i = 0;
                for (i = 0; i < _values.Count; i++)
                {
                    if (type == (int)Type.CatColor)
                    {
                        ret[i] = CatUtils.GetCatColorString((int)_values[i]);
                    }
                    else
                    {
                        ret[i] = _values[i];
                    }

                }
                ret[i++] = "";
                return ret;
            }
        }

        public class Event
        {
            private int _triggerTimestamp;

            public int id;
            public string[] talks;
            public string talk
            {
                get { return Instance.GetEventConfig(id).GetTalk(); }
            }
            public int triggerTime
            {
                get { return _triggerTimestamp - KLaunch.Timestamp; }
                set { _triggerTimestamp = KLaunch.Timestamp + value; }
            }

            public KItem.ItemInfo[] reward;

            public string showTime
            {
                get { return K.Extension.TimeExtension.ToDataTime(_triggerTimestamp).ToLocalTime().ToString("HH:mm"); }
            }

            public bool isShow
            {
                get;
                set;
            }

            public Event Copy()
            {
                var ret = new Event
                {
                    id = this.id,
                    talks = this.talks,
                    triggerTime = 0
                };
                return ret;
            }
        }

        public class Task
        {
            public enum Type
            {
                Daily = 1,//日常
                Limit = 2,//限时
            }

            public enum State
            {
                None = 0,
                Start = 1,
                Success = 2,
                Failure = 3,
            }

            public enum Result
            {
                Failure = 0,
                Success = 1,
            }

            private int _taskPassTimestamp;
            private int _taskRemainTimestamp;
            private int _exploreRemainTimestamp;
            private List<int> _catIds = new List<int>();
            private List<Event> _specialEvents = new List<Event>();
            private List<Condition> _catConditions = new List<Condition>();

            public string name;
            public int id;
            public int configId;
            public int result; // 探索结果 1成功 0失败
            public int state;

            public int type;
            public int totalTime;
            public int[] rewards;
            public int successChance;
            public KItem.ItemInfo savePrice;

            public Event startEvent;
            public Event processEvent;
            public Event finishEvent;

            public Event GetStartEventCopy()
            {
                return startEvent.Copy();
            }

            public Event GetProcessEventCopy()
            {
                return processEvent.Copy();
            }

            public Event GetFinishEventCopy()
            {
                return finishEvent.Copy();
            }

            public List<int> catIds
            {
                get { return _catIds; }
            }

            /// <summary>
            /// 
            /// </summary>
            public List<Condition> catConditions
            {
                get { return _catConditions; }
            }

            /// <summary>
            /// 
            /// </summary>
            public List<Event> specialEvents
            {
                get { return _specialEvents; }
            }

            /// <summary>
            /// 探索剩余时间
            /// </summary>
            public int exploreRemainTime
            {
                get { return _exploreRemainTimestamp - KLaunch.Timestamp; }
                set
                {
                    _exploreRemainTimestamp = KLaunch.Timestamp + value;
                }
            }

            /// <summary>
            /// 任务剩余时间
            /// </summary>
            public int taskRemainTime
            {
                get { return _taskRemainTimestamp - KLaunch.Timestamp; }
                set
                {
                    _taskRemainTimestamp = KLaunch.Timestamp + value;
                }
            }

            /// <summary>
            /// 任务过去多少时间
            /// </summary>
            public int taskPassTime
            {
                get { return KLaunch.Timestamp - _taskRemainTimestamp; }
                set
                {
                    _taskPassTimestamp = KLaunch.Timestamp - value;
                }
            }

            public List<KItem.ItemInfo> specialReward
            {
                get
                {
                    var list = new List<KItem.ItemInfo>();
                    foreach (var enent in _specialEvents)
                    {
                        if (enent.triggerTime <= 0)
                        {
                            foreach (var itemInfo in enent.reward)
                            {
                                bool flag = false;
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var tmp = list[i];
                                    if (tmp.itemID == itemInfo.itemID)
                                    {
                                        tmp.itemCount += itemInfo.itemCount;
                                        list[i] = tmp;
                                        flag = true;
                                    }
                                }
                                if (!flag)
                                {
                                    list.Add(itemInfo);
                                }
                            }
                        }
                    }
                    return list;
                }
            }

            /// <summary>
            /// 成功率百分数
            /// </summary>
            /// <returns></returns>
            public int CalcTaskSuccessChance(List<CatDataVO> cats)
            {
                float exploreAbility = 0;
                if (cats != null && cats.Count > 0)
                {
                    for (int i = 0; i < cats.Count; i++)
                    {
                        if (cats[i] != null)
                        {
                            exploreAbility += cats[i].mCatInfo.ExploreAbility;
                        }
                    }
                }
                return Mathf.Min((int)(exploreAbility * successChance * 0.01f), 100);
            }

            /// <summary>
            /// 奖励倍数
            /// </summary>
            /// <param name="cats"></param>
            /// <returns></returns>
            public int CalcTaskRewardMultiple(List<CatDataVO> cats)
            {
                int count = 0;
                if (cats != null && cats.Count > 0)
                {
                    for (int i = 0; i < cats.Count; i++)
                    {
                        if (cats[i] != null)
                        {
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        return 0;
                    }
                    count = 100 + (count - 1) * 50;
                }
                return count;
            }
        }

        private class TaskConfig
        {
            public int id;
            public int type;//1=日常 2=限时
            //public int difficulty = 0;
            public int totalTime;
            public int startEventId;
            public int processEventId;
            public int finishEventId;
            public int[] rewards;
            public int successChance;
            public int[] savePrice;
            public int nameId;
        }

        private class EventConfig
        {
            public int id;
            public int[] talkIds;

            public string[] GetTalks()
            {
                if (talkIds != null && talkIds.Length > 0)
                {
                    int tLength = talkIds.Length / 2;
                    var ret = new string[tLength];
                    for (int i = 0; i < tLength; i++)
                    {
                        ret[i] = KLocalization.GetLocalString(talkIds[i + i]);
                    }
                    return ret;
                }
                return new string[0];
            }

            public string GetTalk()
            {
                if (talkIds != null && talkIds.Length > 0)
                {
                    int tLength = talkIds.Length / 2;

                    int tWeight = 0;
                    for (int i = 0; i < tLength; i++)
                    {
                        tWeight += talkIds[i + i + 1];
                    }
                    var cWeight = Random.Range(0, tWeight);
                    for (int i = 0; i < tLength; i++)
                    {
                        cWeight -= talkIds[i + i + 1];
                        if (cWeight <= 0)
                        {
                            return KLocalization.GetLocalString(talkIds[i + i]);
                        }
                    }
                }
                return string.Empty;
            }
        }

        private class ConditionConfig
        {
            public int id;
            public int descriptionId;

            public string GetDescription()
            {
                return KLocalization.GetLocalString(descriptionId);
            }
        }

        #endregion

        #region Field

        private List<Task> _allTask = new List<Task>();

        private Dictionary<int, TaskConfig> _taskConfigDictionary = new Dictionary<int, TaskConfig>();
        private Dictionary<int, EventConfig> _eventConfigDictionary = new Dictionary<int, EventConfig>();
        private Dictionary<int, ConditionConfig> _conditionConfigDictionary = new Dictionary<int, ConditionConfig>();

        #endregion

        #region Property

        public Task[] allTask
        {
            get
            {
                _allTask.Sort((t1, t2) =>
                {
                    //新手任务排最前面
                    if (t1.configId == 22141)
                    {
                        return -1;
                    }
                    else if (t2.configId == 22141)
                    {
                        return 1;
                    }

                    int val = 0;

                    val = t2.state.CompareTo(t1.state);
                    if (val != 0) return val;

                    val = t2.type.CompareTo(t1.type);
                    if (val != 0) return val;

                    val = t1.id.CompareTo(t2.id);
                    if (val != 0) return val;

                    return val;
                });
                return _allTask.ToArray();
            }
        }

        /// <summary>
        /// 删除需要的宝石
        /// </summary>
        public int delelteCostStone
        {
            get;
            private set;
        }

        /// <summary>
        /// 删除剩余次数
        /// </summary>
        public int deleteRemainCount
        {
            get;
            private set;
        }

        #endregion

        #region Method

        /// <summary>
        /// 检测是否有探索中的任务
        /// </summary>
        /// <returns></returns>
        public bool CheckExploreingTask()
        {
            bool b = false;
            foreach (var item in _allTask)
            {
                if (item.catIds.Count > 0)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        private TaskConfig GetTaskConfig(int configId)
        {
            TaskConfig taskConfig;
            _taskConfigDictionary.TryGetValue(configId, out taskConfig);
            return taskConfig;
        }

        private EventConfig GetEventConfig(int configId)
        {
            EventConfig eventConfig;
            _eventConfigDictionary.TryGetValue(configId, out eventConfig);
            return eventConfig;
        }

        private ConditionConfig GetConditionConfig(int configId)
        {
            ConditionConfig conditionConfig;
            _conditionConfigDictionary.TryGetValue(configId, out conditionConfig);
            return conditionConfig;
        }

        public void GetAllTask(Callback callback)
        {
            KUser.ExploreGetTasks((code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllTask(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        public void StartTask(int taskId, int[] catIds, Callback callback)
        {
            KUser.ExploreStart(taskId, catIds, (code, message, data) =>
             {
                 if (code == 0)
                 {
                     OnGetAllTask(code, message, data);
                     OnStartTask(code, message, data);
                 }
                 if (callback != null)
                 {
                     callback(code, message, data);
                 }
             });
        }

        public void BreakTask(int taskId, Callback callback)
        {
            KUser.ExploreBreak(taskId, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllTask(code, message, data);
                    OnBreakTask(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }
        /// <summary>
        /// 领取奖励
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="callback"></param>
        public void CompleteTask(int taskId, Callback callback)
        {
            KUser.ExploreComplete(taskId, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllTask(code, message, data);
                    OnCompleteTask(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        /// <summary>
        /// 拯救
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="callback"></param>
        public void SaveTask(int taskId, Callback callback)
        {
            KUser.ExploreSave(taskId, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllTask(code, message, data);
                    OnCompleteTask(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        public void DeleteTask(int taskId, Callback callback)
        {
            KUser.ExploreDelete(taskId, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllTask(code, message, data);
                    OnDeleteTask(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        private void OnGetAllTask(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is Msg.ClientMessage.S2CRetAllExpedition)
                    {
                        _allTask.Clear();

                        var originData = (Msg.ClientMessage.S2CRetAllExpedition)protoData;
                        delelteCostStone = originData.CurChgCost;
                        deleteRemainCount = originData.CurChgCount;

                        var tasks = originData.Tasks;
                        for (int j = 0; j < tasks.Count; j++)
                        {
                            var task = tasks[j];

                            var taskLocal = new Task
                            {
                                id = task.Id,
                                configId = task.TaskId,
                                result = task.Result,
                                state = task.CurState,
                                exploreRemainTime = task.ExpeditionLeftSec,
                                taskRemainTime = task.TaskLeftSec,
                                taskPassTime = task.ExpeditionPassSec,
                            };
                            taskLocal.catIds.AddRange(task.InCatIds);

                            var taskConfig = GetTaskConfig(task.TaskId);
                            taskLocal.type = taskConfig.type;
                            taskLocal.totalTime = taskConfig.totalTime;
                            taskLocal.rewards = taskConfig.rewards;
                            taskLocal.successChance = taskConfig.successChance;
                            taskLocal.savePrice = KItem.ItemInfo.Convert(taskConfig.savePrice);
                            taskLocal.name = KLocalization.GetLocalString(taskConfig.nameId);

                            taskLocal.startEvent = new Event
                            {
                                id = taskConfig.startEventId,
                                triggerTime = 0,
                                //talks = GetEventConfig(taskConfig.startEventId).GetTalks(),
                            };

                            taskLocal.processEvent = new Event
                            {
                                id = taskConfig.processEventId,
                                triggerTime = 1,
                                //talks = GetEventConfig(taskConfig.processEventId).GetTalks(),
                            };

                            taskLocal.finishEvent = new Event
                            {
                                id = taskConfig.finishEventId,
                                triggerTime = task.TaskLeftSec,
                                //talks = GetEventConfig(taskConfig.finishEventId).GetTalks(),
                            };

                            foreach (var te in task.Events)
                            {
                                taskLocal.specialEvents.Add(new Event
                                {
                                    id = te.EventId,
                                    triggerTime = te.Sec - task.ExpeditionPassSec,
                                    reward = KItem.ItemInfo.FromList(te.DropIdNums),
                                    //talks = GetEventConfig(te.EventId).GetTalks(),
                                });
                            }

                            foreach (var tc in task.Conditions)
                            {
                                var tmpCondition = new Condition
                                {
                                    type = tc.ConditionType,
                                    description = GetConditionConfig(tc.ConditionType).GetDescription(),
                                };
                                tc.ConVals.ForEach(v => tmpCondition.values.Add(v));
                                taskLocal.catConditions.Add(tmpCondition);

                            }

                            _allTask.Add(taskLocal);
                        }
                    }
                }
            }
        }

        private void OnStartTask(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is Msg.ClientMessage.S2CStartExpedition)
                    {
                        var originData = (Msg.ClientMessage.S2CStartExpedition)protoData;

                        var task = _allTask.Find(t => t.id == originData.Id);
                        if (task != null)
                        {
                            task.catIds.AddRange(originData.CatIds);
                            task.exploreRemainTime = originData.ExpeditionLeftSec;
                            task.result = originData.Result;
                            task.state = originData.CurState;// (int)Task.State.Start;
                            task.specialEvents.Clear();

                            foreach (var te in originData.Events)
                            {
                                task.specialEvents.Add(new Event
                                {
                                    id = te.EventId,
                                    triggerTime = te.Sec,
                                    reward = KItem.ItemInfo.FromList(te.DropIdNums),
                                    //talks = GetEventConfig(te.EventId).GetTalks(),
                                });
                            }
                        }
                    }
                }
            }
        }

        private void OnBreakTask(int code, string message, object data)
        {

        }

        private void OnCompleteTask(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is Msg.ClientMessage.S2CGetExpeditionReward)
                    {
                        var originData = (Msg.ClientMessage.S2CGetExpeditionReward)protoData;
                    }
                }
            }
        }

        private void OnDeleteTask(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is Msg.ClientMessage.S2CGetExpeditionReward)
                    {
                        var originData = (Msg.ClientMessage.S2CGetExpeditionReward)protoData;
                    }
                }
            }
        }

        private void Load(Hashtable table)
        {
            var taskList = table.GetArrayList("task");
            if (taskList != null && taskList.Count > 0)
            {
                _taskConfigDictionary.Clear();

                var tmpT = new Hashtable();
                var tmpL0 = (ArrayList)taskList[0];
                for (int i = 1; i < taskList.Count; i++)
                {
                    var tmpLi = (ArrayList)taskList[i];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpT[tmpL0[j]] = tmpLi[j];
                    }
                    var taskConfig = new TaskConfig
                    {
                        id = tmpT.GetInt("Id"),
                        type = tmpT.GetInt("Type"),
                        totalTime = tmpT.GetInt("SearchTime"),
                        startEventId = tmpT.GetInt("StartEventId"),
                        processEventId = tmpT.GetInt("ProcessEventId"),
                        finishEventId = tmpT.GetInt("EndEventId"),
                        rewards = tmpT.GetArray<int>("Reward"),
                        successChance = tmpT.GetInt("SearchEventChance"),
                        savePrice = tmpT.GetArray<int>("BuyBack"),
                        nameId = tmpT.GetInt("TaskName"),
                    };
                    _taskConfigDictionary.Add(taskConfig.id, taskConfig);
                }
            }

            var eventList = table.GetArrayList("event");
            if (eventList != null && eventList.Count > 0)
            {
                _eventConfigDictionary.Clear();

                var tmpT = new Hashtable();
                var tmpL0 = (ArrayList)eventList[0];
                for (int i = 1; i < eventList.Count; i++)
                {
                    var tmpLi = (ArrayList)eventList[i];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpT[tmpL0[j]] = tmpLi[j];
                    }

                    var eventConfig = new EventConfig
                    {
                        id = tmpT.GetInt("ClientId"),
                        talkIds = tmpT.GetArray<int>("TalkId"),
                    };
                    _eventConfigDictionary.Add(eventConfig.id, eventConfig);
                }
            }

            var conditionList = table.GetArrayList("condition");
            if (conditionList != null && conditionList.Count > 0)
            {
                _conditionConfigDictionary.Clear();

                var tmpT = new Hashtable();
                var tmpL0 = (ArrayList)conditionList[0];
                for (int i = 1; i < conditionList.Count; i++)
                {
                    var tmpLi = (ArrayList)conditionList[i];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpT[tmpL0[j]] = tmpLi[j];
                    }

                    var conditionConfig = new ConditionConfig
                    {
                        id = tmpT.GetInt("Id"),
                        descriptionId = tmpT.GetInt("DescriptionId"),
                    };
                    _conditionConfigDictionary.Add(conditionConfig.id, conditionConfig);
                }
            }
        }

        #endregion

        #region Unity

        public static KExplore Instance;

        private void Awake()
        {
            Instance = this;
        }

        public override void Load()
        {
            TextAsset textAsset;
            KAssetManager.Instance.TryGetExcelAsset("SearchTask", out textAsset);
            if (textAsset)
            {
                var table = textAsset.bytes.ToJsonTable();
                if (table != null)
                {
                    Load(table);
                }
            }
        }

        #endregion
    }
}
