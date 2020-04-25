//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "CatFeedWindow" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using Spine.Unity;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public partial class CatFeedWindow
//    {
//        #region Field

//        private Button _feedBtn;
//        private Button _backBtn;

//        private Image _expProgress;
//        private Text _gradeText;
//        private Text _requiredText;
//        private Text _totalText;
//        private Text _nameText;

//        private Transform _modelParent;
//        private SkeletonGraphic _animation;
//        private SkeletonGraphic _animation1;
//        #endregion

//        #region Method

//        public void InitView()
//        {
//            _feedBtn = Find<Button>("Feed");
//            _feedBtn.onClick.AddListener(this.OnFeedBtnClick);

//            _backBtn = Find<Button>("Back");
//            _backBtn.onClick.AddListener(this.OnBackBtnClick);

//            _nameText = Find<Text>("Name/Text");
//            _gradeText = Find<Text>("Exp/Grade/Value");
//            _expProgress = Find<Image>("Exp/Progress/Mask");
//            _requiredText = Find<Text>("Required2/Text");
//            _totalText = Find<Text>("Required1/Text");
//            _modelParent = Find<Transform>("Model");
//            _animation = Find<SkeletonGraphic>("Fx_SkillUp_01");
//            _animation1 = Find<SkeletonGraphic>("Fx_SkillUp_02");
//        }

//        public void RefreshView()
//        {
//            if (GetCat().maxGrade<= GetCat().grade)
//            {
//                _feedBtn.GetComponent<Image>().material = Resources.Load<Material>("Materials/UIGray");
//            }
//            else
//            {
//                _feedBtn.GetComponent<Image>().material = null;
//            }
//            _nameText.text = GetNameText();
//            _gradeText.text = GetGradeText();
//            _expProgress.fillAmount = GetExpProgress();
//            _requiredText.text = GetExpRequired();
//            _totalText.text = GetExp();
//            var catModel = GetCatModel();
//            if (catModel)
//            {
//                catModel.transform.SetParent(_modelParent, false);
//                //catModel.gameObject.layer = _modelParent.gameObject.layer;
//                //catModel.GetComponent<Renderer>().sortingOrder = _nameText.canvas.sortingOrder + 2;
//            }
//        }

//        #endregion
//    }
//}

