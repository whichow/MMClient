// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MessageBox.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    partial class ShowAwardList
    {
        #region Field
        private int _id;
        private Image _img_icon;
        private Text _title;
        private Text _txt_first;
        private Text _txt_fourth;
        private Text _txt_fifth;
        private Button _blackback;
        private Transform[] _tras_awardparent;
        private const int AwardLength = 3;
        private GameObject _go_temAward;
        //private KMission _TAData;
        #endregion

        #region Method

        public void InitView()
        {
            _tras_awardparent = new Transform[3];
            for (int i = 0; i < AwardLength; i++)
            {
                _tras_awardparent[i] = Find<Transform>("panelback/trans_" + i.ToString());
            }
            _go_temAward = Find<Transform>("panelback/img_icon").gameObject;
            _go_temAward.SetActive(false);
            _blackback = Find<Button>("blackback");
            _blackback.onClick.AddListener(this.CloseUI);
        }

        public void RefreshView()
        {
            RefreshAwardList();
        }

        private void RefreshAwardList()
        {
            if (_TAData.bonusItems == null || _TAData.bonusItems.Length == 0)
            {
                for (int i = 0; i < AwardLength; i++)
                {
                    _tras_awardparent[i].gameObject.SetActive(false);
                }
                return;
            }
            for (int i = 0; i < AwardLength; i++)
            {
                if (i > _TAData.bonusItems.Length - 1)
                {
                    _tras_awardparent[i].gameObject.SetActive(false);
                }
                else
                {
                    RefreshSingalAward(_TAData.bonusItems[i], i);
                }
            }
        }
        private void RefreshSingalAward(KItem.ItemInfo data, int index)
        {
            GameObject oneAward;
            if (_tras_awardparent[index].childCount == 0)
            {
                oneAward = GameObject.Instantiate(_go_temAward);
                oneAward.transform.SetParent(_tras_awardparent[index], false);
                oneAward.transform.localPosition = Vector3.zero;
                oneAward.transform.localScale = Vector3.one;
            }
            else
            {
                oneAward = _tras_awardparent[index].GetChild(0).gameObject;
                for (int j = 0; j < _tras_awardparent[index].childCount; j++)
                {
                    if (j != 0)
                    {
                        GameObject.Destroy(_tras_awardparent[index].GetChild(j).gameObject);
                    }
                }
            }
            oneAward.SetActive(true);
            Text nm = oneAward.transform.Find("Text").gameObject.GetComponent<Text>();
            Image awIc = oneAward.GetComponent<Image>();
            nm.text = data.itemCount.ToString();
            awIc.overrideSprite = KIconManager.Instance.GetItemIcon(data.itemID);
        }
        #endregion
    }
}

