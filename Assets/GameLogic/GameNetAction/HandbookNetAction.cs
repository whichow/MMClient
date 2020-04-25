/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.5
 * 
 * Author:          Coamy
 * Created:	        2019/3/20 13:23:10
 * Description:     图鉴
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

        #region 新图鉴物品

        public void NewHandbookItemNotifyHandler(S2CNewHandbookItemNotify obj)
        {
            KHandBookManager.Instance.Process(obj);
        }

        #endregion

        #region 新头像

        public void NewHeadNotifyHandler(S2CNewHeadNotify obj)
        {

        }

        #endregion
    }
}