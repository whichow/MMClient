/** 
 *FileName:     DiscoveryIngRewardItem.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-26 
 *Description:    
 *History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using K.Extension;
namespace Game.UI
{
    public class DiscoveryIngEventItem : KUIItem
    {

        #region Field
        private Text _textContent;
        private Text _textTime;
        #endregion
        public void ShowReward(KExplore.Event events)
        {



            _textContent.text = "              " + events.talk;

            _textTime.text = events.showTime;

        }















        private void Awake()
        {
            _textContent = Find<Text>("Item/Text");
            _textTime = Find<Text>("Item/Time");
        }


        void Update()
        {

        }
    }
}
