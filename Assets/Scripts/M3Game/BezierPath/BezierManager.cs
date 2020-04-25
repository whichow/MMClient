/** 
*FileName:     BezierManager.cs 
*Author:       Hejunjie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-12-07 
*Description:    
*History: 
*/
using Game;
using System;
using UnityEngine;

namespace Game.Match3
{
    public static class BezierManager
    {

        public static void PutLocalPath(Transform trans, Vector3[] points, float precision, float time, DG.Tweening.PathType pathType = DG.Tweening.PathType.Linear, Action callBack = null)
        {
            LTBezierPath bezier = new LTBezierPath(points, precision);
            KTweenUtils.DOLocalPath(trans, bezier.beziers[0].pointList.ToArray(), time, pathType, callBack);
        }

        public static void PutPath(Transform trans, Vector3[] points, float precision, float time, DG.Tweening.PathType pathType = DG.Tweening.PathType.Linear, Action callBack = null)
        {
            LTBezierPath bezier = new LTBezierPath(points, precision);
            KTweenUtils.DOPath(trans, bezier.beziers[0].pointList.ToArray(), time, pathType, callBack);
        }
    }
}