using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Build
{
    public class BubbleConfirm : Bubble
    {
        public class Data
        {
            public System.Action onConfirm;
            public System.Action onCancel;
            public System.Action onRotate;
            public System.Action onSell;
            public System.Action onRecorery;
        }

        public Button cancelBtn;
        public Button confirmBtn;
        public Button rotateBtn;
        public Button recoveryBtn;
        public Button sellBtn;
        public static GameObject[] ConfirmBtLst;

        public Button BG;

        private Action onCancel;
        private Action onConfirm;
        private Action onRotate;
        private Action onRecovery;
        private Action onSell;
        private Action onRecorery;

        private TweenBase[] tweenBaseList;
        public void Cancel()
        {
            this.onCancel();
        }

        public void Confirm()
        {
            this.onConfirm();
        }
        public void Rotate()
        {
            this.onRotate();
        }

        public void Sell()
        {
            this.onSell();
        }

        public void Recorery()
        {
            this.onRecorery();
        }
        /// <summary>
        /// 设置 城建 确认，取消按钮事件 
        /// </summary>
        /// <param name="onConfirm"></param>
        /// <param name="onCancel"></param>
        public void Set(Action onConfirm, Action onCancel,Action onRotate=null,Action onSell =null)
        {
            this.onConfirm = onConfirm;
            this.onCancel = onCancel;
            this.onRotate = onRotate;
            this.onSell = onSell;

            //BG.onClick.AddListener(()=>onCancel());
            cancelBtn.gameObject.SetActive(onCancel != null);
            confirmBtn.gameObject.SetActive(onConfirm != null);
            rotateBtn.gameObject.SetActive(onRotate !=null);
            recoveryBtn.gameObject.SetActive(false);
            sellBtn.gameObject.SetActive(onSell !=null);
        }
        /// <summary>
        /// 设置 城建 确认，取消按钮事件 
        /// </summary>
        /// <param name="onConfirm"></param>
        /// <param name="onCancel"></param>
        public void Set( Data data)
        {
            this.onConfirm = data.onConfirm;
            this.onCancel = data.onCancel;
            this.onRotate = data.onRotate;
            this.onSell = data.onSell;
            this.onRecorery = data.onRecorery;
            //BG.onClick.AddListener(()=>onCancel());
            cancelBtn.gameObject.SetActive(onCancel != null);
            confirmBtn.gameObject.SetActive(onConfirm != null);
            rotateBtn.gameObject.SetActive(onRotate != null);
            recoveryBtn.gameObject.SetActive(onRecorery !=null);
            sellBtn.gameObject.SetActive(onSell != null);
        }
        protected override void Init()
        {
            base.Init();
            cancelBtn.onClick.AddListener(Cancel);
            confirmBtn.onClick.AddListener(Confirm);
            rotateBtn.onClick.AddListener(Rotate);
            sellBtn.onClick.AddListener(Sell);
            recoveryBtn.onClick.AddListener(Recorery);

            ConfirmBtLst = new GameObject[5]
            { cancelBtn.gameObject,
                confirmBtn.gameObject,
                rotateBtn.gameObject,
                recoveryBtn.gameObject,
                sellBtn.gameObject
            };
            tweenBaseList = this.GetComponentsInChildren<TweenBase>();

        }
        private void playTween()
        {
            if (tweenBaseList == null)
                return;
            foreach (var item in tweenBaseList)
            {
                item.PlayBack();
            }
        }
        #region Unity

        private void OnEnable()
        {
            playTween();
            //if (!this.gameObject.activeSelf)
            //{
            //    playTween();
            //}
        }
        protected override void LateUpdate()
        {
        }

        private void OnDisable()
        {
            onCancel = null;
            onConfirm = null;
            onRotate = null;
            onRecovery = null;
            onSell = null;
        }

        #endregion
    }
}

