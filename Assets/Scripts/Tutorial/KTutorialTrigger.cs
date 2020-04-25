//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : #DATA#
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "KTutorialTrigger" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//namespace Game
//{
//    using System.Collections;
//    using System.Collections.Generic;
//    using UnityEngine;

//    public class KTutorialTrigger : MonoBehaviour
//    {
//        #region Field

//        public int enableCondition;
//        public int disableCondition;
//        public int updateCondition;
//        public int InM3trigger;

//        #endregion

//        #region Unity

//        private void OnEnable()
//        {
//            if (enableCondition != 0)
//            {
//                if (KTutorialManager.Instance)
//                {
//                    KTutorialManager.Instance.TriggerCondition(enableCondition);
//                }
//            }
//            if (InM3trigger != 0 && KLevelManager.Instance.currLevelID==InM3trigger )
//            {
//                if (KTutorialManager.Instance)
//                {
//                    KTutorialManager.Instance.TriggerCondition(InM3trigger);
//                }
//            }
//        }

//        private void OnDisable()
//        {
//            if (disableCondition != 0)
//            {
//                if (KTutorialManager.Instance)
//                {
//                    KTutorialManager.Instance.TriggerCondition(disableCondition);
//                }
//            }
//        }

//        private void LateUpdate()
//        {
//            if (updateCondition != 0)
//            {
//                if (KTutorialManager.Instance)
//                {
//                    KTutorialManager.Instance.TriggerCondition(updateCondition);
//                }
//            }
//        }

//        #endregion

//    }
//}
