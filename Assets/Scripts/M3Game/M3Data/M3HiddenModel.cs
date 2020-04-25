/** 
 *FileName:     M3HiddenModel.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2018-01-02 
 *Description:    
 *History: 
*/

namespace Game.Match3
{
    public class M3HiddenModel
    {
        public int type;

        public int count;

        public M3HiddenItem[] list;

        public M3HiddenModel Clone()
        {
            M3HiddenModel clone = new M3HiddenModel();
            clone.type = this.type;
            clone.count = this.count;
            M3HiddenItem[] tmp = new M3HiddenItem[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                tmp[i] = list[i].Clone();
            }
            clone.list = tmp;
            return clone;
        }
    }
}