using Msg.ClientMessage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class MailWindow
    {
        private Button _close;
        private GameObject _noMail;
        private GameObject _leftObj;
        private GameObject _rightObj;
        private UIList _mailList;
        private UIList _rewardList;
        private Text _title;
        private Text _time;
        private Text _content;
        private Text _sendName;
        private Text _mailBtnText;
        private GameObject _attObj;
        private Button _allDelete;
        private Button _allGetAtt;
        private Button _mailBtn;
        private MailDataVO _curMailVO;
        private int _curMailId;

        private void InitView()
        {
            _close = Find<Button>("Close");
            _close.onClick.AddListener(OnQuitBtnClick);
            _noMail = Find("NoMail");
            _leftObj = Find("Left");
            _rightObj = Find("Right");
            _mailList = Find<UIList>("Left/List");
            _mailList.SetRenderHandler(RenderHandler);
            _mailList.SetSelectHandler(SelectHandler);
            _rewardList = Find<UIList>("Right/Att/List");
            _rewardList.SetRenderHandler(RewardRenderHandler);
            _rewardList.SetPointerHandler(RewardPointerHandler);
            _title = Find<Text>("Right/Title");
            _time = Find<Text>("Right/Time");
            _content = Find<Text>("Right/Content");
            _sendName = Find<Text>("Right/Content/SendName");
            _mailBtnText = Find<Text>("Right/MailBtn/Text");
            _attObj = Find("Right/Att");
            _allDelete = Find<Button>("Left/AllDelete");
            _allDelete.onClick.AddListener(OnAllDelete);
            _allGetAtt = Find<Button>("Left/AllGetAtt");
            _allGetAtt.onClick.AddListener(OnAllGetAtt);
            _mailBtn = Find<Button>("Right/MailBtn");
            _mailBtn.onClick.AddListener(OnMailGetDelete);
        }

        private void RewardPointerHandler(UIListItem item, int index)
        {
            //µã»÷ÊÂ¼þ
        }

        private void RewardRenderHandler(UIListItem item, int index)
        {
            ItemInfo vo = item.dataSource as ItemInfo;
            if (vo == null)
                return;
            item.GetComp<Image>("Icon").overrideSprite = KIconManager.Instance.GetItemIcon(vo.ItemCfgId);
            item.GetComp<Text>("Num").text = vo.ItemNum.ToString();
            item.GetGameObject("Select").SetActive(_curMailVO.mIsGetAtt);
        }

        private void SelectHandler(UIListItem item, int index)
        {
            MailDataVO vo = item.dataSource as MailDataVO;
            if (vo == null)
                return;
            if (!vo.mIsDetail)
            {
                List<int> lstId = new List<int>();
                lstId.Add(vo.mMailID);
                GameApp.Instance.GameServer.ReqMailDetail(lstId);
            }
            else
            {
                OnMailDetail(vo);
            }
            item.GetComp<KUIImage>("Icon").overrideSprite = item.GetComp<KUIImage>("Icon").sprites[1];
        }

        private void RenderHandler(UIListItem item, int index)
        {
            MailDataVO vo = item.dataSource as MailDataVO;
            if (vo == null)
                return;
            KUIImage img = item.GetComp<KUIImage>("Icon");
            if (vo.mIsRead)
                img.overrideSprite = img.sprites[1];
            else
                img.overrideSprite = img.sprites[0];
            item.GetComp<Text>("Title").text = vo.mTitle;
            item.GetComp<Text>("Time").text = Utils.GetTime(vo.mSendTime, "yyyy-MM-dd");
            KUIImage img1 = item.GetComp<KUIImage>("Img1");
            KUIImage img2 = item.GetComp<KUIImage>("Img2");
            KUIImage img3 = item.GetComp<KUIImage>("Icon");
            if (vo.mIsHasAtt && vo.mIsGetAtt)
            {
                img1.ShowGray(true);
                img2.ShowGray(true);
                img3.ShowGray(true);
                //img1.material = Resources.Load<Material>("Materials/UIGray");
                //img2.material = Resources.Load<Material>("Materials/UIGray");
                //img3.material = Resources.Load<Material>("Materials/UIGray");
            }
            else
            {
                img1.ShowGray(false);
                img2.ShowGray(false);
                img3.ShowGray(false);
                //img1.material = null;
                //img2.material = null;
                //img3.material = null;
            }
        }

        public override void AddEvents()
        {
            base.AddEvents();
            MailDataModel.Instance.AddEvent(MailEvent.MailData, OnMailData);
            MailDataModel.Instance.AddEvent(MailEvent.MailDetail, OnMailDetail);
            MailDataModel.Instance.AddEvent(MailEvent.MailGetAtt, OnMailGetAtt);
            MailDataModel.Instance.AddEvent(MailEvent.MailDelete, OnMailData);
            MailDataModel.Instance.AddEvent(MailEvent.MailNewNotify, OnMailData);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            MailDataModel.Instance.RemoveEvent(MailEvent.MailData, OnMailData);
            MailDataModel.Instance.RemoveEvent(MailEvent.MailDetail, OnMailDetail);
            MailDataModel.Instance.RemoveEvent(MailEvent.MailGetAtt, OnMailGetAtt);
            MailDataModel.Instance.RemoveEvent(MailEvent.MailDelete, OnMailData);
            MailDataModel.Instance.RemoveEvent(MailEvent.MailNewNotify, OnMailData);
        }

        private void OnMailGetAtt(IEventData value)
        {
            List<MailDataVO> lstMailVO = new List<MailDataVO>();
            lstMailVO = MailDataModel.Instance.OnMailData();
            _mailList.Refresh(lstMailVO);
            MailDataVO vo = new MailDataVO();
            vo = MailDataModel.Instance.GetMailVOById(_curMailId);
            OnMailDetail(vo);
            KUIWindow.OpenWindow<GetItemTipsWindow>((value as EventData).Data as List<ItemInfo>);
        }

        private void OnMailDetail(IEventData value)
        {
            MailDataVO vo = new MailDataVO();
            vo = MailDataModel.Instance.GetMailVOById((value as EventData).Integer);
            OnMailDetail(vo);
        }

        private void OnMailDetail(MailDataVO vo)
        {
            _curMailVO = vo;
            _curMailId = _curMailVO.mMailID;
            _title.text = _curMailVO.mTitle;
            _time.text = KLocalization.GetLocalString(53065) + Utils.GetCountTime(_curMailVO.LeftSecsTime);
            _content.text = _curMailVO.mContent;
            _sendName.text = _curMailVO.mSendName;
            _attObj.SetActive(_curMailVO.mIsHasAtt);
            if (_curMailVO.mIsHasAtt && !_curMailVO.mIsGetAtt)
                _mailBtnText.text = KLocalization.GetLocalString(59909);
            else
                _mailBtnText.text = KLocalization.GetLocalString(70205);
            if (_curMailVO.mIsHasAtt)
                _rewardList.DataArray = _curMailVO.mLstAtt;
        }

        private void OnMailData()
        {
            List<MailDataVO> lstMailVO = new List<MailDataVO>();
            lstMailVO = MailDataModel.Instance.OnMailData();
            _mailList.DataArray = lstMailVO;
            _noMail.SetActive(_mailList.DataArray.Count == 0);
            _leftObj.SetActive(_mailList.DataArray.Count > 0);
            _rightObj.SetActive(_mailList.DataArray.Count > 0);
            _mailList.SelectedIndex = -1;
            _mailList.SelectedIndex = 0;
        }

        private void OnAllDelete()
        {
            List<int> lstId = new List<int>();
            List<MailDataVO> lstMailVO = new List<MailDataVO>();
            lstMailVO = MailDataModel.Instance.OnMailData();
            for (int i = 0; i < lstMailVO.Count; i++)
            {
                if (!lstMailVO[i].mIsHasAtt|| lstMailVO[i].mIsHasAtt&& lstMailVO[i].mIsGetAtt)
                    lstId.Add(lstMailVO[i].mMailID);
            }
            if (lstId.Count > 0)
                GameApp.Instance.GameServer.ReqMailDelete(lstId);
        }

        private void OnAllGetAtt()
        {
            List<int> lstId = new List<int>();
            List<MailDataVO> lstMailVO = new List<MailDataVO>();
            lstMailVO = MailDataModel.Instance.OnMailData();
            for (int i = 0; i < lstMailVO.Count; i++)
            {
                if (lstMailVO[i].mIsHasAtt && !lstMailVO[i].mIsGetAtt)
                    lstId.Add(lstMailVO[i].mMailID);
            }
            if (lstId.Count > 0)
                GameApp.Instance.GameServer.ReqMailGetAtt(lstId);
        }

        private void OnMailGetDelete()
        {
            List<int> lstId = new List<int>();
            lstId.Add(_curMailVO.mMailID);
            if (_curMailVO.mIsHasAtt && !_curMailVO.mIsGetAtt)
                GameApp.Instance.GameServer.ReqMailGetAtt(lstId);
            else
                GameApp.Instance.GameServer.ReqMailDelete(lstId);
        }
    }
}
