using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Game.Build;
namespace Game.UI
{
    public class DragItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {

        protected bool Active = true;
        [SerializeField]
        private Image bg;
        protected Build.Building.Category category;
        public bool isDrug;
        public RectTransform myCanvasTransform;
        [SerializeField]
        protected Transform myTransform;
        [SerializeField]
        protected Text needLevelText;
        protected Vector2 pos;
        private Vector2 startPosition;
        public Camera worldCamera;
        public Image image;

        public void ActiveBG(bool key)
        {
            if (this.bg != null)
            {
                this.bg.enabled = key;
                //this.needLevelText.enabled = key;
            }
        }

        protected void Discolor(bool key)
        {
            //base.image.material = !key ? Materials.Get(MaterialType.Grayscale) : null;
            //if (this.bg != null)
            //{
            //    this.bg.material = base.image.material;
            //}
        }

        private void HideHint()
        {
            //if (((this.category == EntityCategory.Factory) || ((this.category == EntityCategory.Farm) && (base.myId > 0))) || base.showHint)
            //{
            //    UIHintInterface.Instance.OnPointerClick(null);
            //}
        }

        public void Init(int id, Build.Building.Category category)
        {
            //this.SetStartPosition();
            //base.image.transform.localPosition = Vector3.zero;
            //this.category = category;
            //base.myId = id;
            //ItemData data = ItemDatabase.Instance[id];
            //base.image.sprite = ItemDatabase.Instance[id].icon;
            //this.Discolor(data.Recipe.Level <= Player.Current.level);
            //if (data.Recipe.Level > Player.Current.level)
            //{
            //    base.countText.text = "lv" + data.Recipe.Level;
            //    base.countText.gameObject.SetActive(true);
            //    this.Active = false;
            //}
            //else
            //{
            //    base.countText.gameObject.SetActive(false);
            //    this.Active = true;
            //}
        }

        //public override void Init(int id, int count, int need)
        //{
        //    this.Active = true;
        //    this.SetStartPosition();
        //    base.Init(id, count, need);
        //}

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            this.HideHint();
            if (this.Active)
            {
                this.startPosition = Input.mousePosition;
                this.isDrug = true;
                this.HideHint();
                this.image.transform.SetParent(this.myTransform.parent);
                this.image.transform.SetAsLastSibling();
                //if (UIEventManager.onReciepeDragStart != null)
                //{
                //    UIEventManager.onReciepeDragStart(base.myId);
                //}
            }
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (this.Active)
            {
                //Canvas myCanvas = KUIRoot.Instance.popup.GetComponent<Canvas>();// UIMainInterface.MainInterface.myCanvas;
                //this.myCanvasTransform = myCanvas.transform as RectTransform;
                //this.worldCamera = myCanvas.worldCamera;

                this.image.transform.SetAsLastSibling();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(this.myCanvasTransform, Input.mousePosition, this.worldCamera, out this.pos);
                this.image.transform.position = this.myCanvasTransform.TransformPoint(this.pos);
            }
        }

        private void OnEnable()
        {
            this.SetStartPosition();
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (this.Active)
            {
                this.image.transform.SetParent(this.myTransform);
                this.image.transform.localPosition = Vector3.zero;
                this.image.transform.SetAsLastSibling();
                this.HideHint();
                //base.MinIcon();
                //if (UIEventManager.onReciepeDragEnd != null)
                //{
                //    UIEventManager.onReciepeDragEnd();
                //}
                //if (UIEventManager.onReciepeAfterDragEnd != null)
                //{
                //    UIEventManager.onReciepeAfterDragEnd();
                //}
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            //this.ShowHint();
            //base.MaximazeIcon();
            //Canvas myCanvas = KUIRoot.Instance.popup.GetComponent<Canvas>();// UIMainInterface.MainInterface.myCanvas;
            //this.myCanvasTransform = myCanvas.transform as RectTransform;
            //this.worldCamera = myCanvas.worldCamera;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            this.isDrug = false;
        }

        private void SetStartPosition()
        {
            this.image.transform.SetParent(this.myTransform);
            this.image.transform.SetSiblingIndex(1);
            this.isDrug = false;
            this.ActiveBG(true);
            //base.MinIcon();
            this.image.transform.localPosition = Vector3.zero;
        }

        protected virtual void ShowHint()
        {
            //if ((this.category == EntityCategory.Factory) || ((this.category == EntityCategory.Farm) && (base.myId > 0)))
            //{
            //    UIHintInterface.ShowSowing(base.myId, base.iconScaleTweener);
            //}
            //else if (base.showHint)
            //{
            //    UIHintInterface.ShowDescription(base.myId, base.iconScaleTweener, HintType._info);
            //}
        }
    }
}
