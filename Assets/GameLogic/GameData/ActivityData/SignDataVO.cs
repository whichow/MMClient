using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;
using Game.DataModel;

public class SignDataVO
{
    public List<SignXDM> mListSignXDM { get; private set; }
    public int mMinIndex { get; private set; }
    public int mMaxIndex { get; private set; }
    public int mSignTime { get; private set; }
    private int _curGroup = 0;

    public void OnInit(S2CSignDataResponse value)
    {
        mMinIndex = value.TakeAwardIndex;
        mMaxIndex = value.SignedIndex;
        mSignTime = value.NextSignRemainSeconds + (int)Time.realtimeSinceStartup;
        OnListConfig(mMinIndex);
    }

    private void OnListConfig(int id)
    {
        SignXDM signXDM = XTable.SignXTable.GetByID(id + 1);
        int group = signXDM.Group;
        if (_curGroup != group)
        {
            if (mListSignXDM != null)
                mListSignXDM.Clear();
            mListSignXDM = new List<SignXDM>();
            List<SignXDM> lstSignXDMs = XTable.SignXTable.GetAllList();
            for (int i = 0; i < lstSignXDMs.Count; i++)
            {
                if (lstSignXDMs[i].Group == group)
                    mListSignXDM.Add(lstSignXDMs[i]);
            }
            mListSignXDM.Sort((x, y) => x.ID.CompareTo(y.ID));
        }
        _curGroup = group;
    }

    public int SignTime
    {
        get { return mSignTime - (int)Time.realtimeSinceStartup; }
    }

    public void OnAward(int indexs)
    {
        mMinIndex = indexs;
        OnListConfig(mMinIndex);
    }
}
