using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class SettingPopChangNameWindow : KUIWindow
    {
        #region Field


        #endregion

        #region Constructor

        public SettingPopChangNameWindow() : base(UILayer.kNormal, UIMode.kSequence)
        {
            uiPath = "SettingPopChangName";
        }

        #endregion

        #region Method

        public override void Awake()
        {
            InitView();

        }

        public override void OnEnable()
        {
            RefreshView();
        }

        public override void AddEvents()
        {
            base.AddEvents();
            PlayerDataModel.Instance.AddEvent(PlayerEvent.ChangeName, OnChangeNameHandler);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            PlayerDataModel.Instance.RemoveEvent(PlayerEvent.ChangeName, OnChangeNameHandler);
        }

        private void OnBackBtnClick()
        {
            CloseWindow<SettingPopChangNameWindow>();
        }

        private void OnChangeNameHandler(IEventData args)
        {
            CloseWindow<SettingPopChangNameWindow>();
            GetWindow<SettingWindow>().RefreshView();
        }

        #endregion 
    }
}
