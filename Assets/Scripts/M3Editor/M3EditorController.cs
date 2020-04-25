/** 
 *FileName:     M3EditorController.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-07-06 
 *Description:    M3编辑控制器
 *History: 
*/
#if UNITY_EDITOR
using Game.DataModel;
using Game.Match3;
using System.Collections;
using UnityEngine;

namespace M3Editor
{
    /// <summary>
    /// 编辑器模式
    /// </summary>
    public enum mEditorMode
    {
        mNone,
        mEditor,
        mPlay,
        mDelete,
        mConveyor,
        mConveyorRemove,

        ShowFish,
        SelectFish,

        mPortal,
        mPortalRemove,
        mHiddenStart,
        mHiddenRemove,
        mRandomHiddenStart,
        mRandomHiddenRemove,

        mBrush,
    }

    public class M3EditorController : MonoBehaviour
    {

        #region Field
        public static M3EditorController instance;
        private mEditorMode editorMode = mEditorMode.mNone;
        [HideInInspector]
        public GameObject top;
        [HideInInspector]
        public CoroutineHelper coroutineHelper;
        [HideInInspector]

        public M3EditorGridCtrl gridCtrl;

        public M3EditorGridSettingCtrl gridsettingCtrl;

        public M3EditorItemGridCtrl itemGridCtrl;

        public M3EditorLevelMenuCtrl levelMenuCtrl;

        public M3EditorStageSettingCtrl stageSettingCtrl;

        public M3EditorTaskCtrl taskCtrl;

        public M3ToolBarCtrl toolBarCtrl;

        public M3GameSettingWindow settingWindow;

        public M3EditorFishCtrl fishCtrl;

        public EditorMain editorMain;

        public M3EditorCatSelect catSelect;

        public M3EditorPopWindow popWindow;
        public mEditorMode EditorMode
        {
            get
            {
                return editorMode;
            }

            set
            {
                editorMode = value;
                M3GameEvent.DispatchEvent(M3EditorEnum.ChangeMode, value);
            }
        }
        #endregion


        #region unity
        private void Awake()
        {
        }
        IEnumerator Start()
        {
            //var asyn = KAssetManager.Instance.LoadAll();
            //while (!asyn.done)
            //{
            //    yield return null;
            //}

            yield return null;
            coroutineHelper = this.GetComponent<CoroutineHelper>();
            instance = this;
            top = transform.Find("Top").gameObject;
            editorMain = new EditorMain();
            //ElementConfig.Init();
            XTable.ElementXTable.Load();
            CtrlInit();

            M3GameEvent.DispatchEvent(M3EditorEnum.ConfigCompelte);
            yield return new WaitForSeconds(0.3f);
            string id = PlayerPrefs.GetString("CurrentEditorLv");

            M3GameEvent.DispatchEvent(M3EditorEnum.StartGenMap, id, M3LevelConfigMgr.Instance.GetLevelConfigData(id));
            M3GameEvent.DispatchEvent(M3EditorEnum.ChangLevel, id);
            M3EditorController.instance.EditorMode = mEditorMode.mEditor;
        }


        private void CtrlInit()
        {
            taskCtrl.Init();
        }
        #endregion
        #region Method


        public void ResetGrid()
        {

        }


        public void OpenToolBar(M3EditorCell cell)
        {
            toolBarCtrl.gameObject.SetActive(true);
            toolBarCtrl.SetData(cell);
        }

        public void OpenSettingWindow()
        {
            settingWindow.gameObject.SetActive(true);
            settingWindow.OnShow();
        }

        public void ShowRandomPopup(string[] titles)
        {
            popWindow.OpenRandomHidden(titles);
        }

        #endregion

    }


}
#endif
