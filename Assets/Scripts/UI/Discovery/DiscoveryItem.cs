/** 
 *FileName:     DiscoveryItem.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-19 
 *Description:    
 *History: 
*/
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    using System;
    using Game.DataModel;
    using K.Extension;
    using UnityEngine.EventSystems;

    public class DiscoveryItem : KUIItem, IPointerClickHandler
    {

        #region Field
        private UIList _list;
        private Transform[] _transAwards;
        private Transform[] _transCats;
        private Button _btnDel;
        private Text _textSurplus;
        private Text _textSurplus1;
        private KExplore.Task _task;
        private Text _surplusINGText;
        private Transform _transTasking;
        private Transform _transCondition;
        private Transform _transTime;
        private Transform _transTimeNone;
        private Transform _transTimeing;
        private Transform _transTaskType;
        private Text _textTaskLeftTime;
        private Image _imageTime;
        private Text _textTaskName;
        private Image _imageBack;
        private float t = 0;
        #endregion

        #region Method

        public void ShowTask(KExplore.Task task)
        {
            _task = task;
            _textTaskName.text = _task.name;
            ShowTask();
            //Debuger.Log("探索任务ID" + _task.id + "     " + "当前任务状态" + _task.state);
            //_textTaskName.text = _task.configId.ToString(); ////test
        }

        private void ShowTask()
        {
            if (_task != null)
            {
                RefreshReward();
                RefreshTaskType();
                RefreshState();
                RefreshCats();
            }
        }

        private void RefreshReward()
        {
            List<ItemInfo> lstInfo = new List<ItemInfo>();
            for (int i = 0; i < _task.rewards.Length; i+=2)
            {
                ItemInfo info = new ItemInfo();
                info.ItemCfgId = _task.rewards[i];
                info.ItemNum = _task.rewards[i + 1];
                lstInfo.Add(info);
            }
            _list.DataArray = lstInfo;
            //if (_task.rewards.Length / 2 == 3)
            //{
            //    for (int i = 1; i <= _task.rewards.Length; i++)
            //    {
            //        if (i % 2 == 0)
            //        {
            //            _transAwards[i / 2 - 1].GetComponentInChildren<Image>().overrideSprite = KIconManager.Instance.GetItemIcon(_task.rewards[i - 2]);
            //            _transAwards[i / 2 - 1].GetComponentInChildren<Text>().text = "X" + _task.rewards[i - 1].ToString();
            //        }
            //    }
            //    for (int i = 0; i < _task.rewards.Length / 2; i++)
            //    {
            //        if (i < _task.rewards.Length / 2)
            //        {
            //            _transAwards[i].gameObject.SetActive(true);
            //        }
            //        else
            //        {
            //            _transAwards[i].gameObject.SetActive(false);
            //        }
            //    }
            //}
            //else
            //{
            //    Debug.Log("探索任务奖励配置出错" + _task.rewards.Length);
            //}
        }

        private void RefreshState()
        {
            switch (_task.state)
            {
                case 0:
                    _transTasking.gameObject.SetActive(false);
                    _textSurplus.text = "";
                    _textSurplus1.text = "";
                    if (_task.catConditions.Count > 0)
                    {
                        _textSurplus.text = string.Format(_task.catConditions[0].description, _task.catConditions[0].GetFormatValues());
                    }
                    if (_task.catConditions.Count > 1)
                    {
                        _textSurplus1.text = string.Format(_task.catConditions[1].description, _task.catConditions[1].GetFormatValues());
                    }
                    //_transCondition.GetComponentInChildren<Text>().text = ret;
                    _transCondition.gameObject.SetActive(true);
                    _transTimeing.gameObject.SetActive(false);
                    _transTimeNone.GetComponentInChildren<Text>().text = TimeExtension.ToTimeString(_task.totalTime);
                    _transTimeNone.gameObject.SetActive(true);
                    _transTime.gameObject.SetActive(true);
                    break;
                case 1:
                    _transTasking.gameObject.SetActive(true);
                    _surplusINGText.text = KLocalization.GetLocalString(52082);
                    _transCondition.gameObject.SetActive(false);
                    _transTimeNone.gameObject.SetActive(false);
                    _transTimeing.gameObject.SetActive(true);
                    _transTime.gameObject.SetActive(true);
                    break;
                case 2:
                    _transTasking.gameObject.SetActive(true);
                    _surplusINGText.text = KLocalization.GetLocalString(58041);
                    _transTimeing.gameObject.SetActive(false);
                    _transTimeNone.gameObject.SetActive(false);
                    _transCondition.gameObject.SetActive(false);
                    _transTime.gameObject.SetActive(false);

                    break;
                case 3:
                    _transTasking.gameObject.SetActive(true);
                    _surplusINGText.text = KLocalization.GetLocalString(52034);
                    _transTimeing.gameObject.SetActive(false);
                    _transTimeNone.gameObject.SetActive(false);
                    _transCondition.gameObject.SetActive(false);
                    _transTime.gameObject.SetActive(false);

                    break;
                default:
                    break;
            }
        }

        private void RefreshTaskType()
        {
            switch (_task.type)
            {
                case 1:
                    _imageBack.color = Color.white;
                    _transTaskType.gameObject.SetActive(false);
                    if (_task.state != 0)
                    {
                        _btnDel.gameObject.SetActive(false);
                        //_transTaskType.gameObject.SetActive(false);
                    }
                    else
                    {
                        _btnDel.gameObject.SetActive(true);
                    }
                    break;
                case 2:
                    _imageBack.color = new Color(255f / 255f, 221f / 255f, 189f / 255f);
                    if (_task.state != 0)
                    {
                        _transTaskType.gameObject.SetActive(false);
                    }
                    else
                    {
                        _transTaskType.gameObject.SetActive(true);
                    }
                    _btnDel.gameObject.SetActive(false);
                    break;


                default:
                    break;
            }
        }
        private void RefreshCats()
        {
            if (_task.catIds.Count > 3)
            {
                return;
            }
            for (int i = 0; i < _task.catIds.Count; i++)
            {
                Image icon;
                KUIImage colorImage;
                KUIImage _specialFlagImage;
                KUIImage _specialFrameImage;
                KUIImage[] _starImages;
                icon = _transCats[i].Find("Cat/Icon").GetComponent<Image>();
                colorImage = _transCats[i].Find("Cat/Fish").GetComponent<KUIImage>();
                _specialFlagImage = _transCats[i].Find("Cat/Flag").GetComponent<KUIImage>();
                _specialFrameImage = _transCats[i].Find("Cat/Frame").GetComponent<KUIImage>();
                var fish = _transCats[i].Find("Cat/Fish");
                _starImages = new KUIImage[fish.childCount];
                for (int j = 0; j < _starImages.Length; j++)
                {
                    _starImages[j] = fish.GetChild(j).GetComponent<KUIImage>();
                }
                var cat = KCatManager.Instance.GetCat(_task.catIds[i]);
                if (cat != null)
                {
                    _transCats[i].Find("Cat/Grade").GetComponent<Text>().text = cat.grade.ToString();
                    ShowCatIcon(icon, XTable.CatXTable.GetByID(cat.shopId).GetIconSprite());
                    ShowColor(colorImage, cat.colors);
                    ShowRarity(_specialFlagImage, _specialFrameImage, cat.rarity);
                    ShowStar(_starImages, cat.star);
                }

            }
            for (int i = 0; i < _transCats.Length; i++)
            {
                if (i < _task.catIds.Count)
                {
                    _transCats[i].Find("Empty").gameObject.SetActive(false);
                    _transCats[i].Find("Cat").gameObject.SetActive(true);
                }
                else
                {
                    _transCats[i].Find("Empty").gameObject.SetActive(true);
                    _transCats[i].Find("Cat").gameObject.SetActive(false);
                }
            }


        }
        public void ShowCatIcon(Image icon, Sprite catIcon)
        {
            icon.overrideSprite = catIcon;
        }
        public void ShowColor(KUIImage _colorImage, int color)
        {
            if (color == (int)KCat.Color.fRed)
            {
                _colorImage.ShowSprite(0);
            }
            else if (color == (int)KCat.Color.fYellow)
            {
                _colorImage.ShowSprite(1);
            }
            else if (color == (int)KCat.Color.fBlue)
            {
                _colorImage.ShowSprite(3);
            }
            else if (color == (int)KCat.Color.fGreen)
            {
                _colorImage.ShowSprite(2);
            }
            else if (color == (int)KCat.Color.fPurple)
            {
                _colorImage.ShowSprite(5);
            }
            else if (color == (int)KCat.Color.fBrown)
            {
                _colorImage.ShowSprite(4);
            }
        }
        public void ShowRarity(KUIImage _specialFlagImage, KUIImage _specialFrameImage, int rarity)
        {
            if (rarity == 2)
            {
                _specialFlagImage.ShowSprite(1);
                _specialFrameImage.gameObject.SetActive(true);
                _specialFrameImage.ShowSprite(1);
            }
            else if (rarity == 3)
            {
                _specialFlagImage.ShowSprite(2);
                _specialFrameImage.gameObject.SetActive(true);
                _specialFrameImage.ShowSprite(2);
            }
            else if (rarity == 4)
            {
                _specialFlagImage.ShowSprite(3);
                _specialFrameImage.gameObject.SetActive(true);
                _specialFrameImage.ShowSprite(3);
            }
            else
            {
                _specialFlagImage.ShowSprite(0);
                _specialFrameImage.gameObject.SetActive(false);
                _specialFrameImage.ShowSprite(0);
            }
        }
        public void ShowStar(KUIImage[] _starImages, int star)
        {
            for (int i = 0; i < _starImages.Length; i++)
            {
                _starImages[i].ShowGray(i >= star);
            }
        }
        protected override void Refresh()
        {

        }

        #endregion

        #region Action




        private void OnDelBtnClick()
        {
            if (KExplore.Instance.deleteRemainCount > 0)
            {
                KUIWindow.OpenWindow<MessageBox>(new MessageBox.Data
                {
                    content = string.Format(KLocalization.GetLocalString(52064), KExplore.Instance.deleteRemainCount),
                    onConfirm = OnConfirmDelete,
                    onCancel = OnCancelDelete,
                });
            }
            else
            {
                KUIWindow.OpenWindow<MessageBox>(new MessageBox.Data
                {

                    content = string.Format(KLocalization.GetLocalString(52065), KExplore.Instance.delelteCostStone),
                    onConfirm = OnConfirmDelete,
                    onCancel = OnCancelDelete,
                });
            }

        }
        private void OnConfirmDelete()
        {
            Debug.Log("确定删除");
            KExplore.Instance.DeleteTask(_task.id, OnDeleteTaskCallback);
        }
        private void OnCancelDelete()
        {
            Debug.Log("取消删除");
        }
        private void OnDeleteTaskCallback(int code, string message, object data)
        {
            KUIWindow.GetWindow<DiscoveryWindow>().RefreshView();
        }
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            switch (_task.state)
            {
                case 0:

                    break;
                case 1:
                    KUIWindow.OpenWindow<DiscoveryIngWindow>(_task);
                    break;
                case 2:
                    KExplore.Instance.CompleteTask(_task.id, OnCompleteTaskCallBackSuccess);
                    break;
                case 3:
                    KExplore.Instance.CompleteTask(_task.id, OnCompleteTaskCallBackFailure);
                    break;

                default:
                    break;
            }
        }
        private void OnCompleteTaskCallBackSuccess(int code, string message, object data)
        {
            var list = data as ArrayList;
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item is S2CGetExpeditionReward)
                    {
                        var reward = item as S2CGetExpeditionReward;
                        var _list = new List<IdNum>();
                        foreach (var enent in reward.Specials)
                        {
                            bool flag = false;
                            for (int i = 0; i < _list.Count; i++)
                            {
                                var tmp = _list[i];
                                if (tmp.Id == enent.Id)
                                {
                                    tmp.Num += enent.Num;
                                    _list[i] = tmp;
                                    flag = true;
                                }
                            }
                            if (!flag)
                            {
                                _list.Add(enent);
                            }
                        }
                        KUIWindow.OpenWindow<DiscoveryRetWindow>(new DiscoveryRetWindow.Data
                        {
                            itemIdNum = reward.Rewards,
                            specialsItemIdNum = _list,
                            task = _task,
                            type = 1,
                        }
                    );
                    }
                }
            }
        }
        private void OnCompleteTaskCallBackFailure(int code, string message, object data)
        {
            var list = data as ArrayList;
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item is S2CGetExpeditionReward)
                    {
                        var reward = item as S2CGetExpeditionReward;
                        KUIWindow.OpenWindow<DiscoveryRetWindow>(new DiscoveryRetWindow.Data
                        {
                            itemIdNum = reward.Rewards,
                            specialsItemIdNum = reward.Specials,
                            task = _task,
                            type = 0,
                        }
                        );
                    }
                }
            }
        }
        private void RefreshDiscovery(int code, string message, object data)
        {
            if (code == 0)
            {
                KUIWindow.GetWindow<DiscoveryWindow>().RefreshView();
            }
            else
            {
                Debug.Log("同步失败");
            }

        }
        private void OnCatBtnClick()
        {
            if (_task.state == 0)
            {
                KUIWindow.OpenWindow<DiscoveryMissionWindow>(_task);
            }
        }

        #endregion

        #region Unity

        private void Awake()
        {
            _list = Find<UIList>("Item/BackTop/List");
            _list.SetRenderHandler(RenderHandler);

            var reward = Find<Transform>("Item/BackTop/IconeGrid");
            _transAwards = new Transform[reward.childCount];
            for (int i = 0; i < _transAwards.Length; i++)
            {
                _transAwards[i] = Find<Transform>("Item/BackTop/IconeGrid/IconeItem" + (i + 1));
            }
            var catGrid = Find<Transform>("Item/CatGrid");
            _transCats = new Transform[catGrid.childCount];
            for (int i = 0; i < _transCats.Length; i++)
            {
                _transCats[i] = Find<Transform>("Item/CatGrid/CatItem" + (i + 1));
                _transCats[i].GetComponent<Button>().onClick.AddListener(OnCatBtnClick);
            }
            _btnDel = Find<Button>("Item/Del");
            _btnDel.onClick.AddListener(OnDelBtnClick);
            _transTime = Find<Transform>("Item/Time");
            _textSurplus = Find<Text>("Item/Surplus/SurplusRe/Text (1)");
            _textSurplus1 = Find<Text>("Item/Surplus/SurplusRe/Text (2)");
            _transTasking = Find<Transform>("Item/Surplus/SurplusING");
            _surplusINGText = Find<Text>("Item/Surplus/SurplusING/Text");
            _transCondition = Find<Transform>("Item/Surplus/SurplusRe");
            _transTimeNone = Find<Transform>("Item/Time/Time1");
            _transTimeing = Find<Transform>("Item/Time/Time2");
            _transTaskType = Find<Transform>("Item/Tilte");
            _textTaskLeftTime = Find<Text>("Item/Tilte/Time");
            _imageTime = Find<Image>("Item/Time/Time2/Image");
            _textTaskName = Find<Text>("Item/Text");
            _imageBack = Find<Image>("Item/Back");
        }

        private void RenderHandler(UIListItem item, int index)
        {
            ItemInfo info = item.dataSource as ItemInfo;
            if (info == null)
                return;
            item.GetComp<Text>("Text").text = "X " + info.ItemNum;
            item.GetComp<Image>("Icon").overrideSprite = KIconManager.Instance.GetItemIcon(info.ItemCfgId);
        }

        void Update()
        {
            t -= Time.deltaTime;
            if (t < 0f)
            {
                t = 1f;
                if (_task != null)
                {
                    _imageTime.fillAmount = (float)_task.exploreRemainTime / _task.totalTime;
                    _transTimeing.GetComponentInChildren<Text>().text = TimeExtension.ToTimeString(_task.exploreRemainTime);
                    _textTaskLeftTime.text = TimeExtension.ToTimeString(_task.taskRemainTime);
                    if (_task.state == (int)KExplore.Task.State.Start)
                    {
                        if (_task.exploreRemainTime <= 0)
                        {
                            KExplore.Instance.GetAllTask(RefreshDiscovery);
                        }
                    }
                    if (_task.type == (int)KExplore.Task.Type.Limit)
                    {
                        if (_task.state == (int)KExplore.Task.State.None)
                        {
                            if (_task.taskRemainTime == 0)
                            {
                                KExplore.Instance.GetAllTask(RefreshDiscovery);
                            }
                        }
                    }

                }

            }
        }


        #endregion
    }
}

