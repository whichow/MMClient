using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendDataVO
{
    public int mPlayerId { get; private set; }//好友玩家ID
    public string mName { get; private set; }//好友昵称
    public int mHeadId { get; private set; }//好友头像ID
    public int mLevel { get; private set; }//好友等级
    public int mVipLevel { get; private set; }//好友VIP等级
    public int mLastLogin { get; private set; }//好友上次登陆时间
    public int mFriendPoint { get; private set; }//友情点
    public int mLeftGiveSecond { get; private set; }//剩余赠送时间
    public int mUnreadMessageNum { get; private set; }//未读消息数量
    public int mZanNum { get; private set; }//赞数量
    public bool mIsZan { get; private set; }//是否赞过
    public bool mIsOnLine { get; private set; }//是否在线


    

    public void OnInit(FriendInfo info)
    {
        mPlayerId = info.PlayerId;
        mName = info.Name;
        mHeadId = info.Head;
        mLevel = info.Level;
        mVipLevel = info.VipLevel;
        mLastLogin = info.LastLogin;
        mFriendPoint = info.FriendPoints;
        mLeftGiveSecond = info.LeftGiveSeconds + (int)Time.realtimeSinceStartup;
        mUnreadMessageNum = info.UnreadMessageNum;
        mZanNum = info.Zan;
        mIsZan = info.IsZan;
        mIsOnLine = info.IsOnline;
    }

    public int LeftGiveSecond
    {
        get { return mLeftGiveSecond - (int)Time.realtimeSinceStartup; }
    }

    public void OnLeftGiveSecond(int time)
    {
        mLeftGiveSecond = time + (int)Time.realtimeSinceStartup;
    }

    public void OnFriendPoint()
    {
        mFriendPoint = 0;
    }

    public void OnZanNum(int num)
    {
        mIsZan = true;
        mZanNum = num;
    }
}
