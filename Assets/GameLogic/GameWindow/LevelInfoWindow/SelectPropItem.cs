///** 
//*FileName:     SelectPropItem.cs 
//*Author:       HASEE 
//*Version:      1.0 
//*UnityVersion：5.6.2f1
//*Date:         2017-10-27 
//*Description:    
//*History: 
//*/
//using UnityEngine;
//using UnityEngine.UI;
//namespace Game.UI
//{
//    public class SelectPropItem : KUIItem
//    {


//        private Image icon;
//        private Toggle toggle;

//        public KItemProp propData;

//        public int shopId;

//        private GameObject selectFlag;

//        private GameObject numObj;

//        private Text numText;

//        private Text moneyText;

//        private Image moneyIcon;

//        public bool isSelect;

//        public void Show(KItemProp prop,int sID)
//        {
//            shopId = sID;
//            isSelect = false;
//            propData = prop;
//            ShowPropInfo();
//            ShowMoney();
//        }


//        void ShowPropInfo()
//        {
//            if (propData == null)
//                return;
//            if (icon != null)
//            {
//                icon.overrideSprite = KIconManager.Instance.GetPropIcon(propData.itemID);
//            }
           
//            RefreshProp(false);
//        }
//        public void RefreshProp(bool arg0)
//        {
//            selectFlag.SetActive(arg0);
//            ShowPropNum(arg0);
//            if (arg0)
//            {
//                //TODO:Click Prop
//                isSelect = true;
//            }
//            else
//            {
//                isSelect = false;
//            }
//        }
//        private void OnItemClick(bool arg0)
//        {
//            if (propData.curCount > 0)
//            {
//                selectFlag.SetActive(arg0);
//                ShowPropNum(arg0);
//                if (arg0)
//                {
//                    //TODO:Click Prop
//                    isSelect = true;
//                }
//                else
//                {
//                    isSelect = false;
//                }
//            }
//            else
//            {
//                KUIWindow.OpenWindow<BuyPopNumWindow>(new BuyPopNumWindow.Data()
//                {
//                    itemId = shopId,
//                    onCancel = delegate () { OnBuyCancel(); },
//                    onConfirm = delegate () { OnBuyProp(); },
//                });
//            }
//        }
//        public void OnBuyProp()
//        {
//            KUIWindow.CloseWindow<BuyPopNumWindow>();
//            RefreshProp(false);
//        }
//        public void OnBuyCancel()
//        {

//        }
//        void ShowPropNum(bool v)
//        {
//            numObj.SetActive(!v);
//            if (!v)
//            {
//                numText.text = propData.curCount.ToString();
//            }
//        }
//        void ShowMoney()
//        {
//            var item=KItemManager.Instance.GetPack(shopId);
//            moneyIcon.overrideSprite = KIconManager.Instance.GetMoneyIcon(item.Money);
//            moneyText.text = string.Format("{0:N0}", item.Cost);
//        }
//        #region Unity
//        private void Awake()
//        {
//            icon = transform.Find("itemback/Icon").GetComponent<Image>();
//            toggle = transform.Find("itemback").GetComponent<Toggle>();
//            selectFlag = transform.Find("itemback/CountOK").gameObject;
//            numObj = transform.Find("itemback/Count1").gameObject;
//            numText = numObj.transform.Find("Text").GetComponent<Text>();
//            moneyText = transform.Find("itemback/Back/Text").GetComponent<Text>();
//            moneyIcon = transform.Find("itemback/Back/Image").GetComponent<Image>();

//            toggle.onValueChanged.AddListener(OnItemClick);
//        }


//        #endregion
//    }
//}
