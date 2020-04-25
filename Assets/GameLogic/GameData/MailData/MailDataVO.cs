using Game;
using Game.DataModel;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailDataVO
{
    public int mMailID { get; private set; }//邮件ID
    public int mMailType { get; private set; }//邮件类型
    public int mMailSubType { get; private set; }//邮件子类型
    public string mTitle { get; private set; }//邮件标题
    public string mSendName { get; private set; }//发送者名称
    public int mSendTime { get; private set; }//发送时间
    public int mLeftSecsTime { get; private set; }//剩余时间
    public bool mIsRead { get; private set; }//是否已读
    public bool mIsGetAtt { get; private set; }//是否已领取
    public bool mIsHasAtt { get; private set; }//是否有附件
    public int mValue { get; private set; }//额外数据
    public string mContent { get; private set; }//邮件内容
    public List<ItemInfo> mLstAtt { get; private set; }//附件
    public MailXDM mMailXDM { get; private set; }
    public bool mIsDetail { get; private set; }
    

    public void OnInit(MailBasicData value)
    {
        mMailID = value.Id;
        mMailType = value.Type;
        mMailSubType = value.Subtype;
        mSendName = value.SenderName;
        mSendTime = value.SendTime;
        mLeftSecsTime = value.LeftSecs + (int)Time.realtimeSinceStartup;
        mIsRead = value.IsRead;
        mIsGetAtt = value.IsGetAttached;
        mIsHasAtt = value.HasAttached;
        mValue = value.Value;
        if (mMailSubType > 0)
        {
            mMailXDM = XTable.MailXTable.GetByID(mMailSubType);
            if (mMailXDM == null)
            {
                mTitle = value.Title;
                Debuger.LogError("ID:" + mMailSubType);
            }
            else
            {
                mTitle = KLocalization.GetLocalString(mMailXDM.MailTitleID);
            }
        }
        else
        {
            mTitle = value.Title;
        }
        mIsDetail = false;
    }

    public int LeftSecsTime
    {
        get { return mLeftSecsTime - (int)Time.realtimeSinceStartup; }
    }

    public void OnDetail(MailDetail value)
    {
        mIsDetail = true;
        if (mMailSubType > 0)
        {
            if (mMailXDM == null)
            {
                mContent = value.Content;
                Debuger.LogError("ID:" + mMailSubType);
            }
            else
            {
                mContent = KLocalization.GetLocalString(mMailXDM.MailContentID);
            }
        }
        else
        {
            mContent = value.Content;
        }
        mLstAtt = new List<ItemInfo>();
        mLstAtt.AddRange(value.AttachedItems);
        mIsRead = true;
    }

    public void OnIsGetAtt()
    {
        mIsRead = true;
        mIsGetAtt = true;
    }
}
