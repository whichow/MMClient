namespace Game.Match3
{
    /// <summary>
    /// 毒液源
    /// </summary>
    public class VenomParentElement : ObstacleElement
    {

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.VenomParentElement;
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
        }

        public override Element Clone()
        {
            Element ele = new VenomParentElement();
            return Clone(ele);
        }

    }
}