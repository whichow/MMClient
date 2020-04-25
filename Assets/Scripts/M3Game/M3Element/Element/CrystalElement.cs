/** 
*FileName:     CrystalElement.cs 
*Author:       Hejunjie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-11-10 
*Description:    
*History: 
*/

namespace Game.Match3
{
    /// <summary>
    /// 水晶球（可变元素）
    /// </summary>
    public class CrystalElement : NormalElement
    {
        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.CrystalElement;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            ChangeCrystalColor(GetColor());
        }

        public override void InitClone(Element ele, object[] args)
        {
            base.InitClone(ele, args);
            ChangeCrystalColor(ele.currentColor);
        }

        public void ChangeCrystalColor(ItemColor color)
        {
            data.SetColor(color);
            if (view != null)
                view.PlayAnimation(data.config.Animations["Change"].ToString());
            switch (color)
            {
                case ItemColor.fNone:
                    break;
                case ItemColor.fRed:
                    currentColor = ItemColor.fRed;
                    if (view != null)
                        view.ChangeViewSkin("red");
                    break;
                case ItemColor.fYellow:
                    currentColor = ItemColor.fYellow;
                    if (view != null)
                        view.ChangeViewSkin("yellow");
                    break;
                case ItemColor.fBlue:
                    currentColor = ItemColor.fBlue;
                    if (view != null)
                        view.ChangeViewSkin("blue");
                    break;
                case ItemColor.fGreen:
                    currentColor = ItemColor.fGreen;
                    if (view != null)
                        view.ChangeViewSkin("green");
                    break;
                case ItemColor.fPurple:
                    currentColor = ItemColor.fPurple;
                    if (view != null)
                        view.ChangeViewSkin("purple");
                    break;
                case ItemColor.fBrown:
                    currentColor = ItemColor.fBrown;
                    if (view != null)
                        view.ChangeViewSkin("brown");
                    break;
                case ItemColor.fEnergy:
                    break;
                case ItemColor.fRandom:
                    break;
                default:
                    break;
            }
            int id = M3ItemManager.Instance.GetCrystalElementID(currentColor);
            data.Init(id);
        }

        public override Element Clone()
        {
            var ele = new CrystalElement();
            ele.currentColor = data.GetColor();
            return Clone(ele);
        }

    }
}