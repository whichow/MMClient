/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/11 12:20:49
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.Build;
using Game.DataModel;
using Game.Match3;
using Game.UI;
using UnityEngine;

namespace Game
{
    public class GuideAction
    {
        public enum EActionType
        {
            kNone = 0,
            /// <summary>
            /// 按钮（强，弱）
            /// </summary>
            kButton = 1,
            /// <summary>
            /// 对话
            /// </summary>
            kTalk = 2,
            /// <summary>
            /// 动画
            /// </summary>
            kAnimation = 3,
            /// <summary>
            /// 弹改名界面
            /// </summary>
            kChangeName = 4,
            /// <summary>
            /// 聚焦（建筑）
            /// </summary>
            kBuiding = 5,
            /// <summary>
            /// 三消
            /// </summary>
            kM3 = 6,
            /// <summary>
            /// 猫咪巴拉
            /// </summary>
            kCatTalk = 7,
            /// <summary>
            /// 播放动画
            /// </summary>
            kMovie = 8,
        }

        private GuideActionXDM xdm;

        public virtual void Execute(int id)
        {
            xdm = XTable.GuideActionXTable.GetByID(id);
            if (xdm.Jump != null && xdm.Jump.Count > 0)
            {
                for (int i = 0; i < xdm.Jump.Count; i++)
                {
                    if (BuildingManager.Instance.isExistBuilding(xdm.Jump[i]))
                    {
                        GuideManager.Instance.CompleteStep();
                        return;
                    }
                }
            }

            if (xdm.MaskType == 1)
            {
                KUIWindow.OpenWindow<BlackMaskStart>();
            }
            else if (xdm.MaskType == 2)
            {
                KUIWindow.CloseWindow<BlackMaskStart>();
            }

            if (xdm.LockButton != null && xdm.LockButton.Count > 0)
            {
                for (int i = 0; i < xdm.LockButton.Count; i++)
                {
                    PlayerDataModel.Instance.mPlayerData.LockFunction((FunctionType)xdm.LockButton[i], true);
                }
            }
            if (xdm.UnlockButton != null && xdm.UnlockButton.Count > 0)
            {
                for (int i = 0; i < xdm.UnlockButton.Count; i++)
                {
                    PlayerDataModel.Instance.mPlayerData.UnlockFunction((FunctionType)xdm.UnlockButton[i], true);
                }
            }

            if (xdm.CloseUi == 1)
            {
                KUIWindow.CloseWindow<CropWindow>();
                KUIWindow.CloseWindow<BuildingShopWindow>();
            }

            if (xdm.IsGameOverType == 1)
            {
                M3GameManager.Instance.isGameoverNeedTutorial = true;
            }

            if (xdm.ExtendMenu == 1)
            {
                BuildingManager.Instance.ShowMainWindowMenu(true);
                KUIWindow.CloseWindow<FunctionWindow>();
            }
            else if (xdm.ExtendMenu == 2)
            {
                BuildingManager.Instance.ShowMainWindowMenu(false);
            }

            if (xdm.BuildingPos != null)
            {
                Int2 pos = new Int2(xdm.BuildingPos[0], xdm.BuildingPos[1]);
                Vector3 vec = MapHelper.GridToPosition(pos);
                Map.Instance.CurBuildingClickMove(pos);
                BuildingManager.Instance.SetBuildingFocus(vec);
                GameCamera.Instance.Zoom(GameCamera.ZoomLevel.Near);
            }

            if (xdm.Vidicon != null)
            {
                if (xdm.Vidicon.Count == 2)
                {
                    BuildingManager.Instance.SetBuildingFocus(new Vector2(xdm.Vidicon[0], xdm.Vidicon[1]));
                }
            }

            if (xdm.FunctionID != 0)
            {
                BuildingManager.Instance.SetBuildingFocus(xdm.FunctionID, CallSetFocus);
                return;
            }

            if (xdm.FunctionArea != 0)
            {
                AreaManager.Instance.FocusArea(xdm.FunctionArea, CallSetFocus);
                return;
            }

            switch ((EActionType)xdm.Type)
            {
                case EActionType.kNone:
                    break;
                case EActionType.kButton:
                    KUIWindow.OpenWindow<NoviceWindow>(new NoviceWindow.Data { action = xdm });
                    break;
                case EActionType.kTalk:
                    KUIWindow.OpenWindow<NoviceTalkWindow>(new NoviceTalkWindow.Data { action = xdm });
                    break;
                case EActionType.kAnimation:
                    KUIWindow.OpenWindow<OpeningWindow>(new OpeningWindow.Data { describeId = xdm.Animation });
                    break;
                case EActionType.kBuiding:
                    break;
                case EActionType.kChangeName:
                    KUIWindow.OpenWindow<NoviceChangeName>(new NoviceChangeName.Data { action = xdm });
                    break;
                case EActionType.kM3:
                    if (xdm.M3Stop == 1)
                    {
                        if (M3GameManager.Instance)
                            M3GameManager.Instance.isPause = true;
                    }
                    KUIWindow.OpenWindow<NoviceMaskWindow>(new NoviceMaskWindow.Data { action = xdm });
                    break;
                case EActionType.kCatTalk:
                    if (xdm.M3Stop == 1)
                    {
                        if (M3GameManager.Instance)
                            M3GameManager.Instance.isPause = true;
                    }
                    KUIWindow.OpenWindow<GiveNameWindow>(new GiveNameWindow.Data { action = xdm });
                    break;
                case EActionType.kMovie:
                    KUIWindow.OpenWindow<GuideMovieWindow>(new GuideMovieWindow.Data
                    {
                        action = xdm,
                    });
                    break;
                default:
                    break;
            }
        }

        private void CallSetFocus()
        {
            if (xdm.Type == (int)EActionType.kButton)
            {
                KUIWindow.OpenWindow<NoviceWindow>(new NoviceWindow.Data { action = xdm });
                return;
            }
        }

    }
}
