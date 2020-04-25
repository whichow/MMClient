using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [AddComponentMenu("BuildingTeween/TweenRotate")]
    public class TweenRotate : TweenBase
    {
        public Vector3 from;
        public Vector3 to;


        Quaternion _originalQuaternion;
        private Vector3 _rotation;
        private float _value;
        [HideInInspector]
        public float value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                this.transform.localRotation = /*_originalQuaternion +*/
                    Quaternion.Lerp(
                        Quaternion.Euler(_rotation + from),
                    Quaternion.Euler(to),
                    _value);
                //this.transform.localRotation=Quaternion.Euler(_rotation + _value);
            }
        }
        protected override void TweenAwake()
        {
            base.TweenAwake();
            _rotation = this.transform.localRotation.eulerAngles;
        }

        protected override void TweenUpData(float time, bool isFinish)
        {
            value = time;// from * (1 - time) + to * time;
        }
    }
}