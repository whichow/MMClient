//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : #DATA#
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "TutorialStage" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//namespace Game
//{
//    using System.Collections;

//    public class KTutorialStage
//    {
//        public enum Status
//        {
//            kNone = 0,
//            kStarted = 1,//已经开始
//            kCompleted = 2,
//        }

//        #region Field

//        public int id
//        {
//            get;
//            private set;
//        }

//        public int step
//        {
//            get;
//            private set;

//        }

//        public int group
//        {
//            get;
//            private set;
//        }

//        public int actionId
//        {
//            get;
//            private set;
//        }

//        public int conditionId
//        {
//            get;
//            private set;
//        }

//        public bool isKey
//        {
//            get;
//            private set;
//        }

//        public bool isForce
//        {
//            get;
//            private set;
//        }

//        public string unlockSystem
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 状态 0初始状态
//        /// </summary>
//        public int status
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 已经完成
//        /// </summary>
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

//        /// <summary>
//        /// 已开始
//        /// </summary>
//        public bool started
//        {
//            get { return status >= (int)Status.kStarted; }
//        }

//        #endregion

//        #region Method

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public KTutorial GetTutorial()
//        {
//            return KTutorialManager.Instance.GetTutorial(group);
//        }

//        /// <summary>
//        /// 获取行为
//        /// </summary>
//        /// <returns></returns>
//        public KTutorialAction GetAction()
//        {
//            return KTutorialManager.Instance.GetAction(actionId);
//        }

//        /// <summary>
//        /// 获取条件
//        /// </summary>
//        /// <returns></returns>
//        public KTutorialCondition GetCondition()
//        {
//            return KTutorialManager.Instance.GetCondition(conditionId);
//        }

//        /// <summary>
//        /// 开始
//        /// </summary>
//        public void Start()
//        {
//            if (conditionId == 0 || GetCondition().GetResult())
//            {
//                this.status = (int)Status.kStarted;
//                this.GetAction().Execute();
//            }
//        }

//        /// <summary>
//        /// 完成
//        /// </summary>
//        public void Complete()
//        {
//            this.status = (int)Status.kCompleted;
//            if (isKey)
//            {
//                Save();
//            }
//        }

//        /// <summary>
//        /// 保存数据
//        /// </summary>
//        public void Save()
//        {
//            KTutorialManager.Instance.SaveStage(this);
//        }

//        public void Load(Hashtable table)
//        {
//            id = table.GetInt("Id");
//            step = table.GetInt("Step");
//            group = table.GetInt("Group");
//            actionId = table.GetInt("Action");
//            conditionId = table.GetInt("Condition");

//            isKey = table.GetInt("IsKey") != 0;
//            isForce = table.GetInt("IsForce") != 0;

//            unlockSystem = table.GetString("Unlock");
//        }

//        #endregion
//    }
//}
