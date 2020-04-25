
using System.Collections.Generic;
/** 
*FileName:     DiscoveryRetWindow.View.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-10-25 
*Description:    
*History: 
*/
using UnityEngine.UI;
using Msg.ClientMessage;
using UnityEngine;
namespace Game.UI
{
    partial class DiscoveryRetWindow
    {
        #region Field

        private Text _title;
        //private Text _textBtnConfirm;

        private Transform _tranSpecialReward;
        //private Button _confirm;
        private Button _cancel;
        private Transform[] _transRewards;
        private KUIItemPool _itemPool;
        #endregion

        #region Method

        public void InitView()
        {
            _title = Find<Text>("Panel/Text");
            //_textBtnConfirm = Find<Text>("Panel/ButtonShare/Text");
            //_confirm = Find<Button>("Panel/ButtonShare");
            //_confirm.onClick.AddListener(OnConfirmBtnClick);
            _cancel = Find<Button>("Panel/ButtonCanel");
            _cancel.onClick.AddListener(OnCancelBtnClick);
            _tranSpecialReward = Find<Transform>("Panel/Scroll View");
            var icon = Find<Transform>("Panel/TopBack/Icon");
            _transRewards = new Transform[icon.childCount];
            for (int i = 0; i < _transRewards.Length; i++)
            {
                _transRewards[i] = Find<Transform>("Panel/TopBack/Icon/RewardItem" + (i+1));
            }
            _itemPool = Find<KUIItemPool>("Panel/Scroll View");
            if (_itemPool && _itemPool.elementTemplate)
            {
                _itemPool.elementTemplate.gameObject.AddComponent<DiscoveryRetItem>();
            }
        }

        public void RefreshView()
        {
            _itemPool.Clear();
            var items = _discoveryRetData.specialsItemIdNum;
            if (items != null && items.Count > 0)
            {
                var elements = _itemPool.SpawnElements(items.Count);
                for (int i = 0; i < elements.Length; i++)
                {
                    elements[i].GetComponent<DiscoveryRetItem>().ShowData(items[i]);
                }
            }
        
                _title.text = "探索成功";
                //_textBtnConfirm.text = "分享";
              
      
      
            RefreshReward(_discoveryRetData.itemIdNum);
        }
        public void RefreshReward(IList<IdNum> idNumList)
        {
            for (int i = 0; i < _transRewards.Length; i++)
            {
                if (i<idNumList.Count)
                {
                    _transRewards[i].Find("Icon").GetComponent<Image>().overrideSprite = KIconManager.Instance.GetItemIcon(idNumList[i].Id);
                    _transRewards[i].Find("Icon/Text").GetComponent<Text>().text = "X"+idNumList[i].Num;
                    _transRewards[i].gameObject.SetActive(true);
                }
                else
                {
                    _transRewards[i].gameObject.SetActive(false);
                }

            }
        }


        #endregion
    }
}

