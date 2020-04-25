/** 
*FileName:     BellElement.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2018-01-03 
*Description:    
*History: 
*/

namespace Game.Match3
{
    /// <summary>
    /// 铃铛
    /// </summary>
    public class BellElement : Element
    {
        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.BellElement;
        }

        public override void OnDisappear()
        {
            base.OnDisappear();
            itemObtainer.isCrushing = true;
            if (itemObtainer.itemInfo.CheckEmpty())
            {
                itemObtainer.itemInfo.EliminateElement(this);
                itemObtainer.isCrushing = false;
                if (itemObtainer.itemInfo.CheckEmpty())
                {
                    int x = itemObtainer.itemInfo.posX;
                    int y = itemObtainer.itemInfo.posY;
                    itemObtainer.RemoveFrom(x, y);
                    itemObtainer.ItemDestroy();
                }
            }
        }

    }
}