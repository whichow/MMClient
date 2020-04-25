
namespace Game.Match3
{
    /// <summary>
    /// 三消连击管理器
    /// </summary>
    public class M3ComboManager
    {
        public int comboCount = -1;
        public bool comboLock = true;

        public M3ComboManager()
        {
        }

        /// <summary>
        /// 添加连击数
        /// </summary>
        /// <param name="count"></param>
        public void AddCombo(int count)
        {
            if (M3GameManager.Instance.modeManager.isGameEnd && !M3GameManager.Instance.isAutoAI)
            {
                ResetCombo();
                return;
            }
            comboCount += count;
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEliminateAudio(comboCount);
            if (comboCount > 1)
            {
                //TODO: 播放音效
            }
        }

        public void ResetCombo()
        {
            comboLock = true;
            this.comboCount = 0;
        }

        public int GetCombo()
        {
            return this.comboCount;
        }

        public void CommitCombo()
        {
            //Debug.Log("Combo  : " + comboCount);
            if (comboCount > 3 && !M3GameManager.Instance.modeManager.isGameEnd)
            {
                M3GameEvent.DispatchEvent(M3FightEnum.Comb, comboCount);
            }
            ResetCombo();
        }

    }
}