using System;
using Msg.ClientMessage;

namespace Game
{
    public class PlayerDataModel : DataModelBase<PlayerDataModel>
    {
        public PlayerDataVO mPlayerData { get; private set; }


        public void ExePlayerData(S2CPlayerInfoResponse value)
        {
            if (mPlayerData == null)
                mPlayerData = new PlayerDataVO();
            mPlayerData.OnInit(value);
            DispatchEvent(PlayerEvent.PlayerDataRefresh);
        }

        public void ExeEnterGame(S2CEnterGameResponse value)
        {
            if (mPlayerData == null)
                mPlayerData = new PlayerDataVO();
            mPlayerData.OnGmae(value.PlayerId, value.Acc);
            GameApp.Instance.GameServer.EnterGameHandler(value);
        }

        public void ExeChangeName(S2CPlayerChangeNameResponse value)
        {
            ChangeName(value.NewName);
        }

        public void ExeChangeHead(S2CPlayerChangeHeadResponse value)
        {
            mPlayerData.ChangeHead(value.NewHead);
            DispatchEvent(PlayerEvent.ChangeHead);
        }

        public void ChangeName(string name)
        {
            mPlayerData.ChangeName(name);
            DispatchEvent(PlayerEvent.ChangeName);
        }

        public int GetCurrency(int money)
        {
            int val = 0;
            switch (money)
            {
                case 2:
                    val = mPlayerData.mGold;
                    break;
                case 3:
                    val = mPlayerData.mDiamond;
                    break;
                default:
                    break;
            }
            return val;
        }

    }
}
