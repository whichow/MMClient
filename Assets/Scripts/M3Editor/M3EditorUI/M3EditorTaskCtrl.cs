/** 
*FileName:     M3EditorTaskCtrl.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-12 
*Description:    
*History: 
*/
#if UNITY_EDITOR
using Game.Match3;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M3Editor
{
    public class M3EditorTaskCtrl : MonoBehaviour
    {

        public static M3EditorTaskCtrl Instance;
        private List<M3EditorTaskItem> taskItemList;
        private List<GameObject> taskItemObj;
        private GameObject taskItemPrefab;
        private Transform taktItemGridTrans;
        private Button addBtn;
        private Button delBtn;

        private GameModeEnum mode;
        private GameTargetEnum target;
        public Button timeModeBtn;
        public Button stepModeBtn;
        public InputField modeValue;


        private Button collectBtn;
        private Button scoreBtn;
        private GameObject collectObj;
        private GameObject scoreObj;

        private InputField scoreInput;

        public GameModeEnum Mode
        {
            get
            {
                return mode;
            }
            set
            {
                switch (value)
                {
                    case GameModeEnum.None:
                        break;
                    case GameModeEnum.TimeMode:
                        stepModeBtn.gameObject.SetActive(false);
                        timeModeBtn.gameObject.SetActive(true);
                        break;
                    case GameModeEnum.StepMode:
                        stepModeBtn.gameObject.SetActive(true);
                        timeModeBtn.gameObject.SetActive(false);
                        break;
                    default:
                        break;
                }
                mode = value;
            }
        }

        public GameTargetEnum Target
        {
            get
            {
                return target;
            }

            set
            {
                switch (value)
                {
                    case GameTargetEnum.None:
                        break;
                    case GameTargetEnum.Collection:
                        scoreBtn.gameObject.SetActive(false);
                        collectBtn.gameObject.SetActive(true);
                        scoreObj.SetActive(false);
                        collectObj.SetActive(true);
                        break;
                    case GameTargetEnum.Score:
                        scoreBtn.gameObject.SetActive(true);
                        collectBtn.gameObject.SetActive(false);
                        scoreObj.SetActive(true);
                        collectObj.SetActive(false);
                        break;
                    default:
                        break;
                }
                target = value;
            }
        }

        private void Awake()
        {
            Instance = this;
            M3GameEvent.AddEvent(M3EditorEnum.ChangLevel, ReSetTask);
        }
        private void Start()
        {
        }
        public void Init()
        {
            taskItemList = new List<M3EditorTaskItem>();
            taskItemObj = new List<GameObject>();
            collectObj = transform.Find("CollectUI").gameObject;
            scoreObj = transform.Find("ScoreUI").gameObject;
            scoreInput = transform.Find("ScoreUI/InputField").GetComponent<InputField>();
            taskItemPrefab = transform.Find("CollectUI/scroll/viewpoint/taskList/taskItem").gameObject;
            taskItemPrefab.SetActive(false);
            taktItemGridTrans = transform.Find("CollectUI/scroll/viewpoint/taskList");
            addBtn = transform.Find("CollectUI/addBtn").GetComponent<Button>();

            timeModeBtn = transform.Find("ModeSelect/timeModeBtn").GetComponent<Button>();
            stepModeBtn = transform.Find("ModeSelect/stepModeBtn").GetComponent<Button>();
            modeValue = transform.Find("ModeSelect/modeValue").GetComponent<InputField>();

            collectBtn = transform.Find("CollectBtn").GetComponent<Button>();
            scoreBtn = transform.Find("ScoreBtn").GetComponent<Button>();

            collectBtn.onClick.AddListener(OnCollectBtnClick);
            scoreBtn.onClick.AddListener(OnScoreBtnClick);

            timeModeBtn.onClick.AddListener(OnTimeModeBtnClick);
            stepModeBtn.onClick.AddListener(OnStepModeBtnClick);

            addBtn.onClick.AddListener(OnAddBtnClick);
            Mode = GameModeEnum.StepMode;
            Target = GameTargetEnum.Collection;

        }

        public int GetTargetScore()
        {
            int value = -1;
            if (int.TryParse(scoreInput.text, out value))
            {
                if (value >= 0)
                    return value;
            }
            Debug.LogError("请输入正确的数字");
            return 0;
        }

        private void OnScoreBtnClick()
        {
            Target = GameTargetEnum.Collection;

        }

        private void OnCollectBtnClick()
        {
            Target = GameTargetEnum.Score;

        }

        private void OnStepModeBtnClick()
        {
            Mode = GameModeEnum.TimeMode;
        }

        private void OnTimeModeBtnClick()
        {
            Mode = GameModeEnum.StepMode;
        }

        private void OnAddBtnClick()
        {
            if (M3EditorController.instance.EditorMode == mEditorMode.mDelete || M3EditorController.instance.EditorMode == mEditorMode.mEditor||M3EditorController.instance.EditorMode== mEditorMode.mBrush)
                AddItem();
        }


        private void AddItem(int eleid = -1, int count = -1)
        {
            GameObject go = Game.TransformUtils.Instantiate(taskItemPrefab, taktItemGridTrans);
            go.SetActive(true);
            M3EditorTaskItem item = go.GetComponent<M3EditorTaskItem>();
            item.clearHandler = RemoveTaskItem;
            item.Init(eleid, count);
            taskItemList.Add(item);
            taskItemObj.Add(go);
        }
        public List<M3EditorTaskItem> GetTakeItem()
        {
            return taskItemList;
        }

        public GameModeEnum GetGameMode()
        {
            return Mode;
        }

        public GameTargetEnum GetGameTarget()
        {
            return Target;
        }

        public int GetModeValue()
        {
            if (string.IsNullOrEmpty(modeValue.text))
                return 0;
            return int.Parse(modeValue.text);
        }
        public void RemoveTaskItem(M3EditorTaskItem item)
        {
            if (taskItemList.Contains(item))
            {
                taskItemList.Remove(item);
                taskItemObj.Remove(item.gameObject);
                item.Clear();
                Destroy(item.gameObject);
            }
        }
        private void ReSetTask(object[] args)
        {
            taskItemList.Clear();
            for (int i = 0; i < taskItemObj.Count; i++)
            {
                Destroy(taskItemObj[i]);
            }
            Mode = GameModeEnum.StepMode;
            Target = GameTargetEnum.Collection;

            stepModeBtn.gameObject.SetActive(true);
            timeModeBtn.gameObject.SetActive(false);
            modeValue.text = "";
            M3LevelData data = M3LevelConfigMgr.Instance.GetLevelConfigData(args[0].ToString());
            if (data == null)
                return;

            Target = (GameTargetEnum)data.gameTarget;
            if (Target == GameTargetEnum.Collection)
            {
                foreach (var item in data.LevelTaskElementDic)
                {
                    AddItem(item.Key, item.Value);
                }
            }
            else {
                scoreInput.text = data.score.ToString();
            }


            Mode = (GameModeEnum)data.gameMode;
            if (Mode == GameModeEnum.StepMode)
            {
                modeValue.text = data.steps.ToString();
            }
            else if (Mode == GameModeEnum.TimeMode)
            {
                modeValue.text = data.time.ToString();

            }


        }


    }
}
#endif
