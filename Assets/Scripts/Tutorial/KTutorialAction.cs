//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : #DATA#
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "TutorialAction" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//namespace Game
//{
//    using Build;
//    using Game.Match3;
//    using System.Collections;
//    using UI;
//    using UnityEngine;

//    public class KTutorialAction
//    {
//        public enum Type
//        {
//            kNone = 0,
//            kButton = 1,
//            kTalk = 2,
//            kAnimation = 3,
//            kChangeName = 4,
//            kBuiding = 5,
//            kM3 = 6,
//            kCatTalk = 7,
//            kMovie = 8,
//        }

//        /// <summary>
//        /// 根据类型创建行为(重载类用)
//        /// </summary>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        public static KTutorialAction CreateAction(int type)
//        {
//            var ret = new KTutorialAction();
//            ret.type = type;
//            return ret;
//        }

//        #region Field

//        public int id
//        {
//            get;
//            private set;
//        }

//        public int type
//        {
//            get;
//            private set;
//        }

//        public int[] text
//        {
//            get;
//            private set;
//        }
//        public int[] anim
//        {
//            get;
//            private set;
//        }
//        public string movie
//        {
//            get;
//            private set;
//        }

//        public string sound
//        {
//            get;
//            private set;
//        }

//        public string button
//        {
//            get;
//            private set;
//        }

//        public int[] speaker
//        {
//            get;
//            private set;
//        }
//        public int[] talking
//        {
//            get;
//            private set;
//        }
//        public ArrayList prominent
//        {
//            get;
//            private set;
//        }
//        public Int2[] change
//        {
//            get;
//            private set;
//        }
//        public int levelTalkingType
//        {
//            get;
//            private set;
//        }
//        public int m3Stop
//        {
//            get;
//            private set;
//        }
//        public int[] mask1Positon
//        {
//            get;
//            private set;
//        }
//        public int[] mask1Size
//        {
//            get;
//            private set;
//        }
//        public int[] mask2Positon
//        {
//            get;
//            private set;
//        }
//        public int[] mask2Size
//        {
//            get;
//            private set;
//        }
//        public int[] handStartPosition
//        {
//            get;
//            private set;
//        }
//        public int[] handEndPosition
//        {
//            get;
//            private set;
//        }
//        public int handTime
//        {
//            get;
//            private set;
//        }
//        public int InM3Lock
//        {
//            get;
//            private set;
//        }
//        public int modelType
//        {
//            get;
//            private set;
//        }
//        public int isGameOver
//        {
//            get;
//            private set;
//        }
//        public int levleImage
//        {
//            get;
//            private set;
//        }
//        public int maskType
//        {
//            get;
//            private set;
//        }
//        public int m3FromBuiding
//        {
//            get;
//            private set;
//        }
//        public int extend
//        {
//            get;
//            private set;
//        }
//        public int[] buildingSenceVec
//        {
//            get;
//            private set;
//        }
//        public int function
//        {
//            get;
//            private set;
//        }
//        public int[] jumpAction
//        {
//            get;
//            private set;
//        }
//        public int closeFarmUi
//        {
//            get;
//            private set;
//        }
//        public int functionArea
//        {
//            get;
//            private set;
//        }
//        public int btnType
//        {
//            get;
//            private set;
//        }
//        public int[] btnLock
//        {
//            get;
//            private set;
//        }
//        public int window
//        {
//            get;
//            private set;
//        }
//        public int IsMask
//        {
//            get;
//            private set;
//        }
//        public int[] unLockButton
//        {
//            get;
//            private set;
//        }
//        #endregion

//        #region Method

//        public virtual void Execute()
//        {
     
//            if (jumpAction != null && jumpAction.Length > 0)
//            {
//                for (int i = 0; i < jumpAction.Length; i++)
//                {
//                    if (BuildingManager.Instance.isExistBuilding(jumpAction[i]))
//                    {
//                        KTutorialManager.Instance.CompleteStage();
//                        return;
//                    }

//                }
//            }
//            if (IsMask==1)
//            {
//                KUIWindow.OpenWindow<BlackMaskStart>();
//            }
//            else if (IsMask==2)
//            {
//                KUIWindow.CloseWindow<BlackMaskStart>();
//            }
//            if (btnLock != null && btnLock.Length > 0)
//            {
//                for (int i = 0; i < btnLock.Length; i++)
//                {
//                    KUser.SelfPlayer.LockFunction((KPlayer.FunctionType)btnLock[i], true);
//                }
//            }
//            if (unLockButton!=null&& unLockButton.Length>0)
//            {
//                for (int i = 0; i < unLockButton.Length; i++)
//                {
//                    KUser.SelfPlayer.UnlockFunction((KPlayer.FunctionType)unLockButton[i], true);
//                }
//            }
//            if (closeFarmUi == 1)
//            {
//                KUIWindow.CloseWindow<CropWindow>();
//                KUIWindow.CloseWindow<BuildingShopWindow>();
//            }
//            if (isGameOver == 1)
//            {
//                M3GameManager.Instance.isGameoverNeedTutorial = true;
//            }
//            if (extend == 1)
//            {
//                Debug.Log("展开~~~~~~~~~~~~~~~");
//                BuildingManager.Instance.ShowMainWindowMenu(true);
//            }
//            else if (extend == 2)
//            {
//                BuildingManager.Instance.ShowMainWindowMenu(false);
//            }

//            if (buildingSenceVec != null)
//            {
//                if (buildingSenceVec.Length == 2)
//                {
//                    BuildingManager.Instance.SetBuildingFocus(new Vector2((float)buildingSenceVec[0] / 1000, (float)buildingSenceVec[1] / 1000));
//                }
//            }
//            if (function != 0)
//            {
//                BuildingManager.Instance.SetBuildingFocus(function, CallSetFocus);

//                return;
//            }
//            if (functionArea != 0)
//            {
//                AreaManager.Instance.FocusArea(functionArea, CallSetFocus);
//                return;
//            }

//            Debug.Log("actionId" + id + "!!!!!!!!");
//            switch ((Type)type)
//            {
//                case Type.kNone:

//                    break;
//                case Type.kButton:
//                    KUIWindow.OpenWindow<NoviceWindow>(new NoviceWindow.Data
//                    {
//                        action = this,
//                    });
//                    break;
//                case Type.kTalk:
//                    KUIWindow.OpenWindow<NoviceTalkWindow>(new NoviceTalkWindow.Data
//                    {
//                        action = this,
//                    });
//                    break;
//                case Type.kAnimation:
//                    KUIWindow.OpenWindow<OpeningWindow>(new OpeningWindow.Data
//                    {
//                        describeId = anim,
//                    });
//                    break;
//                case Type.kBuiding:


//                    break;

//                case Type.kChangeName:
//                    KUIWindow.OpenWindow<NoviceChangeName>(new NoviceChangeName.Data
//                    {
//                        action = this,
//                    });
//                    break;
//                case Type.kM3:
//                    if (m3Stop == 1)
//                    {
//                        M3GameManager.Instance.isPause = true;
//                    }
//                    KUIWindow.OpenWindow<NoviceMaskWindow>(new NoviceMaskWindow.Data
//                    {
//                        action = this,
//                    });
//                    break;
//                case Type.kCatTalk:
//                    if (m3Stop == 1)
//                    {
//                        if (M3GameManager.Instance)
//                        {
//                            M3GameManager.Instance.isPause = true;
//                        }
//                    }
//                    KUIWindow.OpenWindow<GiveNameWindow>(new GiveNameWindow.Data
//                    {
//                        action = this,
//                    });
//                    break;
//                case Type.kMovie:
//                    KUIWindow.OpenWindow<GuideMovieWindow>(new GuideMovieWindow.Data
//                    {
//                        action = this,
//                    });
//                    break;
//                default:
//                    break;
               
//            }
//        }
//        private void CallSetFocus()
//        {
//            if (type == (int)Type.kButton)
//            {
//                KUIWindow.OpenWindow<NoviceWindow>(new NoviceWindow.Data
//                {
//                    action = this,
//                });
//                return;
//            }
//        }
    
//        public void Load(Hashtable table)
//        {
//            id = table.GetInt("Id");
//            type = table.GetInt("Type");
//            text = table.GetArray<int>("Text");
//            movie = table.GetString("Movie");
//            sound = table.GetString("Sound");
//            button = table.GetString("Button");
//            speaker = table.GetArray<int>("Speaker");
//            anim = table.GetArray<int>("Animation");
//            talking = table.GetArray<int>("Talkings");
//            prominent = table.GetList("Prominent");
//            var tmp = table.GetList("Change");
//            if (tmp != null)
//            {
//                change = new Int2[tmp.Count];
//                for (int i = 0; i < tmp.Count; i++)
//                {
//                    var arrTmp = tmp.GetArray<int>(i);
//                    Int2 int2Tmp = new Int2(arrTmp);
//                    change[i] = int2Tmp;
//                }
//            }
//            levelTalkingType = table.GetInt("LevelTalkings");
//            m3Stop = table.GetInt("Stop");
//            mask1Positon = table.GetArray<int>("Mask1Positon");
//            mask1Size = table.GetArray<int>("Mask1Size");
//            mask2Positon = table.GetArray<int>("Mask2Positon");
//            mask2Size = table.GetArray<int>("Mask2Size");
//            handStartPosition = table.GetArray<int>("HandSratPosition");
//            handEndPosition = table.GetArray<int>("HandEndPosition");
//            handTime = table.GetInt("HandTime");
//            InM3Lock = table.GetInt("Lock");
//            modelType = table.GetInt("ModelType");
//            isGameOver = table.GetInt("IsGameOverType");
//            levleImage = table.GetInt("LevelImage");
//            maskType = table.GetInt("IsMask");
//            m3FromBuiding = table.GetInt("M3FromBuiding");
//            extend = table.GetInt("Extend");
//            buildingSenceVec = table.GetArray<int>("Vidicon");
//            function = table.GetInt("Function");
//            jumpAction = table.GetArray<int>("Jump");
//            closeFarmUi = table.GetInt("CloseUi");
//            functionArea = table.GetInt("FunctionArea");
//            btnType = table.GetInt("ButtonType");
//            btnLock = table.GetArray<int>("LockButton");
//            window = table.GetInt("Window");
//            IsMask = table.GetInt("MaskType");
//            unLockButton = table.GetArray<int>("UnlockButton");
//        }

//        #endregion

//    }
//}
