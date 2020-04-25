using Game.DataModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class HeadWindow
    {
        private Button _close;
        private UIList _headList;
        private List<ItemXDM> _lstItemXDM;
        private Image _headIcon;
        private GameObject _iconObj;
        private Button _itemHead;
        private Button _addHead;

        private void InitView()
        {
            _close = Find<Button>("Close");
            _close.onClick.AddListener(OnBackBtnClick);
            _headList = Find<UIList>("Scroll/Cont/List");
            _headList.SetRenderHandler(RenderHandler);
            _headList.SetPointerHandler(PointerHandler);
            _lstItemXDM = new List<ItemXDM>();
            List<ItemXDM> lstItemXDMs = XTable.ItemXTable.GetAllList();
            for (int i = 0; i < lstItemXDMs.Count; i++)
            {
                if (lstItemXDMs[i].Type == ItemTypeConst.Head)
                    _lstItemXDM.Add(lstItemXDMs[i]);
            }
            _lstItemXDM.Sort((x, y) => x.ID.CompareTo(y.ID));
            _headIcon = Find<Image>("Scroll/Cont/Group2/Item1/ImageHead");
            _iconObj = Find("Scroll/Cont/Group2/Item1/BgBlack");
            _itemHead = Find<Button>("Scroll/Cont/Group2/Item1");
            _itemHead.onClick.AddListener(OnItemHead);
            _addHead = Find<Button>("Scroll/Cont/Group2/Item2");
            _addHead.onClick.AddListener(OnAddHead);
        }

        public override void AddEvents()
        {
            base.AddEvents();
            PlayerDataModel.Instance.AddEvent(PlayerEvent.ChangeHead, RefreshView);
            EventManager.Instance.GlobalDispatcher.AddEvent(GlobalEvent.CUSTOM_HEADIMG_PICK_SUCC, OnSelect);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            PlayerDataModel.Instance.RemoveEvent(PlayerEvent.ChangeHead, RefreshView);
            EventManager.Instance.GlobalDispatcher.RemoveEvent(GlobalEvent.CUSTOM_HEADIMG_PICK_SUCC, OnSelect);
        }

        private void PointerHandler(UIListItem item, int index)
        {
            ItemXDM itemXDM = item.dataSource as ItemXDM;
            if (itemXDM == null)
                return;
        }

        private void RenderHandler(UIListItem item, int index)
        {
            ItemXDM itemXDM = item.dataSource as ItemXDM;
            if (itemXDM == null)
                return;
            item.GetComp<Image>("ImageHead").overrideSprite = KIconManager.Instance.GetItemIcon(itemXDM.Icon);
            item.GetGameObject("BgBlack").SetActive(itemXDM.ID == PlayerDataModel.Instance.mPlayerData.mHead);
            item.GetComp<Button>("ImageHead").onClick.RemoveAllListeners();
            item.GetComp<Button>("ImageHead").onClick.AddListener(() => { OnChangeHead(itemXDM.ID); });
        }

        private void OnChangeHead(int headId)
        {
            if (headId != PlayerDataModel.Instance.mPlayerData.mHead)
                GameApp.Instance.GameServer.ReqChangeHead(headId);
        }

        private void RefreshView()
        {
            _headList.DataArray = _lstItemXDM;
            string fileName = string.Format("{0}.jpg", PlayerDataModel.Instance.mPlayerData.mPlayerID);
            ImageUtils.LoadNetImage(fileName, _headIcon, (texture2D) => {
                _itemHead.gameObject.SetActive(texture2D != null);
            });
            _iconObj.SetActive(PlayerDataModel.Instance.mPlayerData.mHead == -1);
        }

        private void OnItemHead()
        {
            GameApp.Instance.GameServer.ReqChangeHead(-1);
        }

        private void OnAddHead()
        {
            CustomHeadImgPlugins.OpenAlbum();
        }

        private void OnSelect(IEventData value)
        {
            Texture2D tex2D = (value as EventData).Data as Texture2D;
            _itemHead.gameObject.SetActive(true);
            Rect rect = new Rect(0, 0, tex2D.width, tex2D.height);
            _headIcon.overrideSprite = Sprite.Create(tex2D, rect, _headIcon.rectTransform.pivot);
        }
    }
}
