using Game.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 障碍物
    /// </summary>
    class BuildingObstacle : Building
    {
        #region field
        BubbleObstacleClear _bubbleObstacleClear;
        ItemInfo[] _gifts;
        Int3[] _giftIconIds;
        private bool _isClear;
        private int _toolNum
        {
            get
            {
                KItem kItem = KItemManager.Instance.GetItem(entityData.removeCost.itemID);
                if (kItem != null)
                    return kItem.curCount;
                else
                    return 0;
            }
            
        }
        #endregion
        #region Method

        public override void DelBuilding()
        {
            _isClear = true;
        }
        protected override void OnTap()
        {
            //base.OnTap();
            //GameCamera.Instance.Show(this.entityView.centerNode.position);
            Debug.Log("障碍物点击事件");
            if (isOneSelf)
            {
                clearviewShow();
            }


        }
        protected override void OnFocus(bool focus)
        {
            base.OnFocus(focus);

            if (!focus)
            {
                if (isOneSelf)
                {
                    clearviewHide();
                }
            }
               



        }
        void clearviewHide()
        {
            if (_bubbleObstacleClear != null)
            {
                //BubbleManager.Instance.HideObstacleClear();
                _bubbleObstacleClear = null;
            }
        }
        void clearviewShow()
        {
            _bubbleObstacleClear = BubbleManager.Instance.ShowObstacleClear(this);
            _bubbleObstacleClear.DragToolImgSet(entityData.removeCost.itemID);
            _bubbleObstacleClear.EndDragSet(EndDrag);
            _bubbleObstacleClear.ClearToolNumSet(KItemManager.Instance.GetItem(entityData.removeCost.itemID).curCount);

        }
        private void EndDrag()
        {
            Debug.Log("拖拽结束");
            clearObstacle();
            clearviewHide();
            //clearviewShow();
        }
        private void SrartDrag()
        {
            if (KItemManager.Instance.GetItem(entityData.removeCost.itemID).curCount <= entityData.removeCost.itemCount)  
            {
                KUIWindow.OpenWindow<MessageBox>(new MessageBox.Data()
                {
                    content = "",
                    onCancel = () => { },
                    onConfirm =() => { },


                });
                clearviewHide();
            }
            
        }
        /// <summary>
        /// 移除障碍物
        /// </summary>
        void clearObstacle()
        {
            //SelectionHighlight.Instance.HideSelection();
            if (_toolNum > 0)
            {
                AnimationFinish = animComplete;
                entityView.PlayAnimation(touchAnimation, false);
                KUser.BuildingRemoveBlock(buildingId, BuildingRemoveBlockBack);
            }
            else
            {
                KUIWindow.OpenWindow<MessageBox>(
                    new MessageBox.Data {

                        content = KLocalization.GetLocalString(52165),
                        onCancel = () => { },
                        onConfirm = () => {
                            KUIWindow.OpenWindow<ShopWindow>(ShopIDConst.Special);
                        },
                    }
                    );
            }
        }
        private void BuildingRemoveBlockBack(int codeId,string content,object data) 
        {
            if (codeId == 0)
            {
                DataParse(data);
                
                Debug.Log("移除成功"+ content);
            }
            else
            {
                Debug.Log("移除失败"+ content);
            }
        }
        /// <summary>
        /// 数据解析
        /// </summary>
        private void DataParse(object data)
        {
            S2CRemoveBlock[] s2CRemoveBlocks;
            BuildingManager.Instance.DataParse(data,out s2CRemoveBlocks);
            if (s2CRemoveBlocks != null)
            {
                ItemInfo[] lst = s2CRemoveBlocks[0].Items.ToArray();

                List<Int3> iconId = new List<Int3>();
                for (int i = 0; i < lst.Length; i++)
                {
                    iconId.Add(new Int3(lst[i].ItemCfgId, lst[i].ItemNum, (int)IconFlyMgr.GiftType.KNone));

                }
                CatInfo[] catInfos= s2CRemoveBlocks[0].Cats.ToArray();
                for (int i = 0; i < catInfos.Length; i++)
                {
                    iconId.Add(new Int3(catInfos[i].CatCfgId,1, (int)IconFlyMgr.GiftType.KCat));

                }
                DepotBuildingInfo[]  depotBuildingInfos = s2CRemoveBlocks[0].DepotBuildings.ToArray();
                for (int i = 0; i < depotBuildingInfos.Length; i++)
                {
                    iconId.Add(new Int3(depotBuildingInfos[i].CfgId, depotBuildingInfos[i].Num, (int)IconFlyMgr.GiftType.KBuilding));

                }
                _giftIconIds = iconId.ToArray();

                Debug.Log("奖励"+ _giftIconIds.Length);
            }
           
        }

        private void animComplete()
        {
            if (!_isClear)
                return;
            IconFlyMgr.Instance.IconPathGroupStart(this.transform.position, 0.2f, _giftIconIds);
            Destroy(this.gameObject);
        }

        // private void 

        #endregion


        #region Unity  

        // Use this for initialization
        private void Start()
        {
            entityView.ShowModel();

        }

        // Update is called once per frame
        private void Update()
        {

        }
        //private void LateUpdate()
        //{
        //    if (bubbleObstacleClear != null)
        //        bubbleObstacleClear.ClearToolPosSet(this.transform.position);
        //}

        #endregion
    }
}
