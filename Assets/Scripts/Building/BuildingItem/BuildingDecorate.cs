using Game.UI;
using Msg.ClientMessage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 装饰物
    /// </summary>
    class BuildingDecorate : Building, IFunCommon
    {


        public bool IsSell
        {
            get
            {
                return false;
            }
        }
        public bool IsRotate
        {
            get
            {
                return _buildingData.rotatable == 1;
            }
        }

        public bool IsRecovery
        {
            get
            {
                return entityData.recovery > 0;
            }
        }
        private MainWindow _mainWindow;
        public MainWindow _MainWindow
        {
            get
            {
                if (_mainWindow == null)
                    _mainWindow = KUIWindow.GetWindow<MainWindow>();
                return _mainWindow;
            }
        }

        /// <summary>
        /// 标记是否在旋转状态
        /// </summary>
        private bool isRotating;
        public void OnSell() { }
        //public void OnRotate()
        //{
        //    BubbleManager.Instance.HideConfirm();
        //    CollisionHighlight.Instance.HideCollisions();
        //    if (IsRotate)
        //    {

        //    }
        //}
        public void OnRotate()
        {
            if (IsRotate)
            {
                isRotating = true;
                entityView.RotateModel();
                MapObject mapObject = GetComponent<MapObject>();
                CollisionHighlight.Instance.ShowCollisions(mapObject, null);

                //Transform rotate = Map.Instance.currMapObject.transform;
                // rotate.localRotation = rotate.localRotation + new Vector3(); new Quaternion();
                // Map.Instance.currMapObject.transform.Rotate(rotate.localRotation);
            }
        }

        public void OnRecovery()
        {
            BubbleManager.Instance.HideConfirm();
            CollisionHighlight.Instance.HideCollisions();
            if (IsRecovery)
            {

                KUser.BuildingRecycle(buildingId, recoveryCallBack);
                Debug.Log("回收");
            }
        }

        private void recoveryCallBack(int codeId,string content,object data)
        {

            if (codeId == 0)
            {
                IconFlyMgr.Instance.IconFlyStart(this.entityView.gmPoint.position, _MainWindow.buildingBagNode.position, KIconManager.Instance.GetBuildingIcon( entityData.iconName));
                GameObject.Destroy(this);
                Debug.Log("回收成功");
            }
            else
            {

            }
        }
        public void OnRotateConfirm()
        {
            Debug.Log("旋转确认");

            if (isRotating)
            {
                //Rotate();
                isRotating = false;
            }
        }
        private void Rotate()
        {
            Debug.Log("发送旋转 协议buildingId：" + buildingId+ "---rotationValue:" + entityView.rotationValue);

            KUser.BuildingChangeDirection(new C2SChgBuildingDir()
            {
                BuildingId = buildingId,
                X = 0,
                Y = entityView.rotationValue,
            }, RotateCallBack
               );
        }

        private void RotateCallBack(int codeId, string content, object data)
        {
            if (codeId == 0)
            {
                Debug.Log("旋转成功");
                //entityView.RotateModel();
                //MapObject mapObject = GetComponent<MapObject>();
                //CollisionHighlight.Instance.ShowCollisions(mapObject, null);
            }
        }
        //public override string idleAnimation
        //{
        //    get
        //    {
        //        return base.idleAnimation;
        //    }
        //}

        //public override string touchAnimation

        //{
        //    get
        //    {
        //        return base.idleAnimation;
        //    }
        //}

        protected override void OnFocus(bool focus)
        {
            base.OnFocus(focus);
            //GameCamera.Instance.Show(entityView.centerNode.position);
        }

        protected override void OnTap()
        {
            base.OnTap();
        }
        private void Start()
        {
            //viewTransform.localScale = Vector3.one * 0.348f;

            entityView.ShowModel();
        }
    }
}
