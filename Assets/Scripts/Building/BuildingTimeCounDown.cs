using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    public class BuildingTimeCounDown : MonoBehaviour
    {
        public delegate void UpdateDelegate(int progress);

        #region Field
        public enum TimeMethod
        {
            None = 0,
            CountDown = -1,
            Start = 1,
        }
        /// <summary>
        /// 
        /// </summary>
        public Image maskImage;
        /// <summary>
        /// 
        /// </summary>
        public Text timeText;

        private UpdateDelegate _updateDelegate;

        #endregion

        #region Property
        private float _porgress;
        public float porgress
        {
            get { return _porgress; }
            set
            {
                _porgress = value;
                this.maskImage.fillAmount = _porgress/ porgressMax;
            }

        }
        public float porgressMax
        { get; set; }
        public void timeTextSet()
        {
             timeText.text = K.Extension.TimeExtension.ToTimeString((int)Math.Ceiling(_porgress)); 
        }

        #endregion

        public void SetUpdateDelegate(UpdateDelegate updateDelegate)
        {
            _updateDelegate = updateDelegate;
        }

        #region Unity  

        private void OnEnable()
        {
            _updateDelegate = null;
        }

        private void Update()
        {

            if (porgress > 0)
            {
                porgress = porgress - Time.deltaTime;
                timeTextSet();
                if (_updateDelegate != null)
                {
                    _updateDelegate((int)Math.Ceiling(this.porgress));
                }

            }
        }

        #endregion
    }
}
