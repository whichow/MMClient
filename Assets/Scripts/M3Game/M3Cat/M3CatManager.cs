using Game.DataModel;
using Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class M3CatManager
    {
        /// <summary>
        /// 当前关卡猫
        /// </summary>
        public KCat GameCat { get; private set; }
        public KCatBehaviour CatBehaviour { get; private set; }
        public M3EffectLauncher EffectLauncher { get; private set; }
        public OperationBase Operation { get; private set; }

        /// <summary>
        /// 回合能量
        /// </summary>
        public float RoundEnergy { get; private set; }

        /// <summary>
        /// 能量花费
        /// </summary>
        private int cnergyValue;

        /// <summary>
        /// 当前能量
        /// </summary>
        private float currentEnergy;

        public bool hasCat = false;
        public bool isReleasingSkill;
        public bool needView = true;

        public GameObject skillRoot;
        private GameObject catRoot;
        private GameObject catObj;

        private ESkillType skillType;
        private Animator catAnimator;

        public List<Action> skillCallBack;


        public void Init(KCat cat)
        {
            if (M3GameManager.Instance.isAutoAI) needView = false;

            GameCat = null;
            if (cat != null)
            {
                GameCat = cat;
                hasCat = true;
                isReleasingSkill = false;
                currentEnergy = 0;
                cnergyValue = GameCat.GetEnergy();
                skillType = (ESkillType)GameCat.GetSkillType();
                if (needView)
                {
                    ShowView();
                    ShowEnergy();
                }
                skillCallBack = new List<Action>();
            }
            EffectLauncher = new M3EffectLauncher(SkillEffectType.Straight);
        }

        public void ShowView()
        {
            catRoot = GameObject.Find("GameScreen/Cat/root");
            if (hasCat)
            {
                CatUtils.GetModel(GameCat.shopId, (gameObject) =>
                {
                    catObj = (GameObject)gameObject;
                    catObj.transform.SetParent(catRoot.transform, false);
                    CatBehaviour = catObj.GetComponent<KCatBehaviour>();
                    skillRoot = catObj.transform.Find("SkillPoint").gameObject;
                });
            }
        }

        public int GetMatchBonus()
        {
            if (GameCat != null)
            {
                if (M3Config.isEditor)
                {
                    return XTable.CatXTable.GetByID(GameCat.shopId).MatchAbilityRange[0];
                }
                else
                {
                    return CatDataModel.Instance.GetCatDataVOById(GameCat.catId).mCatInfo.MatchAbility;
                }
            }
            return 0;
        }

        public int GetMaxEnergy()
        {
            return cnergyValue;
        }

        public float GetCurrentEnergy()
        {
            return currentEnergy;
        }

        public void SetCurrentEnergy(float energy)
        {
            currentEnergy = energy;
        }

        public void AddEnergy(float value)
        {
            if (GameCat == null)
                return;

            if (value > 0)
            {
                RoundEnergy += value;
                currentEnergy += value;
                currentEnergy = Mathf.Min(currentEnergy, cnergyValue);
                ShowEnergy();
            }
            else
            {
                //Debug.Log("能量增加错误！ value : " + value);
            }
        }

        public void ClearEnergy()
        {
            currentEnergy = 0;
            ShowEnergy();
        }

        public bool EnergyCompleted()
        {
            return currentEnergy >= cnergyValue;
        }

        public void ShowEnergy()
        {
            if (needView)
                M3GameEvent.DispatchEvent(M3FightEnum.OnAddEnergy, (float)currentEnergy / cnergyValue);
        }

        public bool OnTryToUseSkill(object[] args)
        {
            if (!hasCat)
            {
                return false;
            }
            if (!EnergyCompleted())
            {
                //ToastBox.ShowText("能量不足！");
                return false;
            }
            if (isReleasingSkill)
            {
                ToastBox.ShowText("正在释放技能, 无法释放技能");
                return false;
            }
            if (M3GameManager.Instance.modeManager.isGameEnd)
            {
                ToastBox.ShowText("关卡结束！");
                return false;
            }
            if (M3GameManager.Instance.modeManager.IsLevelEnd())
            {
                ToastBox.ShowText("关卡结束了");
                return false;
            }
            if (M3GameManager.Instance.gameFsm.GetFSM().GetCurrentStateEnum() != StateEnum.CheckCatSkill)
            {
                ToastBox.ShowText("正在处理游戏 无法释放技能");
                Debug.Log(M3GameManager.Instance.gameFsm.GetFSM().GetCurrentStateEnum().ToString());
                return false;
            }
            else
            {
                M3SkillBase skill = GetSkillEntity();
                if (skill == null)
                {
                    return false;
                }
                skill.InitSkill(GameCat);
                skill.BeforeUseSkill();
                if (!skill.CheckCanUseSkill())
                {
                    skill.OnCancelUse();
                    return false;
                }

                Operation = null;
                switch (skill.skillOperationType)
                {
                    case SkillOperationType.None:
                        Operation = M3GameManager.Instance.skillOperation = new DirectOperation(skill, GameCat);
                        break;
                    case SkillOperationType.PointTouch:
                        Operation = M3GameManager.Instance.skillOperation = new PointTouchOperation(skill, GameCat);
                        break;
                    case SkillOperationType.Drag:
                        break;
                    default:
                        Operation = M3GameManager.Instance.skillOperation = new PointTouchOperation(skill, GameCat);
                        break;
                }
                //operation = M3GameManager.Instance.skillOperation = new DirectOperation(skill, gameCat);
                Operation.OnSkillStart();
                return true;
            }
        }

        public M3SkillBase GetSkillEntity()
        {
            return M3GameManager.Instance.skillManager.GetSkillEntity(skillType);
        }

        public Vector3 GetSkillRootPosition()
        {
            return skillRoot.transform.position;
        }

        public ItemColor GetCatColor()
        {
            switch (GameCat.GetColor())
            {
                case KCat.Color.fRed:
                    return ItemColor.fRed;
                case KCat.Color.fYellow:
                    return ItemColor.fYellow;
                case KCat.Color.fBlue:
                    return ItemColor.fBlue;
                case KCat.Color.fGreen:
                    return ItemColor.fGreen;
                case KCat.Color.fPurple:
                    return ItemColor.fPurple;
                case KCat.Color.fBrown:
                    return ItemColor.fBrown;
                default:
                    return ItemColor.fNone;
            }
        }

        public void ResetVars()
        {
            RoundEnergy = 0;
        }

    }
}