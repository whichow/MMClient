/** 
*FileName:     CurtainElement.cs 
*Author:       Hejunjie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-11-15 
*Description:    
*History: 
*/

namespace Game.Match3
{
    /// <summary>
    /// 白云,窗帘
    /// </summary>
    public class CurtainElement : ObstacleElement
    {

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.CurtainElement;
        }

        public override void OnDisappear()
        {
            base.OnDisappear();
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEleminateCurtain();
        }

        protected override void AddScore()
        {
            int score = M3GameManager.Instance.modeManager.ProcessComboScore(data.config.Point, ItemSpecial.fNormal, M3GameManager.Instance.comboManager.GetCombo(), false);
            M3GameManager.Instance.modeManager.AddScore(score, 1, M3Supporter.Instance.GetItemPositionByGrid(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY));
        }

        public override Element Clone()
        {
            Element ele = new CurtainElement();
            return Clone(ele);
        }

    }
}