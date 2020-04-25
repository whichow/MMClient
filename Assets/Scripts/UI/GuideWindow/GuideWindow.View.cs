/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/30 10:25:15
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using Game.UI;
using System.Collections;

namespace Game
{
    public partial class GuideWindow
    {
        private Image _maskImage;
        //private BoxCollider2D _box2D;
        //private HighLightMask _lightMask;
        //private RectTransform _mask1;
        //private RectTransform _mask2;

        //private bool m_isMouseDown;
        //private Vector3 m_mousePosition;
        //private float m_lastClickTime;

        public void InitView()
        {
            _maskImage = Find<Image>("Image");
            //_box2D = Find<BoxCollider2D>("BlackGround");
            //_lightMask = Find<HighLightMask>("BlackGround");
            //_mask1 = Find<RectTransform>("BlackGround/mask1");
            //_mask2 = Find<RectTransform>("BlackGround/mask2");
            _maskImage.color = new Color(0, 0, 0, 0);
        }

        public void RefreshView()
        {
            //if (_actionData.action.IsMask == 1)
            //{
            //    _lightMask.color = new Color(0, 0, 0, 0.7f);
            //}
            //else
            //{
            //    _lightMask.color = new Color(0, 0, 0, 0);
            //}

            //List<float> hole1Pos = _actionData.action.Hole1Pos;
            //if (hole1Pos != null)
            //{
            //    if (hole1Pos.Count != 2)
            //    {
            //        _lightMask.targetRectTransform1 = null;
            //        _lightMask.targetRectTransform2 = null;
            //    }
            //    else
            //    {
            //        if (hole1Pos.Count == 2)
            //        {
            //            _lightMask.targetRectTransform1 = _mask1;
            //            //if (targetObject != null)
            //            //{
            //            //    _lightMask.targetRectTransform1.position = targetObject.transform.position;
            //            //}
            //            //else
            //            {
            //                _lightMask.targetRectTransform1.anchoredPosition = new Vector2(hole1Pos[0], hole1Pos[1]);
            //            }
            //            if (_actionData.action.Hole1Size.Count == 2)
            //            {
            //                _lightMask.targetRectTransform1.sizeDelta = new Vector2(_actionData.action.Hole1Size[0], _actionData.action.Hole1Size[1]);
            //            }
            //        }

            //        if (_actionData.action.Hole2Pos != null && _actionData.action.Hole2Pos.Count == 2)
            //        {
            //            _mask2.gameObject.SetActive(true);
            //            _lightMask.targetRectTransform2 = _mask2;
            //            _lightMask.targetRectTransform2.anchoredPosition = new Vector2(_actionData.action.Hole2Pos[0], _actionData.action.Hole2Pos[1]);
            //            if (_actionData.action.Hole2Size.Count == 2)
            //            {
            //                _lightMask.targetRectTransform2.sizeDelta = new Vector2(_actionData.action.Hole2Size[0], _actionData.action.Hole2Size[1]);
            //            }
            //        }
            //        else
            //        {
            //            _lightMask.targetRectTransform2 = null;
            //            _mask2.gameObject.SetActive(false);
            //        }
            //    }
            //}

        }


        //void OnMouseDown()
        //{
        //    m_isMouseDown = true;
        //    m_mousePosition = Input.mousePosition;
        //}

        //void OnMouseUp()
        //{
        //    if (m_isMouseDown)
        //    {
        //        bool drag = false;
        //        if (_actionData.action.ButtonType == 1)
        //        {
        //            var vector = Input.mousePosition - m_mousePosition;
        //            if (vector.sqrMagnitude > Screen.dpi) //算拖动
        //            {
        //                drag = true;
        //            }
        //        }
        //        if (drag)
        //        {
        //            m_mousePosition = Vector3.zero;
        //            return;
        //        }

        //        if (!_lightMask.IsRaycastLocationValid(m_mousePosition, KUIRoot.Instance.uiCamera) && !_lightMask.IsRaycastLocationValid(Input.mousePosition, KUIRoot.Instance.uiCamera))
        //        {
        //            StartCoroutine(NextFrameUpdate());
        //        }
        //    }
        //    m_isMouseDown = false;
        //}

        //private IEnumerator NextFrameUpdate()
        //{
        //    yield return new WaitForSeconds(0.0001f);

        //    if (m_lastClickTime > 0.5)
        //    {
        //        //if (KUIWindow.GetWindow<NoviceWindow>().active)
        //        {
        //            //if (_noviceData.action.ButtonType == 2)
        //            //{
        //            //    var go = EventSystem.current.currentSelectedGameObject;
        //            //    if (go)
        //            //    {
        //            //        var btn = go.GetComponent<Button>();
        //            //        if (btn)
        //            //        {
        //            //            CompleteGuideStep();
        //            //        }
        //            //    }
        //            //}
        //            //else if (_noviceData.action.ButtonType == 3)
        //            //{
        //            //    var window = KUIWindow.GetWindow<BuildingShopWindow>();
        //            //    if (window != null && !window.active)
        //            //    {
        //            //        CompleteGuideStep();
        //            //    }
        //            //}
        //            //else
        //            //{
        //            //    CompleteGuideStep();
        //            //}
        //        }
        //    }

        //    m_lastClickTime = 0;
        //}

        //public override void Update()
        //{
        //    base.Update();
        //    if (m_lastClickTime > 1)
        //    {

        //    }
        //    else
        //    {
        //        m_lastClickTime += Time.deltaTime;
        //    }

        //}

    }
}
