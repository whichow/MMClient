// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BubbleManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    public class BubbleManager : SingletonUnity<BubbleManager>
    {

      
        #region Field

        private Dictionary<string, Bubble> _registerBubbles = new Dictionary<string, Bubble>();

        #endregion

        #region Property

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
        #endregion

        #region Method
        /// <summary>
        /// 注册指定类型的UI
        /// </summary>
        /// <param name="bubbleName"></param>
        /// <param name="bubble"></param>
        public void RegisterBubble(string bubbleName, Bubble bubble)
        {
            _registerBubbles.Add(bubbleName, bubble);
        }
        /// <summary>
        /// 显示指定类型UI
        /// </summary>
        /// <param name="bubbleName"></param>
        /// <param name="nodeParent"></param>
        /// <returns></returns>
        public Bubble ShowBubble(string bubbleName, Transform nodeParent)
        {
            Bubble tmpBubble;
            if (_registerBubbles.TryGetValue(bubbleName, out tmpBubble))
            {
                //Debug.Log(Camera.main.WorldToScreenPoint(nodeParent.position));
                tmpBubble.FollowTarget(nodeParent);
                //tmpBubble.transform.SetParent(nodeParent, false);
                tmpBubble.Show();
            }
            return tmpBubble;
        }
        /// <summary>
        /// 隐藏指定类型UI
        /// </summary>
        /// <param name="bubbleName"></param>
        public void HideBubble(string bubbleName)
        {
            Bubble tmpBubble;
            if (_registerBubbles.TryGetValue(bubbleName, out tmpBubble))
            {
                tmpBubble.Hide();
            }
        }
        /// <summary>
        /// 设置城建取消 ，确认等功能弹窗
        /// </summary>
        /// <param name="nodeParent">父节点</param>
        /// <param name="onDragConfirm">确认事件</param>
        /// <param name="onDragCancel">取消事件</param>
        /// <returns></returns>
        public Bubble ShowConfirm(Transform nodeParent, System.Action onDragConfirm, System.Action onDragCancel)
        {
            Debug.Log("------------------------------");
            //var bubble = ShowBubble("Confirm", nodeParent) as Game.Building.BubbleConfirm;
            //bubble.Set(onDragConfirm, onDragCancel);
            return ShowConfirm(nodeParent, onDragConfirm, onDragCancel, null);
        }
        /// <summary>
        /// 设置城建取消 ，确认等功能弹窗
        /// </summary>
        /// <param name="nodeParent">父节点</param>
        /// <param name="onDragConfirm">确认事件</param>
        /// <param name="onDragCancel">取消事件</param>
        /// <returns></returns>
        public Bubble ShowConfirm(Transform nodeParent, System.Action onDragConfirm, System.Action onDragCancel, System.Action onRotate = null, System.Action onSell = null)
        {
            Debug.Log("------------------------------");
            var bubble = ShowBubble("Confirm", nodeParent) as BubbleConfirm;
            bubble.Set(onDragConfirm, onDragCancel, onRotate, onSell);
            return bubble;
        }
        /// <summary>
        /// 设置城建取消 ，确认等功能弹窗
        /// </summary>
        /// <param name="nodeParent">父节点</param>
        /// <param name="onDragConfirm">确认事件</param>
        /// <param name="onDragCancel">取消事件</param>
        /// <returns></returns>
        public Bubble ShowConfirm(Transform nodeParent, BubbleConfirm.Data data)
        {
            Debug.Log("------------------------------");
            var bubble = ShowBubble("Confirm", nodeParent) as BubbleConfirm;
            bubble.Set(data);
            return bubble;
        }
        /// <summary>
        /// 隐藏确认按钮
        /// </summary>
        public void HideConfirm()
        {
            HideBubble("Confirm"); ;
        }
        /// <summary>
        ///显示进度条
        /// </summary>
        /// <param name="nodeParent"></param>
        /// <returns></returns>
        public BubbleTimeProgress ShowTimeProgress(Transform nodeParent)
        {
            var bubble = ShowBubble("TimeProgress", nodeParent) as BubbleTimeProgress;
            return bubble;
        }
        /// <summary>
        /// 隐藏进度条
        /// </summary>
        public void HideTimeProgress()
        {
            HideBubble("TimeProgress");
        }

        /// <summary>
        /// 显示收割
        /// </summary>
        /// <param name="nodeParent"></param>
        /// <returns></returns>
        public BubbleHarvest ShowHarvest(Transform nodeParent)
        {
            var bubble = ShowBubble("Harvest", nodeParent) as BubbleHarvest;
            return bubble;
        }
        /// <summary>
        /// 隐藏收割
        /// </summary>
        public void HideHarvest()
        {
            HideBubble("Harvest");
        }

        /// <summary>
        /// 显示长按
        /// </summary>
        /// <param name="nodeParent"></param>
        /// <returns></returns>
        public BubbleProgress ShowLongPress(Transform nodeParent)
        {
            var bubble = ShowBubble("LongPress", nodeParent) as BubbleProgress;
            return bubble;
        }
        /// <summary>
        /// 隐藏长按箭头UI
        /// </summary>
        public void HideLongPress()
        {
            HideBubble("LongPress");
        }
        /// <summary>
        /// 显示障碍物清理
        /// </summary>
        /// <param name="nodeParent"></param>
        /// <returns></returns>
        public BubbleObstacleClear ShowObstacleClear(Building  building)
        {

            Bubble tmpBubble;
            BubbleObstacleClear bubbleObstacleClear =null;
            if (_registerBubbles.TryGetValue("ObstacleClear", out tmpBubble))
            {
                bubbleObstacleClear = tmpBubble as BubbleObstacleClear;
                bubbleObstacleClear.FollowTarget(building);
                bubbleObstacleClear.Show();
            }

            return bubbleObstacleClear;
        }
        /// <summary>
        /// 隐藏障碍物清理
        /// </summary>
        /// <param name="nodeParent"></param>
        public void HideObstacleClear()
        {
            HideBubble("ObstacleClear");
        }

        /// <summary>
        /// 屏幕坐标转换到UI坐标
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector3 ScreenPointToLocalPointInRectangle(Vector3 position)
        {
            var screenPoint = GameCamera.Instance.mainCamera.WorldToScreenPoint(position);
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, screenPoint, canvasCamera, out pos);
            return pos;
        }
        /// <summary>
        /// 世界坐标转换UI坐标
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector3 WorldPointToLocalPointInRectangle(Vector3 position)
        {
            var screenPoint = RectTransformUtility.WorldToScreenPoint(canvasCamera, position);
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, screenPoint, canvasCamera, out localPoint);
            return localPoint;
        }
        /// <summary>
        /// 鼠标坐标转换的屏幕坐标
        /// </summary>
        /// <returns></returns>
        public Vector3 MousePointToLocalPointInRectangle()
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, Input.mousePosition, canvasCamera, out pos);
            return pos;
        }

        #endregion

        #region Unity 

        private void OnEnable()
        {
            var canvas = this.GetComponentInChildren<Canvas>();
            if (canvas)
            {
                canvasTransform = canvas.transform as RectTransform;
                canvasCamera = canvas.worldCamera;
             }
            GameObject bubblePrefab;
            if (KAssetManager.Instance.TryGetBuildingPrefab("Bubble/Bubbles", out bubblePrefab))
            {
                var bubbleObj = Instantiate(bubblePrefab);
                bubbleObj.transform.SetParent(canvasTransform, false);
            }
        }

        #endregion

    }
}
