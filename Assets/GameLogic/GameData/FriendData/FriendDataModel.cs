using Game;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendTypeConst
{
    public const int Friend = 0;//好友
    public const int AddFriend = 1;//添加好友
}

public class FriendDataModel : DataModelBase<FriendDataModel>
{
    public Dictionary<int, FriendDataVO> _dictAllFriends { get; private set; } = new Dictionary<int, FriendDataVO>();//好友列表
    public Dictionary<int, FriendReq> _dictAllApply { get; private set; } = new Dictionary<int, FriendReq>();//申请列表
    public int mLeftGivePointNum { get; private set; } = 0;//剩余赠送次数
    public bool isData { get; private set; } = false;


    public List<FriendDataVO> GetAllFriend()
    {
        List<FriendDataVO> lstFriendVO = new List<FriendDataVO>();
        foreach (var item in _dictAllFriends)
            lstFriendVO.Add(item.Value);
        lstFriendVO.Sort((x, y) => x.mPlayerId.CompareTo(y.mPlayerId));
        return lstFriendVO;
    }

    public List<FriendReq> GetAllFriendReq()
    {
        List<FriendReq> lstFriendVO = new List<FriendReq>();
        foreach (var item in _dictAllApply)
            lstFriendVO.Add(item.Value);
        return lstFriendVO;
    }

    public void ExeFriendData(S2CRetFriendListResult value)
    {
        isData = true;
        _dictAllFriends.Clear();
        _dictAllApply.Clear();
        for (int i = 0; i < value.FriendList.Count; i++)
        {
            FriendDataVO vo = new FriendDataVO();
            vo.OnInit(value.FriendList[i]);
            _dictAllFriends.Add(value.FriendList[i].PlayerId, vo);
        }
        for (int i = 0; i < value.Reqs.Count; i++)
            _dictAllApply.Add(value.Reqs[i].PlayerId, value.Reqs[i]);
        mLeftGivePointNum = value.LeftGivePointsNum;
        DispatchEvent(FriendEvent.FriendAllData);
    }

    public void ExeAddFriend(S2CAddFriendResult value)
    {
        EventData eventData = new EventData();
        eventData.Integer = value.PlayerId;
        DispatchEvent(FriendEvent.FriendAdd, eventData);
    }

    public void ExeFriendSearch(S2CFriendSearchResult value)
    {
        FriendDataModelVO eventData = new FriendDataModelVO();
        eventData.mLstInfo.AddRange(value.Result);
        DispatchEvent(FriendEvent.FriendSearch, eventData);
    }

    public void ExeFriendNotify(S2CFriendStateNotify value)
    {
        if (value.StateType == 0)
        {
            if (_dictAllFriends.ContainsKey(value.Info.PlayerId))
                _dictAllFriends[value.Info.PlayerId].OnInit(value.Info);
        }
        else
        {
            FriendDataVO vo = new FriendDataVO();
            vo.OnInit(value.Info);
            _dictAllFriends.Add(value.Info.PlayerId, vo);
        }
        DispatchEvent(FriendEvent.FriendNotify);
    }

    public void ExeFriendReqNotify(S2CFriendReqNotify value)
    {
        _dictAllApply.Add(value.Req.PlayerId, value.Req);
        DispatchEvent(FriendEvent.FriendReqNotify);
    }

    public void ExeAgreeFriend(S2CAgreeFriendResult value)
    {
        if (_dictAllApply.ContainsKey(value.PlayerId))
            _dictAllApply.Remove(value.PlayerId);
        DispatchEvent(FriendEvent.FriendReplyAdd);
    }

    public void ExeRefuseFriend(S2CRefuseFriendResult value)
    {
        if (_dictAllApply.ContainsKey(value.PlayerId))
            _dictAllApply.Remove(value.PlayerId);
        DispatchEvent(FriendEvent.FriendRefuse);
    }

    public void ExeRemoveFriend(S2CRemoveFriendResult value)
    {
        if (_dictAllFriends.ContainsKey(value.PlayerId))
            _dictAllFriends.Remove(value.PlayerId);
        DispatchEvent(FriendEvent.FriendRemove);
    }

    public void ExeGiveFriendPoint(S2CGiveFriendPointsResult value)
    {
        mLeftGivePointNum = value.LeftGivePointsNum;
        int backPoint = 0;
        for (int i = 0; i < value.PointsData.Count; i++)
        {
            if (_dictAllFriends.ContainsKey(value.PointsData[i].FriendId))
            {
                if (value.PointsData[i].Error >= 0)
                {
                    _dictAllFriends[value.PointsData[i].FriendId].OnLeftGiveSecond(value.PointsData[i].RemainSeconds);
                    backPoint += value.PointsData[i].BackPoints;
                }
                else
                {
                    //失败错误码
                }
            }
        }
        if (backPoint > 0)
        {
            EventData eventData = new EventData();
            eventData.Integer = backPoint;
            DispatchEvent(FriendEvent.FriendGivePoint, eventData);
        }
    }

    public void ExeGetFriendPoint(S2CGetFriendPointsResult value)
    {
        int pointNum = 0;
        for (int i = 0; i < value.PointsData.Count; i++)
        {
            if (_dictAllFriends.ContainsKey(value.PointsData[i].FriendId))
            {
                _dictAllFriends[value.PointsData[i].FriendId].OnFriendPoint();
                pointNum += value.PointsData[i].Points;
            }
        }
        if (pointNum > 0)
        {
            EventData eventData = new EventData();
            eventData.Integer = pointNum;
            DispatchEvent(FriendEvent.FriendGetPoint, eventData);
        }
    }

    public void ExeZanPlayer(S2CZanPlayerResult value)
    {
        if (_dictAllFriends.ContainsKey(value.PlayerId))
            _dictAllFriends[value.PlayerId].OnZanNum(value.TotalZan);
        DispatchEvent(FriendEvent.FriendZan);
    }
}

public class FriendDataModelVO : IEventData
{
    public List<FriendInfo> mLstInfo = new List<FriendInfo>();
}
