using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Build
{
    [AddComponentMenu("UGUICustom/UICollider")]
    public class UGUICollider : Graphic
    {
        //[HideInInspector]
        //new protected Material m_Material;
        //[HideInInspector]
        Color _color = new Color(0, 0, 0, 0);
        public override Color color
        {
            get
            {
                return _color;
            }
            set {

            }
        }

    }
}
