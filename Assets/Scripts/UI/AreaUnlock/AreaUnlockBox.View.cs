// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "AreaUnlockBox.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class AreaUnlockBox
    {
        #region Field

        private Button _closeBtn;
        private Button _confirmBtn;

        private Text _title;
        private Text _gradeHint;
        private Text _starHint;
        private Text _unlockCost;
        private Image _unlockType;

        private KUIImage _gradeCheck1;
        private KUIImage _gradeCheck2;
        private KUIImage _starCheck1;
        private KUIImage _starCheck2;

        #endregion

        #region Method

        public void InitView()
        {
            _closeBtn = Find<Button>("Close");
            _closeBtn.onClick.AddListener(OnCloseClick);
            _confirmBtn = Find<Button>("Confirm");
            _confirmBtn.onClick.AddListener(OnConfirmClick);

            _title = Find<Text>("Title");
            _gradeHint = Find<Text>("Hint1/Text");
            _starHint = Find<Text>("Hint2/Text");
            _unlockCost = Find<Text>("Confirm/Cost");
            _unlockType = Find<Image>("Confirm/CostType");

            _gradeCheck1 = Find<KUIImage>("Hint1/Check");
            _gradeCheck2 = Find<KUIImage>("Hint1/Check/Image");

            _starCheck1 = Find<KUIImage>("Hint2/Check");
            _starCheck2 = Find<KUIImage>("Hint2/Check/Image");
        }

        public void RefreshView()
        {
            bool meet = false;
            if (PlayerDataModel.Instance.mPlayerData.mLevel >= _myData.grade)
            {
                meet = true;
                _gradeCheck1.ShowGray(false);
                _gradeCheck2.ShowGray(false);
                _gradeHint.color = new Color32(121, 70, 13, 255);
            }
            else
            {
                _gradeCheck1.ShowGray(true);
                _gradeCheck2.ShowGray(true);
                _gradeHint.color = new Color32(227, 38, 38, 255);
            }

            if (PlayerDataModel.Instance.mPlayerData.mHistoricalMaxStar >= _myData.star)
            {
                _starCheck1.ShowGray(false);
                _starCheck2.ShowGray(false);
                _starHint.color = new Color32(121, 70, 13, 255);
            }
            else
            {
                meet = false;
                _starCheck1.ShowGray(true);
                _starCheck2.ShowGray(true);
                _starHint.color = new Color32(227, 38, 38, 255);
            }

            _gradeHint.text = ">=" + _myData.grade;
            _starHint.text = ">=" + _myData.star;
            _unlockCost.text = _myData.cost.ToString("N0");
            if(PlayerDataModel.Instance.mPlayerData.mGold >= _myData.cost)
            {
                _unlockCost.color = Color.white;
            }
            else
            {
                _unlockCost.color = new Color32(227, 38, 38, 255);
            }

            _confirmBtn.interactable = meet;
            ((KUIImage)_confirmBtn.targetGraphic).ShowGray(!meet);
        }

        #endregion
    }
}
