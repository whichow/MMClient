// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BuildingShopItem" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using Game.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 主要负责城建元素列表拖拽事件处理类
    /// </summary>
    public class BuildingShopItem : KUIItem, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        #region Static

        public static readonly Vector2 DragDelta = new Vector2(0.1f, 1f);

        #endregion

        #region Field

        public Transform dragger;

        private GameObject _shopData;
        private GameObject _shop;
        private Text _shopCount;
        private Text _shopCost;
        private GameObject _bagData;
        private Text _bagCount;
        private Text _bagCharm;
        private Text _title;
        private KUIImage _icon;
        private Button _infor;
        private Button _itemBtn;

        private Coroutine _dragCoroutine;
        private bool _dragging;
        private bool _openShop;
        private KUIGrid _scroller;

        private bool _active;
        private bool _showBag;

        BuildingShopWindow _buildingShopWindow;
        #endregion

        #region Property

        private bool active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                _icon.ShowGray(!_active);
                _shop.SetActive(_active);
                _shopCost.enabled = value;
                _itemBtn.enabled = value;
            }
        }

        public KItemBuilding entity { get; protected set; }

        public bool ItemActive
        {
            get
            {
                return this.dragger.gameObject.activeSelf;
            }
            set
            {
                this.dragger.gameObject.SetActive(value);
            }
        }

        private KUIGrid scrollRect
        {
            get
            {
                if (_scroller == null)
                {
                    _scroller = GetComponentInParent<KUIGrid>();
                }
                return _scroller;
            }
        }
        /// <summary>
        /// 当前城建元素最大可建设的数量
        /// </summary>
        private int buildingMaxNum
        {
            get
            {
                return BuildingManager.Instance.GetBuildingMaxCount(entity.itemID);
            }
        }
        /// <summary>
        /// 当前城建元素已建设的数量
        /// </summary>
        private int buildingCurrNum
        {
            get
            {
                return BuildingManager.Instance.GetEntityCount(entity.itemID);
            }
        }

        #endregion

        #region Method

        private void ShowType(bool showBag)
        {
            int curBuildingCount = buildingCurrNum;
            int maxBuildingCount = buildingMaxNum;
            _showBag = showBag;   

            _bagData.SetActive(showBag);
            //_shopCount.gameObject.SetActive(!showBag);

            _shopData.SetActive(!showBag);

            _title.text = this.entity.itemName;
            _icon.overrideSprite = KIconManager.Instance.GetBuildingIcon(entity.iconName);
            //_icon.SetNativeSize();

            dragger = _icon.transform.parent;

            if (showBag)
            {
                _bagCount.text = string.Format("{0:N0}", entity.curCount);
                _bagCharm.text = entity.charm.ToString();
                this.active = entity.curCount > 0;   //设置灰度图片
            }
            else
            {
                _shopCost.text = entity.unlockCost.itemCount.ToString();
                _shopCount.text = string.Format("{0}/{1}", curBuildingCount, maxBuildingCount);
                this.active = PlayerDataModel.Instance.mPlayerData.mLevel >= entity.unlockLevel && curBuildingCount < maxBuildingCount;   //设置灰度图片
                //Debug.Log("类型" + entity.type + "/" + curBuildingCount + "/" + maxBuildingCount);
            }
        }

        private IEnumerator DragReturn()
        {
            if (this.dragger.localPosition.sqrMagnitude > 0.01f)
            {
                this.dragger.localPosition = (this.dragger.localPosition * 0.1f);
                yield return null;

            }
            this.dragger.localPosition = Vector3.zero;
        }

        private void EntityBuilder_OnGlobalBuilt(Game.Build.Building entityBase, bool built)
        {
            //if (this.entity == entityBase.EntityData)
            //{
            //    EntityBuilder.OnGlobalConfirm -= this.EntityBuilder_OnGlobalBuilt;
            //    if (built)
            //    {
            //        this.OpenShop();
            //    }
            //}
        }
        private void showInfor()
        {
            _buildingShopWindow.showInfor(true,_infor.transform);
        }
        private void hideInfor()
        {
            _buildingShopWindow.showInfor(false);
        }

        private void CreateBuilding()
        {
            Debuger.Log("点击建筑，开始创建");
            this.dragger.localPosition = Vector3.zero;
            BuildingItemCreate(false);
        }

        protected override void Refresh()
        {
            this.entity = data as KItemBuilding;
            if (this.entity != null)
            {
                this.ShowType(this.entity.itemTag > 1);
            }
        }

        public void OpenShop()
        {
            if (!this._openShop)
            {
            }
        }

        #endregion

        #region Interface

        public void OnBeginDrag(PointerEventData eventData)
        {
            this._dragging = true;
            this.scrollRect.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (GuideManager.Instance.IsGuideing)
            {
                return;
            }

            if (this._dragging)
            {
                if (!this.active)
                {
                    this.scrollRect.OnDrag(eventData);
                }
                else
                {
                    if (this._dragCoroutine != null)
                    {
                        this.StopCoroutine(this._dragCoroutine);
                        this._dragCoroutine = null;
                    }
                    this.scrollRect.OnDrag(eventData);
                    this.dragger.localPosition = eventData.position - eventData.pressPosition;// new Vector3(eventData.position.x - eventData.pressPosition.x, eventData.position.y - eventData.pressPosition.y);
                    if ((eventData.position.y / Screen.height) > 0.32f)
                    {
                        this._openShop = false;
                        this.OnEndDrag(eventData);
                        this.dragger.localPosition = Vector3.zero;
                        BuildingItemCreate(true);
                    }
                    else
                    {
                        var local = this.dragger.localPosition;
                        this.dragger.localPosition = new Vector3(local.x * DragDelta.x, (Mathf.Pow(Mathf.Abs(local.y), 0.65f) * Mathf.Sign(local.y)) * DragDelta.y);
                    }
                }
            }
        }

        /// <summary>
        /// 城建元素创建
        /// </summary>
        private void BuildingItemCreate(bool isDrag)
        {
            if (_showBag)
            {
                if (entity.curCount <= 0)
                    return;
            }
            else
            {

                if (buildingCurrNum >= buildingMaxNum)
                {
                    return;
                }

            }
            KUIWindow.CloseWindow<Game.UI.BuildingShopWindow>();

            if (this.entity.type == (int)Building.Category.kSurface)
            {
                BuildingSurfaceManager.Instance.EnterEditorMod(this.entity);
            }
            else
            {
                BuildingManager.Instance.CreateBuilding(this.entity, isDrag);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (this._dragging)
            {
                this.scrollRect.OnEndDrag(eventData);
                this._dragging = false;
                if (this.active & (this._dragCoroutine == null))
                {
                    this._dragCoroutine = StartCoroutine(this.DragReturn());
                }
            }
        }

        #endregion

        #region Unity

        private void Awake()
        {
            dragger = Find<Transform>("Item/Dragger");
            _icon = Find<KUIImage>("Item/Dragger/Icon");

            _title = Find<Text>("Item/Title");
            _shopCost = Find<Text>("Item/Shop/CostCount");
            _shopCount = Find<Text>("Item/Count");
            _shopData = Find("Item/Shop/Cost");
            _shop = Find("Item/Shop");
            _infor = Find<Button>("Item/Infor");
            _infor.onClick.AddListener(showInfor);
            _itemBtn = Find<Button>("Item");
            _itemBtn.onClick.AddListener(CreateBuilding);
            _buildingShopWindow = KUIWindow.GetWindow<BuildingShopWindow>();

            _bagCount = Find<Text>("Item/Count");
            _bagCharm = Find<Text>("Item/Shop/CostCount");
            _bagData = Find("Item/Shop/TextLeft");

        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        #endregion
    }
}
