using Game.Build;
using Msg.ClientMessage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class FriendWindow
    {
        private FriendDataVO _curFriendDataVO;
        //好友页签
        private UIList _friendList;
        private GameObject _friendObj;
        private GameObject _tips;
        private GameObject _numObj;
        private GameObject _head;
        private GameObject _operate;
        private GameObject _inputObj;
        private GameObject _emojObj;
        private GameObject _sendObj;
        private GameObject _deleteObj;
        private Button _presentBtn;
        private Button _getBtn;
        private Button _deleteBtn;
        private Button _deleteCloseBtn;
        private Button _deleteCanelBtn;
        private Button _deleteYesBtn;
        private Button _room;
        private Button _visite;
        private Button _zan;
        private KUIImage _zanImg;
        private Text _numText;
        private KUIImage _icon;
        private Text _level;
        private Text _name;
        private Text _zanNum;
        private int _curId = 0;
        //添加页签
        private GameObject _aplicationObj;
        private InputField _inputField;
        private Button _removeBtn;
        private Button _searchBtn;
        private UIList _addLeftList;
        private UIList _addRightList;
        private GameObject _empty;

        private void InitView()
        {
            //好友页签
            _friendList = Find<UIList>("Friend/Left/List");
            _friendList.SetRenderHandler(RenderHandler);
            _friendList.SetSelectHandler(SelectHandler);
            _friendObj = Find("Friend");
            _tips = Find("Friend/Left/Tips");
            _numObj = Find("Friend/Left/Num");
            _head = Find("Friend/Right/Top/Head");
            _operate = Find("Friend/Right/Top/Operate");
            _inputObj = Find("Friend/Right/Bottom/btn_input");
            _emojObj = Find("Friend/Right/Bottom/btn_emoj");
            _sendObj = Find("Friend/Right/Bottom/btn_send");
            _deleteObj = Find("Delete");
            _presentBtn = Find<Button>("Friend/Left/btn_present");
            _getBtn = Find<Button>("Friend/Left/btn_get");
            _deleteBtn = Find<Button>("Friend/Right/Top/Operate/delete");
            _room = Find<Button>("Friend/Right/Top/Operate/room");
            _visite = Find<Button>("Friend/Right/Top/Operate/visite");
            _zan = Find<Button>("Friend/Right/Top/Operate/Zan");
            _zanImg = Find<KUIImage>("Friend/Right/Top/Operate/Zan/Image");
            _deleteCloseBtn = Find<Button>("Delete/Close");
            _deleteCanelBtn = Find<Button>("Delete/ButtonCanel");
            _deleteYesBtn = Find<Button>("Delete/ButtonOK");
            _numText = Find<Text>("Friend/Left/Num/Text");
            _icon = Find<KUIImage>("Friend/Right/Top/Head/head/Image");
            _level = Find<Text>("Friend/Right/Top/Head/Icon/Level/Text");
            _name = Find<Text>("Friend/Right/Top/Head/Name");
            _zanNum = Find<Text>("Friend/Right/Top/Head/Image/Text");
            _presentBtn.onClick.AddListener(OnpresentBtn);
            _getBtn.onClick.AddListener(OnGetBtn);
            _deleteCloseBtn.onClick.AddListener(OnClose);
            _deleteCanelBtn.onClick.AddListener(OnClose);
            _deleteYesBtn.onClick.AddListener(OnDeleteOk);
            //添加页签
            _addLeftList = Find<UIList>("Aplication/Left/List");
            _addLeftList.SetRenderHandler(LeftRenderHandler);
            _addLeftList.SetSelectHandler(LeftSelectHandler);
            _addRightList = Find<UIList>("Aplication/Right/List");
            _addRightList.SetRenderHandler(RightRenderHandler);
            _addRightList.SetSelectHandler(RightSelectHandler);
            _aplicationObj = Find("Aplication");
            _inputField = Find<InputField>("Aplication/InputField");
            _inputField.onValidateInput = OnValidateInput;
            _removeBtn = Find<Button>("Aplication/InputField/clear");
            _removeBtn.onClick.AddListener(OnRemoveBtn);
            _searchBtn = Find<Button>("Aplication/btn_search");
            _searchBtn.onClick.AddListener(OnSearchBtn);
            _empty = Find("Aplication/Right/Empty");

        }

        public override void AddEvents()
        {
            base.AddEvents();
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendSearch, OnFriendSearch);
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendAdd, OnFriendAdd);
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendReqNotify, OnFriendReqNotify);
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendReplyAdd, OnFriendReqNotify);
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendRefuse, OnFriendReqNotify);
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendRemove, OnFriendNotify);
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendGivePoint, OnFriendGivePoint);
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendGetPoint, OnFriendGetPoint);
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendAllData, OnFriendData);
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendNotify, OnFriendData);
            FriendDataModel.Instance.AddEvent(FriendEvent.FriendZan, OnZanPlayer);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendSearch, OnFriendSearch);
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendAdd, OnFriendAdd);
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendReqNotify, OnFriendReqNotify);
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendReplyAdd, OnFriendReqNotify);
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendRefuse, OnFriendReqNotify);
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendRemove, OnFriendNotify);
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendGivePoint, OnFriendGivePoint);
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendGetPoint, OnFriendGetPoint);
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendAllData, OnFriendData);
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendNotify, OnFriendData);
            FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendZan, OnZanPlayer);
        }

        private void SelectHandler(UIListItem item, int index)
        {
            FriendDataVO vo = item.dataSource as FriendDataVO;
            if (vo == null)
                return;
            _curFriendDataVO = vo;
            HeadIconUtils.SetHeadIcon(_curFriendDataVO.mHeadId, _curFriendDataVO.mPlayerId, _icon);
            _level.text = _curFriendDataVO.mLevel.ToString();
            _name.text = _curFriendDataVO.mName;
            _zanNum.text = _curFriendDataVO.mZanNum.ToString();
            _deleteBtn.onClick.RemoveAllListeners();
            _deleteBtn.onClick.AddListener(() => { OnDeleteBtn(_curFriendDataVO.mPlayerId); });
            _room.onClick.RemoveAllListeners();
            _room.onClick.AddListener(() => { OnRoomBtn(_curFriendDataVO.mPlayerId); });
            _visite.onClick.RemoveAllListeners();
            _visite.onClick.AddListener(() => { OnVisiteBtn(_curFriendDataVO.mPlayerId); });
            _zan.onClick.RemoveAllListeners();
            _zan.onClick.AddListener(() => { OnZanBtn(_curFriendDataVO.mPlayerId); });
            if (_curFriendDataVO.mIsZan)
                _zanImg.overrideSprite = _zanImg.sprites[0];
            else
                _zanImg.overrideSprite = _zanImg.sprites[1];
        }

        private void RenderHandler(UIListItem item, int index)
        {
            FriendDataVO vo = item.dataSource as FriendDataVO;
            if (vo == null)
                return;
            item.GetComp<Text>("Head/Icon/Level/Text").text = vo.mLevel.ToString();
            item.GetComp<Text>("Name").text = vo.mName;
            Image icon = item.GetComp<KUIImage>("Head/head/Image");
            HeadIconUtils.SetHeadIcon(vo.mHeadId, vo.mPlayerId, icon);
            item.GetGameObject("Point").SetActive(vo.mUnreadMessageNum > 0);
            item.GetComp<Text>("Point/Text").text = vo.mUnreadMessageNum.ToString();
            item.GetComp<Text>("Time").text = (K.Extension.TimeExtension.ToDataTime(GetTimeStamp(true) - vo.mLastLogin)).ToLocalTime().ToString("HH:mm");
            KUIImage imgInteract = item.GetComp<KUIImage>("btn_interact");
            if (vo.mFriendPoint > 0)
            {
                imgInteract.material = null;
                imgInteract.overrideSprite = imgInteract.sprites[0];
            }
            else
            {
                imgInteract.overrideSprite = imgInteract.sprites[1];
                if (FriendDataModel.Instance.mLeftGivePointNum > 0)
                {
                    if (vo.LeftGiveSecond <= 0)
                        imgInteract.material = null;
                    else
                        imgInteract.material = Resources.Load<Material>("Materials/UIGray");
                }
                else
                {
                    imgInteract.material = Resources.Load<Material>("Materials/UIGray");
                }
            }
            item.GetComp<Button>("btn_interact").onClick.RemoveAllListeners();
            item.GetComp<Button>("btn_interact").onClick.AddListener(() => { OnDrawBtn(vo.mPlayerId, vo.mFriendPoint, vo.LeftGiveSecond); });
        }

        private void OnDrawBtn(int Id, int pointNum,int leftGiveSecond)
        {
            List<int> lstId = new List<int>();
            lstId.Add(Id);
            if (pointNum > 0)
            {
                GameApp.Instance.GameServer.ReqGetFriendPoint(lstId);
            }
            else
            {
                if (leftGiveSecond <= 0)
                    GameApp.Instance.GameServer.ReqGiveFriendPoint(lstId);
                else
                    Debuger.Log(KLocalization.GetLocalString(53003));
                //ToastBox.ShowText(KLocalization.GetLocalString(53003));
            }
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>
        /// <returns></returns>
        public static int GetTimeStamp(bool bflag = true)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            int ret;
            if (bflag)
                ret = Convert.ToInt32(ts.TotalSeconds);
            else
                ret = Convert.ToInt32(ts.TotalMilliseconds);
            return ret;
        }

        private void OnpresentBtn()
        {
            List<FriendDataVO> lstVO = FriendDataModel.Instance.GetAllFriend();
            List<int> lstId = new List<int>();
            int num = 0;
            for (int i = 0; i < lstVO.Count; i++)
            {
                if (lstVO[i].LeftGiveSecond <= 0)
                {
                    num++;
                    if (num > FriendDataModel.Instance.mLeftGivePointNum)
                        break;
                    lstId.Add(lstVO[i].mPlayerId);
                }
            }
            if (lstId.Count > 0)
                GameApp.Instance.GameServer.ReqGiveFriendPoint(lstId);
        }

        private void OnGetBtn()
        {
            List<FriendDataVO> lstVO = FriendDataModel.Instance.GetAllFriend();
            List<int> lstId = new List<int>();
            for (int i = 0; i < lstVO.Count; i++)
            {
                if (lstVO[i].mFriendPoint > 0)
                    lstId.Add(lstVO[i].mPlayerId);
            }
            if (lstId.Count > 0)
                GameApp.Instance.GameServer.ReqGetFriendPoint(lstId);
        }

        private void OnDeleteBtn(int id)
        {
            _deleteObj.SetActive(true);
            _curId = id;
        }

        private void OnRoomBtn(int id)
        {
            GameApp.Instance.GameServer.ReqSpaceOther(id);
        }

        private void OnVisiteBtn(int id)
        {
            BuildingManager.Instance.VisitPlayer(id, true);
            CloseWindow(this);
        }

        private void OnZanBtn(int id)
        {
            if (!_curFriendDataVO.mIsZan)
                GameApp.Instance.GameServer.ReqZanPlayer(id);
        }

        private void OnDeleteOk()
        {
            GameApp.Instance.GameServer.ReqRemoveFirend(_curId);
        }

        private void OnClose()
        {
            _deleteObj.SetActive(false);
        }

        private void RefreshView(int type)
        {
            if (_addLeftList.DataArray != null)
                _addLeftList.Clear();
            _inputField.text = "";
            _friendObj.SetActive(type == FriendTypeConst.Friend);
            _aplicationObj.SetActive(type == FriendTypeConst.AddFriend);
            OnFriendList();
            OnRightList();
        }

        private void OnFriendList()
        {
            _friendList.DataArray = FriendDataModel.Instance.GetAllFriend();
            _numText.text = KLocalization.GetLocalString(53067) + _friendList.DataArray.Count + "/200";
            _tips.SetActive(_friendList.DataArray.Count <= 0);
            _numObj.SetActive(_friendList.DataArray.Count > 0);
            _presentBtn.gameObject.SetActive(_friendList.DataArray.Count > 0);
            _getBtn.gameObject.SetActive(_friendList.DataArray.Count > 0);
            _head.SetActive(_friendList.DataArray.Count > 0);
            _operate.SetActive(_friendList.DataArray.Count > 0);
            _inputObj.SetActive(_friendList.DataArray.Count > 0);
            _emojObj.SetActive(_friendList.DataArray.Count > 0);
            _sendObj.SetActive(_friendList.DataArray.Count > 0);
            _friendList.SelectedIndex = -1;
            _friendList.SelectedIndex = 0;
        }

        private void OnRightList()
        {
            _addRightList.DataArray = FriendDataModel.Instance.GetAllFriendReq();
            _empty.SetActive(_addRightList.DataArray.Count <= 0);
        }

        private void OnFriendSearch(IEventData value)
        {
            _inputField.text = "";
            if ((value as FriendDataModelVO).mLstInfo.Count > 0)
                _addLeftList.DataArray = (value as FriendDataModelVO).mLstInfo;
            else
                ToastBox.ShowText(KLocalization.GetLocalString(53096));
        }

        private void OnFriendAdd(IEventData value)
        {
            _addLeftList.Clear();
        }

        private void OnFriendReqNotify()
        {
            OnRightList();
        }

        private void OnFriendNotify()
        {
            OnClose();
            OnFriendList();
        }

        private void OnFriendGivePoint(IEventData value)
        {
            _friendList.Refresh(FriendDataModel.Instance.GetAllFriend());
            ToastBox.ShowText(string.Format(KLocalization.GetLocalString(53001), (value as EventData).Integer.ToString()));
        }

        private void OnFriendGetPoint(IEventData value)
        {
            _friendList.Refresh(FriendDataModel.Instance.GetAllFriend());
            ToastBox.ShowText(string.Format(KLocalization.GetLocalString(53002), (value as EventData).Integer.ToString()));
        }

        private void RightRenderHandler(UIListItem item, int index)
        {
            FriendReq vo = item.dataSource as FriendReq;
            if (vo == null)
                return;
            item.GetComp<Text>("Head/Icon/Level/Text").text = vo.Level.ToString();
            item.GetComp<Text>("Name").text = vo.Name;
            item.GetComp<Text>("txt_signature").text = KLocalization.GetLocalString(53090);
            Image icon = item.GetComp<Image>("Head/head/Image");
            HeadIconUtils.SetHeadIcon(vo.Head, vo.PlayerId, icon);
            item.GetComp<Button>("btn_agree").onClick.AddListener(() => { OnAgreeBtn(vo.PlayerId); });
            item.GetComp<Button>("btn_ignore").onClick.AddListener(() => { OnRefuseBtn(vo.PlayerId); });
        }

        private void RightSelectHandler(UIListItem item, int index)
        {

        }

        private void LeftRenderHandler(UIListItem item, int index)
        {
            FriendInfo vo = item.dataSource as FriendInfo;
            if (vo == null)
                return;
            item.GetComp<Text>("Head/Icon/Level/Text").text = vo.Level.ToString();
            item.GetComp<Text>("Name").text = vo.Name;
            item.GetComp<Text>("txt_signature").text = KLocalization.GetLocalString(53090);
            Image icon = item.GetComp<Image>("Head/head/Image");
            HeadIconUtils.SetHeadIcon(vo.Head, vo.PlayerId, icon);
            item.GetComp<Button>("btn_interact").onClick.AddListener(() => { OnInteractBtn(vo.PlayerId); });
        }

        private void LeftSelectHandler(UIListItem item, int index)
        {

        }

        public int GetStringLength(string str)
        {
            if (str.Equals(string.Empty))
                return 0;
            int strlen = 0;
            ASCIIEncoding strData = new ASCIIEncoding();
            //将字符串转换为ASCII编码的字节数字
            byte[] strBytes = strData.GetBytes(str);
            for (int i = 0; i <= strBytes.Length - 1; i++)
            {
                if (strBytes[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                    strlen++;
                strlen++;
            }
            return strlen;
        }

        public char OnValidateInput(string text, int charInput, char addedChar)
        {
            if (GetStringLength((text + addedChar).ToString()) > 14)
                return '\0';
            return addedChar;
        }

        private void OnRemoveBtn()
        {
            _inputField.text = "";
        }

        private void OnSearchBtn()
        {
            if (_inputField.text != "")
                GameApp.Instance.GameServer.ReqFriendSearch(_inputField.text);
        }

        private void OnAgreeBtn(int id)
        {
            GameApp.Instance.GameServer.ReqAddFriend(id);
        }

        private void OnRefuseBtn(int id)
        {
            GameApp.Instance.GameServer.ReqRefuseFirend(id);
        }

        private void OnInteractBtn(int id)
        {
            GameApp.Instance.GameServer.ReqAddFriendById(id);
        }

        private void OnZanPlayer()
        {
            _zanImg.overrideSprite = _zanImg.sprites[0];
            _zanNum.text = _curFriendDataVO.mZanNum.ToString();
        }
    }
}
