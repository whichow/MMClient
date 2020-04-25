using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    public class InputManager : MonoBehaviour
    {
        #region Static

        public static InputManager Instance;

        #endregion

        #region Method

        #endregion

        #region Unity

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            Input.multiTouchEnabled = true;
        }

        private void OnDisable()
        {
            Input.multiTouchEnabled = false;
        }

        #endregion
    }
}
