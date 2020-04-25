using System;
using UnityEngine;

namespace Game.Match3
{
    public class LTBezierPath
    {
        public Vector3[] pts;

        public float length;

        public bool orientToPath;

        public bool orientToPath2d;

        public LTBezier[] beziers;

        private float[] lengthRatio;

        private int currentBezier = 0;

        private int previousBezier = 0;

        public LTBezierPath()
        {
        }

        public LTBezierPath(Vector3[] pts_, float precision)
        {
            this.setPoints(pts_, precision);
        }

        public void setPoints(Vector3[] pts_, float precision)
        {
            if (pts_.Length < 4)
            {
                Debug.LogError("LeanTween - When passing values for a vector path, you must pass four or more values!");
            }
            if (pts_.Length % 4 != 0)
            {
                Debug.LogError("LeanTween - When passing values for a vector path, they must be in sets of four: controlPoint1, controlPoint2, endPoint2, controlPoint2, controlPoint2...");
            }
            this.pts = pts_;
            int num = 0;
            this.beziers = new LTBezier[this.pts.Length / 4];
            this.lengthRatio = new float[this.beziers.Length];
            this.length = 0f;
            for (int i = 0; i < this.pts.Length; i += 4)
            {
                this.beziers[num] = new LTBezier(this.pts[i], this.pts[i + 2], this.pts[i + 1], this.pts[i + 3], precision);
                this.length += this.beziers[num].length;
                num++;
            }
            for (int i = 0; i < this.beziers.Length; i++)
            {
                this.lengthRatio[i] = this.beziers[i].length / this.length;
            }

        }

        public Vector3 point(float ratio)
        {
            float num = 0f;
            for (int i = 0; i < this.lengthRatio.Length; i++)
            {
                num += this.lengthRatio[i];
                if (num >= ratio)
                {
                    return this.beziers[i].point((ratio - (num - this.lengthRatio[i])) / this.lengthRatio[i]);
                }
            }
            return this.beziers[this.lengthRatio.Length - 1].point(1f);
        }

        public void place2d(Transform transform, float ratio)
        {
            transform.position = this.point(ratio);
            Debug.Log(this.point(ratio));
            ratio += 0.001f;
            if (ratio <= 1f)
            {
                Vector3 vector = this.point(ratio) - transform.position;
                float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
                transform.eulerAngles = new Vector3(0f, 0f, z);
            }
        }

        public void placeLocal2d(Transform transform, float ratio)
        {
            transform.localPosition = this.point(ratio);
            ratio += 0.001f;
            if (ratio <= 1f)
            {
                Vector3 vector = transform.parent.TransformPoint(this.point(ratio)) - transform.localPosition;
                float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
                transform.eulerAngles = new Vector3(0f, 0f, z);
            }
        }

        public void place(Transform transform, float ratio)
        {
            this.place(transform, ratio, Vector3.up);
        }

        public void place(Transform transform, float ratio, Vector3 worldUp)
        {
            transform.position = this.point(ratio);
            ratio += 0.001f;
            if (ratio <= 1f)
            {
                transform.LookAt(this.point(ratio), worldUp);
            }
        }

        public void placeLocal(Transform transform, float ratio)
        {
            this.placeLocal(transform, ratio, Vector3.up);
        }

        public void placeLocal(Transform transform, float ratio, Vector3 worldUp)
        {
            transform.localPosition = this.point(ratio);
            ratio += 0.001f;
            if (ratio <= 1f)
            {
                transform.LookAt(transform.parent.TransformPoint(this.point(ratio)), worldUp);
            }
        }

        public void gizmoDraw(float t = -1f)
        {
            Vector3 to = this.point(0f);
            for (int i = 1; i <= 120; i++)
            {
                float ratio = (float)i / 120f;
                Vector3 vector = this.point(ratio);
                Gizmos.color = ((this.previousBezier != this.currentBezier) ? Color.grey : Color.magenta);
                Gizmos.DrawLine(vector, to);
                to = vector;
                this.previousBezier = this.currentBezier;
            }
        }
    }
}