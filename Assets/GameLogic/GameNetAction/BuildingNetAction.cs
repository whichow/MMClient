/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.5
 * 
 * Author:          Coamy
 * Created:	        2019/3/20 11:10:10
 * Description:     建筑
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.Build;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Msg.ClientMessage;
using System.Collections.Generic;

namespace Game
{
    public partial class GameServer
    {

        #region 请求地板数据
        public void SurfaceDataRequest()
        {
            var req = new C2SSurfaceDataRequest();
            C2SRequest(req);
        }

        public void SurfaceDataHandler(S2CSurfaceDataResponse obj)
        {
            BuildingSurfaceManager.Instance.OnSyncSurfaceDataHandler(obj.Data);
        }
        #endregion

        #region 更新地板数据
        public void SurfaceUpdateRequest(IList<BuildingInfo> list, IList<BuildingInfo> removeList)
        {
            var req = new C2SSurfaceUpdateRequest();
            if (list != null)
            {
                req.UpdateData.AddRange(list);
            }
            if (removeList != null)
            {
                req.RemoveData.AddRange(removeList);
            }
            C2SRequest(req);
        }

        public void SurfaceUpdateHandler(S2CSurfaceUpdateResponse obj)
        {
            BuildingSurfaceManager.Instance.OnSurfaceUpdate();
        }
        #endregion


    }
}