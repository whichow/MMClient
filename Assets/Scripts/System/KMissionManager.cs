// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-10-24
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KMissionManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    using Callback = System.Action<int, string, object>;

    public class KMissionManager : KGameModule
    {
        #region FIELD

        private Dictionary<int, KMission> _missions = new Dictionary<int, KMission>();

        private List<KMission> _archieveMissions = new List<KMission>();
        private List<KMission> _dailyMissions = new List<KMission>();

        #endregion

        #region PROPERTY

        public KMission[] dailyMissions
        {
            get
            {
                return _dailyMissions.ToArray();
            }
        }

        public KMission[] achievementMissions
        {
            get
            {
                return _archieveMissions.ToArray();
            }
        }

        #endregion

        #region METHOD

        public void GetDaily(Callback callback)
        {
            KUser.MissionGetDaily((code, message, data) =>
            {
                if (code == 0)
                {
                    _dailyMissions.Clear();
                    var protoDatas = data as ArrayList;
                    if (protoDatas != null)
                    {
                        for (int i = 0; i < protoDatas.Count; i++)
                        {
                            var protoData = protoDatas[i];

                            //if (protoData is Msg.ClientMessage.S2CSyncDialyTask)
                            //{
                            //    var originData = (Msg.ClientMessage.S2CSyncDialyTask)protoData;
                            //    foreach (var task in originData.TaskList)
                            //    {
                            //        KMission mission;
                            //        if (_missions.TryGetValue(task.TaskId, out mission))
                            //        {
                            //            mission.curValue = task.TaskValue;
                            //            mission.status = task.TaskState;
                            //            _dailyMissions.Add(mission);
                            //            if (mission.dynamic)
                            //            {
                            //                mission.maxValue = originData.TaskList.Count;
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        public void GetAchievement(Callback callback)
        {
            KUser.MissionGetArchievement((code, message, data) =>
            {
                if (code == 0)
                {
                    _archieveMissions.Clear();
                    var protoDatas = data as ArrayList;
                    if (protoDatas != null)
                    {
                        for (int i = 0; i < protoDatas.Count; i++)
                        {
                            var protoData = protoDatas[i];

                            //if (protoData is Msg.ClientMessage.S2CSyncAchieveData)
                            //{
                            //    var originData = (Msg.ClientMessage.S2CSyncAchieveData)protoData;
                            //    foreach (var archievement in originData.AchieveList)
                            //    {
                            //        KMission mission;
                            //        if (_missions.TryGetValue(archievement.AchieveId, out mission))
                            //        {
                            //            mission.curValue = archievement.AchieveValue;
                            //            mission.status = archievement.AchieveState;
                            //            _archieveMissions.Add(mission);
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        public int[] PointArry()
        {
            int[] pointarry = new int[2];
            int Tnum = 0;
            for (int i = 0; i < _dailyMissions.Count; i++)
            {
                if (_dailyMissions[i].status == 1)
                {
                    Tnum++;
                }
            }
            int Anum = 0;
            for (int i = 0; i < _archieveMissions.Count; i++)
            {
                if (_archieveMissions[i].status == 1)
                {
                    Anum++;
                }
            }
            return new int[2] { Tnum, Anum };
        }

        public void RewardDialy(int id, Callback callback)
        {
            KUser.MissionRewardDaily(id, (code, message, data) =>
             {
                 if (code == 0)
                 {

                 }
                 if (callback != null)
                 {
                     callback(code, message, data);
                 }
             });
        }

        public void RewardAchievement(int id, Callback callback)
        {
            KUser.MissionRewardArchievement(id, (code, message, data) =>
             {
                 if (code == 0)
                 {

                 }
                 if (callback != null)
                 {
                     callback(code, message, data);
                 }
             });
        }

        public void Process(object data)
        {
            //if (data is Msg.ClientMessage.S2CNotifyTaskValueChg)
            //{
            //    var originData = (Msg.ClientMessage.S2CNotifyTaskValueChg)data;
            //    KMission mission;
            //    if (_missions.TryGetValue(originData.TaskId, out mission))
            //    {
            //        mission.curValue = originData.TaskValue;
            //        mission.status = originData.TaskState;
            //    }
            //}

            //if (data is Msg.ClientMessage.S2CNotifyAchieveValueChg)
            //{
            //    var originData = (Msg.ClientMessage.S2CNotifyAchieveValueChg)data;
            //    KMission mission;
            //    if (_missions.TryGetValue(originData.AchieveId, out mission))
            //    {
            //        mission.curValue = originData.AchieveValue;
            //        mission.status = originData.AchieveState;
            //    }
            //}
        }

        public void Load(Hashtable table)
        {
            if (table != null)
            {
                var list = table.GetArrayList("mission");
                if (list != null && list.Count > 0)
                {
                    var tmpT = new Hashtable();
                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        var tmpL0 = (ArrayList)list[0];
                        var tmpLi = (ArrayList)list[i + 1];
                        for (int j = 0; j < tmpL0.Count; j++)
                        {
                            tmpT[tmpL0[j]] = tmpLi[j];
                        }

                        var mission = new KMission();
                        mission.Load(tmpT);
                        _missions.Add(mission.id, mission);
                    }
                }
            }
        }

        #endregion

        #region Unity
        private void Awake()
        {
            Instance = this;
        }

        public override void Load()
        {
            TextAsset textObject;
            if (KAssetManager.Instance.TryGetExcelAsset("mission", out textObject))
            {
                var tmpText = textObject as TextAsset;
                if (tmpText)
                {
                    var tmpJson = tmpText.bytes.ToJsonTable();
                    Load(tmpJson);
                }
            }
        }

        #endregion

        #region STATIC

        public static KMissionManager Instance;

        #endregion
    }
}
