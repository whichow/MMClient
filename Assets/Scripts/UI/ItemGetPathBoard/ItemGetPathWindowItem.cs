// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CatBagItem" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using Game.Build;
using Game.DataModel;

namespace Game.UI
{
    public class ItemGetPathWindowItem : KUIItem, IPointerClickHandler
    {
        #region Field
        private Text _txt_pathName;
        private KUIImage _img_icon;
        private GameObject _go_get;

        private List<int> _lstInt_way;
        private int _int_PathID;
        private int _int_idex;
        private int[] _int_indexs = new int[] { };
        #endregion
        private Dictionary<int, Action> _dictPath = new Dictionary<int, Action>();

        #region Unity
        private void Awake()
        {
            _txt_pathName = Find<Text>("imgBack/Text");
            _img_icon = Find<KUIImage>("imgBack/Image");
            _dictPath.Clear();
            _dictPath.Add(1, GoToShowWindow);
            _dictPath.Add(2, GoToPhotoWindow);
            _dictPath.Add(3, GoToOrnamentShopWindow);
            _dictPath.Add(4, GoToJiyangsuo);
            _dictPath.Add(5, GoToDiscoveryWindow);
            _dictPath.Add(6, GoToMapSelectWindow);
        }
        #endregion

        #region Method
        public void ShowGetPath(int[] paths, int index)
        {
            if (paths == null || paths.Length == 0)
            {
                return;
            }
            _int_PathID = paths[index];
            _txt_pathName.text = KItemGetPathManager.Instance.ShowGetPath(_int_PathID);
            _lstInt_way = new List<int>();
            int[] Nodes = KItemGetPathManager.Instance.DictGetPath[_int_PathID].getPathSequence;
            _int_idex = Nodes[0];
            _int_indexs = Nodes;
            _img_icon.overrideSprite = _img_icon.sprites[_int_idex - 1];
        }
        #endregion

        #region Interface
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _dictPath[_int_idex]();
        }
        private void GoToShowWindow()
        {
            KUIWindow.CloseWindow<ItemGetPath>();
            switch (_int_indexs[1])
            {
                case 1:
                    KUIWindow.OpenWindow<ShopWindow>(ShopIDConst.Special);
                    break;
                case 2:
                    KUIWindow.OpenWindow<ShopWindow>(ShopIDConst.Special);
                    break;
                case 3:
                    KUIWindow.OpenWindow<ShopWindow>(ShopIDConst.Special);
                    break;
                default:
                    break;
            }
        }
        private void GoToPhotoWindow()
        {
            KUIWindow.CloseWindow<ItemGetPath>();
            KUIWindow.OpenWindow<PhotoShopWindow>();
        }
        private void GoToOrnamentShopWindow()
        {
            if (BuildingManager.Instance.isExistBuilding(Building.Category.kManualWorkShop))
            {
                KUIWindow.CloseWindow<ItemGetPath>();
                KUIWindow.OpenWindow<FormulaShopWindow>();
            }
            else
            {
                ToastBox.ShowText("还没有手工作坊！");
            }
        }
        private void GoToDiscoveryWindow()
        {
            KExplore.Instance.GetAllTask((int code, string message, object data) =>
            {
                if (code == 0)
                {
                    KUIWindow.CloseWindow<ItemGetPath>();
                    KUIWindow.OpenWindow<DiscoveryWindow>();
                }
                //else
                //{
                //    Debug.Log("探索地带进入失败");
                //}
            });
        }
        private void GoToJiyangsuo()
        {
            BuildingManager.Instance.OpenFosterCate(oepnstatus => {
                OpenJiYangsuo(oepnstatus);
            });
        }
        private void OpenJiYangsuo(bool isOpenJiyangsuo) {
            if (isOpenJiyangsuo)
            {
                KUIWindow.CloseWindow<ItemGetPath>();
            }
            //else {
            //    Debug.Log("还不能进入寄养所");
            //}
        }
        private void GoToMapSelectWindow()
        {
            int thisMapID = KItemGetPathManager.Instance.DictGetPath[_int_PathID].getPathSequence[1];
            bool flag = XTable.LevelXTable.GetByID(thisMapID).Unlocked;
            if (flag)
            {
                KUIWindow.OpenWindow<MapSelectWindow>(new MapSelectWindow.MapSelectData(XTable.LevelXTable.GetByID(thisMapID), true));
                KUIWindow.CloseWindow<ItemGetPath>();
            }
            else {
                ToastBox.ShowText("尚未解锁的关卡！");
            }
        }
        #endregion
    }
}

