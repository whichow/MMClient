using Game;
using Game.DataModel;
using Msg.ClientMessage;
using System.Collections.Generic;

public class ShopIDConst
{
    public const int Special = 1;//特殊商店
    public const int FriendPoint = 2;//友情点商店
    public const int CharmMedal = 3;//魅力勋章商店
    public const int Diamond = 4;//钻石商店
    public const int SoulStone = 5;//魂石商店
    public const int Other = 6;//其他商店
    public const int Attire = 7;//装扮商店
    public const int BodyPower = 8;//体力商店
}
public class ShopTypeConst
{
    public const int Prop = 0;//道具
    public const int SoulStone = 1;//魂石商店
    public const int Diamond = 2;//钻石商店
    public const int Attire = 3;//装扮商店
}
public class PropShopConst
{
    public const int Special = 1;//特殊商店
    public const int FriendPoint = 2;//友情点商店
    public const int CharmMedal = 3;//魅力勋章商店
}
public class SoulStoneConst
{
    public const int SoulStoneExchange = 0;//魂石兑换
    public const int SoulStoneObtain = 1;//魂石获取
}


public class ShopDataModel : DataModelBase<ShopDataModel>
{
    public Dictionary<int, List<ShopDataVO>> _allShop { get; private set; } = new Dictionary<int, List<ShopDataVO>>();

    public void ExeShopData(S2CShopItemsResult value)
    {
        List<ShopDataVO> lstVO = new List<ShopDataVO>();
        ShopDataVO vo;
        if (value.Items != null && value.Items.Count > 0)
        {
            for (int i = 0; i < value.Items.Count; i++)
            {
                vo = new ShopDataVO();
                vo.OnInit(value.Items[i]);
                lstVO.Add(vo);
            }
            lstVO.Sort((x, y) => x.mItemId.CompareTo(y.mItemId));
            if (_allShop.ContainsKey(value.ShopId))
                _allShop[value.ShopId] = lstVO;
            else
                _allShop.Add(value.ShopId, lstVO);
            EventData eventData = new EventData();
            eventData.Integer = value.ShopId;
            DispatchEvent(ShopEvent.ShopData, eventData);
        }
    }

    public void ExeBuyItem(S2CBuyShopItemResult value)
    {
        if (_allShop.ContainsKey(value.ShopId))
        {
            for (int i = 0; i < _allShop[value.ShopId].Count; i++)
            {
                if (_allShop[value.ShopId][i].mItemId == value.ItemId && _allShop[value.ShopId][i].mItemNum >= 0)
                    _allShop[value.ShopId][i].OnItemNum(value.ItemNum);
            }
            DispatchEvent(ShopEvent.BuyItem);
        }
    }
}
