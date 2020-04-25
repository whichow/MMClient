using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;

namespace Game.Build
{
    interface IDrag : IBeginDragHandler, IEndDragHandler, IDragHandler
    {
    }
}
