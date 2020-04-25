// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KCatManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;

namespace Game
{
    public class KCatManager : SingletonUnity<KCatManager>
    {
        #region Field

        private Dictionary<int, KCat> _allCats = new Dictionary<int, KCat>();

        #endregion

        #region Property

        public KCat[] allCats
        {
            get
            {
                var retArray = new KCat[_allCats.Count];
                _allCats.Values.CopyTo(retArray, 0);
                return retArray;
            }
        }

        public int allCatsCount
        {
            get { return _allCats.Count; }
        }

        #endregion

        #region Method

        public KCat GetCat(int id)
        {
            KCat cat;
            _allCats.TryGetValue(id, out cat);
            return cat;
        }

        //public List<KCat> GetCatsByColor(int color)
        //{
        //    var ret = new List<KCat>();
        //    foreach (var kv in _allCats)
        //    {
        //        int catColor = kv.Value.colors;
        //        if ((catColor & color) != 0)
        //        {
        //            ret.Add(kv.Value);
        //        }
        //    }
        //    return ret;
        //}

        public void OnSyncCats(IList<CatInfo> catInfos)
        {
            _allCats.Clear();

            if (catInfos == null || catInfos.Count == 0)
            {
                return;
            }

            foreach (var catInfo in catInfos)
            {
                _allCats.Add(catInfo.Id, CreateCat(catInfo));
            }
        }

        public void OnCreateCats(IList<CatInfo> catInfos)
        {
            if (catInfos == null || catInfos.Count == 0)
            {
                return;
            }

            foreach (var catInfo in catInfos)
            {
                KCat cat;
                if (_allCats.TryGetValue(catInfo.Id, out cat))
                {
                    SetCat(cat, catInfo);
                }
                else
                {
                    _allCats.Add(catInfo.Id, CreateCat(catInfo));
                }
            }
        }

        public void OnUpdateCats(IList<CatInfo> catInfos)
        {
            if (catInfos == null || catInfos.Count == 0)
            {
                return;
            }

            foreach (var catInfo in catInfos)
            {
                KCat cat;
                if (_allCats.TryGetValue(catInfo.Id, out cat))
                {
                    SetCat(cat, catInfo);
                }
                else
                {
                    _allCats.Add(catInfo.Id, CreateCat(catInfo));
                }
            }
        }

        public void OnDeleteCats(IList<int> cats)
        {
            if (cats == null || cats.Count == 0)
            {
                return;
            }

            for (int i = 0; i < cats.Count; i++)
            {
                var catId = cats[i];
                if (_allCats.Remove(catId))
                {

                }
            }
        }

        public void ProcessCommon(object protoData)
        {
            //猫咪获取
            if (protoData is S2CGetCatInfos)
            {
                var catInfos = (S2CGetCatInfos)protoData;
                OnSyncCats(catInfos.Cats);
            }
            //cat更新
            if (protoData is S2CCatsInfoUpdate)
            {
                var origin = (S2CCatsInfoUpdate)protoData;
                OnCreateCats(origin.AddCats);
                OnDeleteCats(origin.RemoveCats);
                OnUpdateCats(origin.UpdateCats);
            }
        }

        private KCat CreateCat(CatInfo catInfo)
        {
            return new KCat
            {
                catId = catInfo.Id,
                shopId = catInfo.CatCfgId,
                grade = catInfo.Level,
                star = catInfo.Star,
                exp = catInfo.Exp,
                nickName = catInfo.Nick,
                locked = catInfo.Locked,
                skillGrade = catInfo.SkillLevel,
                initCoinAbility = catInfo.CoinAbility,
                initExploreAbility = catInfo.ExploreAbility,
                initMatchAbility = catInfo.MatchAbility,
                state = catInfo.State,
            };
        }

        private void SetCat(KCat cat, CatInfo catInfo)
        {
            cat.grade = catInfo.Level;
            cat.star = catInfo.Star;
            cat.exp = catInfo.Exp;
            cat.nickName = catInfo.Nick;
            cat.locked = catInfo.Locked;
            cat.skillGrade = catInfo.SkillLevel;
            cat.initCoinAbility = catInfo.CoinAbility;
            cat.initExploreAbility = catInfo.ExploreAbility;
            cat.initMatchAbility = catInfo.MatchAbility;
            cat.state = catInfo.State;
        }
        /// <summary>
        /// 相同模板ID(shopId)的猫的总数
        /// </summary>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public int GetCatCountByShopID(int shopid) {
            int sameShopIDCatNum = 0;
            foreach (var item in _allCats.Values)
            {
                if (item.shopId == shopid)
                {
                    sameShopIDCatNum++;
                }
            }
            return sameShopIDCatNum;
        }

        #endregion

        //#region Unity  

        //// Update is called once per frame
        //private void Update()
        //{

        //}

        //#endregion
    }
}

