#if UNITY_EDITOR

using System;
/** 
*FileName:     M3EditorStageSettingCtrl.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-10 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Game;
using Game.Match3;

namespace M3Editor
{
    public class M3EditorStageSettingCtrl : MonoBehaviour
    {
        private Button saveBtn;
        private Button deleteModeBtn;
        private Button editorModeBtn;
        private Button deleBtn;
        private Button catBtn;
        private Text delText;
        public InputField lvText;
        private Button startBtn;

        public InputField randomSeedInput;
        private void Awake()
        {
            lvText = transform.Find("lvName/lvText").GetComponent<InputField>();
            saveBtn = transform.Find("cBtns/saveBtn").GetComponent<Button>();
            deleBtn = transform.Find("cBtns/deleBtn").GetComponent<Button>();

            deleteModeBtn = transform.Find("Btns/delBtn").GetComponent<Button>();
            editorModeBtn = transform.Find("Btns/editorBtn").GetComponent<Button>();
            delText = transform.Find("Btns/delBtn/Text").GetComponent<Text>();
            startBtn = transform.Find("Btns/playBtn").GetComponent<Button>();
            catBtn = transform.Find("Btns/catBtn").GetComponent<Button>();
            saveBtn.image.color = Color.gray;
            M3GameEvent.AddEvent(M3EditorEnum.ChangeMode, OnChangeMode);
            M3GameEvent.AddEvent(M3EditorEnum.ChangLevel, OnChangeLevel);
            Init();
            delText.text = "删除模式";
        }

        private void OnChangeLevel(object[] args)
        {
            lvText.text = args[0].ToString();
        }

        private void OnChangeMode(object[] args)
        {
            var mode = (mEditorMode)args[0];
            if (mode == mEditorMode.mEditor || mode == mEditorMode.mDelete)
            {
                saveBtn.image.color = Color.yellow;
            }
            if (mode == mEditorMode.mNone || mode == mEditorMode.mPlay)
            {
                saveBtn.image.color = Color.gray;
            }
            if (M3EditorController.instance.EditorMode == mEditorMode.mNone)
                return;
            if (M3EditorController.instance.EditorMode == mEditorMode.mDelete)
            {
                delText.text = "返回";
            }
            else
            {
                delText.text = "删除模式";
            }
        }

        private void Init()
        {
            if (saveBtn != null)
                saveBtn.onClick.AddListener(OnSaveBtnClick);
            if (deleteModeBtn != null)
                deleteModeBtn.onClick.AddListener(OnDeleteModeClick);
            if (editorModeBtn != null)
                editorModeBtn.onClick.AddListener(OnEditorModeBtnClick);
            if (deleBtn != null)
                deleBtn.onClick.AddListener(OnDeleteMapBtnClick);
            if (startBtn != null)
                startBtn.onClick.AddListener(StartPlay);

            catBtn.onClick.AddListener(CatBtnClick);
        }

        private void CatBtnClick()
        {
            M3EditorController.instance.catSelect.gameObject.SetActive(true);
        }

        private void StartPlay()
        {
            M3GameEvent.DispatchEvent(M3EditorEnum.LoadScene);
            if (M3Config.levelId == "")
                return;


            int id = PlayerPrefs.GetInt("M3EditorCatID", 10401);
            int s = PlayerPrefs.GetInt("M3Editorstar", 1);
            int lv = PlayerPrefs.GetInt("M3Editorlv", 1);
            int match = PlayerPrefs.GetInt("M3Editormatch", 1);
            int skillLV = PlayerPrefs.GetInt("M3EditorSkillLV", 1);
            KCat cat = null;
            if (id > 0)
            {
                cat = new KCat
                {
                    shopId = id,
                    star = s,
                    grade = lv,
                    initMatchAbility = match,
                    skillGrade = skillLV
                };
            }
            M3Config.editorCat = cat;
            M3GameEvent.DispatchEvent(M3EditorEnum.OnSaveBtnClick,1);
            PlayerPrefs.SetString("CurrentEditorLv", M3EditorController.instance.gridCtrl.LvID.ToString());
            M3EditorController.instance = null;
            M3GameEvent.RemoveAllEvent();
            M3Config.isEditor = true;

            LevelDataModel.Instance.prepareProps = new int[0];
            LevelDataModel.Instance.CurrLevelID = 41001;

            int seed = -1;
            if (int.TryParse(randomSeedInput.text, out seed))
            {
                if (seed != -1)
                    M3RandomMgr.Instance.InitRandomSeed(seed);
            }

            KLoading.LoadAssets();
            foreach (var item in KItemManager.Instance.GetProps())
            {
                item.curCount = 99;
            }
        }

        private void OnDeleteMapBtnClick()
        {
            if (M3EditorController.instance.EditorMode != mEditorMode.mEditor && M3EditorController.instance.EditorMode != mEditorMode.mDelete)
            {
                return;
            }
            M3GameEvent.DispatchEvent(M3EditorEnum.DeleteMap);
        }

        private void OnEditorModeBtnClick()
        {
        }

        private void OnDeleteModeClick()
        {
            if (M3EditorController.instance.EditorMode == mEditorMode.mNone)
                return;
            if (M3EditorController.instance.EditorMode == mEditorMode.mDelete)
            {
                M3EditorController.instance.EditorMode = mEditorMode.mEditor;
            }
            else
            {
                M3EditorController.instance.EditorMode = mEditorMode.mDelete;
            }
        }

        private void OnSaveBtnClick()
        {
            //if (M3EditorController.instance.EditorMode != mEditorMode.mEditor && M3EditorController.instance.EditorMode != mEditorMode.mDelete)
            //{
            //    return;
            //}
            M3GameEvent.DispatchEvent(M3EditorEnum.OnSaveBtnClick);
        }
    }
}
#endif