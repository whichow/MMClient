using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public enum StateEnum
    {
        Global,
        Ready,
        Idle,
        /// <summary>
        /// 检测消除
        /// </summary>
        CheckAndCrush,
        GameOver,
        /// <summary>
        /// 结算
        /// </summary>
        Bonus,
        /// <summary>
        /// 刷新三消单元
        /// </summary>
        Refresh,
        Skill,
        /// <summary>
        /// 灰怪物
        /// </summary>
        CheckCoom,
        /// <summary>
        /// 褐怪物
        /// </summary>
        CheckBrownCoom,
        /// <summary>
        /// 猫窝
        /// </summary>
        CheckCattery,
        /// <summary>
        /// 毒液
        /// </summary>
        CheckVenom,
        /// <summary>
        /// 传送带
        /// </summary>
        CheckConveyor,
        /// <summary>
        /// 鱼
        /// </summary>
        CheckFish,
        /// <summary>
        /// 可变元素处理
        /// </summary>
        CheckCrystal,
        /// <summary>
        /// 能量符散发处理
        /// </summary>
        CheckEnergy,
        /// <summary>
        /// 猫释放技能
        /// </summary>
        CheckCatSkill,
        /// <summary>
        /// 元素跳跃
        /// </summary>
        CheckJump,
        /// <summary>
        /// 回合逻辑处理
        /// </summary>
        RoundLogic,
        /// <summary>
        /// 毛线球
        /// </summary>
        CheckWoolBall,

        AutoAIReady,
    }

    public class StateMachine<entity>
    {
        private GameFSM m_pOwner;
        private State<GameFSM> m_pCurrentState;
        private State<GameFSM> m_pPreviousState;
        private State<GameFSM> m_pGlobalState;
        private StateEnum currentStateEnum;
        private Dictionary<StateEnum, State<GameFSM>> stateDic;

        public StateMachine(GameFSM owner)
        {
            m_pOwner = owner;
            m_pCurrentState = null;
            m_pPreviousState = null;
            m_pGlobalState = null;

            stateDic = new Dictionary<StateEnum, State<GameFSM>>();
            RegistState();
        }

        private void RegistState()
        {
            stateDic.Add(StateEnum.Ready, new M3ReadyState());
            stateDic.Add(StateEnum.Idle, new M3IdleState());
            stateDic.Add(StateEnum.Global, new M3GlobalState());
            stateDic.Add(StateEnum.CheckAndCrush, new M3CheckAndCrushState());
            stateDic.Add(StateEnum.GameOver, new M3GameOverState());
            stateDic.Add(StateEnum.Bonus, new M3BonusState());
            stateDic.Add(StateEnum.Refresh, new M3RefreshState());
            stateDic.Add(StateEnum.Skill, new M3SkillState());
            stateDic.Add(StateEnum.CheckCoom, new M3CheckCoomState());
            stateDic.Add(StateEnum.CheckBrownCoom, new M3CheckBrownCoomState());
            stateDic.Add(StateEnum.CheckCattery, new M3CheckCatteryState());
            stateDic.Add(StateEnum.CheckVenom, new M3CheckVenomState());
            stateDic.Add(StateEnum.CheckConveyor, new M3CheckConveyorState());
            stateDic.Add(StateEnum.CheckFish, new M3CheckFishState());
            stateDic.Add(StateEnum.CheckEnergy, new M3AddEnergyState());
            stateDic.Add(StateEnum.CheckCrystal, new M3CrystalState());
            stateDic.Add(StateEnum.CheckCatSkill, new M3CheckCatSkillState());
            stateDic.Add(StateEnum.CheckJump, new M3CheckJumpState());
            stateDic.Add(StateEnum.RoundLogic, new M3RoundLogicState());
            stateDic.Add(StateEnum.CheckWoolBall, new M3CheckWoolBallState());
        }

        public void GlobalStateEnter()
        {
            m_pGlobalState.Enter(m_pOwner);
        }

        public void SetGlobalStateState(StateEnum stateEnum)
        {
            if (stateDic.ContainsKey(stateEnum))
            {
                m_pGlobalState = stateDic[stateEnum];
            }
            m_pGlobalState.Target = m_pOwner;
            m_pGlobalState.Enter(m_pOwner);
        }

        public void SetCurrentState(StateEnum stateEnum)
        {
            if (stateDic.ContainsKey(stateEnum))
            {
                m_pCurrentState = stateDic[stateEnum];
                currentStateEnum = stateEnum;
            }
            m_pCurrentState.Target = m_pOwner;
            m_pCurrentState.Enter(m_pOwner);
        }

        public State<GameFSM> GetStateInstance(StateEnum stateEnum)
        {
            if (stateDic.ContainsKey(stateEnum))
            {
                return stateDic[stateEnum];
            }
            return null;
        }

        public void SMUpdate()
        {
            if (m_pGlobalState != null)
            {
                m_pGlobalState.Execute(m_pOwner);
            }
            //if (M3GameManager.Instance.isAutoAI)
            //    return;
            if (m_pCurrentState != null)
            {
                m_pCurrentState.Execute(m_pOwner);
            }
        }

        public void ChangeGameState(StateEnum stateEnum)
        {
            if (stateDic.ContainsKey(stateEnum))
            {
                currentStateEnum = stateEnum;
                ChangeState(stateDic[stateEnum]);
            }
        }

        private void ChangeState(State<GameFSM> pNewState)
        {
            if (pNewState == null)
            {
                Debug.LogError("can't find this state");
            }
            m_pCurrentState.Exit(m_pOwner);
            m_pPreviousState = m_pCurrentState;
            m_pCurrentState = pNewState;
            m_pCurrentState.Target = m_pOwner;
            m_pCurrentState.Enter(m_pOwner);
            //m_pCurrentState.Execute(m_pOwner);
            //if (M3GameManager.Instance.isAutoAI)
            //{
            //    m_pCurrentState.Execute(m_pOwner);
            //}
        }

        public void RevertToPreviousState()
        {
            ChangeState(m_pPreviousState);
        }

        public State<GameFSM> CurrentState()
        {
            return m_pCurrentState;
        }

        public StateEnum GetCurrentStateEnum()
        {
            return currentStateEnum;
        }

        public State<GameFSM> GlobalState()
        {
            return m_pGlobalState;
        }

        public State<GameFSM> PreviousState()
        {
            return m_pPreviousState;
        }

    }
}
