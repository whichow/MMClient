using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Callback = System.Action<int, string, object>;
using Msg.ClientMessage;

namespace Game.UI
{
    public partial class PhotoShopGotBuildWindow : KUIWindow
    {

   
        public PhotoShopGotBuildWindow() :
                base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "PhotoShopGotBuild";
        }

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }
        private void OnAgainBtnClick()
        {
            if (GetWindow<PhotoShopPickCardHighWindow>().windowdata._type == 1)
            {

                if (BagDataModel.Instance.GetItemCountById(ItemIDConst.MidCard) > 0)
                {
                    KUser.DrawCard(2, 1, DrawMidCard);
                }
                else
                {
                    LackHintBox.ShowLackHintBox(ItemIDConst.MidCard);
                }
          
            }
            else
            {
                if (BagDataModel.Instance.GetItemCountById(ItemIDConst.Diamond) > 0)
                {
                    KUser.DrawCard(3, 1, DrawHighAgain);
                }
                else
                {
                    LackHintBox.ShowLackHintBox(ItemIDConst.Diamond);
                }
            }
        }
        private void OnFaceBookBtnClick()
        {
            Debug.Log("facebook分享");
        }
        private void OnWeChatBtnClick()
        {
            Debug.Log("微信分享");
        }
        private void OnQQBtnClick()
        {
            Debug.Log("QQ分享");
        }

        public override void Awake()
        {
            InitModel();
            InitView();
        }
        private void DrawHighAgain(int code, string message, object data)
        {
            if(code != 0)
            {
                return;
            }
            Debug.Log("高级抽卡");
            var list = data as ArrayList;
            if (list != null)
            {
                cat = null;
                kShopitem = null;
            
                foreach (var item in list)
                {
                    if (item is S2CDrawResult)
                    {
                        var result = (S2CDrawResult)item;
                        if (result.Cats != null)
                        {

                            for (int i = 0; i < result.Cats.Count; i++)
                            {
                                cat = KCatManager.Instance.GetCat(result.Cats[i].Id);
                            }
                        }
                        if (result.Buildings != null)
                        {
                            for (int i = 0; i < result.Buildings.Count; i++)
                            {
                                kShopitem = KItemManager.Instance.GetItem(result.Buildings[i].CfgId);
                            }
                        }
                    }
                }
            }
      
            if (cat != null)
            {
                RefreshView();
                return;
            }
            if (kShopitem != null)
            {
                RefreshView();
                return;
            }
            else
            {
                Debug.Log("高级抽卡失败");
            }
        }
        private void DrawMidCard(int doce, string message, object data)
        {
            Debug.Log("中级抽卡");
            var list = data as ArrayList;
            if (list != null)
            {
      
                foreach (var item in list)
                {
                    if (item is S2CDrawResult)
                    {
                        var result = (S2CDrawResult)item;
                        if (result.Cats != null)
                        {

                            for (int i = 0; i < result.Cats.Count; i++)
                            {
                                cat = KCatManager.Instance.GetCat(result.Cats[i].Id);
                            }
                        }
                        if (result.Buildings != null)
                        {
                            for (int i = 0; i < result.Buildings.Count; i++)
                            {
                                kShopitem = KItemManager.Instance.GetItem(result.Buildings[i].CfgId);
                            }
                        }
                    }
                }
            }
            if (cat != null)
            {
                RefreshView();
      
                return;
            }
            if (kShopitem != null)
            {
                RefreshView();
                return;
            }
            else
            {
                Debug.Log("中级抽卡失败");
            }
        }



        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
        }

        // Update is called once per frame
        public override void Update()
        {


        }
    }
}
