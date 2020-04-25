/** 
 *FileName:     M3HiddenItem.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2018-01-02 
 *Description:    
 *History: 
*/

namespace Game.Match3
{
    public class M3HiddenItem
    {
        public M3HiddenPoint from;

        public M3HiddenPoint to;

        public M3HiddenItem Clone()
        {
            M3HiddenItem item = new M3HiddenItem();
            item.from = new M3HiddenPoint(this.from.x, this.from.y);
            item.to = new M3HiddenPoint(this.to.x, this.to.y);
            return item;
        }
    }
}