using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    public class Background : MonoBehaviour
    {
        #region Field

        public int sourceWidth = 5760;
        public int sourceHeight = 4320;

        public int currentWidth = 2048;
        public int currentHeight = 1536;

        #endregion

        #region Unity

        private void Start()
        {
            transform.localScale = new Vector3((float)sourceWidth / (float)currentWidth, (float)sourceHeight / (float)currentHeight, 1f);
        }

        #endregion
    }
}
