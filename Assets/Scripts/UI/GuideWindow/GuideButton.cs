///*******************************************************************************
// * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
// * 
// * Author:          Coamy
// * Created:	        2019/4/30 11:16:48
// * Description:     
// * 
// * Update History:  
// * 
// *******************************************************************************/
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using Game;
//using Game.UI;
//using System.Collections;
//using UnityEngine.EventSystems;

//namespace Game
//{
//    [RequireComponent(typeof(Image))]
//    [RequireComponent(typeof(BoxCollider2D))]
//    public class GuideButton : MonoBehaviour
//    {
//        #region static

//        public static event Action OnClickEvent;                                //点击事件
//        public static event Action OnClickEvent_Window;

//        public static bool ClickInvisibleUI = true;                             //点击隐藏UI，不关闭窗口，无法点击
//        public static bool CanDrag = true;                                      //是否可以拖动

//        #endregion

//        public Vector4 Border;

//        private RectTransform RT;
//        private BoxCollider2D Box;

//        private Vector2 m_sizeDelta;
//        private Vector2 m_anchoredPosition;
//        private bool m_isMouseDown;
//        private Vector3 m_mousePosition;

//        private Canvas m_canvas;
//        private RectTransform m_canvasRT;

//        private float m_lastClickTime;
//        private Vector2 m_size;


//        public int ButtonType;
//        public HighLightMask _lightMask;
        

//        public Vector2 Size
//        {
//            get { return Box.size; }
//            set {
//                (transform as RectTransform).sizeDelta = value;
//                //Box.size = value;
//            }
//        }


//        void Awake()
//        {
//            RT = GetComponent<RectTransform>();
//            Box = GetComponent<BoxCollider2D>();

//            m_sizeDelta = RT.sizeDelta;
//            m_anchoredPosition = RT.anchoredPosition;

//        }

//        void Update()
//        {
//            if (m_lastClickTime > 1)
//            {

//            }
//            else
//            {
//                m_lastClickTime += Time.deltaTime;
//            }
//        }

//        void OnMouseDown()
//        {
//            m_isMouseDown = true;
//            m_mousePosition = Input.mousePosition;
//        }

//        void OnMouseUp()
//        {
//            if (m_isMouseDown)
//            {
//                Debuger.Log("OnMouseUp");
//                bool drag = false;
//                if (ButtonType == 1)
//                {
//                    Debuger.Log("ButtonType");
//                    var vector = Input.mousePosition - m_mousePosition;
//                    if (vector.sqrMagnitude > Screen.dpi) //算拖动
//                    {
//                        drag = true;
//                    }
//                }
//                Debuger.Log("drag: " + drag);
//                if (drag)
//                {
//                    m_mousePosition = Vector3.zero;
//                    return;
//                }
//                Debuger.Log("IsRaycastLocationValid");

//                if (!_lightMask.IsRaycastLocationValid(m_mousePosition, KUIRoot.Instance.uiCamera) && !_lightMask.IsRaycastLocationValid(Input.mousePosition, KUIRoot.Instance.uiCamera))
//                {
//                    Debuger.Log("aaaaa");
//                    StartCoroutine(NextFrameUpdate());
//                }
//            }
//            m_isMouseDown = false;
//        }

//        private IEnumerator NextFrameUpdate()
//        {
//            //yield return new WaitForSeconds(0.0001f);
//            yield return new WaitForEndOfFrame();

//            Debuger.Log("bbbb:"+ m_lastClickTime);
//            if (m_lastClickTime > 1)
//            {
//                Debuger.Log("cccc");
//                if (ButtonType == 2)
//                {
//                    Debuger.Log("dddd");
//                    var go = EventSystem.current.currentSelectedGameObject;
//                    if (go)
//                    {
//                        var btn = go.GetComponent<Button>();
//                        if (btn)
//                        {
//                    Debuger.Log("dddd-------------------");
//                            CompleteGuideStep();
//                        }
//                    }
//                }
//                else if (ButtonType == 3)
//                {
//                    Debuger.Log("eeee");
//                    var window = KUIWindow.GetWindow<BuildingShopWindow>();
//                    if (window != null && !window.active)
//                    {
//                    Debuger.Log("eeeee-------------------");
//                        CompleteGuideStep();
//                    }
//                }
//                else
//                {
//                    Debuger.Log("ffff--------------");
//                    CompleteGuideStep();
//                }
//            }

//            m_lastClickTime = 0;

//            yield return null;
//        }

//        private void CompleteGuideStep()
//        {
//            OnClickEvent?.Invoke();
//        }
//    }

//}
