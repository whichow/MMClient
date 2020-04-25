using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

namespace Game
{
    public class KTweenUtils
    {
        public static void MoveY(Transform transform, float y, float duration, Action action)
        {
            transform.DOMoveY(y, duration).OnComplete(delegate () { if (action != null) action(); });
        }

        public static void MoveTo(Transform transform, Vector3 end, float duration)
        {
            transform.DOMove(end, duration);
        }
        public static void MoveTo(Transform transform, Vector3 end, float duration, Action action)
        {
            transform.DOMove(end, duration).OnComplete(delegate () { if (action != null) action(); });
        }

        public static void MoveTo(Transform transform, Vector3 start, Vector3 end, float duration)
        {
            transform.position = start;
            transform.DOMove(end, duration);
        }
        // Hejunjie Add
        public static void LocalMoveTo(Transform transform, Vector3 end, float duration, Action OnComplete)
        {
            transform.DOLocalMove(end, duration).OnComplete(delegate () { if (OnComplete != null) OnComplete(); });
        }
        public static void LocalMoveTo(Transform transform, Vector2 end, float duration, Action OnComplete)
        {
            transform.DOLocalMove(end, duration).OnComplete(delegate () { if (OnComplete != null) OnComplete(); });
        }
        public static void LocalMoveAdd(Transform transform, Vector3 add, float duration, Ease ease, Action OnComplete)
        {
            transform.DOLocalMove(transform.localPosition + add, duration).SetEase(ease).OnComplete(delegate () { if (OnComplete != null) OnComplete(); });

        }
        public static void LocalMoveTo(Transform transform, Vector3 end, float duration)
        {
            transform.DOLocalMove(end, duration);
        }

        public static void LocalMoveTo(Transform transform, Vector3 start, Vector3 end, float duration)
        {
            transform.localPosition = start;
            transform.DOLocalMove(end, duration);
        }

        public static void ScaleTo(Transform transform, Vector3 target, float duration, Action OnComplete = null)
        {
            transform.DOScale(target, duration).OnComplete(delegate () { if (OnComplete != null) OnComplete(); });
        }
        public static void RectTransformLocalMoveTo(RectTransform rect, Vector2 to, float duration, Action OnComplete = null)
        {
            rect.DOAnchorPos(to, duration).OnComplete(delegate () { if (OnComplete != null) OnComplete(); });
        }


        public static void RotateTo(Transform transform, Vector3 vec, float duration, Action OnComplete = null)
        {
            transform.DORotate(vec, duration).OnComplete(delegate () { if (OnComplete != null) OnComplete(); });
        }

        public static void DoImageFade(Image img, float from, float to, float duration, Action OnComplete = null)
        {
            img.DOFade(from, 0);
            img.DOFade(to, duration).OnComplete(delegate () { if (OnComplete != null) OnComplete(); });
        }
        public static Tweener DoFade(GameObject gameObj, float value, float duration, Action OnComplete = null)
        {
            var renderer = gameObj.GetComponent<Renderer>();
            if (!renderer)
            {
                return null;
            }
            var material = renderer.material;
            if (!material)
            {
                return null;
            }
            return material.DOFade(value, duration).OnComplete(delegate () { if (OnComplete != null) OnComplete(); });
        }

        public static void DOPunchScale(Transform transform, Vector3 punch, float duration, int vibrato, int elasticity, TweenCallback onComplete = null)
        {
            transform.DOPunchScale(punch, duration, vibrato, elasticity).OnComplete(onComplete);
        }
        public static void DoImageFillAmount(Image image, float targetValue, float duration, Action action = null)
        {
            image.DOFillAmount(targetValue, duration).OnComplete(delegate () { if (action != null) action(); });

        }
        public static void DoTextColor(Text text, float r, float g, float b, float a, float duration, Action action = null)
        {
            text.DOColor(new Color(r, g, b, a), duration);
        }
        public static Tweener DoText(Text text, string endValue, float duration, bool richTextEnabled = true, string scrambleChars = null)
        {
            return text.DOText(endValue, duration, richTextEnabled);
        }
        public static void DoImageColor(Image image, Color color, float duration)
        {
            image.DOColor(color, duration);
        }


        public static void DOLocalPath(Transform transform, Vector3[] points, float duration, DG.Tweening.PathType pathType, Action onComplete = null)
        {
            transform.DOLocalPath(points, duration, pathType).OnComplete(delegate () { if (onComplete != null) onComplete(); });
        }
        public static void DOPath(Transform transform, Vector3[] points, float duration, DG.Tweening.PathType pathType, Action onComplete = null)
        {
            transform.DOPath(points, duration, pathType).OnComplete(delegate () { if (onComplete != null) onComplete(); });
        }
        public static Tweener DOLocalPath(Transform transform, Vector3[] points, float duration, DG.Tweening.PathType pathType, int loopCount = 1, TweenCallback onComplete = null)
        {
            Tweener tween = null;
            try
            {
                tween = transform.DOLocalPath(points, duration, pathType).OnComplete(onComplete).SetLoops(loopCount);
            }
            catch (System.Exception ex)
            {
                Debuger.LogError("Tween Error! \n" + ex);
                tween.Kill();
            }
            return tween;
        }
        public static void DoRectTransformPosY(RectTransform rectTrans, float valueY, float duration, Action onComplete = null)
        {
            rectTrans.DOAnchorPos(new Vector2(rectTrans.anchoredPosition.x, valueY), duration).OnComplete(delegate () { if (onComplete != null) onComplete(); });
        }
        public static void DoRectTransformPos(RectTransform rectTrans, Vector2 vec, float duration, Action onComplete = null)
        {
            rectTrans.DOAnchorPos(vec, duration).OnComplete(delegate () { if (onComplete != null) onComplete(); });
        }

        public static void DoFloat(float from, float to, float duration, TweenCallback<float> updateAction)
        {
            //DOTween.To(() => mFloat, x => mFloat = x, to, duration);
            DOVirtual.Float(from, to, duration, updateAction).SetEase(Ease.Linear);
        }
        public static void DoTransformKill(Transform trans)
        {
            trans.DOKill();
        }
    }
}