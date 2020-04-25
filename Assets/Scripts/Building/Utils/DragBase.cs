using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Build
{
    public class DragBase : Bubble, IDrag
    {
        ScrollRect _scrollRect;
        public delegate void DragEvent(PointerEventData pointerEventData);


        private DragEvent _onBeginDrag;
        private DragEvent _onDrag;
        private DragEvent _onEndDrag;
        protected override void Awake() 
        {
            base.Awake();
            _scrollRect = this.GetComponentInParent<ScrollRect>();
        }

        #region Field

        //public Image bgImage;
        //public Image image;


        ///// <summary>
        ///// 
        ///// </summary>
        //private RectTransform _canvasTransform;
        ///// <summary>
        ///// 
        ///// </summary>
        //private Camera _canvasCamera;
        ///// <summary>
        ///// 
        ///// </summary>
        //private Transform _parentTransform;

        //protected Vector2 pos;
        //private Vector2 _startPosition;
        //private bool _isDragging;

        //#endregion

        //public virtual void OnBeginDrag(PointerEventData eventData)
        //{
        //    this.HideHint();
        //    ActiveBG(false);

        //    _startPosition = Input.mousePosition;
        //    _isDragging = true;

        //    //this.image.transform.SetParent(_parentTransform.parent, false);
        //    this.image.transform.SetAsLastSibling();
        //    //if (UIEventManager.onReciepeDragStart != null)
        //    //{
        //    //    UIEventManager.onReciepeDragStart(base.myId);
        //    //}
        //}

        //public virtual void OnDrag(PointerEventData eventData)
        //{
        //    this.image.transform.SetAsLastSibling();
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, Input.mousePosition, _canvasCamera, out this.pos);
        //    this.image.transform.position = this._canvasTransform.TransformPoint(this.pos);

        //    var mapObject = GameCamera.Instance.GetScreenPointObject();
        //    if (mapObject)
        //    {
        //        var farm = mapObject.GetComponent<BuildingFarmland>();
        //        if (farm)
        //        {
        //            farm.Harvest();
        //        }
        //    }
        //}

        //public virtual void OnEndDrag(PointerEventData eventData)
        //{
        //    //this.image.transform.SetParent(_parentTransform, false);
        //    this.image.transform.localPosition = Vector3.zero;
        //    this.image.transform.SetAsLastSibling();
        //    this.HideHint();
        //    Hide();
        //    //base.MinIcon();
        //    //if (UIEventManager.onReciepeDragEnd != null)
        //    //{
        //    //    UIEventManager.onReciepeDragEnd();
        //    //}
        //    //if (UIEventManager.onReciepeAfterDragEnd != null)
        //    //{
        //    //    UIEventManager.onReciepeAfterDragEnd();
        //    //}
        //}

        //public void ActiveBG(bool value)
        //{
        //    if (this.bgImage)
        //    {
        //        this.bgImage.enabled = value;
        //    }
        //}

        //private void SetStartPosition()
        //{
        //    //this.image.transform.SetParent(_parentTransform, false);
        //    this.image.transform.SetSiblingIndex(1);

        //    _isDragging = false;
        //    this.ActiveBG(true);
        //    //base.MinIcon();
        //    this.image.rectTransform.anchoredPosition3D = Vector3.zero;
        //}

        //protected override void Init()
        //{
        //    base.Init();
        //    _parentTransform = this.transform;
        //    _canvasCamera = BubbleManager.Instance.canvasCamera;
        //    _canvasTransform = BubbleManager.Instance.canvasTransform;
        //}

        //protected virtual void ShowHint()
        //{
        //}

        //protected virtual void HideHint()
        //{
        //}

        //#region Unity

        //private void OnEnable()
        //{
        //    this.SetStartPosition();
        //}

        #endregion

        #region Interface
        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag(eventData);
            if (_onBeginDrag!=null)
                _onBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag(eventData);
            if (_onDrag != null)
                _onDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_scrollRect)
                _scrollRect.OnBeginDrag(eventData);
            EndDrag( eventData);
            if (_onEndDrag != null)
                _onEndDrag(eventData);
        }


        #endregion

        #region Method
        public void OnBeginDragSet(DragEvent onBeginDrag)
        {
            _onBeginDrag = onBeginDrag;
        }
        public void OnDragSet(DragEvent onDrag)
        {
            _onDrag = onDrag;
        }
        public void OnEndDragSet(DragEvent onEndDrag)
        {
            _onEndDrag = onEndDrag;
        }

        public virtual void BeginDrag(PointerEventData eventData)
        {

        }

        public virtual void Drag(PointerEventData eventData)
        {

        }

        public virtual void EndDrag(PointerEventData eventData)
        {

        }
        #endregion

    }
    }
