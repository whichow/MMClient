/** 
 *FileName:     Conveyor.cs 
 *Author:       Hejunjie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-11-13 
 *Description:    
 *History: 
*/
using Spine.Unity;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 传送带
    /// </summary>
    public class Conveyor : MonoBehaviour
    {
        private SkeletonAnimation skeletonAnimation;
        private bool staright;

        public void Create(int x, int y, bool isStaright)
        {
            staright = isStaright;
            skeletonAnimation = transform.GetComponent<SkeletonAnimation>();
            if (skeletonAnimation != null)
            {
                skeletonAnimation.AnimationName = null;
                skeletonAnimation.AnimationName = "idle" + (isStaright ? 2 : 1);
            }
            transform.SetParent(M3GridManager.Instance.gridCells[x, y].gridView.transform, false);
            transform.localPosition = new Vector3(0, 0, 0.7f);
            transform.localScale = Vector3.one;
            transform.localEulerAngles = Vector3.zero;
            Game.TransformUtils.SetLayer(gameObject, LayerMask.NameToLayer("Default"));
        }

        public void PlayAnimation()
        {
            if (skeletonAnimation != null)
            {
                skeletonAnimation.AnimationName = null;
                skeletonAnimation.AnimationName = "move" + (staright ? 2 : 1);
                skeletonAnimation.loop = false;
            }
        }

    }
}