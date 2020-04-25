using System.Collections.Generic;

using UnityEngine;

namespace Game.Build
{
    class BuildingStateMgr : SingletonUnity<BuildingStateMgr>
    {

        GameObject _bubble;
        GameObject _parentNode;
        GameObject _timeProgress;
        GameObject _bubbleObj;
        /// <summary>
        /// 气泡缓存池
        /// </summary>
        BuildingElementPool<Bubble> _bubbleElementPool;
        /// <summary>
        /// 进度条缓存池
        /// </summary>
        BuildingElementPool<Bubble> _ProgressElementPool;


        Dictionary<Building, Bubble> _bubbleElemetDict;

        Dictionary<Building, Bubble> _ProgressElemetDict;

        public BuildingStateMgr()
        //: base(UILayer.kNormal, UIMode.kNone)
        {
            //uiPath = "CropUp";


            _bubbleElementPool = new BuildingElementPool<Bubble>();
            _bubbleElementPool.NewElementFunSet(
                () =>
                {
                    GameObject newGm = UnityEngine.Object.Instantiate(_bubble);
                    newGm.transform.SetParent(_parentNode.transform, false);
                    newGm.SetActive(true);

                    return newGm.GetComponent<Bubble>();
                }
                );
            _ProgressElementPool = new BuildingElementPool<Bubble>();
            _ProgressElementPool.NewElementFunSet(
                () =>
                {
                    GameObject newGm = UnityEngine.Object.Instantiate(_timeProgress);
                    newGm.transform.SetParent(_parentNode.transform, false);
                    newGm.SetActive(true);
                    return newGm.GetComponent<Bubble>();

                }

                );

            _bubbleElemetDict = new Dictionary<Building, Bubble>();
            _ProgressElemetDict = new Dictionary<Building, Bubble>();

            //LoadAseet();
        }
        public void BubbleAllShow()
        {
            this.gameObject.SetActive(true);
        }
        public void BubbleAllHide()
        {
            this.gameObject.SetActive(false);
        }
        public BuildingGoldBubble BubbleShow(Building building, System.Action onClick = null)
        {
            BuildingGoldBubble buildingStateView = Show<BuildingGoldBubble>(building);

            buildingStateView.FollowTarget(building.entityView.gmPoint);
            buildingStateView.refurbish(building, onClick);
            return buildingStateView as BuildingGoldBubble;
        }
        private void Hide<T>(Building building) where T : Bubble
        {
            Bubble bubble;
            if (typeof(T) == typeof(BuildingGoldBubble))
            {
                if (_bubbleElemetDict.TryGetValue(building, out bubble))
                {
                    bubble.gameObject.SetActive(false);
                    _bubbleElementPool.DelElement(bubble);
                    _bubbleElemetDict.Remove(building);
                }
            }
            else if (typeof(T) == typeof(TimeProgress))
            {

                if (_ProgressElemetDict.TryGetValue(building, out bubble))
                {
                    bubble.gameObject.SetActive(false);
                    _ProgressElementPool.DelElement(bubble);
                    _ProgressElemetDict.Remove(building);
                }
            }


        }
        //显示气泡
        private T Show<T>(Building building, System.Action onClick = null) where T : Bubble
        {
            Bubble buildingStateView = null;
            if (typeof(T) == typeof(BuildingGoldBubble))
            {
                if (!_bubbleElemetDict.TryGetValue(building, out buildingStateView))
                {
                    buildingStateView = _bubbleElementPool.GetElementCanUsed();
                    _bubbleElemetDict.Add(building, buildingStateView);
                }
            }
            else if (typeof(T) == typeof(TimeProgress))
            {
                if (!_ProgressElemetDict.TryGetValue(building, out buildingStateView))
                {

                    buildingStateView = _ProgressElementPool.GetElementCanUsed();
                    _ProgressElemetDict.Add(building, buildingStateView);

                }
            }
            buildingStateView.gameObject.SetActive(true);
            return buildingStateView as T;

        }
        /// <summary>
        /// 隐藏气泡
        /// </summary>
        /// <param name="building"></param>
        public void BubbleHide(Building building)
        {
            Hide<BuildingGoldBubble>(building);
        }

        public TimeProgress ProgressShow(Building building, System.Action onClick = null)
        {
            //TimeProgress = Show(building) as TimeProgress;
            TimeProgress timeProgress = Show<TimeProgress>(building);
            timeProgress.FollowTarget(building.entityView.gmPoint);
            return timeProgress;
        }
        public void ProgressHide(Building building)
        {
            Hide<TimeProgress>(building);
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
        public Camera BuildingMainCamera
        {
            get;
            private set;
        }
        public Vector3 WorldToUI(Vector3 position)
        {
            var screenPoint = RectTransformUtility.WorldToScreenPoint(BuildingMainCamera, position);
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, screenPoint, canvasCamera, out localPoint);
            return localPoint;
        }
        private void OnEnable()
        {
            var canvas = this.GetComponentInChildren<Canvas>();
            canvasTransform = canvas.transform as RectTransform;
            canvasCamera = canvas.worldCamera;
            GameObject bubblePrefab;
            if (KAssetManager.Instance.TryGetUIPrefab("CropUp", out bubblePrefab))
            {
                _bubbleObj = Instantiate(bubblePrefab) as GameObject;
                _bubbleObj.transform.SetParent(canvasTransform, false);

                _bubble = _bubbleObj.transform.Find("Bubble").gameObject;
                _bubble.SetActive(false);
                _parentNode = _bubbleObj;

                _timeProgress = _bubbleObj.transform.Find("TimeProgress").gameObject;
                _timeProgress.SetActive(false);
                //_view = bubbleObj.GetComponent<BuildingStateView>();
            }

            //canvasTransform = _view.transform as RectTransform;
            //canvasCamera = KUIRoot.Instance.uiCamera;
        }


    }
}

