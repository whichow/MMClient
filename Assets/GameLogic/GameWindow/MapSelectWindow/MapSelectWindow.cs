/** 
*FileName:     LevelWindow.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-08-10 
*Description:    
*History: 
*/
using Game.DataModel;
using UnityEngine;

namespace Game.UI
{
    public partial class MapSelectWindow : KUIWindow
    {
        public bool itemLock;

        public MapSelectWindow() : base(UILayer.kBackground, UIMode.kNone)
        {
            uiPath = "MapSelectWindow";
        }

        public override void Awake()
        {
            base.Awake();
            InitModel();
            InitView();
        }

        public override void AddEvents()
        {
            base.AddEvents();
            PlayerDataModel.Instance.AddEvent(PlayerEvent.PlayerDataRefresh, OnPlayerRefresh);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            PlayerDataModel.Instance.RemoveEvent(PlayerEvent.PlayerDataRefresh, OnPlayerRefresh);
        }

        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (_sgTimer != null)
            {
                _sgTimer.Stop();
                _sgTimer = null;
            }
            foreach (var item in itemDic)
            {
                GameObject.Destroy(item.Value.gameObject);
            }
            itemDic.Clear();
            foreach (var item in mapDic)
            {
                GameObject.Destroy(item.Value.gameObject);
            }
            mapDic.Clear();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (_sgTimer != null)
            {
                _sgTimer.Stop();
                _sgTimer = null;
            }
        }

        public void UnLockChapter()
        {
            Debuger.Log("解锁");
            for (int i = 0; i < tmpCloud.Count; i++)
            {
                GameObject.Destroy(tmpCloud[i]);
            }
            tmpCloud.Clear();
            RefreshModel();
            //RefreshView();
            LoadMapPrefab(LevelDataModel.Instance.GetChapterByLevelID(LevelDataModel.Instance.CurrUnlockMaxLevelId).ID);
            SetCloud();
            DoScrollMove(_scrollRect.horizontalNormalizedPosition, 1, 5f);
        }

        private void OnBackClick()
        {
            if (KFramework.FsmManager.GetFsm<GameFsm>().currState is InLevelState)
            {
                LevelDataModel.Instance.ExitLevelScene(null);
            }
            else CloseWindow(this);
        }

        private void OnEnergyClick()
        {
            //KUIWindow.OpenWindow<ShopWindow>(ShopWindow.ShopType.SpecialPack);
            OpenWindow<LackHintWindow>();
        }

        private void OnAddDiamondClick()
        {
            OpenWindow<ShopWindow>(ShopIDConst.Diamond);
        }

        private void OnAddGlodClick()
        {
            OpenWindow<ShopWindow>(ShopIDConst.Special);
        }

        private void OnUnlockClick()
        {
            var chapter = LevelDataModel.Instance.GetChapterByLevelID(LevelDataModel.Instance.CurrUnlockMaxLevelId);
            if (!XTable.ChapterUnlockXTable.ContainsID(chapter.UnlockChapter))
            {
                return;
            }
            Debuger.Log(LevelDataModel.Instance.CurrMaxLevelId);
            Debuger.Log(XTable.LevelXTable.GetByID(LevelDataModel.Instance.CurrUnlockMaxLevelId).NextLevelID);
            //if (KLevelManager.Instance.currMaxLevelId != KLevelManager.Instance.GetLevelById(KLevelManager.Instance.currUnlockMaxLevelId).NextLevelID)
            if (!LevelDataModel.Instance.GetChapterByLevelID(LevelDataModel.Instance.CurrUnlockMaxLevelId).LastLevel.IsFinish)
            {
                OpenWindow<MessageBox>(new MessageBox.Data()
                {
                    content = "请先通关前置关卡",
                    onConfirm = () => openBox(),
                });
                return;
            }

            //没有时间解锁了
            //Action<int, string, object> action = (message, arg0, arg1) =>
            //{
            //    if (message == 0)
            //    {
            //        Debug.Log(chapter.UnlockChapter);
            //        KLevelManager.Instance.CurUnlockChapterID = chapter.UnlockChapter;
            //        KLevelManager.Instance.CurUnlockChapterTime = KLevelManager.Instance.GetChapterByChapterID(chapter.UnlockChapter).unlockTime;
            //        OpenWindow<M3ClearPassWindow>(new M3ClearPassWindow.Data
            //        {
            //            charpterId = chapter.UnlockChapter,
            //            needStartNum = KLevelManager.Instance.GetChapterByChapterID(chapter.UnlockChapter).unlockStarNum,
            //            nowStartNum = KLevelManager.Instance.GetCurrentAllStars(),
            //            unLockCallBack = UnLockChapter,
            //        });
            //    }
            //};
            //if (KLevelManager.Instance.CurUnlockChapterID == 0)
            //{
            //    KUser.ChapterUnlock(0, chapter.UnlockChapter, null, action);
            //}
            //else
            {
                OpenWindow<M3ClearPassWindow>(new M3ClearPassWindow.Data
                {
                    charpterId = chapter.UnlockChapter,
                    needStartNum = XTable.ChapterUnlockXTable.GetByID(chapter.UnlockChapter).UnlockStarNum,
                    nowStartNum = XTable.LevelXTable.GetCurrentAllStars(),
                    unLockCallBack = UnLockChapter,
                });
            }
        }

    }
}
 