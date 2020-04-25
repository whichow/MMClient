using Game;
using Game.DataModel;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDataVO
{
    public List<TaskData> mTaskData { get; private set; }
    public void OnTaskData(S2CTaskDataResponse value)
    {
        if (mTaskData == null)
            mTaskData = new List<TaskData>();
        mTaskData.Clear();
        S2CTaskDataResponse req = value as S2CTaskDataResponse;
        mTaskData.AddRange(req.TaskList);
    }

    public void deleteTask(int taskId)
    {
        if (XTable.MissionXTable.GetByID(taskId).Next > 0)
            mTaskData.RemoveAll(s => (s.Id) == taskId);
    }

    public TaskData GetTaskData(int taskId)
    {
        for (int i = 0; i < mTaskData.Count; i++)
        {
            if (mTaskData[i].Id == taskId)
                return mTaskData[i];
        }
        return null;
    }

    public void TaskDataRefresh(TaskData taskData)
    {
        TaskData data = GetTaskData(taskData.Id);
        if (data == null)
        {
            mTaskData.Add(taskData);
        }
        else
        {
            for (int i = 0; i < mTaskData.Count; i++)
            {
                if (mTaskData[i].Id == taskData.Id)
                    mTaskData[i] = taskData;
            }
        }
    }
}
