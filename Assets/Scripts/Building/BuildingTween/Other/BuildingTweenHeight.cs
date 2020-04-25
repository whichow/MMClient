using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class BuildingTweenHeight
    {
        Vector3 _scale;
        Vector3 target = new Vector3(1, 1.08f, 1);
        Transform targetObj;
        bool isCanStart;

        public void init(GameObject gm)
        {
            isCanStart = gm;
            if (gm)
            {
                _scale = gm.transform.localScale;
                targetObj = gm.transform;
                target.x = gm.transform.localScale.x;
            }
        }
        Tweener tweener;
        public void StartScale()
        {
            if (!isCanStart)
                return;
            DOTween.Init(true, false, LogBehaviour.Default);
            DOTween.defaultLoopType = LoopType.Yoyo;

            //Tween
            tweener = DOTween.To(() => _scale,
               x =>
               {
                   _scale = x;
                   if (targetObj)
                       targetObj.localScale = _scale;
                   //SetColor(_scale);
               },
               target, 0.18f);
            tweener.SetLoops(2);
            tweener.SetId(this.GetHashCode());
        }

        public void ResetBrightness()
        {

        }
    }
}