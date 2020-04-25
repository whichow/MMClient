using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    [AddComponentMenu("BuildingTeween/Other/MapClickFx")]
    class MapClickFx : MonoBehaviour , SceneClickEventBase
    {

        public enum MapType
        {
            Default,
            Ocean,
            Land,
            Sand,
        }
        public MapType mapType;
        KFx kFx;
        private GameObject _fxGameObject;
        public void OnFocus(bool focus)
        {
        }

        public void OnLongPress()
        {
        }

        public void OnTap()
        {
            //Debug.Log("点击事件");
            Vector3 pos = ScreenCoordinateTransform.Instance.sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -36;
            _fxGameObject.SetActive(true);
            _fxGameObject.transform.position = pos;
            _fxGameObject.SetActive(true);

        }
        private void Start()
        {

            kFx = GetComponentInChildren<KFx>();
            _fxGameObject = kFx.gameObject;
            this.gameObject.layer = LayerManager.ClickerLayer;
        }

    }
}
