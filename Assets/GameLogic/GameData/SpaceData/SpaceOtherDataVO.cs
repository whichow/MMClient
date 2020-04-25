using Game;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceOtherDataVO
{
    public int mPlayerId { get; private set; }
    public string mPlayerName { get; private set; }
    public int mPlayerLevel { get; private set; }
    public int mPlayerHead { get; private set; }
    public int mZaned { get; private set; }
    public int mFocusNum { get; private set; }
    public int mCharm { get; private set; }
    public int mGender { get; private set; }
    public List<SpaceCatData> mLstSpaceCat { get; private set; }
    public List<int> mFashionId { get; private set; }

    public void OnInit(S2CSpaceDataResponse value)
    {
        mPlayerId = value.PlayerId;
        mPlayerName = value.PlayerName;
        mPlayerLevel = value.PlayerLevel;
        mPlayerHead = value.PlayerHead;
        mZaned = value.Zaned;
        mFocusNum = value.BeFocusNum;
        mCharm = value.Charm;
        mGender = value.Gender;
        mLstSpaceCat = new List<SpaceCatData>();
        mLstSpaceCat.AddRange(value.Cats);
        mFashionId = new List<int>();
        mFashionId.AddRange(value.FashionIds);
    }

    public void AddFocus()
    {
        mFocusNum += 1;
    }

    public void RemoveFocus()
    {
        mFocusNum -= 1;
    }
}
