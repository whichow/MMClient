/** 
*FileName:     M3EffectLauncher.cs 
*Author:       Hejunjie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-11-08 
*Description:    
*History: 
*/
using System;
using System.Collections;
using UnityEngine;

namespace Game.Match3
{
    public enum SkillEffectType
    {
        None,
        Straight,
    }

    public class M3EffectLauncher
    {
        private SkillEffectType type;
        private KCat cat;

        public M3EffectLauncher(SkillEffectType t)
        {
            type = t;
            cat = M3GameManager.Instance.catManager.GameCat;
        }

        public void DoEffect(object[] args, Action callBack)
        {
            if (cat == null)
                return;
            switch (type)
            {
                case SkillEffectType.None:
                    break;
                case SkillEffectType.Straight:
                    M3GameManager.Instance.StartCoroutine(DoStraight(args, callBack));
                    break;
                default:
                    break;
            }
        }

        IEnumerator DoStraight(object[] args, Action callBack)
        {

            yield return new WaitForSeconds(cat.GetSkillLaunchTime().GetInt(KFxBehaviour.skillTime1) / 1000f);
            if (args[0] == null)
            {
                callBack();
            }
            else if (string.IsNullOrEmpty(args[0].ToString()))
            {
                callBack();
            }
            else
            {
                KFxBehaviour fxBehaviour = M3FxManager.Instance.PlayCatSkill(args[0].ToString());
                fxBehaviour.PlayStraightEffect((Vector3)args[1], (Vector3)args[2], (Vector3)args[3], M3Config.SkillFlySpeed, callBack);
            }
        }

    }
}