/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/6 11:38:45
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/

using System.Collections.Generic;

namespace Game
{
    public interface IEventData
    {

    }

    public class EventData : IEventData
    {
        public object Data;

        public int Integer;

        public int[] Integers;

        public EventData()
        {
        }

    }
}
