using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    class BuildingObstacleClearMgr : UnitySingleton<BuildingObstacleClearMgr>
    {
        BuildingObstacleClearView _view;
        BuildingObstacleClearView view
        {
            get
            {
                if (_view == null)
                {
                    KUIWindow.OpenWindow<BuildingObstacleClearView>();
                    _view = KUIWindow.GetWindow<BuildingObstacleClearView>();

                    BuildingMainCamera = GameCamera.Instance.mainCamera;
                    //var canvas = KUIRoot.Instance.canvas;
                    canvasTransform = _view.transform as RectTransform;
                    canvasCamera = KUIRoot.Instance.uiCamera;
                }
                return _view;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="building"></param>
        /// <param name="onClick"></param>
        /// <returns></returns>
        public BuildingObstacleClearView ClearToolShow(Transform target, System.Action onClick = null)
        {
            view.Show(target,onClick);
            return view;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="building"></param>
        public void ClearToolHide()
        {
            view.Hide();
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
        public Vector3 WorldToUI(Vector3 position,RectTransform rectTransform= null)
        {
            var screenPoint = RectTransformUtility.WorldToScreenPoint(BuildingMainCamera, position);

            return ScreenToUI(screenPoint, rectTransform );
        }
        public Vector3 ScreenToUI(Vector3 position, RectTransform rectTransform = null)
        {
            Vector2 localPoint;
            if(rectTransform == null )
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, position, canvasCamera, out localPoint);
            else
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, canvasCamera, out localPoint);
            return localPoint;
        }
    }
}
