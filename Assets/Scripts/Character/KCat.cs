// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KCat" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using Game.Match3;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class KCat
    {
        #region Const

        public readonly static int kIdleHash = Animator.StringToHash("idle");
        public readonly static int kWalkHash = Animator.StringToHash("walk");
        public readonly static int kEatHash = Animator.StringToHash("eat");
        public readonly static int kEatingHash = Animator.StringToHash("eating");
        public readonly static int kFunHash = Animator.StringToHash("fun");
        public readonly static int kWinHash = Animator.StringToHash("win");
        public readonly static int kSkillHash = Animator.StringToHash("act skill");

        #endregion

        /// <summary>
        /// 颜色枚举
        /// </summary>
        [System.Flags]
        public enum Color
        {
            fRed = 1,
            fYellow = 2,
            fBlue = 4,
            fGreen = 8,
            fPurple = 16,
            fBrown = 32,
        }

        //public enum Rarity
        //{
        //    N = 1,
        //    R,
        //    SR,
        //    SSR
        //}

        //public enum State
        //{
        //    None = 0,
        //    Cattery = 1,//猫舍
        //    Explore = 2,//探索
        //    Foster = 3,//寄养所
        //}

        #region Field

        public int catId;
        public int grade;
        public int star;
        public int exp;
        public bool locked;
        public string nickName;
        public string title;
        public int skillGrade;
        public int state;

        /// <summary>
        /// 参与排序(性能优先)
        /// </summary>
        public int rarity;
        public int mainColor;

        public int initCoinAbility; // 产金能力
        public int initExploreAbility; // 探索能力
        public int initMatchAbility; // 消除能力

        private int _shopId;
        private CatXDM _catItem;

        #endregion

        #region Property

        public int shopId
        {
            get { return _shopId; }
            set
            {
                _shopId = value;
                _catItem = XTable.CatXTable.GetByID(value);
                if (_catItem != null)
                {
                    rarity = _catItem.Rarity;
                    mainColor = _catItem.MainColor;
                    title = _catItem.Name;
                }
            }
        }

        public string name
        {
            get { return _catItem.Name; }
        }

        //public string icon
        //{
        //    get { return _catItem.Icon; }
        //}

        public string model
        {
            get { return _catItem.Model; }
        }

        public int skillId
        {
            get { return _catItem.SkillId; }
        }
        //public int soulStone
        //{
        //    get { return _catItem.Price; }
        //}
        public int colors
        {
            get { return _catItem.Color; }
        }

        //public int maxExp
        //{
        //    get
        //    {
        //        var max = _catItem.GetUpgradeExp(grade);
        //        return max > 0 ? max : 1;
        //    }
        //}

        //public int maxGrade
        //{
        //    get { return _catItem.GetMaxGrade(star); }
        //}
        //public int nextmaxGrade
        //{
        //    get { return _catItem.GetMaxGrade(star + 1); }
        //}

        ///// <summary>
        ///// 照片(全身像)
        ///// </summary>
        //public string photo
        //{
        //    get { return _catItem.Photo; }
        //}

        ///// <summary>
        ///// 产金能力
        ///// </summary>
        //public int coinAbility
        //{
        //    get { return CatDataModel.Instance.GetCatDataVOById(catId).mCatInfo.CoinAbility; }
        //}

        ///// <summary>
        ///// 探索能力
        ///// </summary>
        //public int exploreAbility
        //{
        //    get { return CatDataModel.Instance.GetCatDataVOById(catId).mCatInfo.ExploreAbility; }
        //}

        ///// <summary>
        ///// 消除能力
        ///// </summary>
        //public int matchAbility
        //{
        //    get
        //    {
        //        if (M3Config.isEditor)
        //        {
        //            return XTable.CatXTable.GetByID(shopId).MatchAbilityRange[0];
        //        }
        //        else
        //        {
        //            return CatDataModel.Instance.GetCatDataVOById(catId).mCatInfo.MatchAbility;
        //        }
        //    }
        //}

        #endregion

        #region Method

        ///// <summary>
        ///// 欧气值
        ///// </summary>
        ///// <returns></returns>
        //public int GetCombatValue()
        //{
        //    return coinAbility + exploreAbility + matchAbility + _catItem.GetSkillCombatValue(skillGrade);
        //    //欧气值 = 星级权重 * 星级 + 技能权重 * 技能点数 + 产金能力权重 * 产金 能力 + 探索能力权重 * 探索能力 + 消除能力权重 * 消除能力
        //}

        //public Sprite GetIconSprite()
        //{
        //    return KIconManager.Instance.GetCatIcon(icon);
        //}

        //public string GetSkillName()
        //{
        //    return KItemManager.Instance.GetSkill(skillId).itemName;
        //}

        public int GetEnergy()
        {
            var tmpId = skillId;
            if (tmpId > 0)
            {
                var skill = KItemManager.Instance.GetSkill(tmpId);
                return skill.EnergyCost;
            }
            return 0;
        }

        public int GetSkillType()
        {
            var tmpId = skillId;
            if (tmpId > 0)
            {
                var skill = KItemManager.Instance.GetSkill(tmpId);
                return skill.SkillType;
            }
            return 0;
        }

        //public int GetSkillMaxGrade()
        //{
        //    var tmpId = skillId;
        //    if (tmpId > 0)
        //    {
        //        var skill = KItemManager.Instance.GetSkill(tmpId);
        //        return skill.GetMaxGrade();
        //    }
        //    return 0;
        //}

        public int GetSkillParam()
        {
            var tmpId = skillId;
            if (tmpId > 0)
            {
                var skill = KItemManager.Instance.GetSkill(tmpId);

                return skill.GetSkillParam(skillGrade);
            }
            return 0;
        }

        public Hashtable GetSkillEffectNameTable()
        {
            return KItemManager.Instance.GetSkill(skillId).SkillEffect;
        }

        public string GetSkillEffectName(string key)
        {
            return GetSkillEffectNameTable().GetString(key);
        }

        public Hashtable GetSkillLaunchTime()
        {
            return KItemManager.Instance.GetSkill(skillId).SkillLaunchTime;
        }

        public Sprite GetSkillSprite()
        {
            if (skillId > 0)
            {
                return KIconManager.Instance.GetSkillIcon(skillId);
            }
            return null;
        }


        //public GameObject GetModel()
        //{
        //    return GetModel(model, shopId);
        //}

        ///// <summary>
        ///// 同步
        ///// </summary>
        ///// <param name="modelName"></param>
        ///// <returns></returns>
        //public static GameObject GetModel(string modelName, int shopId)
        //{
        //    GameObject prefab;
        //    if (KAssetManager.Instance.TryGetPet2DPrefab(modelName, out prefab))
        //    {
        //        return CreateModel(prefab, shopId);
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// 异步
        ///// </summary>
        ///// <param name="callback"></param>
        //public void GetModel(System.Action<GameObject> callback)
        //{
        //    GetModel(model, shopId, callback);
        //}

        //public static void GetModel(string modelName, int shopId, System.Action<GameObject> callback)
        //{
        //    KAssetManager.Instance.TryGetPet2DPrefab(modelName, (prefab) =>
        //    {
        //        var go = CreateModel(prefab, shopId);
        //        callback?.Invoke(go);
        //    });
        //}

        //private static GameObject CreateModel(GameObject prefab, int shopId)
        //{
        //    GameObject go = Object.Instantiate(prefab);
        //    go.AddComponent<KCatBehaviour>().catShopId = shopId;
        //    return go;
        //}


        //public GameObject GetUIModel()
        //{
        //    return GetUIModel(model, shopId);
        //}

        //public void GetUIModel(System.Action<GameObject> callback)
        //{
        //    GetUIModel(model, shopId, callback);
        //}

        //public static GameObject GetUIModel(string modelName, int shopId)
        //{
        //    GameObject prefab;
        //    if (KAssetManager.Instance.TryGetPet2DPrefab(modelName, out prefab))
        //    {
        //        return GetUIModel(prefab, shopId);
        //    }
        //    return null;
        //}

        //public static void GetUIModel(string modelName, int shopId, System.Action<GameObject> callback)
        //{
        //    KAssetManager.Instance.TryGetPet2DPrefab(modelName, (prefab) =>
        //   {
        //       var model = GetUIModel(prefab, shopId);
        //       if (callback != null)
        //       {
        //           callback(model);
        //       }
        //   });
        //}

        //private static GameObject GetUIModel(GameObject prefab, int shopId)
        //{
        //    var sAnim = prefab.GetComponent<SkeletonAnimation>();
        //    //Material material = Resources.Load<Material>("Materials/SkeletonGraphicDefault");
        //    var graphic = SkeletonGraphic.NewSkeletonGraphicGameObject(sAnim.skeletonDataAsset, null);//, material);
        //    graphic.rectTransform.pivot = new Vector2(0.5f, 0f);
        //    graphic.rectTransform.sizeDelta = new Vector2(450f, 600f);

        //    var graphicObject = graphic.gameObject;
        //    graphicObject.name = sAnim.name;
        //    graphicObject.layer = LayerMask.NameToLayer("UI");
        //    graphicObject.AddComponent<KCatBehaviour>().catShopId = shopId;
        //    return graphicObject;
        //}

        //public bool ContainColor(int color)
        //{
        //    return (colors & color) != 0;
        //}

        public Color GetColor()
        {
            return (Color)colors;
        }

        public string GetColorText()
        {
            var sb = new System.Text.StringBuilder();
            var tmpColor = GetColor();
            if ((tmpColor & Color.fRed) != 0)
            {
                sb.Append("<color=#f93535>红色</color> ");
            }
            if ((tmpColor & Color.fYellow) != 0)
            {
                sb.Append("<color=#ffc823>黄色</color> ");
            }
            if ((tmpColor & Color.fBlue) != 0)
            {
                sb.Append("<color=#4f4ffc>蓝色</color> ");
            }
            if ((tmpColor & Color.fGreen) != 0)
            {
                sb.Append("<color=#35dc61>绿色</color> ");
            }
            if ((tmpColor & Color.fPurple) != 0)
            {
                sb.Append("<color=#d83eff>紫色</color> ");
            }
            if ((tmpColor & Color.fBrown) != 0)
            {
                sb.Append("<color=#A0522D>棕色</color> ");
            }
            return sb.ToString();
        }

        //public static string GetCatColor(int color)
        //{
        //    var sb = new System.Text.StringBuilder();
        //    var tmpColor = (Color)color;
        //    if ((tmpColor & Color.fRed) != 0)
        //    {
        //        sb.Append("<color=#f93535>红色</color> ");
        //    }
        //    if ((tmpColor & Color.fYellow) != 0)
        //    {
        //        sb.Append("<color=#ffc823>黄色</color> ");
        //    }
        //    if ((tmpColor & Color.fBlue) != 0)
        //    {
        //        sb.Append("<color=#4f4ffc>蓝色</color> ");
        //    }
        //    if ((tmpColor & Color.fGreen) != 0)
        //    {
        //        sb.Append("<color=#35dc61>绿色</color> ");
        //    }
        //    if ((tmpColor & Color.fPurple) != 0)
        //    {
        //        sb.Append("<color=#d83eff>紫色</color> ");
        //    }
        //    if ((tmpColor & Color.fBrown) != 0)
        //    {
        //        sb.Append("<color=#A0522D>棕色</color> ");
        //    }
        //    return sb.ToString();
        //}

        #endregion

        //#region Comparison

        //public static int GetStarComparison(KCat c1, KCat c2)
        //{
        //    int starCp = c2.star.CompareTo(c1.star);
        //    if (starCp != 0)
        //    {
        //        return starCp;
        //    }

        //    int gradeCp = c2.grade.CompareTo(c1.grade);
        //    if (gradeCp != 0)
        //    {
        //        return gradeCp;
        //    }

        //    int rarityCp = c2.rarity.CompareTo(c1.rarity);
        //    if (rarityCp != 0)
        //    {
        //        return rarityCp;
        //    }

        //    int colorCp = c2.mainColor.CompareTo(c1.mainColor);
        //    if (colorCp != 0)
        //    {
        //        return colorCp;
        //    }

        //    int configCp = c2.shopId.CompareTo(c1.shopId);
        //    if (configCp != 0)
        //    {
        //        return configCp;
        //    }

        //    return c2.catId.CompareTo(c1.catId);
        //}

        //public static int GetSkillLvlOrderComparison(KCat c1, KCat c2)
        //{
        //    int starCp = c1.skillGrade.CompareTo(c2.skillGrade);
        //    if (starCp != 0)
        //    {
        //        return starCp;
        //    }

        //    int gradeCp = c2.grade.CompareTo(c1.grade);
        //    if (gradeCp != 0)
        //    {
        //        return gradeCp;
        //    }

        //    int rarityCp = c2.rarity.CompareTo(c1.rarity);
        //    if (rarityCp != 0)
        //    {
        //        return rarityCp;
        //    }

        //    int colorCp = c2.mainColor.CompareTo(c1.mainColor);
        //    if (colorCp != 0)
        //    {
        //        return colorCp;
        //    }

        //    int configCp = c2.shopId.CompareTo(c1.shopId);
        //    if (configCp != 0)
        //    {
        //        return configCp;
        //    }

        //    return c2.catId.CompareTo(c1.catId);
        //}
        //public static int GetGradeComparison(KCat c1, KCat c2)
        //{
        //    int gradeCp = c2.grade.CompareTo(c1.grade);
        //    if (gradeCp != 0)
        //    {
        //        return gradeCp;
        //    }

        //    int starCp = c2.star.CompareTo(c1.star);
        //    if (starCp != 0)
        //    {
        //        return starCp;
        //    }

        //    int rarityCp = c2.rarity.CompareTo(c1.rarity);
        //    if (rarityCp != 0)
        //    {
        //        return rarityCp;
        //    }

        //    int colorCp = c2.mainColor.CompareTo(c1.mainColor);
        //    if (colorCp != 0)
        //    {
        //        return colorCp;
        //    }

        //    int configCp = c2.shopId.CompareTo(c1.shopId);
        //    if (configCp != 0)
        //    {
        //        return configCp;
        //    }

        //    return c2.catId.CompareTo(c1.catId);
        //}

        //public static int GetRarityComparison(KCat c1, KCat c2)
        //{
        //    int rarityCp = c2.rarity.CompareTo(c1.rarity);
        //    if (rarityCp != 0)
        //    {
        //        return rarityCp;
        //    }

        //    int starCp = c2.star.CompareTo(c1.star);
        //    if (starCp != 0)
        //    {
        //        return starCp;
        //    }

        //    int gradeCp = c2.grade.CompareTo(c1.grade);
        //    if (gradeCp != 0)
        //    {
        //        return gradeCp;
        //    }

        //    int colorCp = c2.mainColor.CompareTo(c1.mainColor);
        //    if (colorCp != 0)
        //    {
        //        return colorCp;
        //    }

        //    int configCp = c2.shopId.CompareTo(c1.shopId);
        //    if (configCp != 0)
        //    {
        //        return configCp;
        //    }

        //    return c2.catId.CompareTo(c1.catId);
        //}

        //public static int GetColorComparison(KCat c1, KCat c2)
        //{
        //    int colorCp = c2.mainColor.CompareTo(c1.mainColor);
        //    if (colorCp != 0)
        //    {
        //        return colorCp;
        //    }

        //    int starCp = c2.star.CompareTo(c1.star);
        //    if (starCp != 0)
        //    {
        //        return starCp;
        //    }

        //    int gradeCp = c2.grade.CompareTo(c1.grade);
        //    if (gradeCp != 0)
        //    {
        //        return gradeCp;
        //    }

        //    int rarityCp = c2.rarity.CompareTo(c1.rarity);
        //    if (rarityCp != 0)
        //    {
        //        return rarityCp;
        //    }

        //    int configCp = c2.shopId.CompareTo(c1.shopId);
        //    if (configCp != 0)
        //    {
        //        return configCp;
        //    }

        //    return c2.catId.CompareTo(c1.catId);
        //}
        //public static int GetExploreAbilityComparison(KCat c1, KCat c2)
        //{

        //    int stateCp = c1.state.CompareTo(c2.state);
        //    if (stateCp != 0)
        //    {
        //        return stateCp;
        //    }
        //    int exploreAbility = c2.exploreAbility.CompareTo(c1.exploreAbility);
        //    if (exploreAbility != 0)
        //    {
        //        return exploreAbility;
        //    }
        //    int gradeCp = c2.grade.CompareTo(c1.grade);
        //    if (gradeCp != 0)
        //    {
        //        return gradeCp;
        //    }
        //    int rarityCp = c2.rarity.CompareTo(c1.rarity);
        //    if (rarityCp != 0)
        //    {
        //        return rarityCp;
        //    }
        //    int configCp = c2.shopId.CompareTo(c1.shopId);
        //    if (configCp != 0)
        //    {
        //        return configCp;
        //    }
        //    return c2.catId.CompareTo(c1.catId);
        //}
        ///// <summary>
        ///// 星级>品阶>技能等级>等级>静态ID>实例ID
        ///// </summary>
        ///// <param name="c1"></param>
        ///// <param name="c2"></param>
        ///// <returns></returns>
        //public static int GetCatNoramlComparison(KCat c1, KCat c2)
        //{
        //    int starCp = c2.star.CompareTo(c1.star);
        //    if (starCp != 0)
        //    {
        //        return starCp;
        //    }

        //    int rarityCp = c2.rarity.CompareTo(c1.rarity);
        //    if (rarityCp != 0)
        //    {
        //        return rarityCp;
        //    }
        //    int skillGrade = c1.skillGrade.CompareTo(c2.skillGrade);
        //    if (skillGrade != 0)
        //    {
        //        return skillGrade;
        //    }

        //    int gradeCp = c2.grade.CompareTo(c1.grade);
        //    if (gradeCp != 0)
        //    {
        //        return gradeCp;
        //    }

        //    int configCp = c2.shopId.CompareTo(c1.shopId);
        //    if (configCp != 0)
        //    {
        //        return configCp;
        //    }

        //    return c2.catId.CompareTo(c1.catId);
        //}
        ///// <summary>
        ///// 产金能力排序
        ///// </summary>
        ///// <param name="c1"></param>
        ///// <param name="c2"></param>
        ///// <returns></returns>
        //public static int GetCionComparison(KCat c1, KCat c2)
        //{
        //    int result = c1.state.CompareTo(c2.state);
        //    if (result != 0) return result;

        //    result =  c2.coinAbility.CompareTo(c1.coinAbility);
        //    if (result != 0) return result;

        //    result = c2.star.CompareTo(c1.star);
        //    if (result != 0) return result;

        //    result = c2.rarity.CompareTo(c1.rarity);
        //    if (result != 0) return result;

        //    return c2.catId.CompareTo(c1.catId);
        //}

        //#endregion
    }
}

