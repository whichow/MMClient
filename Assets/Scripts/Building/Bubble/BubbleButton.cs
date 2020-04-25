using System;

namespace Game.Build
{
    public class BubbleButton : Bubble
    {
        public Action onClick;

        protected void OnTap()
        {
            if (this.onClick != null)
            {
                this.onClick();
            }
            else
            {
                var componentInParent = this.GetComponentInParent<MapObject>();
                if (componentInParent != null)
                {
                    componentInParent.SendMessage("OnTap");
                }
            }
        }
    }
}

