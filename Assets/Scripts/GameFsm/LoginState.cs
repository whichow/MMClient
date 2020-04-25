// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "LoginState" company=""></copyright>
// <summary></summary>
// ***********************************************************************
namespace Game
{
    using K.Fsm;
    using Msg.ClientMessage;

    /// <summary>
    /// 登录状态
    /// </summary>
    public class LoginState : GameState
    {
        public const int kStartLogin = 1;
        //public const int kBaseGets = 10;
        //public const int kCatGets = 20;
        //public const int kBuildingGets = 30;
        public const int kGetFinish = 100;
        public const int kLoadFinish = 200;

        private bool _isLogining;
        private IAsyncR _asyncR;

        /// <summary>
        /// 
        /// </summary>
        public float progress
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string progressText
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string tipsText
        {
            get;
            set;
        }

        protected override void OnInit(IFsm<GameFsm> fsm)
        {
            SubscribeEvent(kStartLogin, OnLoginStartEvent);
            //SubscribeEvent(kBaseGets, OnPullBaseEvent);
            //SubscribeEvent(kCatGets, OnCatGetsEvent);
            //SubscribeEvent(kBuildingGets, OnBuildingGetsEvent);
            SubscribeEvent(kGetFinish, OnGetFinishEvent);
            SubscribeEvent(kLoadFinish, OnLoadFinishEvent);
        }

        protected override void OnEnter(IFsm<GameFsm> fsm)
        {
            LoginWindow.Instance.ShowLogin();
        }

        protected override void OnExit(IFsm<GameFsm> fsm)
        {
        }

        protected override void Update(IFsm<GameFsm> fsm, float delta)
        {
            if (_asyncR != null)
            {
                progress = 0.8f + _asyncR.progress * 0.2f;
                progressText = "正在载入大地图...";
                if (_asyncR.done)
                {
                    _asyncR = null;
                    fsm.SendEvent(this, kLoadFinish);
                }
            }
        }

        private void SetLoginState(float progress, string progressHint, string tips)
        {
            LoginWindow.Instance.SetState(progress, progressHint, tips);
        }

        private void OnLoginStartEvent(IFsm<GameFsm> fsm, object sender, object userData)
        {
            if (!_isLogining)
            {
                _isLogining = true;
                SetLoginState(0.1f, KLanguageManager.Instance.GetLocalString(70237), KLanguageManager.Instance.GetLocalString(70238));//"登录帐号...", "每当夜幕降临");
                //KUser.Login(OnLoginCallback);
                GameApp.Instance.LoginServer.ReqLogin(KUser.OpenID, KUser.OpenToken,0);
            }
        }

        //private void OnPullBaseEvent(IFsm<GameFsm> fsm, object sender, object userData)
        //{
        //    _isLogining = true;
        //    SetLoginState(0.2f, KLanguageManager.Instance.GetLocalString(70239), KLanguageManager.Instance.GetLocalString(70240));//"正在把物品放入背包...", "小花猫就显得特别精神");
        //    KUser.PullAllInfos(OnBaseGetsCallback);
        //}

        //private void OnCatGetsEvent(IFsm<GameFsm> fsm, object sender, object userData)
        //{
        //    SetLoginState(0.5f, KLanguageManager.Instance.GetLocalString(70241), KLanguageManager.Instance.GetLocalString(70242));//"正在去喂猫的路上...", "它瞪着圆圆的大眼睛守在老鼠家的门口静静地等待着");
        //    KUser.PullAllInfos1(OnCatGetsCallback);
        //}

        //private void OnBuildingGetsEvent(IFsm<GameFsm> fsm, object sender, object userData)
        //{
        //    SetLoginState(0.7f, KLanguageManager.Instance.GetLocalString(70243), KLanguageManager.Instance.GetLocalString(70244));//"正在摆放建筑...", "一当有目标出现");
        //    KUser.PullAllInfos2(OnBuildingGetsCallback);
        //}

        private void OnGetFinishEvent(IFsm<GameFsm> fsm, object sender, object userData)
        {
            _asyncR = KAssetManager.Instance.LoadGlobalAssets();
            SetLoginState(0.8f, KLanguageManager.Instance.GetLocalString(70236), KLanguageManager.Instance.GetLocalString(70245));// "正在载入大地图...", "小猫会一下子扑过去死死地捉住");
        }

        private void OnLoadFinishEvent(IFsm<GameFsm> fsm, object sender, object userData)
        {
            SetLoginState(1f, "", KLanguageManager.Instance.GetLocalString(70246));// "然后美餐一顿");
            KIconManager.Instance.LoadAllIcons();
            ChangeState<InGameState>(fsm);
        }

        private void OnLoginCallback(int code, string message, object data)
        {
            //if (code == 0)
            //{
            //    var gameFsm = KFramework.FsmManager.GetFsm<GameFsm>();
            //    gameFsm.SendEvent(this, kBaseGets);
            //}
            _isLogining = false;
        }

        //private void OnBaseGetsCallback(int code, string message, object data)
        //{
        //    if (code == 0)
        //    {
        //        var gameFsm = KFramework.FsmManager.GetFsm<GameFsm>();
        //        gameFsm.SendEvent(this, kCatGets);
        //    }
        //}

        //private void OnCatGetsCallback(int code, string message, object data)
        //{
        //    if (code == 0)
        //    {
        //        var gameFsm = KFramework.FsmManager.GetFsm<GameFsm>();
        //        gameFsm.SendEvent(this, kBuildingGets);
        //    }
        //}

        //private void OnBuildingGetsCallback(int code, string message, object data)
        //{
        //    if (code == 0)
        //    {
        //        var gameFsm = KFramework.FsmManager.GetFsm<GameFsm>();
        //        gameFsm.SendEvent(this, kGetFinish);
        //    }
        //}
    }
}

