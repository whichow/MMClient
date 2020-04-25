/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/14 18:35:01
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/

namespace Game.Match3
{
    public class LawnElement : Element
    {
        public override void Init(int id, M3Grid grid)
        {
            base.Init(id, grid);
            eName = M3ElementType.LawnElement;
        }

        public override Element Clone()
        {
            Element ele = new LawnElement();
            return Clone(ele);
        }

    }
}
