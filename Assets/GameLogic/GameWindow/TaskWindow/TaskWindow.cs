using Game.DataModel;
using Game.Match3;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class TaskWindow : KUIWindow
    {
        private Button _closeBtn;
        private Toggle[] _toggles;
        private Dictionary<int, GameObject> _dictPoint;
        private Dictionary<int, Text> _dictText;
        private int _curTaskType;

        public TaskWindow() :
            base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "TaskWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        public override void Awake()
        {
            _closeBtn = Find<Button>("Close");
            _closeBtn.onClick.AddListener(OnCloseBtnClick);
            _dictPoint = new Dictionary<int, GameObject>();
            _dictText = new Dictionary<int, Text>();
            _toggles = new Toggle[2];
            for (int i = 0; i < 2; i++)
            {
                _toggles[i] = Find<Toggle>("ToggleGroup/Tog" + (i + 1));
                GameObject obj = Find("ToggleGroup/Tog" + (i + 1) + "/Point");
                Text text = Find<Text>("ToggleGroup/Tog" + (i + 1) + "/Point/Text");
                _dictPoint.Add(i + 1, obj);
                _dictText.Add(i + 1, text);
            }
            foreach (Toggle tog in _toggles)
                tog.onValueChanged.AddListener((bool blSelect) => { if (blSelect) OnTaskTypeChange(tog); });
            _curTaskType = TaskType.Everyday;
            GameApp.Instance.GameServer.ReqTaskData(TaskType.AllTaskData);
            InitView();
        }

        public override void AddEvents()
        {
            base.AddEvents();
            TaskDataModel.Instance.AddEvent(TaskEvent.TaskData, OnTaskData);
            TaskDataModel.Instance.AddEvent(TaskEvent.TaskReward, OnTaskReward);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            TaskDataModel.Instance.RemoveEvent(TaskEvent.TaskData, OnTaskData);
            TaskDataModel.Instance.RemoveEvent(TaskEvent.TaskReward, OnTaskReward);
        }

        private void OnTaskTypeChange(Toggle tog)
        {
            switch (tog.name)
            {
                case "Tog1":
                    _curTaskType = TaskType.Everyday;
                    break;
                case "Tog2":
                    _curTaskType = TaskType.Achievement;
                    break;
            }
            if (!TaskDataModel.Instance.mDictAllTaskData.ContainsKey(_curTaskType))
                GameApp.Instance.GameServer.ReqTaskData(_curTaskType);
            else
                RefreshView(_curTaskType);
        }

        public override void OnEnable()
        {
            OnTaskTypeChange(_toggles[_curTaskType - 1]);
        }

        private void OnTaskData()
        {
            RefreshView(_curTaskType);
        }

        private void OnTaskNum()
        {
            Dictionary<int, TaskDataVO> dict = TaskDataModel.Instance.mDictAllTaskData;
            foreach (var item in dict)
            {
                int everydayNum = 0;
                switch (item.Key)
                {
                    case TaskType.Everyday:
                        for (int i = 0; i < item.Value.mTaskData.Count; i++)
                        {
                            if (item.Value.mTaskData[i].State == 1)
                                everydayNum++;
                        }
                        _dictPoint[item.Key].SetActive(everydayNum > 0);
                        if (everydayNum > 0)
                            _dictText[item.Key].text = everydayNum.ToString();
                        break;
                    case TaskType.Achievement:
                        for (int i = 0; i < item.Value.mTaskData.Count; i++)
                        {
                            if (item.Value.mTaskData[i].State == 1)
                                everydayNum++;
                        }
                        _dictPoint[item.Key].SetActive(everydayNum > 0);
                        if (everydayNum > 0)
                            _dictText[item.Key].text = everydayNum.ToString();
                        break;
                }
            }
        }

        public override void OnDestroy()
        {

        }

        private void OnTaskReward()
        {
            MissionXDM cfg = XTable.MissionXTable.GetByID(TaskDataModel.Instance.mRewardId);
            if (cfg == null || cfg.Reward.Count % 2 != 0)
                return;
            List<ItemInfo> listInfo = new List<ItemInfo>();
            for (int i = 0; i < cfg.Reward.Count; i += 2)
            {
                ItemInfo info = new ItemInfo();
                info.ItemCfgId = cfg.Reward[i];
                info.ItemNum = cfg.Reward[i + 1];
                listInfo.Add(info);
            }
            if (listInfo.Count > 0)
                KUIWindow.OpenWindow<GetItemTipsWindow>(listInfo);
            RefreshView(_curTaskType);
        }
    }
}
