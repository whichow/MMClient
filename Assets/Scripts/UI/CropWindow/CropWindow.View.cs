// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CropWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.Build;
using UnityEngine;

namespace Game.UI
{
    partial class CropWindow
    {
        #region Field

        private Transform _gridParent;
        private Transform _itemPrefab;
        private KUIItemPool _layoutElementPool;

        #endregion

        #region Method 

        public void InitView()
        {
            _layoutElementPool = gameObject.GetComponentInChildren<KUIItemPool>();

            _gridParent = transform.Find("Scroll View/Viewport/Content");
            _itemPrefab = _gridParent.GetChild(0);
            _itemPrefab.gameObject.SetActive(false);
            _itemPrefab.gameObject.AddComponent<CropItem>();
        }

        public void RefreshView()
        {
            _layoutElementPool.Clear();
            var crops = GetCrops();

            if (crops != null)
            {
                var elements = _layoutElementPool.SpawnElements(crops.Length);
                for (int i = 0; i < crops.Length; i++)
                {
                    var cropItem = elements[i].GetComponent<CropItem>();
                    cropItem.Show(crops[i]);
                }
            }
        }

        #endregion  
    }
}