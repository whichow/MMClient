/** 
*FileName:     DiscoveryMissionWindow.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-10-23 
*Description:    
*History: 
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class DiscoveryMissionWindow : KUIWindow
    {
        #region Constructor

        public DiscoveryMissionWindow()
            : base(UILayer.kNormal, UIMode.kSequence)
        {
            uiPath = "DiscoveryMission";
        }

        #endregion

        #region Method       

        #endregion

        #region Action

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        private void OnStartBtnClick()
        {
            //Debug.Log("开始执行");
            if (PlayerDataModel.Instance.mPlayerData.Vigour < 1)
            {
                //行动力不足
                UI.MessageBox.ShowMessage("提示", KLanguageManager.Instance.GetLocalString(57132));
                return;
            }

            List<int> catIds = new List<int>();
            for (int i = 0; i < catList.Count; i++)
            {
                if (catList[i] != null)
                {
                    catIds.Add(catList[i].mCatInfo.Id);
                }
            }
            KExplore.Instance.StartTask(task.id, catIds.ToArray(), OnStartCallBack);
        }

        private void OnStartCallBack(int code, string message, object data)
        {
            GetWindow<DiscoveryWindow>().RefreshView();
            CloseWindow<DiscoveryMissionWindow>();
        }

        private void OnButton1()
        {
            var catList = GetCat(CatDataModel.Instance.GetAllCatDataVO(), task.catConditions, 0);
            if (catList.Count > 0)
            {
                OpenWindow<ChooseCatWindow>(new ChooseCatWindow.Data
                {
                    catsList = catList,
                    onCancel = OnCancel,
                    onConfirm = OnAddCat,
                    idx = 0,
                    type = 2,
                });
            }
            else
            {
                ToastBox.ShowText(KLocalization.GetLocalString(52106));
            }
        }

        private void OnButton2()
        {
            var catList = GetCat(CatDataModel.Instance.GetAllCatDataVO(), task.catConditions, 1);
            if (catList.Count > 0)
            {
                OpenWindow<ChooseCatWindow>(new ChooseCatWindow.Data
                {
                    catsList = catList,
                    onCancel = OnCancel,
                    onConfirm = OnAddCat,
                    idx = 1,
                    type = 2,
                });
            }
            else
            {
                ToastBox.ShowText(KLocalization.GetLocalString(52106));
            }
        }

        private void OnButton3()
        {
            var catList = GetCat(CatDataModel.Instance.GetAllCatDataVO(), task.catConditions, 2);
            if (catList.Count > 0)
            {
                OpenWindow<ChooseCatWindow>(new ChooseCatWindow.Data
                {
                    catsList = catList,
                    onCancel = OnCancel,
                    onConfirm = OnAddCat,
                    idx = 2,
                    type = 2,
                });
            }
            else
            {
                ToastBox.ShowText(KLocalization.GetLocalString(52106));
            }
        }

        private void OnCancel()
        {
            Debug.Log("取消选猫咪");
        }

        private void OnAddCat(CatDataVO cat, int idx)
        {
            catList[idx] = cat;
            RefreshCats();
            _transCats[idx].Find("CardBig/Cat/Del").GetComponent<Button>().onClick.AddListener(delegate ()
            {
                OnDelCatBtnClick(idx);
            });
            _transevent1.gameObject.SetActive(true);
            _transevent2.gameObject.SetActive(true);
        }
        private void OnDelCatBtnClick(int idx)
        {
            RemoveCat(idx);
        }
        #endregion

        #region Unity

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            RefreshView();
        }

        #endregion
    }
}

