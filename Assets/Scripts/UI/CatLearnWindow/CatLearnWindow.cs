//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "CatLearnWindow" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using Msg.ClientMessage;
//using Spine.Unity;
//using System.Collections;
//using UnityEngine;

//namespace Game.UI
//{
//    public partial class CatLearnWindow : KUIWindow
//    {
//        #region Field 
//        private int catNum;
//        #endregion

//        #region Constructor

//        public CatLearnWindow()
//             : base(UILayer.kNormal, UIMode.kSequenceHide)
//        {
//            uiPath = "CatLearn";
//            uiAnim = UIAnim.kAnim1;
//            hasMask = true;
//        }

//        #endregion

//        #region Action

//        private void OnBackBtnClick()
//        {
//            CloseWindow(this);
//        }

//        private void OnTipsBtnClick()
//        {

//        }

//        private void OnToggleValueChanged(bool value)
//        {
//            RefreshView();
//        }
//        private void OnOkClick()
//        {
//            if (_skillToggle.isOn)
//            {
//                OpenWindow<MessageBox>(new MessageBox.Data
//                {
//                    content = KLocalization.GetLocalString(54050),
//                    onConfirm = CatSkillUp,
//                    onCancel = OnCancel,
//                });

//            }
//            else
//            {
//                if (_cat.maxGrade > _cat.grade)
//                {
//                    ToastBox.ShowText(string.Format(KLocalization.GetLocalString(54115), _cat.maxGrade));
//                    return;
//                }
//                OpenWindow<MessageBox>(new MessageBox.Data
//                {
//                    content = KLocalization.GetLocalString(54051),
//                    onConfirm = CatUpStar,
//                    onCancel = OnCancel,
//                });
//            }
//        }
//        private void OnCancel()
//        {

//        }
//        private void CatUpStar()
//        {
//            catNum = 0;
//            for (int i = 0; i < catList.Length; i++)
//            {
//                if (catList[i] != null)
//                {
//                    catNum++;
//                }
//            }
//            int[] catId = new int[catNum];
//            for (int i = 0; i < catId.Length; i++)
//            {
//                catId[i] = catList[i].catId;
//            }
//            KUser.CatUpstar(_cat.catId, catId, OnStartOkCallBack);
//        }
//        private void CatSkillUp()
//        {
//            catNum = 0;
//            for (int i = 0; i < catSkillList.Length; i++)
//            {
//                if (catSkillList[i] != null)
//                {
//                    catNum++;
//                }
//            }
//            int[] catId = new int[catNum];
//            for (int i = 0; i < catId.Length; i++)
//            {
//                catId[i] = catSkillList[i].catId;
//            }
//            KUser.CatSkillUpgrade(_cat.catId, catId, OnOkCallback);
//        }
//        private void OnOkCallback(int code, string message, object data)
//        {
//            if (code == 0)
//            {
//                var list = data as ArrayList;
//                if (list != null)
//                {

//                    foreach (var item in list)
//                    {
//                        if (item is S2CCatSkillLevelUpResult)
//                        {
//                            StartCoroutine(SkillUpAnimaton(new SkillSuccessWindow.WindowData
//                            {
//                                CatSkillLevelUpdata = item as S2CCatSkillLevelUpResult,
//                                oldCatLevl = oldCatSkillGrade,

//                            }));
//                        }

//                    }
//                }
//            }

//        }
      
//        private void OnStartOkCallBack(int code, string message, object data)
//        {
//            if (code == 0)
//            {
//                StartSuccessWindow.WindowData starData = new StartSuccessWindow.WindowData();
//                SkillSuccessWindow.WindowData skillData = new SkillSuccessWindow.WindowData();
//                var list = data as ArrayList;
//                if (list != null)
//                {
//                    foreach (var item in list)
//                    {

//                        if (item is S2CCatSkillLevelUpResult)
//                        {
//                            skillData = new SkillSuccessWindow.WindowData
//                            {
//                                CatSkillLevelUpdata = item as S2CCatSkillLevelUpResult,
//                                oldCatLevl = oldCatSkillGrade,

//                            };
//                        }
//                        else
//                        {
//                            skillData = null;
//                        }
//                        if (item is S2CCatUpgradeStarResult)
//                        {

//                            starData = new StartSuccessWindow.WindowData
//                            {
//                                CatSkillLevelUpdata = item as S2CCatUpgradeStarResult,
//                                oldCatStart = oldCatStart,
//                                oldCatAddCoin = oldCatAddCoin,
//                                oldCatExplore = oldCatExplore,
//                                oldCatMatch = oldCatMatch,
//                                oldCatMaxGrad = oldCatMaxGrad,

//                            };

//                        }
//                    }
//                    if (starData != null)
//                    {
//                        StartCoroutine(StarUpAnimaton(skillData, starData));
//                    }

//                }

//            }

//        }
//        private IEnumerator StarUpAnimaton(SkillSuccessWindow.WindowData data, StartSuccessWindow.WindowData startData)
//        {
//            //yield return null;
//            for (int i = 0; i < transContenCats.Length; i++)
//            {
//                if (i < catNum)
//                {
//                    var animation = transContenCats[i].Find("Fx/Fx_StarUp_01").GetComponent<SkeletonAnimation>();
//                    animation.AnimationName = null;
//                    transContenCats[i].Find("Fx").localRotation = quaternTwo[i];
//                    //animation.Reset();
//                    animation.GetComponent<Renderer>().sortingOrder = _textConst.canvas.sortingOrder + 1;
//                    animation.loop = false;
//                    animation.AnimationName = "a";
//                }
//            }
//            yield return new WaitForSeconds(0.7f);
//            //_starUpAnimation.Reset();
//            _starUpAnimation.GetComponent<Renderer>().sortingOrder = _textConst.canvas.sortingOrder + 1;
//            _starUpAnimation.AnimationName = null;
//            _starUpAnimation.loop = false;
//            _starUpAnimation.AnimationName = "c";
//            yield return new WaitForSeconds(0.7f);
//            if (data != null)
//            {
//                OpenWindow<SkillSuccessWindow>(data);
//            }
//            OpenWindow<StartSuccessWindow>(startData);
//            RefreshView();
//            RefreshCallBackStartView();
//        }
//        private IEnumerator SkillUpAnimaton(SkillSuccessWindow.WindowData data)
//        {
//            yield return null;
//            for (int i = 0; i < transContenCatSkills.Length; i++)
//            {
//                if (i < catNum)
//                {
//                    var animation = transContenCatSkills[i].Find("Fx/Fx_StarUp_01").GetComponent<SkeletonAnimation>();
//                    animation.AnimationName = null;
//                    //animation.Reset();
//                    animation.GetComponent<Renderer>().sortingOrder = _textConst.canvas.sortingOrder + 1;
//                    animation.loop = false;
//                    if ((i + 1) % 2 == 0)
//                    {
//                        animation.AnimationName = "b";
//                    }
//                    else
//                    {
//                        animation.AnimationName = "a";
//                    }
//                }
//            }
//            yield return new WaitForSeconds(0.7f);
//            //_skillUpAnimation.Reset();
//            _skillUpAnimation.GetComponent<Renderer>().sortingOrder = _textConst.canvas.sortingOrder + 1;
//            _skillUpAnimation.loop = false;
//            _skillUpAnimation.AnimationName = "c";
//            yield return new WaitForSeconds(0.7f);
//            OpenWindow<SkillSuccessWindow>(data);
//            RefreshView();
//            RefreshCallBackSkillView();
//        }
//        #endregion

//        #region Unity

//        public override void Awake()
//        {
//            InitModel();
//            InitView();
//        }

//        public override void OnEnable()
//        {
//            RefreshView();
//        }

//        #endregion
//    }
//}

