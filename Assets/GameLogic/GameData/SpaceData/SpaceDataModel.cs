using Game;
using Game.UI;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenderConst
{
    public const int All = 0;//未设置性别
    public const int Man = 1;//男
    public const int Girl = 2;//女
}
public class FormTypeConst
{
    public const int None = 0;//所以
    public const int Head = 1;//头
    public const int Clothes = 2;//衣服
    public const int Trousers = 3;//裤子
    public const int Shoes = 4;//鞋子
}

public class SpaceDataModel : DataModelBase<SpaceDataModel>
{
    public int mGender { get; private set; } = 0;
    public List<int> mLstFashionId { get; private set; }
    public int mFocusNum { get; private set; } = 0;
    public List<int> mLstCatId { get; private set; }
    public SpaceOtherDataVO mSpaceOtherDataVO { get; private set; }
    public List<FocusPlayer> mLstFocus { get; private set; }

    public bool OnIsExhibition(int catId)
    {
        for (int i = 0; i < mLstCatId.Count; i++)
        {
            if (mLstCatId[i] == catId)
                return true;
        }
        return false;
    }

    public bool IsFocus(int id)
    {
        for (int i = 0; i < mLstFocus.Count; i++)
        {
            if (mLstFocus[i].Id == id)
                return true;
        }
        return false;
    }

    public void ExeFocusData(S2CFocusDataResponse value)
    {
        mFocusNum = value.BeFocusNum;
        if (mLstFocus == null)
            mLstFocus = new List<FocusPlayer>();
        mLstFocus.AddRange(value.Players);
    }

    public void ExeSpaceOtherData(S2CSpaceDataResponse value)
    {
        if (mSpaceOtherDataVO == null)
            mSpaceOtherDataVO = new SpaceOtherDataVO();
        mSpaceOtherDataVO.OnInit(value);
        KUIWindow.OpenWindow<SpaceOtherWindow>();
    }

    public void ExeFocusPlayer(S2CFocusPlayerResponse value)
    {
        //关注玩家
        mFocusNum += 1;
        mLstFocus.Add(value.PlayerInfo);
        mSpaceOtherDataVO.AddFocus();
        DispatchEvent(SpaceEvent.Focus);
    }

    public void ExeFocusCancal(S2CFocusPlayerCancelResponse value)
    {
        //取消关注
        mFocusNum -= 1;
        for (int i = mLstFocus.Count - 1; i >= 0; i--)
        {
            if (mLstFocus[i].Id == value.PlayerId)
                mLstFocus.Remove(mLstFocus[i]);
        }
        mSpaceOtherDataVO.RemoveFocus();
        DispatchEvent(SpaceEvent.FocusCancel);
    }

    public void ExePictureData(S2CMyPictureDataResponse value)
    {
        if (mLstCatId == null)
            mLstCatId = new List<int>();
        for (int i = 0; i < value.CatIds.Count; i++)
            mLstCatId.Add(value.CatIds[i]);
    }

    public void ExeSetPicture(S2CMyPictureSetResponse value)
    {
        if (value.IsCancel)
        {
            for (int i = mLstCatId.Count - 1; i >= 0; i--)
            {
                if (mLstCatId[i] == value.CatId)
                    mLstCatId.Remove(mLstCatId[i]);
            }
        }
        else
        {
            mLstCatId.Add(value.CatId);
        }
        DispatchEvent(SpaceEvent.SetPicture);
    }

    public void ExeSetGender(S2CSpaceSetGenderResponse value)
    {
        mGender = value.Gender;
        DispatchEvent(SpaceEvent.SetGender);
    }

    public void ExeFashionData(S2CSpaceFashionDataResponse value)
    {
        mGender = value.Gender;
        if (mLstFashionId != null)
            mLstFashionId.Clear();
        mLstFashionId = new List<int>();
        mLstFashionId.AddRange(value.FashionIds);
        DispatchEvent(SpaceEvent.FashionData);
    }

    public void ExeFashionSave(S2CSpaceFashionSaveResponse value)
    {
        if (mLstFashionId != null)
            mLstFashionId.Clear();
        mLstFashionId = new List<int>();
        mLstFashionId.AddRange(value.FashionIds);
        DispatchEvent(SpaceEvent.FashionSave);
    }

    public void ExeCatUnlock(S2CSpaceCatUnlockResponse value)
    {
        List<SpaceCatData> lstSpaceCat = new List<SpaceCatData>();
        lstSpaceCat = mSpaceOtherDataVO.mLstSpaceCat;
        for (int i = 0; i < lstSpaceCat.Count; i++)
        {
            if (mSpaceOtherDataVO.mPlayerId == value.PlayerId && lstSpaceCat[i].CatId == value.CatId)
            {
                lstSpaceCat[i].IsUnlock = value.IsUnlock;
                DispatchEvent(SpaceEvent.CatUnlock);
            }
        }
    }
}
