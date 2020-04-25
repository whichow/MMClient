// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CatBagWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************

//using UnityEngine;

namespace Game.UI
{
    public partial class TaskAndAchievementWindow : KUIWindow
    {
        public TaskAndAchievementWindow() :
            base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "TaskAndAchievement";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        private void OnQuitBtnClick()
        {
            CloseWindow(this);
        }

        public void RefreshListAfterGetAward(int code, string message, object data)
        {
            RefreshView();
        }

        private void OnPageChange(bool value)
        {
            var page = GetOnToggle();

            if (page == "tgl_task")
            {
                pageType = PageType.kTask;
            }
            else if (page == "tgl_achi")
            {
                pageType = PageType.kAchievement;
            }
            RefreshView();
        }

        public void OnGetDaily(int code, string message, object data)
        {
            //RefreshModel();
            RefreshView();
        }

        public void OnAchievement(int code, string message, object data)
        {
            //RefreshModel();
            RefreshView();
        }

        public override void Awake()
        {
            InitModel();
            InitView();
        }
        
        public override void Start()
        {

        }

        public override void OnEnable()
        {
            KMissionManager.Instance.GetDaily(OnGetDaily);
            KMissionManager.Instance.GetAchievement(OnAchievement);
            RefreshModel();
            RefreshView();
        }
    }
}

