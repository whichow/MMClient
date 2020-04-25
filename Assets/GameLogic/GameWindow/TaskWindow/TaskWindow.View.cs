using Game.DataModel;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class TaskWindow
    {
        private UIList _taskList;

        public void InitView()
        {
            _taskList = Find<UIList>("List");
            _taskList.SetRenderHandler(RenderHandler);
            _taskList.SetPointerHandler(PointerHandler);
        }

        private void RenderHandler(UIListItem item, int index)
        {
            TaskData taskData = item.dataSource as TaskData;
            if (taskData == null)
                return;
            MissionXDM cfg = XTable.MissionXTable.GetByID(taskData.Id);
            Text fillText;
            Image fillImg;
            fillText = item.GetComp<Text>("Image/Text");
            fillImg = item.GetComp<Image>("Image/Image");
            item.GetComp<Text>("txt_title").text = KLocalization.GetLocalString(cfg.Title);
            item.GetComp<Text>("txt_description").text = KLocalization.GetLocalString(cfg.DescriptionId);
            item.GetGameObject("btn_get").SetActive(taskData.State == 1);
            item.GetGameObject("Image").SetActive(taskData.State != 1);
            item.GetComp<Button>("btn_get").onClick.RemoveAllListeners();
            item.GetComp<Button>("btn_get").onClick.AddListener(() => { OnDrawBtn(taskData.Id); });
            if (taskData.Id == 20001)
            {
                switch (taskData.State)
                {
                    case 0:
                        fillText.text = 0 + "/" + 1;
                        fillImg.fillAmount = 0;
                        break;
                    default:
                        fillText.text = 1 + "/" + 1;
                        fillImg.fillAmount = 1;
                        break;
                }
            }
            else
            {
                if (taskData.Value >= cfg.CompleteNum)
                {
                    fillText.text = cfg.CompleteNum + "/" + cfg.CompleteNum;
                    fillImg.fillAmount = 1;
                }
                else
                {
                    fillText.text = taskData.Value + "/" + cfg.CompleteNum;
                    fillImg.fillAmount = (float)taskData.Value / (float)cfg.CompleteNum;
                }
            }

            UIList taskItemList = item.GetComp<UIList>("List");
            taskItemList.SetRenderHandler(ItemRenderHandler);
            taskItemList.SetPointerHandler(ItemPointerHandler);
            List<ItemInfo> listInfo = new List<ItemInfo>();
            if (cfg.Reward.Count % 2 != 0)
                return;
            for (int i = 0; i < cfg.Reward.Count; i += 2)
            {
                ItemInfo info = new ItemInfo();
                info.ItemCfgId = cfg.Reward[i];
                info.ItemNum = cfg.Reward[i + 1];
                listInfo.Add(info);
            }
            if (listInfo == null || listInfo.Count == 0)
                return;
            taskItemList.DataArray = listInfo;
        }

        private void PointerHandler(UIListItem item, int index)
        {

        }

        private void ItemRenderHandler(UIListItem item, int index)
        {
            ItemInfo vo = item.dataSource as ItemInfo;
            if (vo == null)
                return;
            item.GetComp<Image>("Icon").overrideSprite = KIconManager.Instance.GetItemIcon(vo.ItemCfgId);
            item.GetComp<Text>("Num").text = vo.ItemNum.ToString();
        }

        private void ItemPointerHandler(UIListItem item, int index)
        {
            ItemInfo vo = item.dataSource as ItemInfo;
            if (vo == null)
                return;
            Debuger.Log("ID:" + vo.ItemCfgId);
            //µã»÷ÊÂ¼þ
        }

        private void OnDrawBtn(int id)
        {
            GameApp.Instance.GameServer.ReqTaskReward(id);
        }

        public void RefreshView(int type)
        {
            if (!active)
                return;
            OnTaskNum();
            TaskDataVO taskDataVO = TaskDataModel.Instance.mDictAllTaskData[type];
            taskDataVO.mTaskData.Sort(SortTask);
            _taskList.DataArray = taskDataVO.mTaskData;
        }

        private int SortTask(TaskData v1, TaskData v2)
        {
            if (v1.State == v2.State)
                return v1.Id < v2.Id ? -1 : 1;
            else if (v1.State == 0)
                return v2.State == 1 ? 1 : -1;
            else if (v1.State == 1)
                return -1;
            else if (v1.State == 2)
                return 1;
            else
                return 0;
        }
    }
}
