using Game.DataModel;
using System;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 冰块
    /// </summary>
    public class IceElement : Element
    {
        public override void Init(int id, M3Grid grid)
        {
            base.Init(id, grid);
            eName = M3ElementType.IceElement;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            if (view != null)
                view.PlayAnimation(data.config.IdleAnim, true);
        }

        public override void Crush()
        {
            base.Crush();
            DoLogic();
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
            base.ProcessSpecialEliminate(special, args, ignoreEffect);
            Crush();
        }

        private void DoLogic()
        {
            if (data.config.ClearTransforID != 0)
            {
                var tarElement = XTable.ElementXTable.GetByID(data.config.ClearTransforID);
                if (view != null)
                    view.PlayTweenAnim(data.config.ClearAnim, tarElement.IdleAnim, () => { });
                TransformElement(tarElement.ID);
            }
            else if (!gridObtainer.isCrushing)
            {
                gridObtainer.isCrushing = true;

                Action action = delegate
                {
                    gridObtainer.gridInfo.RemoveFloorElement();
                    DestroyElement();
                    gridObtainer.isCrushing = false;
                };
                if (view != null)
                    view.PlayAnimation(data.config.ClearAnim, null);
                M3GameManager.Instance.modeManager.GameModeCtrl.AddTarget(data.config.Collect, (view != null) ? view.eleTransform.position : Vector3.zero, this);
                FrameScheduler.instance.Add(M3Config.ElementDisapperFrame, action);
            }
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEleminateIce();
            AddScore();
        }

        private void AddScore()
        {
            int score = M3GameManager.Instance.modeManager.ProcessComboScore(data.config.Point, ItemSpecial.fNormal, M3GameManager.Instance.comboManager.GetCombo(), false);
            M3GameManager.Instance.modeManager.AddScore(score, 1, M3Supporter.Instance.GetItemPositionByGrid(gridObtainer.gridInfo.posX, gridObtainer.gridInfo.posY));
        }

        public override void DestroyElement()
        {
            if (view != null)
                GameObject.Destroy(view.eleGameObject);
        }

        public override Element Clone()
        {
            Element ele = new IceElement();
            return Clone(ele);
        }

    }
}