/** 
 *FileName:     DiscoveryIngWindow.Model.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-26 
 *Description:    
 *History: 
*/
namespace Game.UI
{
    partial class DiscoveryIngWindow
    {
        #region WindowData


        #endregion

        #region Field



        #endregion

        #region Method

        public void InitModel()
        {

        }

        public void RefreshModel()
        {

        }
        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }
        private void OnRightBtnClick()
        {
            isNext = true;
            isShowCatModel = false;
            int num = 0;
            for (int i = 0; i < taksingList.Count; i++)
            {
                if (_task.id == taksingList[i].id)
                {
                    num = i;
                }
            }
            if (num< taksingList.Count-1)
            {
                num++;
                nextTask = taksingList[num];
                RefreshView();
            }
        }
        private void OnLeftBtnClick()
        {
            isNext = true;
            isShowCatModel = false;
            int num = 0;
            for (int i = 0; i < taksingList.Count; i++)
            {
                if (_task.id == taksingList[i].id)
                {
                    num = i;
                }
            }
            if (num >0)
            {
                num--;
                nextTask = taksingList[num];
                RefreshView();
            }
        }
        private void OnStopBtnClick()
        {
            OpenWindow<MessageBox>(new MessageBox.Data
            {
                content = "现在中断任务将不能获得任何奖励，是否仍然中断",
                onCancel = OnCancel,
                onConfirm=OnConfirm,

            });
        }
        private void OnCancel()
        {

        }
        private void OnConfirm()
        {
            KExplore.Instance.BreakTask(_task.id, OnStopBtnCallBack);
        }
        private void OnStopBtnCallBack(int code, string message, object data)
        {
            KUIWindow.GetWindow<DiscoveryWindow>().RefreshView();
            CloseWindow(this);
        }

        #endregion
    }
}

