/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.5
 * 
 * Author:          Coamy
 * Created:	        2019/3/20 14:40:10
 * Description:     猫舍
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game;
using Game.UI;
using Msg.ClientMessage;

namespace Game
{
    public partial class GameServer
    {

        #region 获取单个猫舍
        public void GetCatHouseInfoRequest(int id)
        {
            C2SGetCatHouseInfo req = new C2SGetCatHouseInfo();
            req.CatHouseId = id;
            C2SRequest(req);
        }

        public void GetCatHouseInfoResultHandler(S2CGetCatHouseInfoResult obj)
        {
            //KCattery.Instance.Process(obj);
        }
        #endregion

        #region 猫舍开始升级
        public void CatHouseStartLevelupRequest(int id)
        {
            C2SCatHouseStartLevelup req = new C2SCatHouseStartLevelup();
            req.CatHouseId = id;
            C2SRequest(req);
        }

        public void CatHouseStartLevelupResultHandler(S2CCatHouseStartLevelupResult obj)
        {

        }
        #endregion

        #region 猫舍开始产金
        public void CatHouseProduceGoldRequest(int id)
        {
            C2SCatHouseProduceGold req = new C2SCatHouseProduceGold();
            req.CatHouseId = id;
            C2SRequest(req);
        }

        public void CatHouseProduceGoldResultHandler(S2CCatHouseProduceGoldResult obj)
        {
            //KCattery.Instance.Process(obj);
            //if (FunctionWindow.RefurbishFunView != null)
            //    FunctionWindow.RefurbishFunView();
        }
        #endregion

    }
}