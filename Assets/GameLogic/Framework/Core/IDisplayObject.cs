using UnityEngine;

namespace Framework.Core
{
    public interface IDisplayObject
    {
        GameObject mDisplayObject { get; }
        RectTransform mRectTransform { get; }
        Transform mTransform { get; }
        void SetDisplayObject(GameObject gameObject);
    }
}