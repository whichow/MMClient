using Game.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 宝箱
    /// </summary>
    class BuildingBox : Building
    {
        #region field
        enum BoxType
        {
            Wood = 1,
            Silver,
            Gold,

        }
        private bool isOpenBox;
        #endregion property
        private BoxType entityBoxType
        {
            get
            {
                if (entityData.removeCost.itemID == 0)
                    return BoxType.Wood;
                return (BoxType)entityData.removeCost.itemID;
            }
        }
        private Int3[] _giftIconIds;
        #region  

        #endregion
        #region method
        /// <summary>
        /// 删除建筑 回调
        /// </summary>
        public override void DelBuilding()
        {
            isOpenBox = true;
        }
        /// <summary>
        /// 标记是否已经打开
        /// </summary>
        private bool isOpened;
        /// <summary>
        /// 开宝箱
        /// </summary>
        private void openBox()
        {
            //SelectionHighlight.Instance.HideSelection();
            isOpened = true;
            openBoxBack();
        }
        //
        private void openBoxBack()
        {
            int playerId = 0;
            if (!BuildingManager.Instance.IsOneSelf)
                playerId = BuildingManager.Instance.otherPlayerInfo.PlayerId;
            Debug.LogFormat("打开宝箱playerId{0},buildingId{1}" , playerId, buildingId);
            KUser.BuildingOpenBox(playerId,buildingId, (codeId, content, data) =>
            {
                if (codeId == 0)
                {
                    isOpenBox = true;
                    DataParse(data);
                    entityView.PlayAnimation(touchAnimation, false);
                    if (_giftIconIds != null && _giftIconIds.Length > 0)
                    {
                        IconFlyMgr.Instance.IconPathGroupStart(this.transform.position, 0.5f, _giftIconIds);
                    }
                    else
                    {
                        Debuger.Log("宝箱奖励为空");
                    }
                }
                else
                {
                    isOpenBox = false;
                    Debug.Log("宝箱打开失败");
                }
            });
        }
        /// <summary>
        /// 数据解析
        /// </summary>
        private void DataParse(object data)
        {
            S2COpenMapChest[] s2COpenMapChests;
            BuildingManager.Instance.DataParse(data, out s2COpenMapChests);
            if (s2COpenMapChests != null)
            {
                ItemInfo[] lst = s2COpenMapChests[0].Items.ToArray();

                List<Int3> iconId = new List<Int3>();
                for (int i = 0; i < lst.Length; i++)
                {
                    iconId.Add(new Int3(lst[i].ItemCfgId, lst[i].ItemNum,(int)IconFlyMgr.GiftType.KNone));

                }
                CatInfo[] catInfos = s2COpenMapChests[0].Cats.ToArray();
                for (int i = 0; i < catInfos.Length; i++)
                {
                    iconId.Add(new Int3(catInfos[i].CatCfgId, 1,(int)IconFlyMgr.GiftType.KCat));

                }
                DepotBuildingInfo[] depotBuildingInfos = s2COpenMapChests[0].DepotBuildings.ToArray();
                for (int i = 0; i < depotBuildingInfos.Length; i++)
                {
                    iconId.Add(new Int3(depotBuildingInfos[i].CfgId, depotBuildingInfos[i].Num,(int)IconFlyMgr.GiftType.KBuilding));

                }
                _giftIconIds = iconId.ToArray();

                Debuger.Log("奖励" + _giftIconIds.Length);
            }

        }


        private void animationFinish()
        {
            if (!isOpenBox)
                return;
            isOpenBox = false;
            AnimationFinish = null;


            

            Debug.Log("销毁建筑");

            Destroy(this.gameObject);

        }
        /// <summary>
        /// 城建元素获取焦点事件
        /// </summary>
        protected override void OnTap()
        {
            //base.OnTap();
            //Debug.Log("宝箱打开");
            if (!BuildingManager.Instance.isFriend&&! BuildingManager.Instance.IsOneSelf)
            {
                ToastBox.ShowText(KLocalization.GetLocalString(53015));
                return;
            }

            var cost = entityData.removeCost;
            string boxContent;
            if (entityBoxType == BoxType.Wood)
            {
                boxContent = KLocalization.GetLocalString(52105);
            }
            else
            {
                KItem kItem = KItemManager.Instance.GetItem(cost.itemID);
                if (entityBoxType == BoxType.Silver)
                    boxContent = string.Format(KLocalization.GetLocalString(52104), cost.itemCount, kItem.itemName, KLocalization.GetLocalString(52029));
                else
                    boxContent = string.Format(KLocalization.GetLocalString(52104), cost.itemCount, kItem.itemName, KLocalization.GetLocalString(52030));


            }
            if (!isOpened)
            {
                KUIWindow.OpenWindow<MessageBox>(new MessageBox.Data()
                {
                    title = KLocalization.GetLocalString(58210),

                    content = boxContent,
                    onConfirm = () => openBox(),
                    onCancel = () => { },
                });
            }
            //var cost = entityData.removeCost;
            //if (cost.itemID > 0 && cost.itemCount > 0)
            //{
               
            //}
            //else
            //{
            //    openBox();
            //}         
           
        }

        #endregion

        #region Unity  

        // Use this for initialization
        private void Start()
        {
            entityView.ShowModel();
            AnimationFinish = animationFinish;

        }

        // Update is called once per frame
        private void Update()
        {

        }

        #endregion
    }
}
