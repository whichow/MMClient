using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace  Game.Build
{
    public class BubbleObstacleClear : DragBase
    {
        #region Field

        //public Image bgImage;
        //public Text clearToolNum;

        RectTransform _dragTool;

        Image _dragToolImg;
        //SkeletonGraphic _dragSkeletonGraphic;
        RectTransform _dragToolRect;
        GameObject _collide;
        GameObject _hammer;
        GameObject _axe;
        GameObject _waterNet;
        GameObject _bg;
        SkeletonGraphic _hammerSkeletonGraphic;
        SkeletonGraphic _axeSkeletonGraphic;
        SkeletonGraphic _waterNetSkeletonGraphic;
        RectTransform _collideRect;
        RectTransform _rootRect;
        Text _clearToolNum;
        Vector3 _dragToolPos;

        RectTransform _rootTrans;

        Transform _targettrans;

        bool isEndDrag = true;
        bool isSelect = false;
        BubbleObstacleClear _bubbleObstacleClear;

        TweenPos _tweenPosClearBG;

        TweenScl _clearToolNumTweenScl;

        TweenBase[] tweenBaseList;
        private enum toolTep
        {
             hammer=101,
             axe =102,
             waterNet =103,
        }

        toolTep _toolTep;

        float height;
        private BuildingObstacleClearCom.DragEvent _onBeginDrag;
        private BuildingObstacleClearCom.DragEvent _onDrag;
        private BuildingObstacleClearCom.DragEvent _clearObstacle;
        private Action _onEndDrag;

        
        /// <summary>
        /// 
        /// </summary>
        private RectTransform _canvasTransform;
        /// <summary>
        /// 
        /// </summary>
        private Camera _canvasCamera;
        /// <summary>
        /// 
        /// </summary>
        private Transform _parentTransform;

        protected Vector2 pos;
        private Vector2 _startPosition;
        private bool _isDragging;

        #endregion



        /// <summary>
        /// 显示障碍物清理工具
        /// </summary>
        /// <param name="endDragCallBack"></param>
        public override void Show()
        {
            base.Show();
            _bg.SetActive(true);
            isEndDrag = true;
            _dragToolRect.anchoredPosition = new Vector2(0, 0);
            _dragTool.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            playerTween();
        }
        public override void Hide()
        {
            base.Hide();
            isEndDrag = false;
            //_clearBG.SetActive(false);
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
            _toolTep = (toolTep)iconId;
            toggleType();
            //_dragToolImg.overrideSprite = KIconManager.Instance.GetItemIcon(iconId);
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
        public override void Drag(PointerEventData pointerEventData)
        {
            base.Drag(pointerEventData);

            if (isEndDrag)
            {
                //Vector3 pos = ScreenToWorid(pointerEventData.position);
                //Debug.Log("拖拽坐标"+ pos+"//"+ pointerEventData.position);
                //_dragToolRect.position =  _rootRect.TransformPoint(pos);  //转换成统一的世界坐标
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, pointerEventData.position, _canvasCamera, out this.pos);
                _dragToolRect.position = this._canvasTransform.TransformPoint(this.pos);
                //_dragToolRect.position = ScreenToWorid(pointerEventData.position);
                var mapObject = GameCamera.Instance.GetScreenPointObject();
                if (mapObject == GameCamera.Instance.curClickMapObject.transform)
                {
                    isEndDrag = false;
                    isSelect = true;
                    if (_onEndDrag != null)
                        _onEndDrag();

                    var animName = "touch";//trackEntry.Animation.Name;

                    PlayAnimation(animName, false);

                    //this.Hide();

                }

            }
        }
        public override void BeginDrag(PointerEventData eventData)
        {
            base.BeginDrag(eventData);
            isSelect = false;
            _bg.SetActive(false);
        }

        /// <summary>
        /// 鼠标拖拽结束
        /// </summary>
        /// <param name="pointerEventData"></param>
        public override void EndDrag(PointerEventData pointerEventData)
        {
            if (!isSelect)
            {
                this.Hide();
            }
            //_dragToolRect.anchoredPosition = new Vector2(0, 0);
        }
        private void animationEnd(Spine.TrackEntry trackEntry)
        {
            //if (trackEntry.Loop)
            //{
            //    return;
            //}


            this.Hide();
        }
        private void toggleType()
        {
            _hammerSkeletonGraphic.gameObject.SetActive(_toolTep == toolTep.hammer);
             _axeSkeletonGraphic.gameObject.SetActive(_toolTep == toolTep.axe);
            _waterNetSkeletonGraphic.gameObject.SetActive(_toolTep == toolTep.waterNet);
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="loop"></param>
        public void PlayAnimation(string animation, bool loop)
        {
            //SkeletonGraphic _hammerSkeletonGraphic;
            //SkeletonGraphic _axeSkeletonGraphic;
            //SkeletonGraphic _waterNetSkeletonGraphic;
            SkeletonGraphic skeletonGraphic = null;
            if (_toolTep == toolTep.hammer)
            {
                 
                skeletonGraphic = _hammerSkeletonGraphic;
            }

            else if(_toolTep == toolTep.axe)
                skeletonGraphic = _axeSkeletonGraphic;
            else if(_toolTep == toolTep.waterNet)
                skeletonGraphic = _waterNetSkeletonGraphic;

            if (skeletonGraphic)
            {

                if (!string.IsNullOrEmpty(animation))
                {
                    skeletonGraphic.AnimationState.SetAnimation(0, animation, loop);
                    //_dragSkeletonGraphic.loop = loop;
                    //skeletonGraphic.AnimationName = animation;
                }
            }

        }

        private void playerTween()
        {
            if (tweenBaseList == null)
                return;
            foreach (var item in tweenBaseList)
            {
                item.PlayBack();
            }
        }
        protected override void Init()
        {
            base.Init();
            _parentTransform = this.transform;
            //_canvasCamera = BubbleManager.Instance.canvasCamera;
            //_canvasTransform = BubbleManager.Instance.canvasTransform;
        }

        protected virtual void ShowHint()
        {
        }

        protected virtual void HideHint()
        {
        }

        public RectTransform canvasTransform
        {
            get;
            private set;
        }

        public Camera canvasCamera
        {
            get;
            private set;
        }
        public Camera buildingMainCamera
        {
            get;
            private set;
        }
        public Vector3 WorldToUI(Vector3 position, RectTransform rectTransform = null)
        {
            var screenPoint = RectTransformUtility.WorldToScreenPoint(buildingMainCamera, position);

            return ScreenToUI(screenPoint, rectTransform);
        }

        public Vector3 ScreenToUI(Vector3 position, RectTransform rectTransform = null)
        {
            Vector2 localPoint;
            if (rectTransform == null)
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, position, _canvasCamera, out localPoint);
            else
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, _canvasCamera, out localPoint);
            return localPoint;
        }
        Vector2 localPoint;
        public Vector3 ScreenToWorid(Vector3 position)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, position, _canvasCamera, out localPoint);
            localPoint = this._canvasTransform.TransformPoint(localPoint);

            return localPoint;
        }

        //设置UI跟随的目标，并刷新位置
        public override void FollowTarget(Transform target)
        {
        }
        public void FollowTarget(Building target)
        {
            _targettrans = target.entityView.centerNode;
            height = target.entityView.ModelHeight.y;
            LateUpdate();
        }
        #region Unity


        protected override void Awake()
        {
            base.Awake();


        }

        protected override void Start()
        {
            base.Start();

            GameObject gm = this.transform.Find("Collide/DragTool").gameObject;
            _dragTool = gm.GetComponent<RectTransform>();
            GameObject gmImg = this.transform.Find("Collide/DragToolImg").gameObject;
            _dragToolImg = gmImg.GetComponent<Image>();

            _hammer= this.transform.Find("Collide/DragTool/Hammer").gameObject; 
            _axe=this.transform.Find("Collide/DragTool/Axe").gameObject;
            _waterNet= this.transform.Find("Collide/DragTool/WaterNet").gameObject;

            _hammerSkeletonGraphic = _hammer.GetComponent<SkeletonGraphic>();
            _hammerSkeletonGraphic.AnimationState.Complete += animationEnd;
            _axeSkeletonGraphic = _axe.GetComponent<SkeletonGraphic>();
            _axeSkeletonGraphic.AnimationState.Complete += animationEnd;
            _waterNetSkeletonGraphic = _waterNet.GetComponent<SkeletonGraphic>();
            _waterNetSkeletonGraphic.AnimationState.Complete += animationEnd;

            _dragToolRect = gm.GetComponent<RectTransform>();
            _collide = this.transform.Find("Collide").gameObject;
            _collideRect = _collide.GetComponent<RectTransform>();
            _clearToolNum = this.transform.Find("BG/Image/ClearToolNum").GetComponent<Text>();
            _rootRect = gameObject.GetComponent<RectTransform>();
            _rootTrans = GetComponent<RectTransform>();
            _bg = this.transform.Find("BG").gameObject;

            tweenBaseList = this.GetComponentsInChildren<TweenBase>();

            this.OnDragSet(Drag);
            this.OnEndDragSet(EndDrag);
            //canvasTransform = BubbleManager.Instance.canvasTransform;
            //canvasCamera = BubbleManager.Instance.canvasCamera;
        }

        //protected override void Start()
        //{

        //}
        protected override void LateUpdate()
        {
            //base.LateUpdate();
            if (_targettrans)
            {
                var anchoredPosition = BubbleManager.Instance.ScreenPointToLocalPointInRectangle(_targettrans.position);
                _rootTrans.anchoredPosition = anchoredPosition + new Vector3(0, 100, 0);
            }

            //_rectTrans.anchoredPosition += new Vector2(0,100);
            //this.transform.position += new Vector3(0,60,0);

        }

        private void OnEnable()
        {
            _canvasCamera = BubbleManager.Instance.canvasCamera;
            _canvasTransform = BubbleManager.Instance.canvasTransform;
            // this.SetStartPosition();
        }

        #endregion

    }
}
