/** 
 *FileName:     UpGradeBuildWindow.View.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-11-03 
 *Description:    
 *History: 
*/
using UnityEngine.UI;
using K.Extension;
using UnityEngine;

namespace Game.UI
{
    partial class UpGradeBuildWindow
    {
        #region Field
        private Text _textNowGrade;
        private Text _textNextGrade;
        private Text _textNowCatNum;
        private Text _textNextCatNum;
        private Text _textCatInfor;
        private Text _textNowCoinNum;
        private Text _textNextCoinNum;
        private Text _textTimeDown;
        private Text _textCoinUpNum;
        private Button _buttonUp;
        private Button _buttonClose;
        private Transform _modelParent;
        private KCattery.CatteryInfo catTery;
        #endregion

        #region Method

        public void InitView()
        {
            _textNowGrade = Find<Text>("Level/LevelCurrent");
            _textNextGrade = Find<Text>("Level/Level01Next");
            _textNowCatNum = Find<Text>("Function/CatNum/CatNumCurrent");
            _textNextCatNum = Find<Text>("Function/CatNum/CatNumNext");
            _textCatInfor = Find<Text>("Describe/TextDescribe");
            _textNowCoinNum = Find<Text>("Function/GoldNum/CatNumCurrent");
            _textNextCoinNum = Find<Text>("Function/GoldNum/CatNumNext");
            _textTimeDown = Find<Text>("Clock/Text");
            _textCoinUpNum = Find<Text>("Button/TextUpGrade/TextUpGradeNum");
            _buttonUp = Find<Button>("Button");
            _buttonUp.onClick.AddListener(OnUpBtnClick);
            _buttonClose = Find<Button>("Quit");
            _buttonClose.onClick.AddListener(OnCloseBtnClick);
            _modelParent = Find<Transform>("ObjBuild");
        }
        public void RefreshView()
        {
            if (_modelParent.childCount>0)
            {
                Object.Destroy(_modelParent.GetChild(0).gameObject);
            }
            dataReal = data as Data;
            catTery = KCattery.Instance.GetCatteryInfo(dataReal.BuildingId);
            if (catTery!=null)
            {
                GameObject modelPrefab = dataReal.Model;
                if (modelPrefab != null)
                {
                    if (modelPrefab.transform.childCount > 0)
                    {
                        /* Vector3  vec=*/
                        modelPrefab.transform.GetChild(0).localScale = Vector3.one;
                        modelPrefab.transform.GetChild(0).localPosition = Vector3.zero;
                        //vec.x = Mathf.Max(0.001f, vec.x);
                        //vec.y = Mathf.Max(0.001f, vec.y);
                        //modelPrefab.transform.localScale = new Vector3(1f/vec.x,1f/vec.y,1f);
                    }
                    modelPrefab.transform.SetParent(_modelParent, false);
                    TransformUtils.SetLayerSameWithParent(modelPrefab);
                    modelPrefab.GetComponentInChildren<Renderer>().sortingOrder = _textNextCatNum.canvas.sortingOrder + 1;
                }
                else
                {
                    Debug.Log("加载猫舍建筑失败"+dataReal.BuildingId);
                }
                _textNowGrade.text = catTery.grade.ToString();
                _textNextGrade.text = (catTery.grade + 1).ToString();
                _textNowCatNum.text = catTery.catStorage.ToString();
                _textNextCatNum.text = catTery.nextCatStorage.ToString();
                _textNowCoinNum.text = catTery.coinStorage.ToString();
                _textNextCoinNum.text = catTery.nextCoinStorage.ToString();
                _textCoinUpNum.text = catTery.nextUpgradeCost.ToString();
                _textTimeDown.text = TimeExtension.ToTimeString(catTery.nextUpgradeTime);
            }
        }

        public override void UpdatePerSecond()
        {

        }
        #endregion
    }
}

