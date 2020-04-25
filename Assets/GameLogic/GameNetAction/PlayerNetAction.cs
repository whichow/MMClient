/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.5
 * 
 * Author:          Coamy
 * Created:	        2019/3/19 10:10:10
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game;
using Msg.ClientMessage;

namespace Game
{
    public partial class GameServer
    {

        #region 改名
        public void ChangeNameRequest(string name)
        {
            C2SChgName req = new C2SChgName();
            req.Name = name;
            C2SRequest(req);
        }

        public void ChangeNameHandler(S2CChgName obj)
        {
            //var player = KUser.SelfPlayer;
            //player.nickName = obj.Name;
            //player.changeNameCount = obj.ChgNameCount;
            PlayerDataModel.Instance.ChangeName(obj.Name);
        }
        #endregion

        #region 今天剩余帮助别人的次数

        public void DayHelpUnlockCountHandler(S2CRetDayHelpUnlockCount obj)
        {
            //KMailManager.Instance.UpdateLeftHelpData(obj);
        }

        #endregion

        #region 抽奖
        public void DrawRequest(int drawType, int drawCount)
        {
            C2SDraw req = new C2SDraw();
            req.DrawType = drawType;
            req.DrawCount = drawCount;
            C2SRequest(req);
        }

        public void DrawResultHandler(S2CDrawResult obj)
        {

        }
        #endregion
    }
}