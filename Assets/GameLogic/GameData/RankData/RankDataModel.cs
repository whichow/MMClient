using Game;
using Game.Match3;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankType
{
    public const int Checkpoint= 1;//关卡
    public const int Charm = 2;//魅力
    public const int OuQi  = 3;//欧气
    public const int Fabulous = 4;//赞
}
public class RankDataModel : DataModelBase<RankDataModel>
{
    public Dictionary<int, List<RankItemInfo>> mDictRankData { get; private set; } = new Dictionary<int, List<RankItemInfo>>();
    public Dictionary<int, int> mDictSelfMaxRank { get; private set; } = new Dictionary<int, int>();
    public Dictionary<int, int> mDictSelfRank { get; private set; } = new Dictionary<int, int>();
    public Dictionary<int, int> mDictSelfValue1 { get; private set; } = new Dictionary<int, int>();
    public Dictionary<int, int> mDictSelfValue2 { get; private set; } = new Dictionary<int, int>();

    //排行榜数据
    public void ExeRankData(S2CRankListResponse value)
    {
        List<RankItemInfo> listRankVO = new List<RankItemInfo>();
        listRankVO.AddRange(value.RankItems);
        if (mDictRankData.ContainsKey(value.RankListType))
            mDictRankData[value.RankListType] = listRankVO;
        else
            mDictRankData.Add(value.RankListType, listRankVO);
        if (mDictSelfMaxRank.ContainsKey(value.RankListType))
            mDictSelfMaxRank[value.RankListType] = value.SelfHistoryTopRank;
        else
            mDictSelfMaxRank.Add(value.RankListType, value.SelfHistoryTopRank);
        if (mDictSelfRank.ContainsKey(value.RankListType))
            mDictSelfRank[value.RankListType] = value.SelfRank;
        else
            mDictSelfRank.Add(value.RankListType, value.SelfRank);
        if (mDictSelfValue1.ContainsKey(value.RankListType))
            mDictSelfValue1[value.RankListType] = value.SelfValue;
        else
            mDictSelfValue1.Add(value.RankListType, value.SelfValue);
        if (mDictSelfValue2.ContainsKey(value.RankListType))
            mDictSelfValue2[value.RankListType] = value.SelfValue2;
        else
            mDictSelfValue2.Add(value.RankListType, value.SelfValue2);
        DispatchEvent(RankEvent.RankData);
    }
}
