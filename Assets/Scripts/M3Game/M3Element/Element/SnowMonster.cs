/** 
*FileName:     SnowMonster.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2018-01-25 
*Description:    
*History: 
*/

namespace Game.Match3
{
    /// <summary>
    /// 雪怪
    /// </summary>
    public class SnowMonster : Element
    {

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.SnowMonster;
        }

        public override void Crush()
        {
            base.Crush();
            if (itemObtainer.isCrushing)
                return;
            DoLogic();
        }

        private void DoLogic()
        {
            hp--;
            if (hp <= 0)
            {

            }
        }

        public void AddIce()
        {
            int x = itemObtainer.itemInfo.posX;
            int y = itemObtainer.itemInfo.posY;
        }

    }
}