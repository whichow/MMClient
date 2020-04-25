using Game;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailTypeConst
{
    public const int System = 1;//ÏµÍ³ÓÊ¼þ
}

public class MailDataModel : DataModelBase<MailDataModel>
{
    public Dictionary<int,MailDataVO> mAllMailData { get; private set; }

    public MailDataVO GetMailVOById(int mailId)
    {
        if (mAllMailData.ContainsKey(mailId))
            return mAllMailData[mailId];
        return null;
    }

    public List<MailDataVO> OnMailData()
    {
        List<MailDataVO> lstMailVO = new List<MailDataVO>();
        foreach (var item in mAllMailData)
            lstMailVO.Add(item.Value);
        lstMailVO.Sort(SortMail);
        return lstMailVO;
    }

    private int SortMail(MailDataVO V0, MailDataVO V1)
    {
        if (V0.mIsRead == V1.mIsRead && V0.mIsHasAtt == V1.mIsHasAtt && V0.mIsGetAtt == V1.mIsGetAtt)
            return V0.mSendTime > V1.mSendTime ? -1 : 1;
        else if (V0.mIsRead == V1.mIsRead && V0.mIsHasAtt == V1.mIsHasAtt)
            return V0.mIsGetAtt == false ? -1 : 1;
        else if (V0.mIsRead == V1.mIsRead)
            return V0.mIsHasAtt == true ? -1 : 1;
        else if (V0.mIsRead != V1.mIsRead)
            return V0.mIsRead == false ? -1 : 1;
        return 0;
    }

    public void ExeMailData(S2CMailListResponse value)
    {
        if (mAllMailData != null)
            mAllMailData.Clear();
        mAllMailData = new Dictionary<int, MailDataVO>();
        if (value.Mails != null && value.Mails.Count > 0)
        {
            for (int i = 0; i < value.Mails.Count; i++)
            {
                MailDataVO vo = new MailDataVO();
                vo.OnInit(value.Mails[i]);
                mAllMailData.Add(value.Mails[i].Id, vo);
            }
        }
        DispatchEvent(MailEvent.MailData);
    }

    public void ExeMailDetail(S2CMailDetailResponse value)
    {
        for (int i = 0; i < value.Mails.Count; i++)
        {
            if (mAllMailData.ContainsKey(value.Mails[i].Id))
                mAllMailData[value.Mails[i].Id].OnDetail(value.Mails[i]);
        }
        EventData eventData = new EventData();
        eventData.Integer = value.Mails[0].Id;
        DispatchEvent(MailEvent.MailDetail, eventData);
    }

    public void ExeMailGetAtt(S2CMailGetAttachedItemsResponse value)
    {
        for (int i = 0; i < value.MailIds.Count; i++)
        {
            if (mAllMailData.ContainsKey(value.MailIds[i]))
                mAllMailData[value.MailIds[i]].OnIsGetAtt();
        }
        List<ItemInfo> lstInfo = new List<ItemInfo>();
        lstInfo.AddRange(value.AttachedItems);
        EventData eventData = new EventData();
        eventData.Data = lstInfo;
        DispatchEvent(MailEvent.MailGetAtt, eventData);
    }

    public void ExeMailDelete(S2CMailDeleteResponse value)
    {
        for (int i = 0; i < value.MailIds.Count; i++)
        {
            if (mAllMailData.ContainsKey(value.MailIds[i]))
                mAllMailData.Remove(value.MailIds[i]);
        }
        DispatchEvent(MailEvent.MailDelete);
    }

    public void ExeNewNotify(S2CMailsNewNotify value)
    {
        if (mAllMailData == null)
            mAllMailData = new Dictionary<int, MailDataVO>();
        for (int i = 0; i < value.Mails.Count; i++)
        {
            if (mAllMailData.ContainsKey(value.Mails[i].Id))
            {
                mAllMailData[value.Mails[i].Id].OnInit(value.Mails[i]);
            }
            else
            {
                MailDataVO vo = new MailDataVO();
                vo.OnInit(value.Mails[i]);
                mAllMailData.Add(value.Mails[i].Id, vo);
            }
        }
        DispatchEvent(MailEvent.MailNewNotify);
    }
}
