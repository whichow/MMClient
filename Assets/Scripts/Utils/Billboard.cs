using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [AddComponentMenu("Miscellaneous/Billboard"), ExecuteInEditMode]
    public class Billboard : MonoBehaviour
    {
        [Header("Options")]
        public bool Flip;
        public bool FlipRandom;
        public bool FlipWithScale;
        [SerializeField, HideInInspector]
        private SpriteRenderer[] NoneRenderers;
        private Transform trans;
        [SerializeField, HideInInspector]
        private Sprite UsedSprite;
        [SerializeField]
        private bool useWarmup;

        public static void ApplyBillboardRotation(Transform _objectTransform, Camera _camera, bool _flip = false, bool _flipWithScale = false)
        {
            _objectTransform.rotation = _camera.transform.rotation;
            Quaternion rotation = _camera.transform.rotation;
            Vector3 eulerAngles = rotation.eulerAngles;
            eulerAngles.z = 0f;
            rotation.eulerAngles = eulerAngles;
            _objectTransform.rotation = rotation;
            if (_flip)
            {
                if (!_flipWithScale)
                {
                    _objectTransform.Rotate(0f, 180f, 0f);
                }
                else
                {
                    _objectTransform.localScale = Vector3.Scale(_objectTransform.localScale, new Vector3(-1f, 1f, 1f));
                }
            }
            else if (_objectTransform.localScale.x < 0f)
            {
                _objectTransform.localScale = _objectTransform.localScale;
            }
        }

        public static void ApplyBillboardRotation(Transform _objectTransform, Quaternion _rotation, bool _flip = false, bool _flipWithScale = false)
        {
            _objectTransform.rotation = _rotation;
            Quaternion quaternion = _rotation;
            Vector3 eulerAngles = quaternion.eulerAngles;
            eulerAngles.z = 0f;
            quaternion.eulerAngles = eulerAngles;
            _objectTransform.rotation = quaternion;
            if (_flip)
            {
                if (!_flipWithScale)
                {
                    _objectTransform.Rotate(0f, 180f, 0f);
                }
                else
                {
                    _objectTransform.localScale = Vector3.Scale(_objectTransform.localScale, new Vector3(-1f, 1f, 1f));
                }
            }
            else if (_objectTransform.localScale.x < 0f)
            {
                _objectTransform.localScale = _objectTransform.localScale;
            }
        }

        private bool GetFlipped()
        {
            if (!this.FlipRandom)
            {
                return this.Flip;
            }
            if (this.trans == null)
            {
            }
            this.trans = base.transform;
            return (((Mathf.Pow(this.trans.position.x * 4172.452f, 1.8f) + Mathf.Pow(this.trans.position.y * 2409.124f, 4.2f)) % 20f) >= 10f);
        }

        private void OnEnable()
        {
            this.trans = base.transform;
            this.Rotate(Camera.main);
            SpriteRenderer[] componentsInChildren = null;
            if (this.UseWarmup)
            {
                if (Application.isEditor || (this.NoneRenderers == null))
                {
                    componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>(true);
                    List<SpriteRenderer> list = new List<SpriteRenderer>(componentsInChildren.Length);
                    this.UsedSprite = null;
                    foreach (SpriteRenderer renderer in componentsInChildren)
                    {
                        if (renderer.sprite != null)
                        {
                            if (this.UsedSprite == null)
                            {
                                this.UsedSprite = renderer.sprite;
                            }
                        }
                        else
                        {
                            list.Add(renderer);
                        }
                    }
                    this.NoneRenderers = list.ToArray();
                }
                foreach (SpriteRenderer renderer2 in this.NoneRenderers)
                {
                    if (renderer2 != null)
                    {
                        renderer2.sprite = this.UsedSprite;
                        renderer2.sprite = null;
                    }
                }
            }
        }

        private void OnTransformParentChanged()
        {
            this.Rotate(Camera.main);
        }

        private void Rotate(Camera cam = null)
        {
            if (cam == null)
            {
            }
            cam = Camera.current;
            if (cam != null)
            {
                ApplyBillboardRotation(base.transform, cam, this.GetFlipped(), this.FlipWithScale);
            }
        }

        public bool UseWarmup
        {
            get
            {
                return this.useWarmup;
            }
            set
            {
                this.useWarmup = value;
                this.OnEnable();
            }
        }
    }
}