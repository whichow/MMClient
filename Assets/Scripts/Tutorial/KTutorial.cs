//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : #DATE#
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "Tutorial" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//namespace Game
//{
//    using System.Collections;
//    using System.Collections.Generic;
//    using UnityEngine;

//    public class KTutorial
//    {
//        public enum Status
//        {
//            kNone = 0,
//            kStarted = 1,//已经开始
//            kCompleted = 2,
//        }

//        #region Field

//        /// <summary>
//        /// 跳过条件
//        /// </summary>
//        private int[] _jumpIds;
//        private int[] _triggerIds;
//        private int[] _conditionIds;

//        private List<KTutorialStage> _stages;
//        private List<KTutorialCondition> _jumps;
//        private List<KTutorialCondition> _triggers;
//        private List<KTutorialCondition> _conditions;

//        #endregion

//        #region Property

//        public int id
//        {
//            get;
//            private set;
//        }

//        public int priority
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public int status
//        {
//            get;
//            private set;
//        }

//        public bool completed
//        {
//            get
//            {
//                return status == (int)Status.kCompleted;
//            }
//            set
//            {
//                status = (int)Status.kCompleted;
//            }
//        }

//        public KTutorialStage currStage
//        {
//            get;
//            private set;
//        }

//        public KTutorialStage nextStage
//        {
//            get
//            {
//                return currStage != null ? GetNextStage(currStage.step) : null;
//            }
//        }

//        public List<KTutorialStage> stages
//        {
//            get
//            {
//                if (_stages == null)
//                {
//                    _stages = new List<KTutorialStage>();
//                    KTutorialManager.Instance.GetAllStages(id, _stages);
//                    _stages.Sort((a, b) => a.step.CompareTo(b.step));
//                }
//                return _stages;
//            }
//        }

//        public List<KTutorialCondition> triggers
//        {
//            get
//            {
//                if (_triggers == null)
//                {
//                    _triggers = new List<KTutorialCondition>();
//                    if (_triggerIds != null)
//                    {
//                        for (int i = 0; i < _triggerIds.Length; i++)
//                        {
//                            var condition = KTutorialManager.Instance.GetCondition(_triggerIds[i]);
//                            if (condition != null)
//                            {
//                                _triggers.Add(condition);
//                            }
//                        }
//                    }
//                }
//                return _triggers;
//            }
//        }

//        public List<KTutorialCondition> conditions
//        {
//            get
//            {
//                if (_conditions == null)
//                {
//                    _conditions = new List<KTutorialCondition>();
//                    if (_conditionIds != null && _conditionIds.Length > 0)
//                    {
//                        for (int i = 0; i < _conditionIds.Length; i++)
//                        {
//                            _conditions.Add(KTutorialManager.Instance.GetCondition(_conditionIds[i]));
//                        }
//                    }
//                }
//                return _conditions;
//            }
//        }

//        public List<KTutorialCondition> jumps
//        {
//            get
//            {
//                if (_jumps == null)
//                {
//                    _jumps = new List<KTutorialCondition>();
//                    if (_jumpIds != null && _jumpIds.Length > 0)
//                    {
//                        for (int i = 0; i < _jumpIds.Length; i++)
//                        {
//                            _jumps.Add(KTutorialManager.Instance.GetCondition(_jumpIds[i]));
//                        }
//                    }
//                }
//                return _jumps;
//            }
//        }

//        #endregion

//        #region Method

//        /// <summary>
//        /// 检查
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public bool Check(int id)
//        {
//            bool flag = false;
//            foreach (var item in triggers)
//            {
//                if (item.id == id)
//                {
//                    flag = true;
//                    break;
//                }
//            }

//            if (flag)
//            {
//                foreach (var item in conditions)
//                {
//                    if (!item.GetResult())
//                    {
//                        return false;
//                    }
//                }
//                return true;
//            }
//            return false;
//        }

//        public bool Jump()
//        {
//            foreach (var jump in jumps)
//            {
//                if (jump != null && jump.GetResult())
//                {
//                    Complete();
//                    return true;
//                }
//            }
//            return false;
//        }
//        /// <summary>
//        /// 触发
//        /// </summary>
//        /// <returns></returns>
//        public bool Trigger()
//        {
//            var activeS = GetValidStage();
//            if (activeS != null)
//            {
//                activeS.Start();
//                currStage = activeS;
//                return true;
//            }
//            return false;
//        }

//        /// <summary>
//        /// 完成当前步
//        /// </summary>
//        public void CompleteStep()
//        {
//            if (currStage != null)
//            {
//                Debug.Log(currStage.id + "step" + currStage.group + "group");
//                currStage.Complete();
//                if (!HasNext())
//                {
//                    currStage = null;
//                    status = (int)Status.kCompleted;
//                }
//            }
//        }

//        public bool HasNext()
//        {
//            return currStage != null ? currStage.step < stages.Count : false;
//        }

//        public void Complete()
//        {
//            status = (int)Status.kCompleted;
//            foreach (var stage in stages)
//            {
//                stage.Complete();
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="step"></param>
//        /// <returns></returns>
//        public KTutorialStage GetStage(int step)
//        {
//            if (step > 0 && step <= stages.Count)
//            {
//                return stages[step - 1];
//            }
//            return null;
//        }

//        public KTutorialStage GetNextStage()
//        {
//            int step = currStage.step;
//            if (step > 0 && step < stages.Count)
//            {
//                return stages[step];
//            }
//            return null;
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="step"></param>
//        /// <returns></returns>
//        public KTutorialStage GetNextStage(int step)
//        {
//            if (step > 0 && step < stages.Count)
//            {
//                return stages[step];
//            }
//            return null;
//        }

//        public KTutorialStage GetValidStage()
//        {
//            var tmpStages = stages;
//            int validIndex = 0;
//            for (int i = tmpStages.Count - 1; i >= 0; i--)
//            {
//                var tmpStage = tmpStages[i];
//                if (tmpStage.isKey && tmpStage.completed)
//                {
//                    validIndex = i + 1;
//                    break;
//                }
//            }

//            if (validIndex < tmpStages.Count)
//            {
//                return tmpStages[validIndex];
//            }

//            completed = true;
//            return null;
//        }

//        public bool HasValidStage()
//        {
//            if (!completed)
//            {
//                foreach (var item in stages)
//                {
//                    if (item.isKey && item.status == 0)
//                    {
//                        return true;
//                    }
//                }
//            }
//            return false;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="table"></param>
//        public void Load(Hashtable table)
//        {
//            id = table.GetInt("Id");
//            priority = table.GetInt("Priority");

//            _jumpIds = table.GetArray<int>("Jump");
//            _triggerIds = table.GetArray<int>("Trigger");
//            _conditionIds = table.GetArray<int>("Condition");
//        }

//        internal void Update()
//        {
//            if (currStage != null)
//            {
//                if (currStage.completed)
//                {
//                    currStage = nextStage;
//                    if (currStage != null)
//                    {
//                        currStage.Start();
//                    }
//                    else
//                    {
//                        currStage = null;
//                        status = (int)Status.kCompleted;
//                    }
//                }
//                else if (!currStage.started)
//                {
//                    currStage.Start();
//                }
//            }
//        }

//        #endregion
//    }
//}
