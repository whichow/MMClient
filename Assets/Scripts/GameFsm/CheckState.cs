// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CheckState" company=""></copyright>
// <summary></summary>
// ***********************************************************************
namespace Game
{
    using K.Fsm;
    /// <summary>
    /// 检查资源
    /// </summary>
    public class CheckState : GameState
    {
        public const int kDownloadRes = 1;

        private int _state = 0;
        private IAsyncR _asyncR;

        protected override void OnInit(IFsm<GameFsm> fsm)
        {
            SubscribeEvent(kDownloadRes, OnLoginStartEvent);
        }

        protected override void OnEnter(IFsm<GameFsm> fsm)
        {
            _state = 2;
        }


        protected override void Update(IFsm<GameFsm> fsm, float delta)
        {
            switch (_state)
            {
                case 0:
                    //_asyncR = KCDNManager.Instance.DownConfig();
                    SetCheckState(0f, "正在检查更新...", "");
                    _state = 1;
                    break;
                case 1:
                    if (_asyncR.done)
                    {
                        _state = 2;
                        _asyncR = null;
                    }
                    else
                    {
                        SetCheckState(_asyncR.progress, "正在检查更新...", "");
                    }
                    break;
                case 2:
                    _state = 3;
                    KAssetManager.Instance.LoadTables();
                    KGameModuleManager.Instance.Load();
                    ChangeState<LoginState>(fsm);
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }

        private void SetCheckState(float progress, string progressHint, string tips)
        {
            LoginWindow.Instance.SetState(progress, progressHint, tips);
        }

        private void OnLoginStartEvent(IFsm<GameFsm> fsm, object sender, object userData)
        {
        }
    }
}

