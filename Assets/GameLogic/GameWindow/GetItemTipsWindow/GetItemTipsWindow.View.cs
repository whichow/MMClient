using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class GetItemTipsWindow
    {
        private UIList _tipsList;

        public void InitView()
        {
            _tipsList = Find<UIList>("Panel/List");
            _tipsList.SetRenderHandler(RenderHandler);
            _tipsList.SetPointerHandler(PointerHandler);
        }

        private void RenderHandler(UIListItem item, int index)
        {
            ItemInfo vo = item.dataSource as ItemInfo;
            if (vo == null)
                return;
            item.GetComp<Image>("Icon").overrideSprite = KIconManager.Instance.GetItemIcon(vo.ItemCfgId);
            item.GetComp<Text>("Num").text = vo.ItemNum.ToString();
        }

        private void PointerHandler(UIListItem item, int index)
        {
            //µã»÷ÊÂ¼þ
        }

        public void RefreshView()
        {
            _tipsList.DataArray = data as List<ItemInfo>;
        }
    }
}
