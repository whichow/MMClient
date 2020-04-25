// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KItemSkill" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using Game.Match3;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 技能
    /// </summary>
    public class KItemSkill : KItem
    {
        #region Field
        
        /// <summary>
        /// 射击描述ID
        /// </summary>
        private int _shortDescriptionId;

        /// <summary>
        /// 技能类型
        /// </summary>
        public int SkillType { get; private set; }

        /// <summary>
        /// 技能作用颜色类型
        /// </summary>
        public ESkillColorType SkillColorType { get; private set; }
        /// <summary>
        /// 特效元素类型
        /// </summary>
        public ElementSpecial[] EleSpecial { get; private set; }

        /// <summary>
        /// 步数模式 参数为等级对应下标
        /// </summary>
        public int[] SkillParam { get; private set; }

        /// <summary>
        /// 能量花费
        /// </summary>
        public int EnergyCost { get; private set; }
        
        public Hashtable SkillEffect { get; private set; }
        public Hashtable SkillLaunchTime { get; private set; }

        /// <summary>
        /// 射击描述
        /// </summary>
        public string SkillShootDescription
        {
            get { return KLocalization.GetLocalString(_shortDescriptionId); }
        }

        #endregion

        #region Method

        public int GetMaxGrade()
        {
            return SkillParam.Length;
        }

        public string GetSkillShootDescription(int grade)
        {
            grade = UnityEngine.Mathf.Clamp(grade, 1, GetMaxGrade());
            return string.Format(SkillShootDescription, SkillParam[grade - 1]);
        }

        public int GetSkillParam(int grade)
        {
            grade = UnityEngine.Mathf.Clamp(grade, 1, GetMaxGrade());
            return SkillParam[grade - 1];
        }

        public override void Load(Hashtable table)
        {
            base.Load(table);
            _shortDescriptionId = table.GetInt("ShortDescriptionId");
            SkillType = table.GetInt("Type");
            SkillColorType = (ESkillColorType)table.GetInt("SkillColorType");
            EleSpecial = table.GetArray<ElementSpecial>("ElementSpecial");
            SkillParam = table.GetArray<int>("SkillParam");
            EnergyCost = table.GetInt("EnergyCost");
            SkillEffect = table.GetTable("SkillEffect");
            SkillLaunchTime = table.GetTable("SkillEffectTime");
        }

        #endregion
    }
}

