/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/11 12:19:08
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.Build;
using Game.DataModel;
using Game.UI;
using Msg.ClientMessage;

namespace Game
{
    public class GuideManager : KGameModule
    {
        public enum EGuideStatus
        {
            None = 0,
            Waitint = 1,   //等待执行
            Started = 2,   //已经开始
            Completed = 3,
        }

        #region Static
        public static GuideManager Instance;
        #endregion

        #region Member

        public bool closeGuide;

        private GuideAction m_action = new GuideAction();
        private GuideCondition m_condition = new GuideCondition();

        private int m_curStepIndex;
        private GuideDM m_guideDM;

        public EGuideStatus status;
        public GuideXDM CurGuide { get; private set; }
        public GuideStepXDM CurGuideStep { get; private set; }
        public GuideActionXDM CurGuideAction { get; private set; }

        public bool IsGuideing
        {
            get
            {
                return CurGuide != null;
            }
        }

        private bool m_completeStepIng;

        #endregion

        #region C&D
        public GuideManager()
        {
        }
        #endregion

        #region Method

        public bool CheckConditionResult(int id)
        {
            return m_condition.GetResult(id);
        }

        public void SetGuideData(GuideDM guideDM)
        {
            m_guideDM = guideDM;
        }

        public void CompleteStep()
        {
            //if (m_completeStepIng)
            //{
            //    return;
            //}
            //m_completeStepIng = true;

            //第二关卡失败，特殊处理
            if (CurGuide!= null && CurGuide.ID == 3 && CurGuideStep.ID == 308)
            {
                var window = KUIWindow.GetWindow<GameOverWindow>();
                if (window != null && !window.IsWin)
                {
                    Trigger(3, 210, false);
                    return;
                }
            }

            NextStep();

            if (CurGuide != null)
            {
                GameApp.Instance.GameServer.GuideDataSaveRequest(CurGuide.ID, CurGuideStep.ID);
            }
        }

        private void Trigger(int guideID, int stepID, bool isBreak)
        {
            if (closeGuide)
            {
                GuideOver();
                return;
            }

            Debuger.LogFormat("[GuideManager.Trigger] guideID:{0} stepID:{1} isBreak:{2}", guideID, stepID, isBreak);
            if (guideID == 0)
            {
                return;
            }

            CurGuide = XTable.GuideXTable.GetByID(guideID);
            if (CurGuide == null)
            {
                NextGuide(isBreak);
                return;
            }

            m_curStepIndex = -1;
            foreach (var item in CurGuide.Steps)
            {
                m_curStepIndex++;
                var step = XTable.GuideStepXTable.GetByID(item);

                if (stepID == 0)
                {
                    CurGuideStep = step;
                    break;
                }
                else if (stepID == step.ID)
                {
                    CurGuideStep = step;
                    break;
                }

                if (isBreak && step.IsKey)
                {
                    NextGuide(isBreak);
                    return;
                }
            }
            if (isBreak)
            {
                m_curStepIndex = 0;
                CurGuideStep = XTable.GuideStepXTable.GetByID(CurGuide.GetStepByIndex(0));
            }
            status = EGuideStatus.Waitint;
            //KUIWindow.OpenWindow<GuideWindow>();
        }

        private void StartGuide()
        {
            if (CurGuideStep != null && (CurGuideStep.ConditionID == 0 || m_condition.GetResult(CurGuideStep.ConditionID)))
            {
                KUIWindow.OpenWindow<GuideWindow>();
                status = EGuideStatus.Started;
                CurGuideAction = XTable.GuideActionXTable.GetByID(CurGuideStep.ActionID);
                Debuger.LogFormat("[GuideManager.StartGuide] guideID:{0} stepID:{1} actionID:{2}", CurGuide.ID, CurGuideStep.ID, CurGuideAction.ID);
#if UNITY_EDITOR
                guideID = CurGuide.ID;
                stepID = CurGuideStep.ID;
                actionID = CurGuideAction.ID;
#endif
                m_action.Execute(CurGuideAction.ID);
            }
        }

        private void NextGuide(bool isBreak)
        {
            int nextGuideID = CurGuide.TriggerGuideID;
            int nextGuideStepID = isBreak ? 0 : CurGuide.TriggerStep;
            if (nextGuideID > 0)
            {
                Trigger(nextGuideID, nextGuideStepID, false);
            }
            else
            {
                Debuger.Log("[Guide]没有引导了!");
                GuideOver();
            }
        }

        private void NextStep()
        {
            if (closeGuide)
            {
                GuideOver();
                return;
            }

            if (CurGuide == null) return;

            m_curStepIndex++;
            if (m_curStepIndex < CurGuide.Steps.Count)
            {
                CurGuideStep = XTable.GuideStepXTable.GetByID(CurGuide.Steps[m_curStepIndex]);
                status = EGuideStatus.Waitint;
                //KUIWindow.OpenWindow<GuideWindow>();
            }
            else
            {
                NextGuide(false);
            }
        }

        private void GuideOver()
        {
            status = EGuideStatus.Completed;
            CurGuide = null;
            CurGuideStep = null;
            CurGuideAction = null;
            KUIWindow.CloseWindow<GuideWindow>();
            KUIWindow.CloseWindow<BuildingShopWindow>();
        }

        #endregion

        #region Unity

        private void Awake()
        {
            Instance = this;
        }

        //public override void Load()
        //{
        //    KAssetManager.Instance.LoadExcelAsset("GuideConfig", new IXTable[] {
        //        XTable.GuideXTable,
        //        XTable.GuideStepXTable,
        //        XTable.GuideActionXTable,
        //        XTable.GuideConditionXTable
        //    });
        //}

        private void Update()
        {
            if (m_guideDM != null)
            {
                if (BuildingManager.Instance.isCreateFinish)
                {
                    Trigger(m_guideDM.GuideID, m_guideDM.StepID, true);
                    m_guideDM = null;
                }
                return;
            }

            //m_completeStepIng = false;

            if (status == EGuideStatus.Waitint)
            {
                StartGuide();
            }


#if UNITY_EDITOR
            if (doAction)
            {
                doAction = false;
                DoAction(actionID);
            }

            if (trigger)
            {
                trigger = false;
                Trigger(guideID, stepID, isBreak);
            }
#endif
        }

        #endregion

        #region 测试用
#if UNITY_EDITOR
        public void DoAction(int ActionId)
        {
            m_action.Execute(ActionId);
        }

        public bool isBreak;
        public int guideID;
        public int stepID;
        public int actionID;

        public bool trigger;
        public bool doAction;
#endif
        #endregion

    }
}
