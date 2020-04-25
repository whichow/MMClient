namespace Game.Match3
{
    public class GameFSM
    {
        private StateMachine<GameFSM> pStateMachine;

        public GameFSM()
        {
   
        }

        public void Init()
        {
            pStateMachine = new StateMachine<GameFSM>(this);

            if (!M3GameManager.Instance.isAutoAI)
                pStateMachine.SetCurrentState(StateEnum.Ready);
            else {
                pStateMachine.SetCurrentState(StateEnum.Refresh);
            }
        }

        public StateMachine<GameFSM> GetFSM()
        {
            return pStateMachine;
        }

    }
}
