// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "GiveNameWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using DG.Tweening;
using Game.Match3;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 引导类型7 剧情对话 大猫 有演示动画
    /// </summary>
    partial class GiveNameWindow
    {
        #region Field

        private GameObject _picBgGO;
        private Image _lightMask;

        private RectTransform _catModeRT;

        private Text _talkTxt;
        private Text _talkNameTxt;
        private RectTransform _talkNameRT;
        private GameObject _talkNameGO;

        private GameObject[] transMovie;

        private Button _btnSkip;
        private Button _nextBtn;

        private int talkIndx;
        private Tweener _nowTween;

        #endregion

        #region Method

        public void InitView()
        {
            _picBgGO = Find("Bg");
            _lightMask = Find<Image>("Image");
            _catModeRT = Find<RectTransform>("4008_Baigongzhu");
            _talkTxt = Find<Text>("Talk/Text");
            _talkNameTxt = Find<Text>("Talk/Image/Name");
            _talkNameRT = Find<RectTransform>("Talk/Image");
            _talkNameGO = Find("Talk/Image");

            _btnSkip = Find<Button>("ButtonSkip");
            _btnSkip.onClick.AddListener(OnSkipBtnClick);

            var movieParent = Find<Transform>("GuideMovie");
            transMovie = new GameObject[movieParent.childCount];
            for (int i = 0; i < transMovie.Length; i++)
            {
                transMovie[i] = Find("GuideMovie/Guide_" + (i + 1));
            }

            _nextBtn = Find<Button>("Talk");
            _nextBtn.onClick.AddListener(OnNextBtnClick);
        }

        private void OnSkipBtnClick()
        {
            if (_giveNameData.action != null)
            {
                GuideManager.Instance.CompleteStep();
                if (_giveNameData.action.M3Stop == 1)
                {
                    M3GameManager.Instance.isPause = false;
                }
                CloseWindow(this);
                if (_giveNameData.action.IsGameOverType == 2)
                {
                    M3GameManager.Instance.isGameoverNeedTutorial = false;
                    M3GameManager.Instance.modeManager.ShowGameOver();
                }
                _giveNameData.action = null;
            }
        }

        private void OnNextBtnClick()
        {
            if (_nowTween != null)
            {
                _nowTween.Kill();
                _talkTxt.text = string.Empty;
            }
            talkIndx++;
            if (talkIndx < _giveNameData.action.Dialogs.Count)
            {
                _talkTxt.text = string.Empty;
                RefreshView();
            }
            else
            {
                OnSkipBtnClick();
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            talkIndx = 0;
        }

        public void RefreshView()
        {
            for (int i = 0; i < transMovie.Length; i++)
            {
                transMovie[i].SetActive(false);
            }

            if (_giveNameData.action.IsMask == 1)
            {
                _lightMask.color = new Color(0, 0, 0, 0.7f);
            }
            else
            {
                _lightMask.color = new Color(0, 0, 0, 0);
            }

            int index = _giveNameData.action.LevelImage;
            if (index != 0 && transMovie[index] != null)
            {
                transMovie[index].SetActive(true);
            }

            if (_giveNameData.action.ModelType == 1)
            {
                _catModeRT.localScale = new Vector3(-0.86251f, _catModeRT.localScale.y, _catModeRT.localScale.z);
                _talkNameRT.anchoredPosition = new Vector2(-358, 20);
                _catModeRT.anchoredPosition = new Vector2(-344, 0);
            }
            else
            {
                _catModeRT.localScale = new Vector3(0.86251f, _catModeRT.localScale.y, _catModeRT.localScale.z);
                _catModeRT.anchoredPosition = new Vector2(344, 0);
                _talkNameRT.anchoredPosition = new Vector2(358, 20);
            }

            if (_giveNameData.action.LevelTalkingType == 1)
            {
                _picBgGO.SetActive(false);
            }
            else
            {
                _picBgGO.SetActive(true);
            }

            _talkTxt.text = string.Empty;
            if (_giveNameData.action.Dialogs != null)
            {
                int type = _giveNameData.action.Speaker[talkIndx];
                if (type == 0)
                {
                    _talkNameGO.SetActive(false);
                    _talkNameGO.SetActive(false);
                }
                else if (type == 1)
                {
                    string name = PlayerDataModel.Instance.mPlayerData.mName;
                    if (string.IsNullOrEmpty(name))
                    {
                        _catModeRT.gameObject.SetActive(false);
                        _talkNameGO.SetActive(true);
                        _talkNameTxt.text = KLocalization.GetLocalString(129300);
                    }
                    else
                    {
                        _catModeRT.gameObject.SetActive(false);
                        _talkNameGO.SetActive(true);
                        _talkNameTxt.text = name;
                    }
                }
                else if (type == 2)
                {
                    _catModeRT.gameObject.SetActive(true);
                    _talkNameGO.SetActive(true);
                    _talkNameTxt.text = KLocalization.GetLocalString(129301);
                }

                string str1 = string.Empty;
                if (KLocalization.GetLocalString(_giveNameData.action.Dialogs[talkIndx]) != null)
                {
                    if (KLocalization.GetLocalString(_giveNameData.action.Dialogs[talkIndx]).Contains("{0}"))
                    {
                        str1 = string.Format(KLocalization.GetLocalString(_giveNameData.action.Dialogs[talkIndx]), PlayerDataModel.Instance.mPlayerData.mName);
                    }
                    else
                    {
                        str1 = KLocalization.GetLocalString(_giveNameData.action.Dialogs[talkIndx]);
                    }
                }
                _nowTween = KTweenUtils.DoText(_talkTxt, str1, 1.5f);
            }
        }

        #endregion
    }
}

