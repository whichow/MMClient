
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/** 
*FileName:     DiscoveryIngWindow.View.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-10-26 
*Description:    
*History: 
*/
using UnityEngine.UI;
using K.Extension;
using Msg.ClientMessage;

namespace Game.UI
{
    partial class DiscoveryIngWindow
    {
        #region Field
        private Transform[] _imageCatIcon;

        private KUIItemPool _layoutElementPool;
        private KUIItemPool _layoutElementPool1;
        private KExplore.Task _task;
        private Button _btnClose;
        private Button _btnCancel;
        private Text _textTime;
        private List<KExplore.Event> events;
        private Button _btnStop;
        private Text _textHaveTask;
        private Button _btnRightTask;
        private Button _btnLeftTask;
        private int time = 0;
        private int num = 0;
        private List<KExplore.Task> taksingList;
        private KExplore.Task nextTask;
        private bool isShowCatModel = false;
        private ScrollRect _outScrollRect;
        private bool isNext;
        private bool isGetReward;
        private bool isAddStartEvent;
        private float speed=0.03f;
        private RawImage imageBack;
  
        #endregion

        #region Method

        public void InitView()
        {
            var catImage = Find<Transform>("LeftBack/CatImage/CatGroup");
            _imageCatIcon = new Transform[catImage.childCount];
            for (int i = 0; i < _imageCatIcon.Length; i++)
            {
                _imageCatIcon[i] = Find<Transform>("LeftBack/CatImage/CatGroup/Cat" + (i + 1));
            }
            _btnClose = Find<Button>("Panel/Close");
            _btnClose.onClick.AddListener(OnCloseBtnClick);
            _btnStop = Find<Button>("Button/ButtonD");
            _btnStop.onClick.AddListener(OnStopBtnClick);
            _btnCancel = Find<Button>("Button/ButtonClose");
            _btnCancel.onClick.AddListener(OnCloseBtnClick);
            _layoutElementPool = Find<KUIItemPool>("LeftBack/Scroll View");
            _textTime = Find<Text>("Button/ButtonD/Back/Text");
            if (_layoutElementPool && _layoutElementPool.elementTemplate)
            {
                _layoutElementPool.elementTemplate.gameObject.AddComponent<DiscoveryIngRewardItem>();
            }

            _layoutElementPool1 = Find<KUIItemPool>("RightBack/Scroll View");
            _outScrollRect = Find<ScrollRect>("RightBack/Scroll View");
            if (_layoutElementPool1 && _layoutElementPool1.elementTemplate)
            {
                _layoutElementPool1.elementTemplate.gameObject.AddComponent<DiscoveryIngEventItem>();
            }
            _textHaveTask = Find<Text>("Panel/Percent/Text");
            _btnRightTask = Find<Button>("Panel/Right");
            _btnRightTask.onClick.AddListener(OnRightBtnClick);
            _btnLeftTask = Find<Button>("Panel/Left");
            _btnLeftTask.onClick.AddListener(OnLeftBtnClick);
            imageBack = Find<RawImage>("LeftBack/CatImage/ImageBack");
       
            events = new List<KExplore.Event>();

        }
        public void RefreshView()
        {
            if (isNext)
            {
                if (nextTask != null)
                {
                    _task = nextTask;
                    events.Clear();
                    nextTask = null;
                    isAddStartEvent = false;

                }
            }
            else
            {
                _task = data as KExplore.Task;

            }
            if (_task.exploreRemainTime <= 0)
            {
                isGetReward = true;
                _btnStop.gameObject.SetActive(false);
                _btnCancel.gameObject.SetActive(true);
            }
            else
            {
                isGetReward = false;
                _btnStop.gameObject.SetActive(true);
                _btnCancel.gameObject.SetActive(false);
            }
            if (!isAddStartEvent)
            {
                AddStartEvent();
                isAddStartEvent = true;
            }


            RefreshNum();
            RefrshEvent();
            if (!isShowCatModel)
            {
                for (int i = 0; i < _imageCatIcon.Length; i++)
                {
                    if (_imageCatIcon[i].childCount > 0)
                    {
                        Object.Destroy(_imageCatIcon[i].GetChild(0).gameObject);
                    }
                }

                StartCoroutine(RefreshViewCat());
            }

            FillElements();
            FillElements1();

        }
        public override void OnDisable()
        {
            events.Clear();
            isNext = false;
            isAddStartEvent = false;
            isShowCatModel = false;
        }
        private void AddStartEvent()
        {
            events.Add(_task.GetStartEventCopy());
        }
        private void RefrshEvent()
        {
            for (int i = 0; i < _task.specialEvents.Count; i++)
            {
                if (_task.specialEvents[i].triggerTime < 0 && !events.Contains(_task.specialEvents[i]))
                {
                    events.Add(_task.specialEvents[i]);
                }
            }
        }
        private IEnumerator RefreshViewCat()
        {
            yield return null;

            for (int i = 0; i < _imageCatIcon.Length; i++)
            {
                if (i < _task.catIds.Count)
                {
                    GameObject modelPrefab = CatUtils.GetModel(KCatManager.Instance.GetCat(_task.catIds[i]).shopId) ;
                    if (modelPrefab != null)
                    {
                        modelPrefab.layer = _imageCatIcon[i].gameObject.layer;
                        modelPrefab.transform.SetParent(_imageCatIcon[i].transform, false);
                        modelPrefab.GetComponent<Renderer>().sortingOrder = _textHaveTask.canvas.sortingOrder + 1;
                        modelPrefab.GetComponent<KCatBehaviour>().Walk();
                        _imageCatIcon[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("没有猫咪模型" + _task.catIds[i]);
                        yield return null;
                    }
                }
                else
                {
                    _imageCatIcon[i].gameObject.SetActive(false);
                }
            }
            isShowCatModel = true;
        }

        //public override void OnDisable()
        //{
        //    isShowCatModel = false;
        //}

        private void RefreshNum()
        {
            taksingList = new List<KExplore.Task>();
            num = 0;
            int indx = 0;
            for (int i = 0; i < KExplore.Instance.allTask.Length; i++)
            {
                if (KExplore.Instance.allTask[i].state == 1)
                {
                    num++;
                    taksingList.Add(KExplore.Instance.allTask[i]);
                }

            }
            if (_task != null)
            {
                for (int i = 0; i < taksingList.Count; i++)
                {

                    if (_task.id == taksingList[i].id)
                    {
                        indx = i;
                    }
                }
            }
            _textHaveTask.text = (indx + 1) + "/" + num;
        }
        private void FillElements()
        {
            _layoutElementPool.Clear();
            var rewards = _task.specialReward;

            for (int i = 0; i < rewards.Count; i++)
            {
                var element = _layoutElementPool.SpawnElement();
                var catItem = element.GetComponent<DiscoveryIngRewardItem>();
                catItem.ShowReward(rewards[i]);
            }

        }
        private void FillElements1()
        {
            _layoutElementPool1.Clear();
            for (int i = 0; i < events.Count; i++)
            {
                var element = _layoutElementPool1.SpawnElement();
                var catItem = element.GetComponent<DiscoveryIngEventItem>();
                catItem.ShowReward(events[i]);
            }

            if (_outScrollRect)
            {
                _outScrollRect.verticalNormalizedPosition = 0;
            }

        }
        public override void UpdatePerSecond()
        {
            time++;
            _textTime.text = TimeExtension.ToTimeString(_task.exploreRemainTime);
            if (_task.exploreRemainTime <= 0)
            {
                if (!isGetReward)
                {
                    events.Add(_task.GetFinishEventCopy());
                    RefreshView();
                    KExplore.Instance.CompleteTask(_task.id, OnCompleteTaskCallBackSuccess);
                }

            }
            Debug.Log("特殊事件数量" + _task.specialEvents.Count + "特殊奖励" + _task.specialReward.Count);
            for (int i = 0; i < _task.specialEvents.Count; i++)
            {

                if (_task.specialEvents[i].triggerTime < 0 && !events.Contains(_task.specialEvents[i]))
                {
                    events.Add(_task.specialEvents[i]);
                    RefreshView();
                }
            }
            if (time % 5 == 0)
            {
                if (!isGetReward)
                {
                    events.Add(_task.GetProcessEventCopy());
                    RefreshView();
                }
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
                        KUIWindow.OpenWindow<DiscoveryRetWindow>(new DiscoveryRetWindow.Data
                        {
                            itemIdNum = reward.Rewards,
                            specialsItemIdNum = reward.Specials,
                            task = _task,
                            type = 1,
                        }
                        );
                    }
                }
            }
        }
        public override void Update()
        {
            base.Update();
            float amtToMove = speed * Time.deltaTime;
            imageBack.uvRect = new Rect(new Vector2(imageBack.uvRect.x+Vector2.left.x *amtToMove,0), Vector2.one);


        }
        #endregion
    }
}

