/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/25 16:49:18
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Framework;

namespace Game
{
    public class DataModelBase<T> :  EventDispatcher where T : new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

    }
}
