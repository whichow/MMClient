using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Build
{
    class BuildingObstacleClearView : KUIWindow
    {
        GameObject _clearBG;
        RectTransform _dragTool;

        RectTransform _clearBGRect;
        Image _dragToolImg;
        RectTransform _dragToolRect;
        GameObject _collide;
        RectTransform _collideRect;
        RectTransform _rootRect;
        Text _clearToolNum;
        Vector3 _dragToolPos;

        bool isEndDrag = true;
        BuildingObstacleClearCom buildingObstacleClearCom;

        private BuildingObstacleClearCom.DragEvent _onBeginDrag;
        private BuildingObstacleClearCom.DragEvent _onDrag;
        private BuildingObstacleClearCom.DragEvent _clearObstacle;
        private Action _onEndDrag;




        public BuildingObstacleClearView() 
            : base(UILayer.kNormal, UIMode.kNone)
        {
            uiPath = "ObstacleFunction";

        }

        /// <summary>
        /// 
        /// </summary>
       public GameObject ClearBG
        {
            get
            {
                return _clearBG;
            }
        }

        /// <summary>
        /// 显示障碍物清理工具
        /// </summary>
        /// <param name="endDragCallBack"></param>
         public void Show(Transform target, Action endDragCallBack)
         {
            _clearBG.SetActive(true);
            isEndDrag = true;
            _dragToolRect.anchoredPosition = new Vector2(0, 0);
            EndDragSet(endDragCallBack);
            _dragTool.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        public void Hide()
        {
            isEndDrag = false;
            _clearBG.SetActive(false);
        }
        /// <summary>
        /// 设置障碍物拖拽结束 回调，----进行障碍物清理
        /// </summary>
        /// <param name="endDragCallBack"></param>
        public void EndDragSet(Action endDragCallBack)
        {
            _onEndDrag = endDragCallBack;
        }

        /// <summary>
        /// 障碍物清理工具 图标设置
        /// </summary>
        /// <param name="iconId"></param>
        public void DragToolImgSet(int iconId)
        {
            _dragToolImg.overrideSprite = KIconManager.Instance.GetItemIcon(iconId);
        }

        /// <summary>
        /// 障碍物清理工具 图标设置
        /// </summary>
        /// <param name="iconId"></param>
        public void ClearToolNumSet(int num)
        {
            _clearToolNum.text = num.ToString();
        }
        /// <summary>
        /// 鼠标拖拽事件
        /// </summary>
        /// <param name="pointerEventData"></param>
        public void Drag(PointerEventData pointerEventData)
        {
            if (isEndDrag)
            {
                Vector2 pos = BuildingObstacleClearMgr.Instance.ScreenToUI(pointerEventData.position, _rootRect);
                _dragToolRect.position = _rootRect.TransformPoint(pos);  //转换成统一的世界坐标
                var mapObject = GameCamera.Instance.GetScreenPointObject();
                if (mapObject == GameCamera.Instance.curClickMapObject.transform)
                {
                    isEndDrag = false;
                    if (_onEndDrag != null)
                        _onEndDrag();
                    this.Hide();
                    //if (_clearObstacle != null)
                    //    _clearObstacle();
                }

            }
        }
        /// <summary>
        /// 鼠标拖拽结束
        /// </summary>
        /// <param name="pointerEventData"></param>
        public void EndDrag(PointerEventData pointerEventData)
        {
            //isEndDrag = false;
            //if (_onEndDrag!=null)
            //    _onEndDrag();
            _dragToolRect.anchoredPosition = new Vector2(0, 0);
        }
        
        #region Unity
        public override void Awake()
        {
            base.Awake();

            _clearBG = Find("ClearBG");
            _clearBGRect = _clearBG.GetComponent<RectTransform>();
            GameObject gm = Find("ClearBG/Collide/DragTool");
            _dragTool = gm.GetComponent<RectTransform>();
            _dragToolImg = gm.GetComponent<Image>();
            _dragToolRect = _dragToolImg.GetComponent<RectTransform>();
            _collide = Find("ClearBG/Collide");
            _collideRect = _collide.GetComponent<RectTransform>();
            _clearToolNum = Find("ClearBG/ClearToolNum").GetComponent<Text>();
            _rootRect = gameObject.GetComponent<RectTransform>();

            buildingObstacleClearCom = _collide.AddComponent<BuildingObstacleClearCom>();
            buildingObstacleClearCom.OnDragSet(Drag);
            buildingObstacleClearCom.OnEndDragSet(EndDrag);

        }
        /// <summary>
        /// 设置障碍物工具位置
        /// </summary>
        /// <param name="pos"></param>
        public void ClearToolPosSet(Vector3 pos)
        {
            _clearBGRect.anchoredPosition = BuildingObstacleClearMgr.Instance.WorldToUI(pos)+new Vector3(0,200,0);
        }

        #endregion
    }
}
