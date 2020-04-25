using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Game.Build
{
    class BuildingObstacleClearCom : DragBase
    {
        #region Field

        //public BuildingObstacleClearCom()
        //{
        //    bubbleName = "obstacleClear";
        //}
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



        #endregion

        protected override void  Start()
        {

        }
    }


}

