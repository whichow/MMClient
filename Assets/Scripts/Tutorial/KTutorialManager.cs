//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : #DATA#
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "TutorialManager" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//namespace Game
//{
//    using System.Collections;
//    using System.Collections.Generic;
//    using UnityEngine;

//    /// <summary>
//    /// 新手引导管理类
//    /// </summary>
//    public class KTutorialManager : KGameModule
//    {
//        #region Field

//        /// <summary>
//        /// 关闭教程
//        /// </summary>
//        public bool closeTutorial;

//        Dictionary<int, KTutorial> _tutorials = new Dictionary<int, KTutorial>();
//        Dictionary<int, KTutorialStage> _tutorialStages = new Dictionary<int, KTutorialStage>();
//        Dictionary<int, KTutorialAction> _tutorialActions = new Dictionary<int, KTutorialAction>();
//        Dictionary<int, KTutorialCondition> _tutorialConditions = new Dictionary<int, KTutorialCondition>();

//        /// <summary>
//        /// 当前
//        /// </summary>
//        private KTutorial _currTutorial;
//        /// <summary>
//        /// 活跃的
//        /// </summary>
//        private List<KTutorial> _validTutorials = new List<KTutorial>();
//        /// <summary>
//        /// 已经触发的
//        /// </summary>
//        private List<KTutorial> _triggeredTutorials = new List<KTutorial>();

//        #endregion

//        #region Property

//        /// <summary>
//        /// 当前引导
//        /// </summary>
//        public KTutorial currTutorial
//        {
//            get { return _currTutorial; }
//        }

//        #endregion

//        #region Method

//        /// <summary>
//        /// 触发条件(驱动)
//        /// </summary>
//        /// <param name="condition"></param>
//        public void TriggerCondition(int condition)
//        {
//            if (closeTutorial)
//            {
//                return;
//            }

//            foreach (var item in _validTutorials)
//            {
//                if (!item.completed && item.Check(condition))
//                {
//                    //Debug.Log(item.id + "condition  gropId");
//                    if (_triggeredTutorials.Contains(item))
//                        continue;
//                    _triggeredTutorials.Add(item);
//                }
//            }
//        }

//        /// <summary>
//        /// 完成
//        /// </summary>
//        public void CompleteStage()
//        {
//            if (_currTutorial != null)
//            {
//                _currTutorial.CompleteStep();
//                if (_currTutorial.completed)
//                {
//                    _validTutorials.Remove(_currTutorial);
//                    _currTutorial = null;
//                }
//            }
//        }

//        /// <summary>
//        /// 获取指定组
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        internal KTutorial GetTutorial(int id)
//        {
//            KTutorial ret;
//            _tutorials.TryGetValue(id, out ret);
//            return ret;
//        }

//        /// <summary>
//        /// 获取指定步
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        internal KTutorialStage GetStage(int id)
//        {
//            KTutorialStage ret;
//            _tutorialStages.TryGetValue(id, out ret);
//            return ret;
//        }

//        /// <summary>
//        /// 获取指定所有步
//        /// </summary>
//        /// <param name="group"></param>
//        /// <param name="stages"></param>
//        internal void GetAllStages(int group, List<KTutorialStage> stages)
//        {
//            foreach (var kv in _tutorialStages)
//            {
//                var item = kv.Value;
//                if (item.group == group)
//                {
//                    stages.Add(item);
//                }
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        internal KTutorialAction GetAction(int id)
//        {
//            KTutorialAction ret;
//            _tutorialActions.TryGetValue(id, out ret);
//            return ret;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        internal KTutorialCondition GetCondition(int id)
//        {
//            KTutorialCondition ret;
//            _tutorialConditions.TryGetValue(id, out ret);
//            return ret;
//        }

//        public void Load(Hashtable table)
//        {
//            _tutorialConditions.Clear();
//            var conditionList = table.GetList("condition");
//            conditionList.Resolve((t) =>
//            {
//                var tmp = new KTutorialCondition();
//                tmp.Load(t);
//                _tutorialConditions.Add(tmp.id, tmp);
//            });

//            _tutorialActions.Clear();
//            var actionList = table.GetList("action");
//            actionList.Resolve((t) =>
//            {
//                var type = t.GetInt("Type");
//                var tmp = KTutorialAction.CreateAction(type);
//                tmp.Load(t);
//                _tutorialActions.Add(tmp.id, tmp);
//            });

//            _tutorialStages.Clear();
//            var stageList = table.GetList("stage");
//            stageList.Resolve((t) =>
//            {
//                var tmp = new KTutorialStage();
//                tmp.Load(t);
//                _tutorialStages.Add(tmp.id, tmp);
//            });

//            _tutorials.Clear();
//            var groupList = table.GetList("group");
//            groupList.Resolve((t) =>
//            {
//                var tmp = new KTutorial();
//                tmp.Load(t);
//                _tutorials.Add(tmp.id, tmp);
//            });
//        }

//        public void SaveStage(KTutorialStage stage)
//        {
//            string key = "p_" + KUser.SelfPlayer.id + "_ts_" + stage.id;
//            PlayerPrefs.SetInt(key, 1);
//            PlayerPrefs.Save();
//        }

//        public void ProcessData(object data)
//        {
//            foreach (var kv in _tutorialStages)
//            {
//                var item = kv.Value;
//                var key = "p_" + KUser.SelfPlayer.id + "_ts_" + item.id;
//                bool complete = PlayerPrefs.HasKey(key);
//                if (complete)
//                {
//                    item.completed = complete;
//                }
//            }

//            _validTutorials.Clear();
//            foreach (var kv in _tutorials)
//            {
//                var item = kv.Value;
//                if (item.HasValidStage())
//                {
//                    _validTutorials.Add(item);
//                }
//            }
//            _validTutorials.Sort((a, b) => a.priority.CompareTo(b.priority));
//        }

//        #endregion

//        #region Unity

//        public static KTutorialManager Instance;

//        private void Awake()
//        {
//            Instance = this;
//        }

//        // Use this for initialization
//        public override void Load()
//        {
//            TextAsset tmpText;
//            if (KAssetManager.Instance.TryGetExcelAsset("tutorials", out tmpText))
//            {
//                if (tmpText)
//                {
//                    var tmpJson = tmpText.bytes.ToJsonTable();
//                    Load(tmpJson);
//                }
//            }

//            //本地存           
//        }

//        private void Update()
//        {
//            if (_currTutorial == null && _triggeredTutorials.Count > 0)
//            {
//                _currTutorial = _triggeredTutorials[0];
//                _currTutorial.Trigger();
//                _triggeredTutorials.Clear();
//            }

//            if (_currTutorial != null)
//            {
//                _currTutorial.Update();
//                if (_currTutorial.completed)
//                {
//                    _currTutorial = null;
//                }
//            }

//            for (int i = _validTutorials.Count - 1; i >= 0; i--)
//            {
//                var tutorial = _validTutorials[i];
//                if (tutorial.Jump())
//                {
//                    _validTutorials.RemoveAt(i);
//                }
//            }
//        }

//        #endregion
//    }
//}
