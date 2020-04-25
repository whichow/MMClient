using Game;
using Game.DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadItem : KUIItem
{
    private Image _img;
    private Button _btn;
    private GameObject _obj;
    private ItemXDM _itemXDM;

    private void Awake()
    {
        _img = Find<Image>("ImageHead");
        _btn = Find<Button>("ImageHead");
        _btn.onClick.AddListener(OnBtn);
        _obj = Find("BgBlack");
    }

    public void ShowData(ItemXDM itemXDM)
    {
        _itemXDM = itemXDM;
        _img.overrideSprite = KIconManager.Instance.GetItemIcon(_itemXDM.Icon);
        _obj.SetActive(_itemXDM.ID == PlayerDataModel.Instance.mPlayerData.mHead);
    }

    private void OnBtn()
    {
        if (_itemXDM.ID != PlayerDataModel.Instance.mPlayerData.mHead)
            GameApp.Instance.GameServer.ReqChangeHead(_itemXDM.ID);
    }
}
