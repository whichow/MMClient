/** 
 *FileName:     GameGoalWindow.cs 
 *Author:       Hejunjie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-11-07 
 *Description:    
 *History: 
*/
using Game.DataModel;
using Game.Match3;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 三消目标提示窗口
    /// </summary>
    public partial class GameGoalWindow
    {
        private KUIList targetParentList;
        private GameObject collectBoard;
        private GameObject scoreBoard;
        private Text targetScoreText;

        private Image bg;
        public void InitView()
        {
            targetParentList = transform.Find("Goal/Collect/CatGroup").GetComponent<KUIList>();
            collectBoard = transform.Find("Goal/Collect").gameObject;
            scoreBoard = transform.Find("Goal/Score").gameObject;
            targetScoreText = transform.Find("Goal/Score/score").GetComponent<Text>();
            bg = transform.Find("Image").GetComponent<Image>();
        }

        private void RefreshView()
        {

            KTweenUtils.DoImageFade(bg, 0, 0.6f, 1);

            var target = modeManager.target;


            if (target == GameTargetEnum.Collection)
            {
                collectBoard.SetActive(true);
                scoreBoard.SetActive(false);
                M3TargetItem item = null;
                int index = 0;
                var dic = mapData.LevelTaskElementDic;
                foreach (var t in dic)
                {
                    var tmp = targetParentList.GetItem(index);
                    item = (M3TargetItem)tmp;
                    item.UpdateData(XTable.ElementXTable.GetByID(t.Key).Icon, t.Value);
                    index++;
                }
            }
            else if (target == GameTargetEnum.Score)
            {
                collectBoard.SetActive(false);
                scoreBoard.SetActive(true);
                targetScoreText.text = modeManager.GetScore().ToString();
            }

        }
    }
}
