/** 
 *FileName:     M3RankItem.cs 
 *Author:       Hejunjie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-12-12 
 *Description:    
 *History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Msg.ClientMessage;
using Game;

namespace Game.UI
{
    public class M3RankItem : KUIItem
    {

        private Text rankName;
        private Text rankScore;
        private KUIImage rankIcon;

        private Text _txt_sequece;
        private KUIImage _img_sequence;
        GameOverWindow.RankType rankType;

        private void Awake()
        {
            rankName = Find<Text>("NameBack/Name");
            rankScore = Find<Text>("NameBack/NameNum");
            rankIcon = Find<KUIImage>("NameBack/img_head/ImageMask/Iconhead");

            _txt_sequece = Find<Text>("NameBack/Image/txt_sequece");
            _img_sequence = Find<KUIImage>("NameBack/Image");
        }

        protected override void Refresh()
        {
            base.Refresh();
            int int_DataRank = 1;
            if (data is GameOverFriendRankData)
            {
                var rData = (GameOverFriendRankData)data;
                rankName.text = rData.friendRankInfo.Name;
                rankScore.text = rData.friendRankInfo.Score.ToString();
                int_DataRank = rData.Rank + 1;
                HeadIconUtils.SetHeadIcon(rData.friendRankInfo.Head, rData.friendRankInfo.PlayerId, rankIcon);
            }
            //else if (data is RankingListItemInfo)
            //{
            //    var rData = (RankingListItemInfo)data;
            //    rankName.text = rData.PlayerName;
            //    rankScore.text = rData.PlayerStageScore.ToString();
            //    int_DataRank = rData.Rank;
            //    rankIcon.overrideSprite = KIconManager.Instance.GetHeadIcon(rData.PlayerHead);
            //}

            if (int_DataRank <= 3 && int_DataRank >= 1)
            {
                _img_sequence.enabled = true;
                _img_sequence.overrideSprite = _img_sequence.sprites[int_DataRank - 1];
                _txt_sequece.gameObject.SetActive(false);
            }
            //else if (int_DataRank >= KRankManager.int_const_maxShowSequence || int_DataRank == 0)
            //{
            //    _img_sequence.enabled = true;
            //    _img_sequence.overrideSprite = _img_sequence.sprites[3];
            //    _txt_sequece.gameObject.SetActive(true);
            //    _txt_sequece.text = "--";
            //    _txt_sequece.gameObject.SetActive(false);
            //}
            else
            {
                //_img_sequence.overrideSprite = _img_sequence.sprites[3];
                _img_sequence.enabled = false;
                _txt_sequece.gameObject.SetActive(true);
                _txt_sequece.text = int_DataRank.ToString();
            }

        }
    }
}