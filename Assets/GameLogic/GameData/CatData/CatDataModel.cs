using Msg.ClientMessage;
using System.Collections.Generic;

namespace Game
{
    public class CatRarityConst
    {
        public const int All = 0;
        public const int N = 1;
        public const int R = 2;
        public const int SR = 3;
        public const int SSR = 4;
    }
    public class CatColorConst
    {
        public const int Red = 1;
        public const int Yellow = 2;
        public const int Blue = 4;
        public const int Green = 8;
        public const int Purple = 16;
        public const int Brown = 32;
    }
    public class CatStateConst
    {
        public const int None = 0;
        public const int Cattery = 1;//猫舍
        public const int Explore = 2;//探索
        public const int Foster = 3;//寄养所
    }
    public class CatSortConst
    {
        public const int Rarity = 0;
        public const int Color = 1;
    }
    public class CatOpenType
    {
        public const int Normal = 0;//正常
        public const int Space = 1;//自己空间
        public const int SpaceOther = 2;//别人空间
    }

    public class CatDataModel : DataModelBase<CatDataModel>
    {
        private Dictionary<int, CatDataVO> _dictAllCats;

        #region Get

        public List<CatDataVO> GetAllCatDataVO()
        {
            List<CatDataVO> lstVO = new List<CatDataVO>();
            foreach (var item in _dictAllCats)
                lstVO.Add(item.Value);
            return lstVO;
        }

        public List<CatDataVO> GetCatsByColor(int color)
        {
            List<CatDataVO> lstVO = new List<CatDataVO>();
            foreach (var vo in _dictAllCats)
            {
                int catColor = vo.Value.mCatXDM.Color;
                if ((catColor & color) != 0)
                    lstVO.Add(vo.Value);
            }
            return lstVO;
        }

        public CatDataVO GetCatDataVOById(int catId)
        {
            if (_dictAllCats.ContainsKey(catId))
                return _dictAllCats[catId];
            return null;
        }

        public List<CatDataVO> GetLstCatDataById(List<int> lstId)
        {
            List<CatDataVO> lstCatVO = new List<CatDataVO>();
            for (int i = 0; i < lstId.Count; i++)
            {
                if (_dictAllCats.ContainsKey(lstId[i]))
                    lstCatVO.Add(_dictAllCats[lstId[i]]);
            }
            return lstCatVO;
        }

        public List<CatDataVO> GetCatDataByType(int type)
        {
            List<CatDataVO> lstCatInfo = new List<CatDataVO>();
            if (type > 0)
            {
                foreach (var item in _dictAllCats)
                {
                    if (item.Value.mCatXDM.Rarity == type)
                        lstCatInfo.Add(item.Value);
                }
            }
            else
            {
                foreach (var item in _dictAllCats)
                    lstCatInfo.Add(item.Value);
            }
            return lstCatInfo;
        }
 
        #endregion

        #region Set

        public void ExeCatUpdate(S2CCatsInfoUpdate value)
        {
            if (value.AddCats.Count > 0)
            {
                for (int i = 0; i < value.AddCats.Count; i++)
                {
                    CatDataVO vo = new CatDataVO();
                    vo.OnInit(value.AddCats[i]);
                    _dictAllCats.Add(value.AddCats[i].Id, vo);
                }
            }
            if (value.UpdateCats.Count > 0)
            {
                for (int i = 0; i < value.UpdateCats.Count; i++)
                {
                    if (_dictAllCats.ContainsKey(value.UpdateCats[i].Id))
                    {
                        _dictAllCats[value.UpdateCats[i].Id].OnInit(value.UpdateCats[i]);
                    }
                    else
                    {
                        CatDataVO vo = new CatDataVO();
                        vo.OnInit(value.UpdateCats[i]);
                        _dictAllCats.Add(value.UpdateCats[i].Id, vo);
                    }
                }
            }
            DispatchEvent(CatEvent.CatDataRefresh);
        }

        public void ExeCatData(S2CGetCatInfos value)
        {
            if (_dictAllCats != null)
                _dictAllCats.Clear();
            _dictAllCats = new Dictionary<int, CatDataVO>();
            for (int i = 0; i < value.Cats.Count; i++)
            {
                CatDataVO vo = new CatDataVO();
                vo.OnInit(value.Cats[i]);
                _dictAllCats.Add(value.Cats[i].Id, vo);
            }
            DispatchEvent(CatEvent.CatAllDataRefresh);
        }

        public void ExeCatNick(S2CCatRenameNickResult value)
        {
            _dictAllCats[value.CatId].mCatInfo.Nick = value.NewNick;
            DispatchEvent(CatEvent.CatNick);
        }

        public void ExeCatDecompose(S2CCatDecomposeResult value)
        {
            for (int i = 0; i < value.CatId.Count; i++)
            {
                if (_dictAllCats.ContainsKey(value.CatId[i]))
                    _dictAllCats.Remove(value.CatId[i]);
            }
            EventData eventData = new EventData();
            eventData.Integer = value.GetSoulStone;
            DispatchEvent(CatEvent.CatDecompose, eventData);
        }

        #endregion

        #region 排序

        public int NormalSortItem(CatDataVO a, CatDataVO b)
        {
            int val = 0;
            if (val == 0) val = b.mCatXDM.Rarity.CompareTo(a.mCatXDM.Rarity);
            if (val == 0) val = b.mCatInfo.Star.CompareTo(a.mCatInfo.Star);
            if (val == 0) val = b.mCatXDM.ID.CompareTo(a.mCatXDM.ID);
            if (val == 0) val = b.mCatInfo.Id.CompareTo(a.mCatInfo.Id);
            return val;
        }

        public int MatchSortItem(CatDataVO a, CatDataVO b)
        {
            int val = 0;
            if (val == 0) val = b.mCatInfo.MatchAbility.CompareTo(a.mCatInfo.MatchAbility);
            if (val == 0) val = b.mCatXDM.Rarity.CompareTo(a.mCatXDM.Rarity);
            if (val == 0) val = b.mCatInfo.Star.CompareTo(a.mCatInfo.Star);
            if (val == 0) val = b.mCatXDM.ID.CompareTo(a.mCatXDM.ID);
            if (val == 0) val = b.mCatInfo.Id.CompareTo(a.mCatInfo.Id);
            return val;
        }

        public int CionSortItem(CatDataVO a, CatDataVO b)
        {
            int val = 0;
            if (val == 0) val = a.mCatInfo.State.CompareTo(b.mCatInfo.State);
            if (val == 0) val = b.mCatInfo.CoinAbility.CompareTo(a.mCatInfo.CoinAbility);
            if (val == 0) val = b.mCatInfo.Star.CompareTo(a.mCatInfo.Star);
            if (val == 0) val = b.mCatXDM.Rarity.CompareTo(a.mCatXDM.Rarity);
            if (val == 0) val = b.mCatXDM.ID.CompareTo(a.mCatXDM.ID);
            if (val == 0) val = b.mCatInfo.Id.CompareTo(a.mCatInfo.Id);
            return val;
        }

        public int ExploreSortItem(CatDataVO a, CatDataVO b)
        {
            int val = 0;
            if (val == 0) val = a.mCatInfo.State.CompareTo(b.mCatInfo.State);
            if (val == 0) val = b.mCatInfo.ExploreAbility.CompareTo(a.mCatInfo.ExploreAbility);
            if (val == 0) val = b.mCatInfo.Star.CompareTo(a.mCatInfo.Star);
            if (val == 0) val = b.mCatXDM.Rarity.CompareTo(a.mCatXDM.Rarity);
            if (val == 0) val = b.mCatXDM.ID.CompareTo(a.mCatXDM.ID);
            if (val == 0) val = b.mCatInfo.Id.CompareTo(a.mCatInfo.Id);
            return val;
        }

        #endregion

    }

}