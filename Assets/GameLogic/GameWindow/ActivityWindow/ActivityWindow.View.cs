using Game.DataModel;
using Msg.ClientMessage;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class ActivityWindow
    {
        private GameObject _oneObj;
        private GameObject _twoObj;
        private GameObject _fourObj;
        private UIList _oneList;
        private Text _description;
        private Button _receiveBtn;
        private Button _payBtn;
        private Text _receiveText;
        private KUIImage _receiveImg;
        private KUIImage _cardBtnImg;
        private Text _cardText;
        private Text _cardMessage;
        private Button _cardBtn;
        private UIList _everydayList;


        public void InitView()
        {
            _oneObj = Find("OneTab");
            _twoObj = Find("TwoTab");
            _fourObj = Find("FourTab");
            _oneList = Find<UIList>("OneTab/List");
            _oneList.SetRenderHandler(RenderHandler);
            _description = Find<Text>("OneTab/Description");
            _receiveBtn = Find<Button>("OneTab/Action/ReceiveBtn");
            _receiveBtn.onClick.AddListener(OnReceive);
            _payBtn = Find<Button>("OneTab/Action/PayBtn");
            _payBtn.onClick.AddListener(OnTab1PayBtnClick);
            _receiveText = Find<Text>("OneTab/Action/ReceiveBtn/Text");
            _receiveImg = Find<KUIImage>("OneTab/Action/ReceiveBtn");
            _cardText = Find<Text>("FourTab/Card/Num");
            _cardBtnImg = Find<KUIImage>("FourTab/BuyBtn");
            _cardMessage = Find<Text>("FourTab/Message/Text");
            _cardBtn = Find<Button>("FourTab/BuyBtn");
            _cardBtn.onClick.AddListener(OnBuyCard);
            _everydayList = Find<UIList>("TwoTab/List");
            _everydayList.SetRenderHandler(EverydayRenderHandler);
            _everydayList.SetPointerHandler(EverydayPointerHandler);
        }

        private void EverydayPointerHandler(UIListItem item, int index)
        {
            SignXDM XDM = item.dataSource as SignXDM;
            if (XDM == null)
                return;
            if (XDM.ID > ActivityDataModel.Instance.mSignDataVO.mMinIndex && XDM.ID <= ActivityDataModel.Instance.mSignDataVO.mMaxIndex)
                GameApp.Instance.GameServer.ReqSignReward(ActivityDataModel.Instance.mSignDataVO.mMinIndex + 1);
        }

        private void EverydayRenderHandler(UIListItem item, int index)
        {
            SignXDM XDM = item.dataSource as SignXDM;
            if (XDM == null)
                return;
            item.GetComp<Text>("Count").text = XDM.Reward[1].ToString();
            item.GetComp<Image>("Icon").overrideSprite = KIconManager.Instance.GetItemIcon(XDM.Reward[0]);
            item.GetGameObject("Get").SetActive(XDM.ID <= ActivityDataModel.Instance.mSignDataVO.mMinIndex);
            item.GetGameObject("ImageBgLight").SetActive(XDM.ID > ActivityDataModel.Instance.mSignDataVO.mMinIndex && XDM.ID <= ActivityDataModel.Instance.mSignDataVO.mMaxIndex);
        }

        private void RenderHandler(UIListItem item, int index)
        {
            ItemInfo info = item.dataSource as ItemInfo;
            if (info == null)
                return;
            item.GetComp<Text>("Count").text = info.ItemNum.ToString();
            item.GetComp<Image>("Icon").overrideSprite = KIconManager.Instance.GetItemIcon(info.ItemCfgId);
        }

        public void RefreshView()
        {
            _oneObj.SetActive(_ActivityType == ActivityTypeConst.FirstCharge);
            _twoObj.SetActive(_ActivityType == ActivityTypeConst.EverydayReward);
            _fourObj.SetActive(_ActivityType == ActivityTypeConst.MonthlyCard);
            if (_ActivityType == ActivityTypeConst.FirstCharge)
            {
                _oneList.DataArray = ActivityDataModel.Instance.mChargeReward;
                _description.text = KLocalization.GetLocalString(55031);//Ê×³ä
                _receiveBtn.gameObject.SetActive(ActivityDataModel.Instance.mFirstChargeState != 0);
                _payBtn.gameObject.SetActive(ActivityDataModel.Instance.mFirstChargeState == 0);
                if (ActivityDataModel.Instance.mFirstChargeState == 2)
                {
                    _receiveImg.material = Resources.Load<Material>("Materials/UIGray");
                    _receiveText.text = KLocalization.GetLocalString(53701);
                }
                else
                {
                    _receiveImg.material = null;
                    _receiveText.text = KLocalization.GetLocalString(53704);
                }
            }
            else if(_ActivityType == ActivityTypeConst.MonthlyCard)
            {
                MonthCardData monthCardData = ActivityDataModel.Instance.mMonthCardData;
                _cardText.gameObject.SetActive(monthCardData != null);
                if (monthCardData != null)
                {
                    _cardBtnImg.material = Resources.Load<Material>("Materials/UIGray");
                    _cardText.text = monthCardData.SendMailNum + "/30" + KLocalization.GetLocalString(54189);
                }
                else
                {
                    _cardBtnImg.material = null;
                }
                _cardMessage.text = KLocalization.GetLocalString(55032);//ÔÂ¿¨
            }
            else
            {
                _everydayList.DataArray = ActivityDataModel.Instance.mSignDataVO.mListSignXDM;
            }
        }

        private void OnReceive()
        {
            if (ActivityDataModel.Instance.mFirstChargeState == 1)
                GameApp.Instance.GameServer.ReqChargeReward();
        }

        private void OnBuyCard()
        {
            if (ActivityDataModel.Instance.mMonthCardData == null)
                KUIWindow.OpenWindow<PayWindow>(1);
            else
                ToastBox.ShowText(KLocalization.GetLocalString(54193));
        }
    }
}
