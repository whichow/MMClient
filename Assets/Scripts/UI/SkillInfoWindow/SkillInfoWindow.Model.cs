// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "SkillInfoWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public partial class SkillInfoWindow
    {
        private int _skillId;

        public Sprite GetIcon()
        {
            return KIconManager.Instance.GetSkillIcon(_skillId);
        }

        public string GetTitle()
        {
            return XTable.SkillXTable.GetByID(_skillId).Name;
        }

        public string GetEnergyCost()
        {
            var skill = KItemManager.Instance.GetSkill(_skillId);
            if (skill != null)
                return "能量消耗:" + skill.EnergyCost;
            return "";
        }

        public string GetDescription()
        {
            var skill = KItemManager.Instance.GetSkill(_skillId);
            if (skill != null)
                return skill.description;
            return "";
        }

        public void RefreshModel()
        {
            var cat = int.Parse(data.ToString());
            _skillId = cat;
        }
    }
}

