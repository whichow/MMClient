/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/24 13:40:18
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/

namespace Game.DataModel
{
    public partial class GuideActionXDM
    {
        private Int2[] m_M3ChangePos;

        public Int2[] M3ChangePos
        {
            get
            {
                if (m_M3ChangePos == null)
                {
                    if (Change != null)
                    {
                        m_M3ChangePos = new Int2[Change.Count];
                        for (int i = 0; i < Change.Count; i++)
                        {
                            var arrTmp = Change.GetArray<int>(i);
                            Int2 int2Tmp = new Int2(arrTmp);
                            m_M3ChangePos[i] = int2Tmp;
                        }
                    }
                }
                return m_M3ChangePos;
            }
        }

    }
}
