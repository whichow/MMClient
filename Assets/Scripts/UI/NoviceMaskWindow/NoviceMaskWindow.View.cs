// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "NoviceMaskWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using DG.Tweening;
using Game.Build;
using Game.Match3;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 引导类型6 剧情对话7（大猫）+控孔点击1（手指）（没有小猫文本提示和演示动画） 一般用于三消引导，依赖于三消操作后完成引导，m3FromBuiding字段控制点击完成
    /// </summary>
    partial class NoviceMaskWindow
    {
        #region Field

        private HighLightMask _lightMask;
        private RectTransform _mask1;
        private RectTransform _mask2;
        private GameObject _arrowGO;
        private RectTransform _arrowRT;
        //
        private GameObject _giveNameGO;
        private Transform _catMode;

        private GameObject _talkNameGO;
        private Transform _talkNameTf;
        private Text _talkNameTxt;
        private Text _talkTxt;

        private GameObject _picBgGO;
        private Button _btnSkip;
        private Button _nextBtn;

        private int talkIndx;
        private Tweener _nowTween;
        private Tweener tween;

        #endregion

        #region Method

        public void InitView()
        {
            _lightMask = transform.GetComponent<HighLightMask>();
            _mask1 = Find< RectTransform>("mask1");
            _mask2 = Find<RectTransform>("mask2");
            _arrowGO = Find("arrow");
            _arrowRT = Find<RectTransform>("arrow");
            
            //InM3Talk
            _giveNameGO = Find("GiveName");
            _catMode = Find<Transform>("GiveName/4008_Baigongzhu");

            _talkNameGO = Find("GiveName/Talk/Image");
            _talkNameTf = Find<Transform>("GiveName/Talk/Image");
            _talkNameTxt = Find<Text>("GiveName/Talk/Image/Name");
            _talkTxt = Find<Text>("GiveName/Talk/Text");

            _picBgGO = Find("GiveName/Bg");
            _btnSkip = Find<Button>("GiveName/ButtonSkip");
            _btnSkip.onClick.AddListener(OnSkipBtnClick);
            _nextBtn = Find<Button>("GiveName/Talk");
            _nextBtn.onClick.AddListener(OnNextBtnClick);
        }

        public void RefreshView()
        {
            if (M3GameManager.Instance != null)
            {
                if (_noviceData.action.InM3Lock == 0)
                {
                    M3GameManager.Instance.LockM3Item(_noviceData.action.M3ChangePos, CallBalck);
                }
                else if (_noviceData.action.InM3Lock == 1)
                {
                    M3GameManager.Instance.LockM3Item(new Int2[0], null);
                }
                else
                {
                    M3GameManager.Instance.LockM3Item(null, null);
                }
            }

            if (_noviceData.action.Hole1Pos != null && _noviceData.action.Hole1Pos.Count == 2)
            {
                _lightMask.targetRectTransform1 = _mask1;
                _lightMask.targetRectTransform1.anchoredPosition = new Vector2(_noviceData.action.Hole1Pos[0], _noviceData.action.Hole1Pos[1]);
                if (_noviceData.action.Hole1Size.Count == 2)
                {
                    _lightMask.targetRectTransform1.sizeDelta = new Vector2(_noviceData.action.Hole1Size[0], _noviceData.action.Hole1Size[1]);
                }

                if (_noviceData.action.Hole2Pos != null && _noviceData.action.Hole2Pos.Count == 2)
                {
                    _mask2.gameObject.SetActive(true);
                    _lightMask.targetRectTransform2 = _mask2;
                    _lightMask.targetRectTransform2.anchoredPosition = new Vector2(_noviceData.action.Hole2Pos[0], _noviceData.action.Hole2Pos[1]);
                    if (_noviceData.action.Hole2Size.Count == 2)
                    {
                        _lightMask.targetRectTransform2.sizeDelta = new Vector2(_noviceData.action.Hole2Size[0], _noviceData.action.Hole2Size[1]);
                    }
                }
                else
                {
                    _lightMask.targetRectTransform2 = null;
                    _mask2.gameObject.SetActive(false);
                }
            }
            else
            {
                _lightMask.targetRectTransform1 = null;
                _lightMask.targetRectTransform2 = null;
            }

            if (_noviceData.action.HandStartPosition != null && _noviceData.action.HandStartPosition.Count == 2)
            {
                Vector2 start = new Vector2(_noviceData.action.HandStartPosition[0], _noviceData.action.HandStartPosition[1]);
                _arrowGO.SetActive(true);
                _arrowRT.anchoredPosition = start;

                if (_noviceData.action.HandEndPosition != null && _noviceData.action.HandEndPosition.Count == 2)
                {
                    if (tween != null) tween.Kill();
                    Vector2 end = new Vector2(_noviceData.action.HandEndPosition[0], _noviceData.action.HandEndPosition[1]);
                    tween = KTweenUtils.DOLocalPath(_arrowRT, new Vector3[] { start, end, start }, _noviceData.action.HandTime, DG.Tweening.PathType.Linear, -1);
                }
            }
            else
            {
                _arrowGO.SetActive(false);
            }

            RefrshTalk();
        }

        private void RefrshTalk()
        {
            if (_noviceData.action != null)
            {
                if (_noviceData.action.ModelType == 1)
                {
                    _talkNameTf.localPosition = new Vector3(-347f, 100f, 0f);
                    _catMode.localPosition = new Vector3(-382f, -361f, 0f);
                    _catMode.localScale = new Vector3(-0.86251f, _catMode.localScale.y, _catMode.localScale.z);
                }
                else
                {
                    _talkNameTf.localPosition = new Vector3(368f, 100f, 0f);
                    _catMode.localPosition = new Vector3(329f, -361f, 0f);
                    _catMode.localScale = new Vector3(0.86251f, _catMode.localScale.y, _catMode.localScale.z);
                }
            }

            if (_noviceData.action.LevelTalkingType == 1)
            {
                _picBgGO.SetActive(false);
                //_giveNameGO.SetActive(true);
            }
            else
            {
                _picBgGO.SetActive(true);
                //_giveNameGO.SetActive(false);
            }

            _talkTxt.text = string.Empty;
            if (_noviceData.action.Dialogs != null)
            {
                _giveNameGO.SetActive(true);
                int type = _noviceData.action.Speaker[talkIndx];
                if (type == 0)
                {
                    _catMode.gameObject.SetActive(false);
                    _talkNameGO.SetActive(false);
                }
                else if (type == 1)
                {
                    string name = PlayerDataModel.Instance.mPlayerData.mName;
                    if (string.IsNullOrEmpty(name))
                    {
                        _catMode.gameObject.SetActive(false);
                        _talkNameGO.SetActive(true);
                        _talkNameTxt.text = KLocalization.GetLocalString(129300);
                    }
                    else
                    {
                        _catMode.gameObject.SetActive(false);
                        _talkNameGO.SetActive(true);
                        _talkNameTxt.text = name;
                    }
                }
                else if (type == 2)
                {
                    _catMode.gameObject.SetActive(true);
                    _talkNameGO.SetActive(true);
                    _talkNameTxt.text = KLocalization.GetLocalString(129301);
                }

                string str1 = string.Empty;
                if (KLocalization.GetLocalString(_noviceData.action.Dialogs[talkIndx]).Contains("{0}"))
                {
                    str1 = string.Format(KLocalization.GetLocalString(_noviceData.action.Dialogs[talkIndx]), PlayerDataModel.Instance.mPlayerData.mName);
                }
                else
                {
                    str1 = KLocalization.GetLocalString(_noviceData.action.Dialogs[talkIndx]);
                }
                _nowTween = KTweenUtils.DoText(_talkTxt, str1, 1.5f);

            }
            else
            {
                _giveNameGO.SetActive(false);
            }
        }

        private void OnSkipBtnClick()
        {
            GuideManager.Instance.CompleteStep();
            if (_noviceData.action.M3Stop == 1)
            {
                if (M3GameManager.Instance)
                    M3GameManager.Instance.isPause = false;
            }
            CloseWindow(this);
            if (M3GameManager.Instance)
                M3GameManager.Instance.LockM3Item(null, null);
        }

        private void OnNextBtnClick()
        {
            if (_nowTween != null)
            {
                _nowTween.Kill();
                _talkTxt.text = string.Empty;
            }
            talkIndx++;
            if (talkIndx < _noviceData.action.Dialogs.Count)
            {
                _talkTxt.text = string.Empty;
                RefreshView();
            }
            else
            {
                GuideManager.Instance.CompleteStep();
                if (_noviceData.action.M3Stop == 1)
                {
                    if (M3GameManager.Instance)
                        M3GameManager.Instance.isPause = false;
                }
                CloseWindow(this);
                if (M3GameManager.Instance)
                    M3GameManager.Instance.LockM3Item(null, null);
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            talkIndx = 0;
        }

        public override void Update()
        {
            base.Update();

            if (_lightMask)
            {
                #region window
                //拍照小屋退出
                if (_noviceData.action.Window != 0)
                {
                    KUIWindow window = null;
                    switch (Mathf.Abs(_noviceData.action.Window))
                    {
                        case 11:
                            window = KUIWindow.GetWindow<PhotoShopWindow>();
                            break;
                        default:
                            break;
                    }
                    if (window != null)
                    {
                        if ((_noviceData.action.Window > 0 && window.active) || (_noviceData.action.Window < 0 && !window.active))
                        {
                            GuideManager.Instance.CompleteStep();
                            if (M3GameManager.Instance != null)
                            {
                                if (_noviceData.action.M3Stop == 1)
                                {
                                    M3GameManager.Instance.isPause = false;
                                }
                                M3GameManager.Instance.LockM3Item(null, null);
                            }
                            CloseWindow(this);
                            return;
                        }
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                #endregion

                if (Input.GetMouseButtonUp(0))
                {
                    if (!_lightMask.IsRaycastLocationValid(Input.mousePosition, KUIRoot.Instance.uiCamera))
                    {
                        if (_noviceData.action.M3FromBuiding == 1)
                        {
                            GuideManager.Instance.CompleteStep();
                            if (M3GameManager.Instance != null)
                            {
                                if (_noviceData.action.M3Stop == 1)
                                {
                                    M3GameManager.Instance.isPause = false;
                                }
                                M3GameManager.Instance.LockM3Item(null, null);
                            }
                            CloseWindow(this);
                            return;
                        }

                        //if (建造建筑，移动后，可以安放)
                        if (Map.Instance && Map.Instance.IsBuildingMoveState && Map.Instance.LastAllowed)
                        {
                            GuideManager.Instance.CompleteStep();
                            CloseWindow(this);
                            return;
                        }
                    }
                }
            }
        }

        private void CallBalck()
        {
            GuideManager.Instance.CompleteStep();
            if (_noviceData.action.M3Stop == 1)
            {
                if (M3GameManager.Instance)
                    M3GameManager.Instance.isPause = false;
            }
            //全部解锁null
            //全部锁住 new Int2[0] 
            if (M3GameManager.Instance)
                M3GameManager.Instance.LockM3Item(null, null);
            CloseWindow(this);
        }

        #endregion

    }
}

