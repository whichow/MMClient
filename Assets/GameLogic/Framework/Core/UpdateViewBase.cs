using Framework.Core;
using UnityEngine;

namespace Framework.UI
{
    public abstract class UpdateViewBase : IDisplayObject, IUpdateable, IDispose
    {
        public string mEffectName { get; protected set; }
        protected bool _blShow;
        public UpdateViewBase()
        {
            _blShow = false;
        }

        public virtual void SetDisplayObject(GameObject gameObject)
        {
            _displayObject = gameObject;
            mEffectName = gameObject.name;
            _transform = _displayObject.transform;
            if (_transform is RectTransform)
                _rectTransform = _transform as RectTransform;
        }

        protected virtual void Show()
        {
            if (mDisplayObject == null)
                return;
            if (!_blShow)
            {
                if (!mDisplayObject.activeSelf)
                    mDisplayObject.SetActive(true);
                _blEnable = true;
                GameComponent.Instance.AddUpdateComponent(this);
            }
            _blShow = true;
        }

        protected virtual void Hide()
        {
            if (mDisplayObject == null)
                return;
            if (_blShow)
            {
                if (mDisplayObject.activeSelf)
                    mDisplayObject.SetActive(false);
                _blEnable = false;
                GameComponent.Instance.RemoveUpdateComponent(this);
            }
            _blShow = false;
        }

        public virtual void Update()
        {

        }

        #region getter
        protected bool _blEnable;
        public bool blEnable
        {
            get { return _blEnable; }
        }

        private GameObject _displayObject;
        public GameObject mDisplayObject
        {
            get { return _displayObject; }
        }

        private RectTransform _rectTransform;
        public RectTransform mRectTransform
        {
            get { return _rectTransform; }
        }

        private Transform _transform;
        public Transform mTransform
        {
            get { return _transform; }
        }
        #endregion

        public virtual void Dispose()
        {
            if (_displayObject != null)
            {
                GameObject.Destroy(_displayObject);
                _displayObject = null;
                _rectTransform = null;
                _transform = null;
            }
            _blShow = false;
            _blEnable = false;
            GameComponent.Instance.RemoveUpdateComponent(this);
        }
    }
}