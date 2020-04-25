using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    public static class Extensions
    {
        public static Ray MouseRay(this Camera camera)
        {
            return camera.ScreenPointToRay(Input.mousePosition);
        }

        public static Vector3 MouseViewport(this Camera camera)
        {
            return camera.ScreenToViewportPoint(Input.mousePosition);
        }

        public static bool IsInside(this Bounds r1, Bounds r2)
        {
            return r1.min.x >= r2.min.x && r1.min.z >= r2.min.z && r1.max.x <= r2.max.x && r1.max.z <= r2.max.z;
        }

        public static bool IsInside(this Vector3 r1, Bounds r2)
        {
            return r1.x >= r2.min.x && r1.z >= r2.min.z && r1.x <= r2.max.x && r1.z <= r2.max.z;
        }
    }
}
