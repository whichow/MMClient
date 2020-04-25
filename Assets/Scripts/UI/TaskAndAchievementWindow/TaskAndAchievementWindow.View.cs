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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class TaskAndAchievementWindow
    {
        #region Field

        private Button _quitBtn;
        private ToggleGroup _tglGp_TA;
        private Toggle _tgl_Task;
        private Toggle _tgl_Achievement;
        private KUIItemPool _layoutElementPool;

        private List<Toggle> _rarityToggles;

        private GameObject _point_T;
        private GameObject _point_A;
        private Text _txt_Tnum;
        private Text _txt_Anum;
        #endregion

        #region Method

        private void InitView()
        {
            _quitBtn = Find<Button>("btn_close");
            _quitBtn.onClick.AddListener(this.OnQuitBtnClick);
            _tglGp_TA = Find<ToggleGroup>("ToggleGroup");
            _tgl_Task = Find<Toggle>("ToggleGroup/tgl_task");
            _tgl_Achievement = Find<Toggle>("ToggleGroup/tgl_achi");

            _rarityToggles = new List<Toggle>(_tglGp_TA.GetComponentsInChildren<Toggle>());
            for (int i = 0; i < _rarityToggles.Count; i++)
            {
                _rarityToggles[i].onValueChanged.AddListener(this.OnPageChange);
            }
            _layoutElementPool = Find<KUIItemPool>("Scroll View");
            if (_layoutElementPool && _layoutElementPool.elementTemplate)
            {
                _layoutElementPool.elementTemplate.gameObject.AddComponent<TaskAndAchievementItem>();
            }
            _point_T = Find<Transform>("ToggleGroup/tgl_task/Point").gameObject;
            _point_A = Find<Transform>("ToggleGroup/tgl_achi/Point").gameObject;
            _txt_Tnum = Find<Text>("ToggleGroup/tgl_task/Point/Text");
            _txt_Anum = Find<Text>("ToggleGroup/tgl_achi/Point/Text");
        }

        private void RefreshView()
        {
            StartCoroutine(FillElements());
        }

        private IEnumerator FillElements()
        {
            switch (pageType)
            {
                case PageType.kTask:
                    _tgl_Task.isOn = true;
                    _tgl_Achievement.isOn = false;
                    break;
                case PageType.kAchievement:
                    _tgl_Task.isOn = false;
                    _tgl_Achievement.isOn = true;
                    break;
                default:
                    break;
            }
            _layoutElementPool.Clear();
            var TorAdata = GetTorAData();
            RefreshPoint();
            //此处排序：置顶已完成的

            List<KMission> msnLst = new List<KMission>(TorAdata);
            msnLst.Sort((x, y) =>
            {
                if (x.status == y.status)
                {
                    if (x.id > y.id)
                    {
                        return 1;
                    }
                    else if (x.id == y.id)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (y.status == 2 && x.status != 2)
                {
                    return -1;
                }
                else if (x.status == 2 && y.status != 2)
                {
                    return 1;
                }
                else if (x.status == 1 && y.status == 0)
                {
                    return -1;
                }
                else if (y.status == 1 && x.status == 0)
                {
                    return 1;
                }
                else
                {
                    Debug.LogError("排序异常");
                    return 0;
                }
                //return 1;
            });
            TorAdata = msnLst.ToArray();

            //Debug.Log("当前页签：" + pageType + "数据长度：" + TorAdata.Length);
            for (int i = 0; i < TorAdata.Length; i++)
            {
                var element = _layoutElementPool.SpawnElement();
                var catItem = element.GetComponent<TaskAndAchievementItem>();
                catItem.ShowItem(TorAdata, i);
            }
            yield return null;
        }

        private void RefreshPoint()
        {
            int[] pointNum = KMissionManager.Instance.PointArry();
            _point_T.SetActive(!(pointNum[0] == 0));
            _point_A.SetActive(!(pointNum[1] == 0));
            _txt_Tnum.text = pointNum[0].ToString();
            _txt_Anum.text = pointNum[1].ToString();
        }

        private string GetOnToggle()
        {
            foreach (var toggle in _rarityToggles)
            {
                if (toggle.isOn)
                {
                    return toggle.name;
                }
            }
            return null;
        }

        #endregion
    }
}

