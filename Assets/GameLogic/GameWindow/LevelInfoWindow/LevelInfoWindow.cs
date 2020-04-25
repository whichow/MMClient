using UnityEngine;

namespace Game.UI
{
    public partial class LevelInfoWindow : KUIWindow
    {
        public CatDataVO currentSelect;

        #region Constructor

        public LevelInfoWindow() : base(UILayer.kNormal, UIMode.kSequence)
        {
            uiPath = "LevelInfoWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        #endregion

        #region Action

        public void OnStartBtnClick()
        {
            if (isStart) return;

            if (PlayerDataModel.Instance.mPlayerData.mSpirit < levelData.NeedPower)
            {
                LackHintBox.ShowLackHintBox(5);
                return;
            }

            isStart = true;

            PlayEnergyFlyAnimation(delegate ()
            {
                if (currentSelect != null)
                {
                    LevelDataModel.Instance.CurrentCatID = currentSelect.mCatInfo.Id;
                    PlayerPrefs.SetInt("LastSelectCatID" + PlayerDataModel.Instance.mPlayerData.mPlayerID, currentSelect.mCatInfo.Id);
                }
                else
                {
                    LevelDataModel.Instance.CurrentCatID = -1;
                }
                LevelDataModel.Instance.CurrLevelID = levelData.ID;
                LevelDataModel.Instance.CurrChapterID = levelData.ChapterID;

                //var selectPropArr = selectPropList.FindAll(a => a.isSelect).ToArray();
                //int[] arr = new int[selectPropArr.Length];
                //for (int i = 0; i < selectPropArr.Length; i++)
                //{
                //    arr[i] = selectPropArr[i].propData.itemID;
                //}
                //KLevelManager.Instance.prepareProps = arr;

                GameApp.Instance.GameServer.StageBeginRequest(levelData.ID, currentSelect.mCatInfo.Id, null);
            });
        }

        private void OnCloseBtnCLick()
        {
            CloseWindow(this);
        }

        #endregion

        #region Method

        public override void Awake()
        {
            base.Awake();
            InitModel();
            InitView();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            RefreshModel();
            RefreshView();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            isStart = false;
        }

        #endregion
    }
}
