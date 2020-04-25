using Game.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class M3PropManager
    {
        public bool startUseProp = false;
        public GamePropItem current = null;

        private GameObject frameObj = null;
        private int[] prepareProps = null;
        private List<int> usedPropID;

        public M3PropManager()
        {
            Init();
        }

        private void Init()
        {
            prepareProps = LevelDataModel.Instance.prepareProps;
            usedPropID = new List<int>();
            if (prepareProps != null)
                foreach (var item in prepareProps)
                {
                    Debug.Log(item);
                }
        }

        public void SaveUsedPropID(int id)
        {
            if (usedPropID != null)
                usedPropID.Add(id);
        }

        public List<int> GetUsedPropID()
        {
            return usedPropID;
        }

        public int GetPrepareStep()
        {
            if (prepareProps == null)
                return 0;
            for (int i = 0; i < prepareProps.Length; i++)
            {
                if ((PropType)prepareProps[i] == PropType.Step)
                {
                    return AddStep.step;
                }
            }
            return 0;
        }

        public void UseDrop(int itemID, GamePropItem propItem)
        {
            if (!M3GameManager.Instance.CheckSteady() || startUseProp)
            {
                return;
            }
            if (M3GameManager.Instance.modeManager.IsLevelFinish() || M3GameManager.Instance.modeManager.isGameEnd || M3GameManager.Instance.modeManager.GameModeCtrl.isEnd)
                return;

            current = propItem;
            M3GameManager.Instance.ClearSelectedItem();
            startUseProp = true;
            propItem.SetFrame(true);
            PropType type = (PropType)itemID;
            switch (type)
            {
                case PropType.Refresh:
                    Refresh refresh = new Refresh(delegate () { OnPropUsedOver(); });
                    refresh.OnPropClick();
                    break;
                case PropType.Hammer:
                    Hammer hammerItem = new Hammer(delegate () { OnPropUsedOver(); });
                    hammerItem.OnPropClick();
                    break;
                case PropType.Exchanged:
                    ExchangedItem exchangedItem = new ExchangedItem(delegate () { OnPropUsedOver(); });
                    exchangedItem.OnPropClick();
                    break;
                case PropType.MagicCol:
                    MagicColum magicCol = new MagicColum(delegate () { OnPropUsedOver(); });
                    magicCol.OnPropClick();
                    break;
                case PropType.MagicRow:
                    MagicRow magicRow = new MagicRow(delegate () { OnPropUsedOver(); });
                    magicRow.OnPropClick();
                    break;
                case PropType.Step:
                    AddStep addStep = new AddStep(delegate () { OnPropUsedOver(); });
                    addStep.OnPropClick();
                    break;
                case PropType.Time:
                    AddTime addTimer = new AddTime(delegate () { OnPropUsedOver(); });
                    addTimer.OnPropClick();
                    break;
                case PropType.MagicBroom:
                    MagicBroom magicBroom = new MagicBroom(delegate () { OnPropUsedOver(); });
                    magicBroom.OnPropClick();
                    break;
                case PropType.MagicCat:
                    MagicCat magicCat = new MagicCat(delegate () { OnPropUsedOver(); });
                    magicCat.OnPropClick();
                    break;
                case PropType.QuickTip:
                    break;
                case PropType.AddScore:
                    break;
                case PropType.AddGold:
                    break;
                default:
                    break;
            }
        }

        public void CancelUseProp()
        {
            if (startUseProp)
            {
                M3GameManager.Instance.propItemLock = false;
                M3GameManager.Instance.propItem = null;
                M3Supporter.Instance.ResetPiece();

                if (current != null)
                {
                    this.startUseProp = false;
                    current.SetFrame(false);
                    current = null;
                }
            }
        }

        private void OnPropUsedOver()
        {
            startUseProp = false;
            if (current != null)
            {
                current.OnUsed();
                this.startUseProp = false;
                current.SetFrame(false);
                current = null;
            }
        }

    }
}