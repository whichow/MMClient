using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AttractedItem : MonoBehaviour
    {
        public Transform target;
        public Vector3 targetPosition;
        public AnimationCurve curve;

        public float remainingLifetime = 2;
        public float startLifetime = 2f;

        private IEnumerator AttractionRoutine()
        {
            while (true)
            {
                var progress = 1.1f - (remainingLifetime / startLifetime);
                transform.position = Vector3.Lerp(transform.position, this.position, curve.Evaluate(progress));

                yield return new WaitForEndOfFrame();
                remainingLifetime -= Time.deltaTime;

                if (remainingLifetime < 0f)
                {
                    break;
                }
            }

            Destroy(gameObject);
        }

        private Vector3 position
        {
            get
            {
                return ((this.target == null) ? this.targetPosition : this.target.position);
            }
        }

        private void Start()
        {
            targetPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.825f, 0.935f, 5.4f));

            StartCoroutine(AttractionRoutine());
        }

        public void OnGUI()
        {
            //if (GUI.Button(new Rect(0, 200, 200, 200), "Test"))
            //{
            //    StartCoroutine(AttractionRoutine());
            //}
        }
    }
}