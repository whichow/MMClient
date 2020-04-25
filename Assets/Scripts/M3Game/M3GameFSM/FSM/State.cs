namespace Game.Match3
{
    public class State<entity>
    {
        public entity Target;

        public virtual void Enter(entity entityType)
        {
            //Debuger.Log(this.ToString() + " Enter " + FrameScheduler.instance.GetCurrentFrame());
        }

        public virtual void Execute(entity entityType)
        {
            //Debuger.Log(this.ToString() + " Execute " + FrameScheduler.instance.GetCurrentFrame());
        }

        public virtual void Exit(entity entityType)
        {
            //Debuger.Log(this.ToString() + " Exit " + FrameScheduler.instance.GetCurrentFrame());
        }
    }
}
