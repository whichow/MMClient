/** 
*FileName:     GameOverRewardItem.cs 
*Author:       Hejunjie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-12-13 
*Description:    
*History: 
*/
using Game.DataModel;
using Msg.ClientMessage;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameOverRewardItem : KUIItem
    {
        public KUIImage iconImage;
        public Text countText;
        public Text flagText;

        public void ShowReward(ItemInfo iteminfo, GameOverWindow.RewardType type)
        {
            //Debug.Log("奖励ID: " + iteminfo.ItemCfgId + "  数量: " + iteminfo.ItemNum);
            ItemXDM xdm = XTable.ItemXTable.GetByID(iteminfo.ItemCfgId);
            iconImage.overrideSprite = KIconManager.Instance.GetItemIcon(xdm.Icon);
            countText.text = iteminfo.ItemNum.ToString();
            switch (type)
            {
                case GameOverWindow.RewardType.Normal:
                    flagText.gameObject.SetActive(false);
                    break;
                case GameOverWindow.RewardType.First:
                    flagText.gameObject.SetActive(true);
                    flagText.text = KLocalization.GetLocalString(51053);
                    break;
                case GameOverWindow.RewardType.ThreeStar:
                    flagText.gameObject.SetActive(true);
                    flagText.text = KLocalization.GetLocalString(51054);
                    break;
                default:
                    break;
            }
        }

        public void Awake()
        {
            iconImage = Find<KUIImage>("Prop01");
            countText = Find<Text>("Prop01/Text");
            flagText = Find<Text>("TextFirst");
        }

    }
}