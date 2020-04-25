/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/25 16:35:24
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Framework;

namespace Game
{
    public class EventManager : Singleton<EventManager>
    {
        public EventDispatcher InputDispatcher { get; private set; }
        public EventDispatcher UIEvtDispatcher { get; private set; }
        public EventDispatcher GlobalDispatcher { get; private set; }
        public EventDispatcher GuideDispatcher { get; private set; }

        public void Init()
        {
            if (_blInited) return;

            InputDispatcher = new EventDispatcher();
            UIEvtDispatcher = new EventDispatcher();
            GlobalDispatcher = new EventDispatcher();
            GuideDispatcher = new EventDispatcher();

            _blInited = true;
        }

    }
}
