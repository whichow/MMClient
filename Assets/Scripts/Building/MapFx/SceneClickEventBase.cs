using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    public interface SceneClickEventBase
    {
           void OnTap();
          void OnFocus(bool focus);
          void OnLongPress();
    }
}
