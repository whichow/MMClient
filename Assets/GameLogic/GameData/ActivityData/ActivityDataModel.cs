using Game;
using Game.DataModel;
using Msg.ClientMessage;
using System.Collections.Generic;

public class ActivityTypeConst
{
    public const int FirstCharge = 0;//首充
    public const int EverydayReward = 1;//每日奖励
    public const int MonthlyCard = 2;//月卡
}

public class ActivityDataModel : DataModelBase<ActivityDataModel>
{
    public MonthCardData mMonthCardData { get;private set; }
    public int mFirstChargeState { get; private set; }
    public List<int> mLstStrId { get; private set; }
    public SignDataVO mSignDataVO { get; private set; }
    public List<ItemInfo> mChargeReward { get; private set; }

    
    //充值数据
    public void ExeChargeData(S2CChargeDataResponse value)
    {
        if (value.Datas != null && value.Datas.Count > 0)
        {
            mMonthCardData = new MonthCardData();
            mMonthCardData = value.Datas[0];
        }
        if (mLstStrId == null)
            mLstStrId = new List<int>();
        mFirstChargeState = value.FirstChargeState;
        mLstStrId.AddRange(value.ChargedIds);
        if (mChargeReward!=null)
            mChargeReward.Clear();
        mChargeReward = new List<ItemInfo>();
        GlobalXDM globalXDM = XTable.GlobalXTable.GetByID(3);
        if (globalXDM.intListVal != null && globalXDM.intListVal.Count % 2 == 0)
        {
            for (int i = 0; i < globalXDM.intListVal.Count; i += 2)
            {
                ItemInfo itemInfo = new ItemInfo();
                itemInfo.ItemCfgId = globalXDM.intListVal[i];
                itemInfo.ItemNum = globalXDM.intListVal[i + 1];
                mChargeReward.Add(itemInfo);
            }
        }
        DispatchEvent(ActivityEvent.ChargeData);
    }
    //首充领奖
    public void ExeChargeReward(S2CChargeFirstAwardResponse value)
    {
        mFirstChargeState = 2;
        EventData eventData = new EventData();
        List<ItemInfo> itemInfos = new List<ItemInfo>();
        itemInfos.AddRange(value.Rewards);
        eventData.Data = itemInfos;
        DispatchEvent(ActivityEvent.ChargeReward, eventData);
    }
    //首充通知
    public void ExeChargeNotify(S2CChargeFirstRewardNotify value)
    {
        mFirstChargeState = 1;
        DispatchEvent(ActivityEvent.ChargeNotify);
    }
    //签到数据
    public void ExeSignData(S2CSignDataResponse value)
    {
        if (mSignDataVO == null)
            mSignDataVO = new SignDataVO();
        mSignDataVO.OnInit(value);
        DispatchEvent(ActivityEvent.SignData);
    }
    //签到领奖
    public void ExeSignReward(S2CSignAwardResponse value)
    {
        mSignDataVO.OnAward(value.Index);
        EventData eventData = new EventData();
        List<ItemInfo> itemInfos = new List<ItemInfo>();
        itemInfos.AddRange(value.Rewards);
        eventData.Data = itemInfos;
        DispatchEvent(ActivityEvent.SignReward, eventData);
    }
}
