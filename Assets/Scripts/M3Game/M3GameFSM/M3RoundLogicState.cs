/** 
 *FileName:     M3RoundLogicState.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2018-01-06 
 *Description:    
 *History: 
*/
namespace Game.Match3
{
    /// <summary>
    /// 回合逻辑处理
    /// </summary>
    public class M3RoundLogicState : State<GameFSM>
    {
        public bool haveChecked = true;
        private bool hold;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            this.hold = false;
            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckConveyor);
            }
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);
            if (this.hold) return;

            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckConveyor);
            }
            else
            {
                this.haveChecked = true;
                this.hold = true;
                M3Item item = null;
                for (int i = 0; i < M3Config.GridHeight; i++)
                {
                    for (int j = 0; j < M3Config.GridWidth; j++)
                    {
                        item = M3ItemManager.Instance.gridItems[i, j];
                        if (item != null && item.itemInfo.GetElement() != null)
                        {
                            item.itemInfo.GetElement().ProcessRoundLogic();
                        }
                    }
                }
            }
            M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckConveyor);
        }

        public override void Exit(GameFSM entityType)
        {
            base.Exit(entityType);
        }

    }
}
