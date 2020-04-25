// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "NoviceWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using DG.Tweening;
using Game.Build;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 引导类型1 挖孔点击，小猫提示文本，箭头指示 
    /// </summary>
    partial class NoviceWindow
    {
        #region Field

        private Image _maskImage;
        private HighLightMask _lightMask;
        private RectTransform _mask1;
        private RectTransform _mask2;
        private RectTransform _arrowRT;
        private GameObject _arrowGO;

        private GameObject _catGO;
        private Transform _catModel;
        private Text _talkTxt;
        private Transform _talkTxtTf;

        private Vector3 vecDown;
        private Vector3 vecUp;
        private Tweener tween;
        private Tweener _nowTween;

        private float m_lastClickTime;
        #endregion

        #region Method

        public void InitView()
        {
            _maskImage = Find<Image>("Image");
            _lightMask = Find<HighLightMask>("BlackGround");
            _mask1 = Find<RectTransform>("BlackGround/mask1");
            _mask2 = Find<RectTransform>("BlackGround/mask2");
            _arrowRT = Find<RectTransform>("BlackGround/arrow");
            _arrowGO = Find("BlackGround/arrow");

            _catGO = Find("BlackGround/Cat");
            _catModel = Find<Transform>("BlackGround/Cat/SkeletonGraphic (4008_Baigongzhu)");
            _talkTxt = Find<Text>("BlackGround/Cat/Talk/Text");
            _talkTxtTf = Find<Transform>("BlackGround/Cat/Talk");

            _maskImage.raycastTarget = true;
            _maskImage.color = new Color(0, 0, 0, 0);
        }

        public void RefreshView()
        {
            Debuger.Log("刷新，不能点击 ！！！！！" + _noviceData.action.ID);
            _maskImage.raycastTarget = true;
            m_lastClickTime = 0;

            #region 点击约束
            GameObject targetObject = null;
            if (_noviceData.action.FunctionID != 0 || _noviceData.action.FunctionArea != 0)
            {
                var list = new List<GameObject>();
                if (_noviceData.action.FunctionArea != 0)
                {
                    var area = AreaManager.Instance.GetAreaObject(_noviceData.action.FunctionArea);
                    if (area != null)
                    {
                        list.Add(area);
                    }
                }
                if (_noviceData.action.FunctionID != 0)
                {
                    var go = BuildingManager.Instance.GetEntitieObj(_noviceData.action.FunctionID);
                    if (go != null)
                    {
                        list.Add(go);
                    }
                }

                if (list.Count > 0)
                {
                    GameCamera.Block(this.gameObject, GameCamera.Restrictions.Map);
                    GameCamera.Allow(this.gameObject, GameCamera.Restrictions.Click, list);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(_noviceData.action.ButtonName))
                {
                    var list = new List<GameObject>();
                    targetObject = GameObject.Find(_noviceData.action.ButtonName);
                    if (targetObject != null)
                    {
                        list.Add(targetObject);
                        GameCamera.Block(this.gameObject, GameCamera.Restrictions.UI);
                        GameCamera.Allow(this.gameObject, GameCamera.Restrictions.Click, allowedUI: list);
                    }
                }
                else
                {
                    GameCamera.Disallow(this.gameObject);
                    GameCamera.Unblock(this.gameObject);
                }
                GameCamera.Block(this.gameObject, GameCamera.Restrictions.Map);
            }
            #endregion

            if (_noviceData.action.ModelType == 1)
            {
                _catModel.localPosition = new Vector3(-479f, -403f, 0f);
                _talkTxtTf.localPosition = new Vector3(-267f, -154f, 0f);
                _catModel.localScale = new Vector3(-0.6563154f, _catModel.localScale.y, _catModel.localScale.z);
            }
            else
            {
                _talkTxtTf.localPosition = new Vector3(548f, -154f, 0f);
                _catModel.localPosition = new Vector3(750f, -403f, 0f);
                _catModel.localScale = new Vector3(0.6563154f, _catModel.localScale.y, _catModel.localScale.z);
            }

            if (_noviceData.action.Dialogs != null)
            {
                if (_noviceData.action.Dialogs.Count > 0)
                {
                    _talkTxt.text = KLocalization.GetLocalString(_noviceData.action.Dialogs[0]);
                    _catGO.gameObject.SetActive(true);
                }
            }
            else
            {
                _catGO.gameObject.SetActive(false);
            }

            if (_noviceData.action.IsMask == 1)
            {
                _lightMask.color = new Color(0, 0, 0, 0.7f);
            }
            else
            {
                _lightMask.color = new Color(0, 0, 0, 0);
            }

            //_goBtn = GameObject.Find(_noviceData.action.button);
            //if (_goBtn != null)
            //{
            //    _goArrow.transform.position = _goBtn.transform.position;
            //    _lightMask.targetRectTransform1 = (RectTransform)_goBtn.transform;
            //}
            //else
            //{
            //    Debug.Log("找不到button");
            //}

            #region 挖孔
            List<float> hole1Pos = _noviceData.action.Hole1Pos;
            if (hole1Pos != null)
            {
                if (hole1Pos.Count != 2)
                {
                    _lightMask.targetRectTransform1 = null;
                    _lightMask.targetRectTransform2 = null;
                }
                else
                {
                    if (hole1Pos.Count == 2)
                    {
                        _lightMask.targetRectTransform1 = _mask1;
                        if (targetObject != null)
                        {
                            _lightMask.targetRectTransform1.position = targetObject.transform.position;
                        }
                        else
                        {
                            _lightMask.targetRectTransform1.anchoredPosition = new Vector2(hole1Pos[0], hole1Pos[1]);
                        }
                        if (_noviceData.action.Hole1Size.Count == 2)
                        {
                            _lightMask.targetRectTransform1.sizeDelta = new Vector2(_noviceData.action.Hole1Size[0], _noviceData.action.Hole1Size[1]);
                        }
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
            }
            #endregion

            if (_noviceData.action.HandStartPosition != null && _noviceData.action.HandStartPosition.Count == 2)
            {
                Vector2 start = new Vector2(_noviceData.action.HandStartPosition[0], _noviceData.action.HandStartPosition[1]);
                _arrowGO.gameObject.SetActive(true);
                _arrowRT.anchoredPosition = start;

                if (tween != null) tween.Kill();
                if (_noviceData.action.HandEndPosition != null && _noviceData.action.HandEndPosition.Count == 2)
                {
                    Vector2 end = new Vector2(_noviceData.action.HandEndPosition[0], _noviceData.action.HandEndPosition[1]);
                    tween = KTweenUtils.DOLocalPath(_arrowRT, new Vector3[] { start, end, start }, _noviceData.action.HandTime, DG.Tweening.PathType.Linear, -1);
                }
            }
            else
            {
                _arrowGO.gameObject.SetActive(false);
            }

            if (_noviceData.action.Window == 1)
            {
                KUIWindow.CloseWindow<FunctionWindow>();
            }

        }

        public override void Update()
        {
            if (m_lastClickTime > 0.5)
            {
                if (_maskImage.raycastTarget)
                {
                    //Debuger.Log("时间到了，可以点击了 " + _noviceData.action.ID);
                    _maskImage.raycastTarget = false;
                }
            }
            else
            {
                m_lastClickTime += Time.deltaTime;
            }


            base.Update();
            if (Input.GetMouseButtonDown(0))
            {
                vecDown = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                vecUp = Input.mousePosition;
            }
            if (_lightMask)
            {
                #region window
                if (_noviceData.action.Window != 0)
                {
                    KUIWindow window = null;
                    switch (Mathf.Abs(_noviceData.action.Window))
                    {
                        case 1:
                            window = KUIWindow.GetWindow<FunctionWindow>();
                            break;
                        case 2:
                            window = KUIWindow.GetWindow<LevelInfoWindow>();
                            break;
                        case 3:
                            window = KUIWindow.GetWindow<MapSelectWindow>();
                            break;
                        case 4:
                            window = KUIWindow.GetWindow<BagWindow>();
                            break;
                        case 5:
                            window = KUIWindow.GetWindow<CatWindow>();
                            break;
                        case 6:
                            window = KUIWindow.GetWindow<AreaUnlockBox>();
                            break;
                        case 7:
                            window = KUIWindow.GetWindow<PhotoShopPickCardHighWindow>();
                            break;
                        case 8:
                            window = KUIWindow.GetWindow<PhotoShopGotBuildWindow>();
                            break;
                        case 9:
                            window = KUIWindow.GetWindow<DiscoveryWindow>();
                            break;
                        case 10:
                            window = KUIWindow.GetWindow<FormulaShopWindow>();
                            break;
                        case 11:
                            window = KUIWindow.GetWindow<PhotoShopWindow>();
                            break;
                        case 12:
                            window = KUIWindow.GetWindow<OrnamentShopWindow>();
                            break;
                        case 13:
                            window = KUIWindow.GetWindow<ChooseCatWindow>();
                            break;
                        case 14:
                            window = KUIWindow.GetWindow<BuildingShopWindow>();
                            break;
                        case 15:
                            window = KUIWindow.GetWindow<DiscoveryMissionWindow>();
                            break;
                        case 16:
                            window = KUIWindow.GetWindow<MessageBox>();
                            break;
                        case 17:
                            window = KUIWindow.GetWindow<GameOverWindow>();
                            break;
                        default:
                            break;
                    }

                    if (window != null)
                    {
                        if ((_noviceData.action.Window > 0 && window.active) || (_noviceData.action.Window < 0 && !window.active))
                        {
                            //Debuger.Log("Window      " + _noviceData.action.Window + window.active);
                            CompleteGuideStep();
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

                if (_noviceData.action.CompleteCondition > 0)
                {
                    if (GuideManager.Instance.CheckConditionResult(_noviceData.action.CompleteCondition))
                    {
                        CompleteGuideStep();
                    }
                    return;
                }

                if (vecUp != Vector3.zero && vecDown != Vector3.zero)
                {
                    bool drag = false;
                    if (_noviceData.action.ButtonType == 1 || _noviceData.action.ButtonType == 0)
                    {
                        var vector = vecUp - vecDown;
                        if (vector.sqrMagnitude > Screen.dpi) //算拖动
                        {
                            drag = true;
                        }
                    }
                    //else if (_noviceData.action.ButtonType == 0 && vecUp != vecDown)
                    //{
                    //    drag = true;
                    //}
                    //if (_noviceData.action.ButtonType == 1 && vecUp != vecDown)
                    if (drag)
                    {
                        vecDown = Vector3.zero;
                        vecUp = Vector3.zero;
                        return;
                    }

                    if (!_lightMask.IsRaycastLocationValid(vecDown, KUIRoot.Instance.uiCamera) && !_lightMask.IsRaycastLocationValid(vecUp, KUIRoot.Instance.uiCamera))
                    {
                        if (_noviceData.action.ButtonType == 2)
                        {
                            var go = EventSystem.current.currentSelectedGameObject;
                            if (go)
                            {
                                bool isBtn = false;
                                var btn = go.GetComponent<Button>();
                                if (btn)
                                {
                                    isBtn = true;
                                }
                                else
                                {
                                    var toggle = go.GetComponent<Toggle>();
                                    if (toggle)
                                    {
                                        isBtn = true;
                                    }
                                }
                                if (isBtn)
                                {
                                    CompleteGuideStep();
                                    return;
                                }
                            }
                        }
                        else if (_noviceData.action.ButtonType == 3)
                        {
                            var window = KUIWindow.GetWindow<BuildingShopWindow>();
                            if (window != null && !window.active)
                            {
                                CompleteGuideStep();
                                return;
                            }
                        }
                        else
                        {
                            CompleteGuideStep();
                            return;
                        }

                        vecDown = Vector3.zero;
                        vecUp = Vector3.zero;
                    }
                }
            }
        }

        private void CompleteGuideStep()
        {
            vecDown = Vector3.zero;
            vecUp = Vector3.zero;

            if (m_lastClickTime < 0.5)
            {
                Debuger.Log("时间未到！！！！！" + _noviceData.action.ID);
                _maskImage.raycastTarget = true;
                return;
            }
            //m_lastClickTime = 0;

            Debuger.Log("完成当前步！！！！！" + _noviceData.action.ID);
            if (_noviceData.action != null)
            {
                _lightMask.targetRectTransform1 = null;
                _lightMask.targetRectTransform2 = null;

                GuideManager.Instance.CompleteStep();
                if (_noviceData.action.LockButton != null && _noviceData.action.LockButton.Count > 0)
                {
                    PlayerDataModel.Instance.mPlayerData.RecoverFunction();
                }
                CloseWindow(this);
                GameCamera.Disallow(this.gameObject);
                GameCamera.Unblock(this.gameObject);
                _noviceData.action = null;
            }
        }

        #endregion

    }
}

